using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject interactPrompt;

    [Header("Dialogue Content")]
    [TextArea(3, 5)]
    [SerializeField]
    private string[] dialogueLines = new string[]
    {
        "Hello traveler!",
        "Welcome to our small village.",
        "Watch out for the dark gray mailbox over there!"
    };

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    public void ShowPrompt(bool show)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(show);
    }

    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            DisplayNextLine();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        currentLineIndex = 0;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
    }

    private void DisplayNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}