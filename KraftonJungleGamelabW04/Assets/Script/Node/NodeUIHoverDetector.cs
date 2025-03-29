using UnityEngine;

public class NodeUIHoverDetector : MonoBehaviour
{
    private NodeMarkerUI _nodeMarkerUI;

    private void Start()
    {
        _nodeMarkerUI = GetComponentInParent<NodeMarkerUI>();
    }

    private void OnMouseExit()
    {
        _nodeMarkerUI.DeactivateNodeUI(0);
    }
}
