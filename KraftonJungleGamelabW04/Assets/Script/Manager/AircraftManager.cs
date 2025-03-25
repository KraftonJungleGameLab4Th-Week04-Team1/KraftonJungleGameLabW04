using UnityEngine;

public class AircraftManager : MonoBehaviour
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

    }

    void Start()
    {
        //_food = GameManager.Info.InitialFood; 
        //_fuel = GameManager.Info.InitialFuel;
        _bolt = 0;
        _nut = 0;
    }

    //////////자원 업데이트 함수. GameManager의 Action에 등록해야함.
    private void AddResources(int foodToAdd, int boltToAdd, int nutToAdd, int fuelToAdd)
    {
        _food += foodToAdd;
        _bolt += boltToAdd;
        _nut += nutToAdd;
        _fuel += fuelToAdd;
    }

    private void DecreaseResources(int foodToDecrease, int boltToDecrease, int nutToDecrease, int fuelToDecrease)
    {
        _food -= foodToDecrease;
        _bolt -= boltToDecrease;
        _nut -= nutToDecrease;
        _fuel -= fuelToDecrease;
    }

    private void DamageAircraft(int damage) // += 착륙시의 액션에 구독되어야함. 한 경우 v.
    {
        _currentAircraftState -= damage;
    }

    private void RepairAircraft(int repairValue) // += 수리 액션을 구독해야함. 한 경우에 v.
    {
        _currentAircraftState += repairValue;
        if(_currentAircraftState > _maxAircraftState)
        {
            _currentAircraftState = _maxAircraftState;
        }
    }
}
