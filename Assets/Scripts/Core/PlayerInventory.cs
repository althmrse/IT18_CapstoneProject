using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [Header("Inventory Items")]
    public bool hasPruningShears = false;
    public bool hasCelesteLetter = false;

    private void Awake()
    {
        // Enforce Singleton Pattern so Instance is always accessible
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}