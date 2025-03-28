using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    #region Managers
    public static UIManager UI { get { return Instance._uiManager; } }
    public static AircraftManager Aircraft { get { return Instance._aircraftManager; } }
    //public static SolarController Solor { get { return Instance._solarController; } }
    public static NodeManager NodeManager { get { return Instance._nodeManager; } }
    public static InfoManager Info { get { return Instance._infoManager; } }

    private UIManager _uiManager = new UIManager();
    private AircraftManager _aircraftManager = new AircraftManager();
    //private SolarController _solarController = new SolarController();
    private NodeManager _nodeManager = new NodeManager();
    private InfoManager _infoManager = new InfoManager();
    #endregion
    
    #region Actions

    public Action<float> OnChangedGameTimeAction;
    
    public Action<int> OnSelectNodeAction;
    public Action<int> OnMoveNodeAction;
    public Action<int> OnArriveAction; //선택 노드 도착시 시행할 액션.
    //aircraftFood, aircraftBolt, aircraftNut, aircraftFuel, repairValue, nodeFood, nodeBolt, nodeNut, nodeFuel
    public Action<int, int, int, int, int, int, int, int, int> OnConfirmAction;
    #endregion

    #region Properties

    public int CurrentNodeIndex { get;  set; } = 1;
    private LayerMask _layerMask;
    
    public int CurrentTurn { get; private set; } = 0;
    [SerializeField] public float GameTime { get; private set; }
    private readonly float _timeInterval = 0.5f; //2초당 인게임 시간 1분 증가.
    private float _time;
    
    private bool _isGameStarted = false;
    public bool IsEscapable { get; set; } = false;
    public bool IsMoving;
    public float moveDuration = 5f;

    private GameState _gameState;
    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            _uiManager.ChangeUI(value);
        }
    }

    #endregion

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
            Init();
        }
    }
    
    // 사실 매니저 빼서 GameManager를 따로 관리해야 했으나, 이번 프로젝트에선 GameManager가 모든 매니저 통하기 때문에 Manager역할을 합니다.
    private void Init()
    {
        // 객체별 초기화 순서를 정하기 위한 구조 = 각 매니저별로 Awake()를 호출하지 않아도 됩니다.
        NodeManager.Init();
        Aircraft.Init();
        //Solor.Init();
        Info.Init();
        //반드시 UI가 가장 마지막에 초기화되어야 합니다.
        UI.Init();
        
        GameStart();
    }

    private void OnDestroy()
    {
        //액션 해제
        OnChangedGameTimeAction = null;
        OnSelectNodeAction = null;
        OnMoveNodeAction = null;
        OnConfirmAction = null;
    }

    private void GameStart()
    {
        _layerMask = LayerMask.GetMask("NodeMarker");
        GameState = GameState.Title;
        CurrentNodeIndex = 1;

        //무빙 확인시 현재 노드 인덱스 변경.

        OnArriveAction += ChangeCurrentNodeIndex;
    }
    
    public void StartGameTimer(bool isStart)
    {
        _isGameStarted = isStart;
    }

    public void ChangeGameTime(float time)
    {
        GameTime += time;
        OnChangedGameTimeAction?.Invoke(GameTime);
    }

    private void Update()
    {
        if (!_isGameStarted)
        {
            return;
        }

        // Get game time
        _time += Time.deltaTime;
        if (_time >= _timeInterval)
        {
            _time -= _timeInterval;
            GameTime++;
            OnChangedGameTimeAction?.Invoke(GameTime);
        }
        
        // Get selected node
        if (Input.GetMouseButtonDown(0))
        {
            // Check if UI is activated
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {

                _nodeManager.SelectedNode = hit.collider.GetComponent<NodeMarkerUI>().Node;
                
                if(_nodeManager.SelectedNode.NodeNum != Instance.CurrentNodeIndex)
                {
                    Instance.OnSelectNodeAction?.Invoke(_nodeManager.SelectedNode.NodeNum);
                }
                else
                {
                    Instance.OnArriveAction?.Invoke(Instance.CurrentNodeIndex);
                }
            }
        }
    }

    void ChangeCurrentNodeIndex(int index)
    {
        CurrentNodeIndex = index;
        Debug.Log("currentNode : " + index);
        Invoke("SpawnReport", 0.1f);
    }

    void SpawnReport()
    {
        //Instantiate((GameObject)Resources.Load("HW/Canvas_ReportUI"));

        // 새 리포트 UI 테스트
        Instantiate(Resources.Load<GameObject>("GJ/Prefabs/Canvas_ReportUI_Slider"));
    }
}
