using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("노드 명")]
    [SerializeField] private string _NodeName;
    [SerializeField] private int _nodeIdx;

    [Header("노드 자원")]
    [SerializeField] private int _food;
    [SerializeField] private int _bolt;
    [SerializeField] private int _nut;
    [SerializeField] private int _fuel;

    [Header("특수 노드 여부")]
    public NodeType _nodeType;

    public int NodeIdx => _nodeIdx;
    public int Food { get => _food; set => _food = value; }
    public int Bolt { get => _bolt; set => _bolt = value; }
    public int Not { get => _nut; set => _nut = value; }
    public int Fuel { get => _fuel; set => _fuel = value; }
}
