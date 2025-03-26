using UnityEngine;

public class UIManager
{
    private Canvas _basicUI;

    public void Init()
    {
        _basicUI = GameObject.FindAnyObjectByType<BasicUI>().GetComponent<Canvas>();
        
    }
}
