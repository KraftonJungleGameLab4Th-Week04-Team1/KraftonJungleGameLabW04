using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeMarkerUI : MonoBehaviour
{
    private Node _node;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private TMP_Text _boltText;
    [SerializeField] private TMP_Text _nutText;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _riskText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private Button _moveBtn;

    private void Start()
    {
        GameManager.Instance.OnSelectNodeAction += ActivateNodeMarkerUI;
    }

    public void Init(int index)
    {
        _node = GameManager.NodeManager.NodeDic[index];
        _moveBtn.onClick.AddListener(() => OnClickMoveBtn(_node.NodeIdx));
    }

    // Activate or deactivate node ui
    private void ActivateNodeMarkerUI(int index)
    {
        _canvas.enabled = true;
        ChangeNodeMarkerUI(_node);
    }

    // Change node ui data
    private void ChangeNodeMarkerUI(Node node)
    {
        _nameText.text = $"{node.name}";
        _foodText.text = $"{node.Food}";
        _boltText.text = $"{node.Bolt}";
        _nutText.text = $"{node.Nut}";
        _fuelText.text = $"{node.Fuel}";
        _riskText.text = $"{node.Risk}";
        _typeText.text = $"{node.NodeType.ToString()}";
    }

    private void OnClickMoveBtn(int index)
    {
        GameManager.Instance.OnMoveNodeAction?.Invoke(index);
    }
}
