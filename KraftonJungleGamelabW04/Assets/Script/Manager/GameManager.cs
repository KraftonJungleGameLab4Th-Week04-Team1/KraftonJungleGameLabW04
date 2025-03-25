using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    #region
    public static UIManager UI { get { return Instance._uiManager; } }
    public static AircraftManager Aircraft { get { return Instance._aircraftManager; } }
    public static SolarController Solor { get { return Instance._solarController; } }
    public static NodeManager NodeManager { get { return Instance._nodeManager; } }

    private UIManager _uiManager = new UIManager();
    private AircraftManager _aircraftManager = new AircraftManager();
    private SolarController _solarController = new SolarController();
    private NodeManager _nodeManager = new NodeManager();
    #endregion

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
    }

    // 사실 매니저 빼서 GameManager를 따로 관리해야 했으나, 이번 프로젝트에선 GameManager가 모든 매니저 통하기 때문에 Manager역할을 합니다.
    private void Init()
    {
        GameStart();

        // 객체별 초기화 순서를 정하기 위한 구조 = 각 매니저별로 Awake()를 호출하지 않아도 됩니다.
        UI.Init();
        NodeManager.Init();
        Aircraft.Init();
        Solor.Init();
    }

    private void GameStart()
    {
        //TODO
        /*
        게임 시작시 필요한 처리
         */
    }
}
