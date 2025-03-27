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

    [SerializeField] private TMP_Text _foodToGoText;
    [SerializeField] private TMP_Text _fuelToGoText;
    [SerializeField] private TMP_Text _etaText;

    private RectTransform _canvasRect;
    private bool _isMoving; //비행기 움직이면 Move 입력을 막는 변수.

    private void Start()
    {
        _canvasRect = _canvas.GetComponent<RectTransform>();
        
        _moveBtn.onClick.AddListener(() => OnClickMoveBtn(Node.NodeNum));
        GameManager.Instance.OnSelectNodeAction += ActivateNodeMarkerUI;
        GameManager.Instance.OnArriveAction += OnNotMove;

        ActivateNodeMarkerCanvas(false);
    }
    
    // Activate node marker UI
    private void ActivateNodeMarkerUI(int index)
    {
        if (Node.NodeNum != index)
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
        _nameText.text = $"{node.NodeName}";
        _foodText.text = node.IsVisited ? $"{node.Food}" : "??";
        _boltText.text = node.IsVisited ? $"{node.Bolt}" : "??";
        _nutText.text = node.IsVisited ? $"{node.Nut}" : "??";
        _fuelText.text = $"{node.Fuel}";
        _riskText.text = $"{node.Risk}";

        if(node.NodeType == NodeType.RepairNode)
        {
            _typeText.text = $"ABLE TO REPAIR";
        }
        else if(node.NodeType == NodeType.SpaceNode)
        {
            _typeText.text = $"Escape Point";
        }
        else
        {
            _typeText.text = "";
        }

        int xDistance = Mathf.Abs(node.NodeIdx - NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex].NodeIdx);
        int foodToUse = GameManager.Info.GetFoodRequiredBetweenNodes(xDistance);
        int fuelToUse = GameManager.Info.GetFuelRequiredBetweenNodes(xDistance);
        _foodToGoText.text = foodToUse.ToString();
        _fuelToGoText.text = fuelToUse.ToString();
        _etaText.text = "ETA : " + GameManager.Info.GetTimeRequiredBetweenNodes(xDistance);

    }

    // Move btn click action
    private void OnClickMoveBtn(int index)
    {
        if (GameManager.Instance.IsMoving) return;

        if (Node.NodeNum == index)
        {
            GameManager.Aircraft.UseResourceForFly(index);
            GameManager.Instance.OnMoveNodeAction?.Invoke(index);
            GameManager.Instance.IsMoving = true;
        }
    }

    private void OnNotMove(int newIndex)
    {
        GameManager.Instance.IsMoving = false;
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
