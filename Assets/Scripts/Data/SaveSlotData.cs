using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class SaveSlotData
{
    [FirestoreProperty] public SlotMetaData slotMetaData { get; set; } = new SlotMetaData();
    [FirestoreProperty] public PlayerPosition playerTransform { get; set; } = new PlayerPosition();
    [FirestoreProperty] public Dictionary<string, LetterStatus> letters { get; set; } = new Dictionary<string, LetterStatus>();
    [FirestoreProperty] public Dictionary<string, string> choicesHistory { get; set; } = new Dictionary<string, string>();
    [FirestoreProperty] public WorldState worldState { get; set; } = new WorldState();
}

[FirestoreData]
public class SlotMetaData
{
    [FirestoreProperty] public int playtimeSeconds { get; set; } = 0;
    [FirestoreProperty] public string lastSaved { get; set; } = "";
    [FirestoreProperty] public bool isSlotEmpty { get; set; } = true;
}

[FirestoreData]
public class PlayerPosition
{
    [FirestoreProperty] public string currentZoneId { get; set; } = "village_start";
    [FirestoreProperty] public float x { get; set; } = 0f;
    [FirestoreProperty] public float y { get; set; } = 0f;
    [FirestoreProperty] public int sortingLayerOrder { get; set; } = 0;
}

[FirestoreData]
public class LetterStatus
{
    [FirestoreProperty] public bool isCollected { get; set; } = false;
    [FirestoreProperty] public bool isRead { get; set; } = false;
    [FirestoreProperty] public bool isDelivered { get; set; } = false;
    [FirestoreProperty] public string collectedAt { get; set; } = "";
}

[FirestoreData]
public class WorldState
{
    [FirestoreProperty] public int natureGrowthStage { get; set; } = 1;
    [FirestoreProperty] public List<string> destroyedObstacles { get; set; } = new List<string>();
    [FirestoreProperty] public List<string> unlockedShortcuts { get; set; } = new List<string>();
    [FirestoreProperty] public List<string> interactedObjects { get; set; } = new List<string>();
}