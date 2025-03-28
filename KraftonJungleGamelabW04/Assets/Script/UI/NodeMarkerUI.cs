using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeMarkerUI : MonoBehaviour
{
    // 드래그 앤 드롭으로 정보 가져오고 있음.
    public Node Node;
    // 결국 select할때만 필요. 따라서 코드에는 사용하지 않게. 그래야 액션을 글로벌하게 활용할 수 있음.
    private int _thisNodeNum;

    #region
    //주 패널.
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private TMP_Text _boltText;
    [SerializeField] private TMP_Text _nutText;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _riskText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private Button _moveBtn;

    //서브패널.
    [SerializeField] private TMP_Text _foodToGoText;
    [SerializeField] private TMP_Text _fuelToGoText;
    [SerializeField] private TMP_Text _etaText;
    #endregion

    private RectTransform _canvasRect;

    private void Start()
    {
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _thisNodeNum = Node.NodeNum;

        _moveBtn.onClick.AddListener(() => OnClickMoveBtn(GameManager.Instance.CurrentNodeIndex,_thisNodeNum));
        GameManager.Instance.OnSelectNodeAction += ActivateNodeMarkerUI;
        //GameManager.Instance.OnMoveNodeAction += ChangeNodeMarkerUI;
        GameManager.Instance.OnArriveAction += OnNotMove;

        ActivateNodeMarkerCanvas(false);
    }
    
    // Activate node marker UI
    private void ActivateNodeMarkerUI(int currentIdx, int selectedIdx)
    {
        // 고른 노드가 자기 자신이라면
        if (_thisNodeNum != selectedIdx)
        {
            ActivateNodeMarkerCanvas(false);
        }
        else
        {
            Debug.Log("셀렉액션");
            ChangeNodeMarkerUI(currentIdx, selectedIdx);
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
    private void ChangeNodeMarkerUI(int currentIdx, int selectedIdx)
    {
        var thisNode = NodeManager.NodeDic[selectedIdx]; 
        _nameText.text = $"{thisNode.NodeName}";
        _foodText.text = thisNode.IsVisited ? $"{thisNode.Food}" : "??";
        _boltText.text = thisNode.IsVisited ? $"{thisNode.Bolt}" : "??";
        _nutText.text = thisNode.IsVisited ? $"{thisNode.Nut}" : "??";
        _fuelText.text = thisNode.IsVisited ? $"{thisNode.Fuel}" : "??";
        _riskText.text = $"{thisNode.Risk}";

        if(thisNode.NodeType == NodeType.RepairNode)
        {
            _typeText.text = $"수리 가능 지역";
        }
        else if(thisNode.NodeType == NodeType.SpaceNode)
        {
            _typeText.text = $"탈출 가능 지역";
        }
        else
        {
            _typeText.text = "";
        }

        Tuple<int, int, int> distanceResource = CalculateResource(currentIdx, selectedIdx);
        _foodToGoText.text = distanceResource.Item1.ToString();
        _fuelToGoText.text = distanceResource.Item2.ToString();
        _etaText.text = "ETA : " + distanceResource.Item3;
    }

    private Tuple<int,int,int> CalculateResource(int currentIdx, int selectedIdx)
    {
        int xDistance = (selectedIdx - currentIdx);

        if (xDistance < 0)
        {
            int distance1 = 32 + xDistance;
            int distance2 = -xDistance;
            xDistance = Mathf.Min(distance1, distance2);
        }

        int foodToUse = GameManager.Info.GetFoodRequiredBetweenNodes(xDistance);
        int fuelToUse = GameManager.Info.GetFuelRequiredBetweenNodes(xDistance);
        int eta = GameManager.Info.GetTimeRequiredBetweenNodes(xDistance);

        return new Tuple<int, int, int>(foodToUse, fuelToUse, eta);
    }

    // Move btn click action
    private void OnClickMoveBtn(int currentIdx, int selectedIdx)
    {
        if (GameManager.Instance.IsMoving) return;
        // if(_thisNodeNum != index) return;

        Tuple<int, int, int> distanceResource = CalculateResource(currentIdx, selectedIdx);
        int foodToUse = distanceResource.Item1;
        int fuelToUse = distanceResource.Item2;
        if (GameManager.Aircraft.Fuel >= fuelToUse && GameManager.Aircraft.Food >= foodToUse)
        {
            // UseResourceForFly 액션 순서 파악하고 넣는거로 리팩토링 예정
            GameManager.Aircraft.UseResourceForFly(selectedIdx);
            GameManager.Instance.OnMoveNodeAction?.Invoke(selectedIdx);
            GameManager.Instance.IsMoving = true;
        }

        return;
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

    public void OnButtonExit()
    {
        _canvas.enabled = false;
    }
}
