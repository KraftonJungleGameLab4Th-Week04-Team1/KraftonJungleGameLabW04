using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private Button startButton;
    [SerializeField] private Button howToButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Canvas howToCanvas;

    public void Init()
    {
        _canvas = GetComponent<Canvas>();
        
        _canvas.enabled = true;

        startButton.onClick.AddListener(OnClickStartBtn);
        howToButton.onClick.AddListener(OnClickHowToBtn);
        quitButton.onClick.AddListener(OnClickQuitBtn);
        backButton.onClick.AddListener(OnClickBackBtn);

        howToCanvas.enabled = false;
    }

    private void OnClickStartBtn()
    {
        _canvas.enabled = false;
        GameManager.Instance.GameState = GameState.MainPlay;
        GameManager.Instance.StartGameTimer(true);
    }

    private void OnClickHowToBtn()
    {
        howToCanvas.enabled = true;
    }

    private void OnClickBackBtn()
    {
        howToCanvas.enabled = false;
    }


    private void OnClickQuitBtn()
    {
        Application.Quit();
    }
}
