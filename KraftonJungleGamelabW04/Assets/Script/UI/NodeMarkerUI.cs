using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeMarkerUI : MonoBehaviour
{
    public Node Node;
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private TMP_Text _boltText;
    [SerializeField] private TMP_Text _nutText;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _riskText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private Button _moveBtn;

    private RectTransform _canvasRect;

    private void Start()
    {
        _canvasRect = _canvas.GetComponent<RectTransform>();
        
        _moveBtn.onClick.AddListener(() => OnClickMoveBtn(Node.NodeIdx));
        GameManager.Instance.OnSelectNodeAction += ActivateNodeMarkerUI;

        ActivateNodeMarkerCanvas(false);
    }
    
    // Activate node marker UI
    private void ActivateNodeMarkerUI(int index)
    {
        if (Node.NodeIdx != index)
        {
            ActivateNodeMarkerCanvas(false);
        }
        else
        {
            ChangeNodeMarkerUI(Node);
            ActivateNodeMarkerCanvas(true);
        }
    }

    // Activate node marker UI canvas
    private void ActivateNodeMarkerCanvas(bool isActive)
    {
        if (!IsCanvasVisible())
        {
            var newPos = new Vector3(_canvas.transform.localPosition.x, -4.5f, _canvas.transform.localPosition.z);
            _canvas.transform.localPosition = newPos;
        }
        _canvas.enabled = isActive;
        
    }

    // Change node UI data
    private void ChangeNodeMarkerUI(Node node)
    {
        _nameText.text = $"{node.name}";
        _foodText.text = $"{node.Food}";
        _boltText.text = $"{node.Bolt}";
        _nutText.text = $"{node.Nut}";
        _fuelText.text = $"{node.Fuel}";
        _riskText.text = $"{node.Risk}";
        _typeText.text = node.NodeType == NodeType.RepairNode ? "ABLE TO REPAIR" : "";
    }

    // Move btn click action
    private void OnClickMoveBtn(int index)
    {
        if (Node.NodeIdx == index)
        {
            GameManager.Instance.OnMoveNodeAction?.Invoke(index);
        }
    }
    
    // Check canvas in camera view
    private bool IsCanvasVisible()
    {
        Vector3[] worldCorners = new Vector3[4];
        _canvasRect.GetWorldCorners(worldCorners);
        
        bool isVisible = true;

        for (int index = 0; index < worldCorners.Length; index++)
        {
            Vector3 corner = worldCorners[index];
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(corner);

            if (viewportPos.z < 0 || viewportPos.y < 0 || viewportPos.y > 1)
            {
                isVisible = false;
                break;
            }
        }

        return isVisible;
    }
}
