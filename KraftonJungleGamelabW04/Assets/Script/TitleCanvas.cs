using UnityEngine;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Init()
    {
        _canvas = GetComponent<Canvas>();
        
        _canvas.enabled = true;
        startButton.onClick.AddListener(OnClickStartBtn);
        quitButton.onClick.AddListener(OnClickQuitBtn);
    }

    private void OnClickStartBtn()
    {
        _canvas.enabled = false;
    }
    
    private void OnClickQuitBtn()
    {
        Application.Quit();
    }
}
