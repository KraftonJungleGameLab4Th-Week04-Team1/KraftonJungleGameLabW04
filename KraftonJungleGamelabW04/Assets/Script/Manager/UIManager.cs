using UnityEngine;

public class UIManager : MonoBehaviour
{
    private TitleUI _titleUI;
    public NodeMarkerUI _nodeMarkerUI = new NodeMarkerUI();
    public BasicUI _basicUI = new BasicUI();

    public void Init()
    {
        _titleUI = FindAnyObjectByType<TitleUI>();
        _titleUI.Init();
        
        _basicUI.Init();
    }
}
