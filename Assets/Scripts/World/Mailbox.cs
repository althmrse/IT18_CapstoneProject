using UnityEngine;

public class Mailbox : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mailboxPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject interactPrompt;

    private bool isPanelOpen = false;
    private bool isUpdatingState = false; // Prevents recursive call loops

    public void ShowPrompt(bool show)
    {
        if (interactPrompt != null && !isPanelOpen)
        {
            interactPrompt.SetActive(show);
        }
    }

    public void Interact()
    {
        if (isUpdatingState) return;

        isPanelOpen = !isPanelOpen;
        SetPanelStates(isPanelOpen);
    }

    // Must be 'public' so Unity's UI Inspector can see it
    public void CloseMailbox()
    {
        if (!isPanelOpen || isUpdatingState) return;

        isPanelOpen = false;
        SetPanelStates(false);
    }

    private void SetPanelStates(bool openMailbox)
    {
        isUpdatingState = true;

        // Toggle Mailbox Panel
        if (mailboxPanel != null && mailboxPanel.activeSelf != openMailbox)
        {
            mailboxPanel.SetActive(openMailbox);
        }

        // Toggle Main Panel
        if (mainPanel != null && mainPanel.activeSelf == openMailbox)
        {
            mainPanel.SetActive(!openMailbox);
        }

        // Toggle Interact Prompt
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(!openMailbox);
        }

        isUpdatingState = false;
    }

}