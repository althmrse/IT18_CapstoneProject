using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;

    [Header("Login Inputs")]
    [SerializeField] private TMP_InputField loginUsernameInput;
    [SerializeField] private TMP_InputField loginPasswordInput;
    [SerializeField] private TMP_Text loginMessageText;

    [Header("Register Inputs")]
    [SerializeField] private TMP_InputField registerUsernameInput;
    [SerializeField] private TMP_InputField registerPasswordInput;
    [SerializeField] private TMP_InputField registerConfirmPasswordInput;
    [SerializeField] private TMP_Text registerMessageText;

    private FirebaseAuth auth;
    private FirebaseFirestore db;

    private void Start()
    {
        // Initialize Firebase instances
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        // Default view: Show Login panel, hide Register panel
        ShowLoginPanel();
    }

    // --- PANEL NAVIGATION ---
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        ClearFields();
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        ClearFields();
    }

    // --- REGISTER ACCOUNT ---
    public void RegisterAccount()
    {
        string username = registerUsernameInput.text.Trim();
        string password = registerPasswordInput.text;
        string confirmPassword = registerConfirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            registerMessageText.text = "Fields cannot be empty!";
            return;
        }

        if (password != confirmPassword)
        {
            registerMessageText.text = "Passwords do not match!";
            return;
        }

        registerMessageText.text = "Creating account...";

        // Map username to internal Firebase email format
        string internalEmail = $"{username.ToLower()}@yourgame.internal";

        auth.CreateUserWithEmailAndPasswordAsync(internalEmail, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                registerMessageText.text = "Registration failed. Username may already exist!";
                return;
            }

            AuthResult result = task.Result;
            string userId = result.User.UserId;

            // Create initial user document in Cloud Firestore
            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "username", username },
                { "createdAt", FieldValue.ServerTimestamp }
            };

            db.Collection("users").Document(userId).SetAsync(userData).ContinueWithOnMainThread(dbTask =>
            {
                if (dbTask.IsFaulted)
                {
                    registerMessageText.text = "Account created, but database setup failed.";
                    return;
                }

                registerMessageText.text = "Registration successful!";

                // Switch back to Login panel after successful registration
                Invoke(nameof(ShowLoginPanel), 1.5f);
            });
        });
    }

    // --- LOGIN ACCOUNT ---
    public void LoginAccount()
    {
        string username = loginUsernameInput.text.Trim();
        string password = loginPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            loginMessageText.text = "Please enter both credentials.";
            return;
        }

        loginMessageText.text = "Logging in...";

        // Map username to internal Firebase email format
        string internalEmail = $"{username.ToLower()}@yourgame.internal";

        auth.SignInWithEmailAndPasswordAsync(internalEmail, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                loginMessageText.text = "Incorrect username or password!";
                return;
            }

            loginMessageText.text = "Login Successful! Loading...";

            // Proceed to MainMenu scene
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void ClearFields()
    {
        if (loginUsernameInput) loginUsernameInput.text = "";
        if (loginPasswordInput) loginPasswordInput.text = "";
        if (loginMessageText) loginMessageText.text = "";

        if (registerUsernameInput) registerUsernameInput.text = "";
        if (registerPasswordInput) registerPasswordInput.text = "";
        if (registerConfirmPasswordInput) registerConfirmPasswordInput.text = "";
        if (registerMessageText) registerMessageText.text = "";
    }
}