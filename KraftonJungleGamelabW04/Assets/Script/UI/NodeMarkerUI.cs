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

    private void Start()
    {
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
        _typeText.text = $"{node.NodeType.ToString()}";
    }

    // Move btn click action
    private void OnClickMoveBtn(int index)
    {
        if (Node.NodeIdx == index)
        {
            GameManager.Instance.OnMoveNodeAction?.Invoke(index);
        }
    }
}
