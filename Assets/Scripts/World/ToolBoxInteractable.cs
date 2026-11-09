using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ToolBoxInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private GameObject dialoguePanel; // Reference to Canvas > MainPanel > DialoguePanel
    [SerializeField] private TextMeshProUGUI dialogueText;  // Reference to Canvas > MainPanel > DialoguePanel > Speech

    private bool isPlayerInRange = false;
    private bool cutterObtained = false;

    private void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        if (cutterObtained)
        {
            if (dialogueText != null)
                dialogueText.text = "The toolbox is empty.";
            return;
        }

        cutterObtained = true;

        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.hasPruningShears = true; // Sets cutter item to true in inventory
        }

        if (dialogueText != null)
            dialogueText.text = "You obtained the cutter!";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        }
    }
}