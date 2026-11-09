using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Track active interactables
    private NPC currentNPC;
    private Mailbox currentMailbox;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 1. Movement Input
        float moveX = 0f;
        float moveY = 0f;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveY += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveY -= 1f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX += 1f;

        moveInput = new Vector2(moveX, moveY).normalized;

        // 2. Interaction Input
        if (keyboard.eKey.wasPressedThisFrame)
        {
            if (currentNPC != null)
            {
                currentNPC.Interact();
            }
            else if (currentMailbox != null)
            {
                currentMailbox.Interact();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<NPC>(out var npc))
        {
            currentNPC = npc;
            npc.ShowPrompt(true);
        }
        else if (other.TryGetComponent<Mailbox>(out var mailbox))
        {
            currentMailbox = mailbox;
            mailbox.ShowPrompt(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<NPC>(out var npc) && currentNPC == npc)
        {
            npc.ShowPrompt(false);
            npc.CloseDialogue();
            currentNPC = null;
        }
        else if (other.TryGetComponent<Mailbox>(out var mailbox) && currentMailbox == mailbox)
        {
            mailbox.ShowPrompt(false);
            mailbox.CloseMailbox();
            currentMailbox = null;
        }
    }
}