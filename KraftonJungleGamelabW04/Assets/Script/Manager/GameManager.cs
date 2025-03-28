using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    #region Managers
    public static UIManager UI { get { return Instance._uiManager; } }
    public static AircraftManager Aircraft { get { return Instance._aircraftManager; } }
    public static NodeManager NodeManager { get { return Instance._nodeManager; } }
    public static InfoManager Info { get { return Instance._infoManager; } }

    private UIManager _uiManager = new UIManager();
    private AircraftManager _aircraftManager = new AircraftManager();
    private SolarSystem _solarSystem;
    private NodeManager _nodeManager = new NodeManager();
    private InfoManager _infoManager = new InfoManager();
    #endregion
    
    #region Actions
    //GameTime이 인터벌마다 업데이트 되면 실행.
    public Action<float> OnChangedGameTimeAction; 
    //다른 노드를 눌렀을 때 창 띄우기 등 : int = currentNodeIdx, int = selectedNodeIdx
    public Action<int, int> OnSelectNodeAction; 
    //다른 노드로의 움직임을 시작했을 때 : int = nextNodeIdx
    public Action<int> OnMoveNodeAction; 
    //현재 노드를 클릭하거나, 다른 노드에 도착 했을 때의 액션 : int = arrivedNodeIdx
    public Action<int> OnArriveAction; 

    // 현재 최종값을 받아오는 구조라 이렇게 두개의 dto를 넘기는 형식으로 구성했습니다.
    // 격차를 받아오는 식이라면 dto 하나만 받아서 메서드 내부에서 계산할 수 있을 것같아서 고민입니다.
    // 액션 분리도 고려해볼만 합니다.
    // 노드최종값, 비행기최종값을 받아서 갱신하는 액션
    public Action<ResourceDto, ResourceDto> OnConfirmAction;
    #endregion

    #region Properties
    [Header("시간 관련")]
    private float _gameTime;
    public float GameTime => _gameTime;
    private readonly float _timeInterval = 0.5f; //_timeInterval당 인게임 시간 1분 증가.
    private float _time;

    [Header("노드 관련")]
    private int _currentNodeIndex;
    public int CurrentNodeIndex => _currentNodeIndex;
    private Node _selectedNextNode;

    [Header("게임 상태")]
    private bool _isGameStarted = false;
    public bool IsMoving;
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
    
    private void Init()
    {
        // 객체별 초기화 순서를 정하기 위한 구조 = 각 매니저별로 Awake()를 호출하지 않아도 됩니다.
        NodeManager.Init();
        Aircraft.Init();
        Info.Init();
        _solarSystem = FindFirstObjectByType<SolarSystem>();
        _solarSystem.Init();
        UI.Init();

        GameStart();
    }

    private void OnDestroy()
    {
        //액션 해제
        OnArriveAction = null;
        OnChangedGameTimeAction = null;
        OnSelectNodeAction = null;
        OnMoveNodeAction = null;
        OnConfirmAction = null;
    }

    private void GameStart()
    {
        GameState = GameState.Title;
        _currentNodeIndex = 1;

        //무빙 확인시 현재 노드 인덱스 변경.
        OnArriveAction += ChangeCurrentNodeIndex; //현재 노드인덱스는 도착시 변경.
    }
    
    public void StartGameTimer(bool isStart)
    {
        _isGameStarted = isStart;
    }

    public void ChangeGameTime(float time)
    {
        _gameTime += time;
        OnChangedGameTimeAction?.Invoke(GameTime);
    }

    /// <summary>
    /// 게임 시간 업데이트하고, 업데이트마다 뿌리는 중.
    /// </summary>
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
            _gameTime++;
            Debug.Log(GameTime);
            OnChangedGameTimeAction?.Invoke(GameTime);
        }
        
        // Get selected node
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMoving)
            {
                return;
            }
            
            if (FindAnyObjectByType<ReportManager>())
            {
                return;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(nameof(LayerName.NodeMarker))))
            {
                _selectedNextNode = hit.collider.GetComponent<NodeMarkerUI>().Node;
                
                if(_selectedNextNode.NodeNum != _currentNodeIndex)
                {
                    OnSelectNodeAction?.Invoke(_currentNodeIndex ,_selectedNextNode.NodeNum);
                }
                else
                {
                    // 현재 노드를 클릭했을 때.
                    OnArriveAction?.Invoke(_currentNodeIndex);
                }
            }
        }
    }

    private void ChangeCurrentNodeIndex(int arrivedNodeIdx)
    {
        _currentNodeIndex = arrivedNodeIdx;
        Debug.Log("currentNode : " + arrivedNodeIdx);
        Invoke("SpawnReport", 0.1f);
    }

    private void SpawnReport()
    {
        Instantiate((GameObject)Resources.Load("HW/Canvas_ReportUI"));

    }
}
