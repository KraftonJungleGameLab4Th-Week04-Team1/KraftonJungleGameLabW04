using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    public void Init()
    {
        _canvas = GetComponent<Canvas>();
        
        _canvas.enabled = true;
        startButton.onClick.AddListener(OnClickStartBtn);
        quitButton.onClick.AddListener(OnClickQuitBtn);
    }

    private void OnClickStartBtn()
    {
        _canvas.enabled = false;
        GameManager.Instance.StartGameTimer(true);
    }
    
    private void OnClickQuitBtn()
    {
        Application.Quit();
    }
}
