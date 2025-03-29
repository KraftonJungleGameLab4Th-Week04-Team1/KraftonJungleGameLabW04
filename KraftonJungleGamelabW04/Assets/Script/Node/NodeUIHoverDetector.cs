using UnityEngine;

public class NodeUIHoverDetector : MonoBehaviour
{
    private NodeMarkerUI _nodeMarkerUI;

    private void Start()
    {
        _nodeMarkerUI = GetComponentInParent<NodeMarkerUI>();
    }

    private void OnMouseEnter()
    {
        Debug.Log($"{_nodeMarkerUI}");
        _nodeMarkerUI?.HandleMouseEnter();
    }

    private void OnMouseExit()
    {
        Debug.Log($"{_nodeMarkerUI}");
        _nodeMarkerUI?.HandleMouseExit();
    }
}
