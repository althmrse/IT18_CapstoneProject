using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject confirmQuitPanel;

    // --- START BUTTON ---
    public void LoadSaveSlots()
    {
        // Loads the scene named "SaveSlots"
        SceneManager.LoadScene("SaveSlots");
    }

    // --- SETTINGS BUTTON ---
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // --- CREDITS BUTTON ---
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // --- QUIT BUTTON ---
    public void OpenQuitConfirmation()
    {
        confirmQuitPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        confirmQuitPanel.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Debug.Log("Quit Game triggered!");

        // Quits the application when built
        Application.Quit();

        // Optional: Stops play mode inside the Unity Editor for testing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}