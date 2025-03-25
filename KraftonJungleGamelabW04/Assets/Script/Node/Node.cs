using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("노드 명")]
    [SerializeField] private string _NodeName;

    private int _nodeIdx;

    [Header("노드 자원")]
    [SerializeField] private int _food;
    [SerializeField] private int _bolt;
    [SerializeField] private int _not;
    [SerializeField] private int _fuel;

    [Header("특수 노드 여부")]
    public NodeType _nodeType;

    public int Food => _food;
    public int Bolt => _bolt;
    public int Not => _not;
    public int Fuel => _fuel;
}
