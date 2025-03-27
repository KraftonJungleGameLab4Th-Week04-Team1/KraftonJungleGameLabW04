using TMPro;
using UnityEngine;

public class AircraftManager
{
    //변수
    public int Food => _food; private int _food;
    public int Bolt => _bolt; private int _bolt;
    public int Nut => _nut; private int _nut;
    public int Fuel => _fuel; private int _fuel;
    public int CurrentWeight => _currentWeight; private int _currentWeight; //현재무게
    public int MaxWeight => _maxWeight; private int _maxWeight; //최대 수용가능 무게
    public int CurrentAircraftState => _currentAircraftState; private int _currentAircraftState; //현재 기체상태. max에 가까울 수록 좋음.
    public int MaxAircraftState => _maxAircraftState; private int _maxAircraftState; //최대 기체상태. 


    /// <summary>
    /// GameManager 초기화 후 초기화. 초기에 플레이어가 가질 자원량 설정.
    /// </summary>
    public void Init()
    {
        // 추가
        GameManager.Instance.OnArriveAction += _ => UpdateAircraftWeight();
        // 우주정거장건설, 기체수리에 쓰이는 모든 자원을 보고서로부터 한번에 받아옵니다.
        // 따라서 UIManager에서는 Node 객체만들어 담아서 액션을 Invoke해야합니다.
        _food = 30;
        _fuel = 50;
        _currentWeight = 0;
        _currentAircraftState = 100;
        _maxAircraftState = 100;
        _maxWeight = 300;
        _bolt = 0;
        _nut = 0;

    }

    void Start()
    {
        //_food = GameManager.Info.InitialFood; 
        //_fuel = GameManager.Info.InitialFuel;
        
    }

    public void UpdateAircraftResources(int newFood, int newBolt, int newNut, int newFuel)
    {
        _food = newFood;
        _bolt = newBolt;
        _nut = newNut;
        _fuel = newFuel;

        // 무게 갱신까지 자동으로.
        _currentWeight = GameManager.Info.CalculateCurrentWeight();
    }

    // 추가
    public void UpdateAircraftWeight()
    {
        _currentWeight = GameManager.Info.CalculateCurrentWeight();
    }

    public void RepairAircraftByInputValue(int value)
    {
        _currentAircraftState += value;
        if (_currentAircraftState > 100) _currentAircraftState = 100;
    }

    
    //////////자원 업데이트 함수. GameManager의 Action에 등록해야함.
    public void GainResources(Node toAircraft)
    {
        _food += toAircraft.Food;
        _bolt += toAircraft.Bolt;
        _nut += toAircraft.Nut;
        _fuel += toAircraft.Fuel;
        
        _currentWeight = GameManager.Info.GetCurrentWeight(_food, _bolt, _nut, _fuel);
    }

    public void UseResources(Node fromAircraft)
    {
        _food -= fromAircraft.Food;
        _bolt -= fromAircraft.Bolt;
        _nut -= fromAircraft.Nut;
        _fuel -= fromAircraft.Fuel;

        _currentWeight = GameManager.Info.GetCurrentWeight(_food, _bolt, _nut, _fuel);
    }

    /* [Legacy] 격차가 컨펌일때 사용하던 메서드들
    // 우주정거장이랑 기체수리정비소랑 같은 노드가 아니라는 전제로 코드를 짰습니다.
    public void RepairAircraft(Node fromAircraft)
    {
        _currentAircraftState += GameManager.Info.GetRepairValue(fromAircraft.Bolt, fromAircraft.Nut);
        if (_currentAircraftState > _maxAircraftState)
        {
            _currentAircraftState = _maxAircraftState;
        }
    }
    */

    public void DamageAircraft(int damage)
    {
        Debug.Log(_currentAircraftState);
        if(_currentAircraftState >= damage)
        {
            _currentAircraftState -= damage;
        }
        else
        {
            _currentAircraftState = 0;
        }
        Debug.Log(_currentAircraftState);
    }

    public void UseResourceForFly(int destinationIndex)
    {
        Debug.Log("CurrentState : " + _currentAircraftState);
        bool foodLack = false;
        bool fuelLack = false;

        int xDistance = Mathf.Abs(NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex].NodeIdx - NodeManager.NodeDic[destinationIndex].NodeIdx);
        int foodToUse = GameManager.Info.GetFoodRequiredBetweenNodes(xDistance);
        int fuelToUse = GameManager.Info.GetFuelRequiredBetweenNodes(xDistance);
        //int damage = NodeManager.NodeDic[destinationIndex].Risk;
        //DamageAircraft(damage);

        if(foodToUse > _food)
        {
            _food = 0;
            foodLack = true;
        }
        else
        {
            _food -= foodToUse;
        }

        if(fuelToUse > _fuel)
        {
            _fuel = 0;
            fuelLack = true;
        }
        else
        {
            _fuel -= fuelToUse;
        }

        if(fuelLack || foodLack)
        {
            _currentAircraftState -= 10;
            //GameManager.Instance.MakeEvent();

            
        }

        GameObject ResourceLog = GameObject.Find("ResourceLog");
        ResourceLog.GetComponent<TextMeshProUGUI>().text = "Lost " + foodToUse + " Foods. " + _food + " Left.\n" +
            "Lost " + fuelToUse + " Fuels. " + _fuel + " Left.";
    }
}
