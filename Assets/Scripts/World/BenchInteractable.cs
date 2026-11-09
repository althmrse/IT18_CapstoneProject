using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BenchInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Letter Display Panel")]
    [SerializeField] private GameObject letterPanel;
    [SerializeField] private TextMeshProUGUI letterDialogueText;

    [Header("Visual Effects & Objects")]
    [SerializeField] private GameObject vineOverlay;

    private bool isPlayerInRange = false;
    private bool vinesCut = false;
    private bool letterObtained = false;

    private void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (letterPanel != null) letterPanel.SetActive(false);
    }

    private void Update()
    {
        // Disable interaction completely if letter is already obtained
        if (letterObtained) return;

        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void Interact()
    {
        // Case 1: Vines are cut -> Open LetterPanel and pick up letter
        if (vinesCut)
        {
            letterObtained = true;

            if (PlayerInventory.Instance != null)
                PlayerInventory.Instance.hasCelesteLetter = true;

            if (dialoguePanel != null) dialoguePanel.SetActive(false);

            if (letterPanel != null)
            {
                letterPanel.SetActive(true);
                letterPanel.transform.SetAsLastSibling(); // Brings panel to the top render layer
            }

            if (letterDialogueText != null)
                letterDialogueText.text = "You found Celeste's Letter and August's Old Postcard tucked beneath the bench!";
            return;
        }

        // Standard Dialogue Panel handling for un-cut state
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        // Case 2: Player has cutter -> Cut vines
        if (PlayerInventory.Instance != null && PlayerInventory.Instance.hasPruningShears)
        {
            vinesCut = true;
            if (vineOverlay != null) vineOverlay.SetActive(false);

            if (dialogueText != null)
                dialogueText.text = "You used the cutter to remove the vines! Press E again to inspect the letter.";
        }
        // Case 3: No cutter yet
        else
        {
            if (dialogueText != null)
                dialogueText.text = "There's vines covering the letter, better find something to remove them with.";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Block interaction trigger if letter has already been collected
        if (letterObtained) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (letterPanel != null) letterPanel.SetActive(false);
        }
    }
}