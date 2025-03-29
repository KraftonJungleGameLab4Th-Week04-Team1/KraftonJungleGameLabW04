using UnityEngine;

public class UIManager
{
    private BasicUI _basicUI;
    private TitleUI _titleUI;
    private GameOverUI _gameOverUI;
    private CalculateUI _calculateUI;
    private Canvas _titleUICanvas;
    private Canvas _basicUICanvas;
    private Canvas _gameOverUICanvas;
    private Canvas _calculateUICanvas;

    public void Init()
    {
        _basicUI = GameObject.FindFirstObjectByType<BasicUI>();
        _basicUICanvas = _basicUI.GetComponent<Canvas>();
        _basicUI.Init();
        
        _titleUI = GameObject.FindFirstObjectByType<TitleUI>();
        _titleUICanvas = _titleUI.GetComponent<Canvas>();
        _titleUI.Init();

        _gameOverUI = GameObject.FindFirstObjectByType<GameOverUI>();
        _gameOverUICanvas = _gameOverUI.GetComponent<Canvas>();
        //_gameOverUI.Init();

        _calculateUI = GameObject.FindFirstObjectByType<CalculateUI>();
        _calculateUICanvas = _calculateUI.GetComponent<Canvas>();
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
                _calculateUICanvas.enabled = true;
                break;
            case GameState.GameOver:
                _gameOverUICanvas.enabled = true;
                _gameOverUI.Show();
                break;
        }
    }

    public void UpdateGameTimeUI(float time)
    {
        _basicUI.UpdateGameTime(time);
    }

    private void DeactivateUI()
    {
        _titleUICanvas.enabled = false;
        _basicUICanvas.enabled = false;
        _calculateUICanvas.enabled = false;
        _gameOverUICanvas.enabled = false;
    }
}
