using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirestoreGameSaver : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private FirebaseFirestore db;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    private void Start()
    {
        LoadPlayerDataFromFirestore();
    }

    public void LoadPlayerDataFromFirestore()
    {
        string currentSlot = SaveSlotsManager.SelectedSlotId;
        DocumentReference docRef = db.Collection("save_slots").Document(currentSlot);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully && task.Result.Exists)
            {
                DocumentSnapshot snapshot = task.Result;
                Dictionary<string, object> data = snapshot.ToDictionary();

                if (data.TryGetValue("playerX", out object x) && data.TryGetValue("playerY", out object y))
                {
                    float posX = System.Convert.ToSingle(x);
                    float posY = System.Convert.ToSingle(y);

                    if (playerTransform != null)
                    {
                        playerTransform.position = new Vector3(posX, posY, playerTransform.position.z);
                    }
                }
            }
        });
    }

    public void SavePlayerDataToFirestore()
    {
        if (playerTransform == null) return;

        string currentSlot = SaveSlotsManager.SelectedSlotId;
        DocumentReference docRef = db.Collection("save_slots").Document(currentSlot);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "playerX", playerTransform.position.x },
            { "playerY", playerTransform.position.y },
            { "lastSaved", Timestamp.GetCurrentTimestamp() }
        };

        docRef.SetAsync(updates, SetOptions.MergeAll);
    }
}