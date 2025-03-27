using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image _fadeInImage;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private float _fadeDuration = 2f;

    [SerializeField] private float _timer = 0f;
    [SerializeField] private float _startTextTiming = 5f;
    [SerializeField] private float _endTiming = 10f;

    private bool _isFading = false;

    public void Init()
    {
        // 자식오브젝트의 이름으로 가져옵니다. 그 이름을 가진 오브젝트는 하나입니다.
        //_fadeInImage = transform.Find("GameOverPanel").GetComponent<Image>();
        //_gameOverText = transform.Find("GameOverText").GetComponent<TMP_Text>();
        Color c = _fadeInImage.color;
        Color c2 = _gameOverText.color;
        c.a = 0f;
        c2.a = 0f;
        _fadeInImage.color = c;
        _gameOverText.color = c2;
        _isFading = false;
        // 진엔딩이랑 같은 연출
    }

    public void Show()
    {
        _timer = 0f;
        _isFading = true;
    }

    private void Update()
    {
        if(_isFading)
        {
            _timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(_timer / _fadeDuration);
            Color c = _fadeInImage.color;
            c.a = alpha;
            _fadeInImage.color = c;

            if (_timer > _startTextTiming)
            {
                float alpha2 = Mathf.Clamp01((_timer - _startTextTiming) / _fadeDuration);
                Color c2 = _gameOverText.color;
                c2.a = alpha2;
                _gameOverText.color = c2;
            }

            if (_timer > _endTiming)
            {
                if (Input.anyKeyDown)
                {
                    //재로드시 솔라시스템 초기화되는지 몰라서 일단 끕니다.
                    //SceneManager.LoadScene("MainScene");
                    Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    }
}
