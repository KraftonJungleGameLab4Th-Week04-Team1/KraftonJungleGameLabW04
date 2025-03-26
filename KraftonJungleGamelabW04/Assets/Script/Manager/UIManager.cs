using UnityEngine;

public class UIManager
{
    private BasicUI _basicUI;
    private TitleUI _titleUI;
    private Canvas _titleUICanvas;
    private Canvas _basicUICanvas;

    public void Init()
    {
        ///////씬 세팅하고 주석 풀어주세요. ui는 테스트 진행중입니다
        //_basicUI = GameObject.FindFirstObjectByType<BasicUI>();
        //_basicUICanvas = _basicUI.GetComponent<Canvas>();
        //_basicUI.Init();

        //_titleUI = GameObject.FindFirstObjectByType<TitleUI>();
        //_titleUICanvas = _titleUI.GetComponent<Canvas>();
        //_titleUI.Init();
    }

    public void ChangeUI(GameState state)
    {
        DeactivateUI();

        switch (state)
        {
            case GameState.Title:
                _titleUICanvas.enabled = true;
                break;
            case GameState.MainPlay:
                _basicUICanvas.enabled = true;
                break;
        }
    }

    private void DeactivateUI()
    {
        _titleUICanvas.enabled = false;
        _basicUICanvas.enabled = false;
    }
}
