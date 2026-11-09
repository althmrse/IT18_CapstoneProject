using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MovementTutorialNewInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private InputActionProperty moveAction; // Bind to Player/Move

    private bool movedUp = false;
    private bool movedDown = false;
    private bool movedLeft = false;
    private bool movedRight = false;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input.y > 0.5f) movedUp = true;
        if (input.y < -0.5f) movedDown = true;
        if (input.x < -0.5f) movedLeft = true;
        if (input.x > 0.5f) movedRight = true;

        if (movedUp && movedDown && movedLeft && movedRight)
        {
            if (tutorialText != null) tutorialText.gameObject.SetActive(false);
            enabled = false; // Stop checking updates
        }
    }
}