using UnityEngine;

[CreateAssetMenu(fileName = "CraftData", menuName = "Scriptable Objects/CraftData")]
public class CraftData : ScriptableObject
{
    public string partName;       // e.g., "PartA", "PartB", etc.
    public int requiredBolts;     // Number of bolts required
    public int requiredNuts;      // Number of nuts required
    public int partIndex;         // Index in spaceStationParts array (0-5)
}
