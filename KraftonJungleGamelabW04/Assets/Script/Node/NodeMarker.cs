using UnityEngine;

public class NodeMarker : MonoBehaviour
{
    private LayerMask _layerMask;
    private NodeMarkerUI _nodeMarkerUI;
    [SerializeField] private int _nodeIndex;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("NodeMarker");
        
        _nodeMarkerUI = GetComponent<NodeMarkerUI>();
        _nodeMarkerUI.Init(_nodeIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                GameManager.Instance.OnSelectNodeAction?.Invoke(_nodeIndex);
            }
        }
    }
}
