using UnityEngine;

public class UIManager
{
    private TitleUI _titleUI;
    private Canvas _basicUI;

    public void Init()
    {
        _titleUI = FindAnyObjectByType<TitleUI>();
        _titleUI.Init();
        _basicUI = GameObject.FindAnyObjectByType<BasicUI>().GetComponent<Canvas>();
    }
}
