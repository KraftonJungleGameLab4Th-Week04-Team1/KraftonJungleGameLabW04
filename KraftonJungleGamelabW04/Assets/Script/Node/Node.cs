using UnityEngine;

[CreateAssetMenu(menuName = "Node Data")]
public class Node : ScriptableObject
{
    [Header("노드 명")]
    [SerializeField] private string _NodeName;
    [SerializeField] private int _nodeIdx;

    [Header("노드 자원")]
    [SerializeField] private int _food;
    [SerializeField] private int _bolt;
    [SerializeField] private int _nut;
    [SerializeField] private int _fuel;

    [Header("가변 변수")]
    [SerializeField] private int _risk;

    [Header("특수 노드 여부")]
    private NodeType _nodeType;
    private int _spaceStationLevel;

    public int NodeIdx => _nodeIdx;
    public string NodeName => _NodeName;
    public NodeType NodeType => _nodeType;
    public int Food { get => _food; set => _food = value; }
    public int Bolt { get => _bolt; set => _bolt = value; }
    public int Nut { get => _nut; set => _nut = value; }
    public int Fuel { get => _fuel; set => _fuel = value; }
    public int Risk { get => _risk; set => _risk = value; }
    public int SpaceStationLevel { get => _spaceStationLevel; set => _spaceStationLevel = value; }
}
