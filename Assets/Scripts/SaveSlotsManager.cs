using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class SaveSlotsManager : MonoBehaviour
{
    [System.Serializable]
    public struct SlotUI
    {
        public TMP_Text slotTitleText;
        public GameObject deleteButton;
    }

    [Header("Slot UI Elements (Slot 1, 2, 3)")]
    [SerializeField] private SlotUI slot1UI;
    [SerializeField] private SlotUI slot2UI;
    [SerializeField] private SlotUI slot3UI;

    [Header("Status Feedback")]
    [SerializeField] private TMP_Text statusText;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private string userId;

    public static string SelectedSlotId { get; private set; } // Used when loading the gameplay scene

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            // If not logged in, return to Auth scene
            SceneManager.LoadScene("Auth");
            return;
        }

        userId = auth.CurrentUser.UserId;
        LoadAllSlots();
    }

    public void LoadAllSlots()
    {
        if (statusText != null)
            statusText.text = "Fetching save slots...";

        FetchSlotData("slot_1", slot1UI);
        FetchSlotData("slot_2", slot2UI);
        FetchSlotData("slot_3", slot3UI);
    }

    private void FetchSlotData(string slotId, SlotUI ui)
    {
        DocumentReference docRef = db.Collection("users").Document(userId).Collection("saveSlots").Document(slotId);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || !task.Result.Exists)
            {
                DisplayEmptySlot(ui);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            SaveSlotData data = snapshot.ConvertTo<SaveSlotData>();

            if (data.slotMetaData.isSlotEmpty)
            {
                DisplayEmptySlot(ui);
            }
            else
            {
                if (ui.slotTitleText != null)
                    ui.slotTitleText.text = $"Save Slot ({slotId.Replace("_", " ").ToUpper()})";

                if (ui.deleteButton != null)
                    ui.deleteButton.SetActive(true);
            }

            if (statusText != null)
                statusText.text = "Select a save slot.";
        });
    }

    private void DisplayEmptySlot(SlotUI ui)
    {
        if (ui.slotTitleText != null)
            ui.slotTitleText.text = "Empty Slot";

        if (ui.deleteButton != null)
            ui.deleteButton.SetActive(false);
    }

    // Call when clicking Slot 1, 2, or 3 Button
    public void OnSelectSlot(string slotId)
    {
        SelectedSlotId = slotId;

        DocumentReference docRef = db.Collection("users").Document(userId).Collection("saveSlots").Document(slotId);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.Result.Exists || task.Result.ConvertTo<SaveSlotData>().slotMetaData.isSlotEmpty)
            {
                // Create Initial Save Slot
                InitializeNewSlot(slotId);
            }
            else
            {
                if (statusText != null)
                    statusText.text = $"Loading {slotId}...";

                // Load your main gameplay scene here (e.g. SceneManager.LoadScene("GameplayZone"));
            }
        });
    }

    private void InitializeNewSlot(string slotId)
    {
        if (statusText != null)
            statusText.text = "Creating new save file...";

        SaveSlotData newData = new SaveSlotData
        {
            slotMetaData = new SlotMetaData
            {
                isSlotEmpty = false,
                lastSaved = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
                playtimeSeconds = 0
            },
            playerTransform = new PlayerPosition
            {
                currentZoneId = "overgrown_village_west",
                x = 0f,
                y = 0f,
                sortingLayerOrder = 0
            },
            letters = new System.Collections.Generic.Dictionary<string, LetterStatus>
            {
                { "letter_1", new LetterStatus() },
                { "letter_2", new LetterStatus() },
                { "letter_3", new LetterStatus() }
            },
            worldState = new WorldState
            {
                natureGrowthStage = 1
            }
        };

        db.Collection("users").Document(userId).Collection("saveSlots").Document(slotId)
            .SetAsync(newData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    if (statusText != null)
                        statusText.text = "Failed to create save file.";
                    return;
                }

                if (statusText != null)
                    statusText.text = "Save created! Starting game...";

                // Load your main gameplay scene here
            });
    }

    // Call when clicking Delete Slot Button
    public void OnDeleteSlot(string slotId)
    {
        if (statusText != null)
            statusText.text = $"Deleting {slotId}...";

        SaveSlotData emptyData = new SaveSlotData
        {
            slotMetaData = new SlotMetaData { isSlotEmpty = true }
        };

        db.Collection("users").Document(userId).Collection("saveSlots").Document(slotId)
            .SetAsync(emptyData).ContinueWithOnMainThread(task =>
            {
                LoadAllSlots();
            });
    }
    // Call when clicking the Back Button
    public void OnClickBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}