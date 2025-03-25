using UnityEngine;

public class InfoManager : MonoBehaviour
{
    //각 자원의 단위무게 설정.
    public int WeightPerFood => _weightPerFood; private int _weightPerFood;
    public int WeightPerBolt => _weightPerBolt; private int _weightPerBolt;
    public int WeightPerNut => _weightPerNut; private int _weightPerNut;
    public int WeightPerFuel => _weightPerFuel; private int _weightPerFuel;
    public int MaxWeight => _maxWeight; private int _maxWeight;
    public int MaxAircraftState => _maxAircraftState; private int _maxAircraftState;
    public int FuelPerDistance => _fuelPerDistance; private int _fuelPerDistance;
    public int QuotinentByWeight => _quotinentByWeight; private int _quotinentByWeight; //중량이 가득 찼을 때 드는 연료는 몇 배인지 설정.
    public int QuotinentByAircraftState => _quotinentByAircraftState; private int _quotinentByAircraftState; //기체 상태가 0%일 때 드는 연료는 몇 배인지 설정.


    void Init()
    {
        //임시 값.
        _weightPerFood = 1;
        _weightPerBolt = 2;
        _weightPerNut = 3;
        _weightPerFuel = 2;
        _maxWeight = 300;
        _maxAircraftState = 100;
        _fuelPerDistance = 1;
        _quotinentByAircraftState = 2;
        _quotinentByWeight = 2;
    }
 
    /// <summary>
    /// 두 지점의 x거리를 이동하는 데에 얼마만큼의 연료가 필요한지 계산합니다.
    /// </summary>
    /// <param name="xDistance"></param>
    /// <returns></returns>
    public int FuelRequired(int xDistance)
    {
        AircraftManager aircraftManager = GameManager.Aircraft;
        int baseValue = xDistance * FuelPerDistance; //거리와 거리당 기본 소모연료의 곱.
        int fuelRequired = baseValue * (_maxWeight + aircraftManager.CurrentWeight) * 
            (2 * aircraftManager.MaxAircraftState - aircraftManager.AircraftState) *
             _quotinentByAircraftState * _quotinentByWeight / _maxWeight / _maxAircraftState;

        return fuelRequired;
    }

    /// <summary>
    /// 현재 무게를 정수로 반환하는 함수입니다. 수량 조절을 통해 무게를 실시간으로 확인해야하는 경우 사용하면 됩니다.
    /// </summary>
    /// <param name="food"></param>
    /// <param name="bolt"></param>
    /// <param name="nut"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public int CalculateCurrentWeight(int food, int bolt, int nut, int fuel)
    {
        return food * _weightPerFood + bolt * _weightPerBolt + nut * _weightPerNut + fuel * _weightPerFuel;
    }

    /// <summary>
    /// "현재 플레이어의 무게 계산값"을 정수형으로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int CalculateCurrentWeight()
    {
        AircraftManager aircraftManager;
        aircraftManager = GameManager.Aircraft;
        return aircraftManager.Food * _weightPerFood + aircraftManager.Bolt * _weightPerBolt + aircraftManager.Nut * _weightPerNut + aircraftManager.Fuel * _weightPerFuel;
    }

    /// <summary>
    /// 네 자원을 모두 입력해 가능한 무게값인지 bool값을 반환합니다.
    /// </summary>
    /// <param name="food"></param>
    /// <param name="bolt"></param>
    /// <param name="nut"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public bool IsPossibleWeight(int food, int bolt, int nut, int fuel)
    {
        return CalculateCurrentWeight(food, bolt, nut, fuel) < _maxWeight;
    }

    public bool IsPossibleWeight()
    {
        return CalculateCurrentWeight() < _maxWeight;
    }
}
