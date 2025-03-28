using UnityEngine;

public class InfoManager
{
    public int InitialFood => _inititalFood; private int _inititalFood; //시작시 가질 음식의 양
    public int InitialFuel => _inititalFuel; private int _inititalFuel; //시작시 가질 연료의 양
    public int WeightPerFood => _weightPerFood; private int _weightPerFood; //음식의 단위무게
    public int WeightPerBolt => _weightPerBolt; private int _weightPerBolt; //볼트의 단위무게
    public int WeightPerNut => _weightPerNut; private int _weightPerNut; //너트의 단위무게
    public int WeightPerFuel => _weightPerFuel; private int _weightPerFuel; //연료의 단위무게
    public int MaxWeight => _maxWeight; private int _maxWeight; //기체의 최대 중량
    public int MaxAircraftState => _maxAircraftState; private int _maxAircraftState; //기체의 최대상태. 체력. 100
    public int FuelPerDistance => _fuelPerDistance; private int _fuelPerDistance; //거리당 소모 연료
    public int FoodPerDistance => _foodPerDistance; private int _foodPerDistance; //거리당 소모 식량.
    public int TimePerDistance => _timePerDistance; private int _timePerDistance; //거리당 소요시간.
    public int BaseRequiredFuel => _baseRequiredFuel; private int _baseRequiredFuel; //기본 소모연료
    public int BaseRequiredFood => _baseRequiredFood; private int _baseRequiredFood; //기본 소모 식량.
    public int QuotinentByWeight => _quotinentByWeight; private int _quotinentByWeight; //중량이 가득 찼을 때 드는 연료는 몇 배인지 설정.
    public int QuotinentByAircraftState => _quotinentByAircraftState; private int _quotinentByAircraftState; //기체 상태가 0%일 때 드는 연료는 몇 배인지 설정.
    public int BoltRepairValue => _boltRepairValue; private int _boltRepairValue; //볼트 하나당 기체 수리되는 정도.
    public int NutRepairValue => _nutRepairValue; private int _nutRepairValue; //너트 하나당 기체가 수리되는 정도.


    public void Init()
    {
        _inititalFood = 20;
        _inititalFuel = 100;
        _weightPerFood = 2;
        _weightPerBolt = 3;
        _weightPerNut = 4;
        _weightPerFuel = 1;
        _maxWeight = 300;
        _maxAircraftState = 100;
        _fuelPerDistance = 3;
        _foodPerDistance = 2;
        _timePerDistance = 30;
        _baseRequiredFood = 1;
        _baseRequiredFuel = 5;
        _quotinentByAircraftState = 1;
        _quotinentByWeight = 1;
        _boltRepairValue = 3;
        _nutRepairValue = 5;
    }

    /// <summary>
    /// 노드의 인덱스를 넣어 두 노드간 거리를 계산합니다.
    /// </summary>
    /// <param name="startNodeIndex"></param>
    /// <param name="destinationNodeIndex"></param>
    /// <returns></returns>
    public int GetDistance(int startNodeIndex, int destinationNodeIndex)
    {
        int xDistance = NodeManager.NodeDic[destinationNodeIndex].NodeIdx - NodeManager.NodeDic[startNodeIndex].NodeIdx;
        if(xDistance < 0) // 음수 처리..
        {
            int possibleDistance1 = -xDistance;
            int possibleDistance2 = 32 + xDistance;

            xDistance = Mathf.Min(possibleDistance1, possibleDistance2);
        }
        else if (xDistance >= 0) //인덱스 역초과 및 뒤로가기
        {
            int possibleDistance1 = xDistance;
            int possibleDistance2 = 32 - xDistance;

            xDistance = Mathf.Min(possibleDistance1, possibleDistance2);
        }

        return xDistance;
    }

    /// <summary>
    /// GameManager의 CurrentIndex를 기준으로 거리를 계산합니다.
    /// </summary>
    /// <param name="destinationNodeIndex"></param>
    /// <returns></returns>
    public int GetDistanceFromCurrentIndex(int destinationNodeIndex)
    {
        int xDistance = NodeManager.NodeDic[destinationNodeIndex].NodeIdx - NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex].NodeIdx;
        if (xDistance < 0) // 인덱스 초과 및 앞으로가기.
        {
            int possibleDistance1 = -xDistance;
            int possibleDistance2 = 32 + xDistance;

            xDistance = Mathf.Min(possibleDistance1, possibleDistance2);
        }
        else if(xDistance >= 0) //인덱스 역초과 및 뒤로가기
        {
            int possibleDistance1 = xDistance;
            int possibleDistance2 = 32 - xDistance;

            xDistance = Mathf.Min(possibleDistance1, possibleDistance2);
        }

        return xDistance;
    }




    /// <summary>
    /// 두 지점의 x거리를 이동하는 데에 얼마만큼의 시간이 필요한지 계산합니다. 분 단위로 반환합니다.
    /// </summary>
    /// <param name="xDistance"></param>
    /// <returns></returns>
    public int GetTimeRequiredBetweenNodes(int xDistance)
    {
        if(xDistance < 0) xDistance *= -1;

        return xDistance * _timePerDistance;
    }
 
    /// <summary>
    /// 두 지점의 x거리를 이동하는 데에 얼마만큼의 연료가 필요한지 계산합니다.
    /// </summary>
    /// <param name="xDistance"></param>
    /// <returns></returns>
    public int GetFuelRequiredBetweenNodes(int xDistance)
    {
        if(xDistance < 0) xDistance *= -1;

        AircraftManager aircraftManager = GameManager.Aircraft;
        int baseValue = xDistance * FuelPerDistance; //거리와 거리당 기본 소모연료의 곱.
        int fuelRequired = baseValue * (_maxWeight + aircraftManager.CurrentWeight) * 
            (2 * aircraftManager.MaxAircraftState - aircraftManager.CurrentAircraftState) *
             _quotinentByAircraftState * _quotinentByWeight / _maxWeight / _maxAircraftState + _baseRequiredFuel;

        return fuelRequired;
    }

    /// <summary>
    /// 두 지점의 x거리를 이동하는 데에 얼마만큼의 식량이 필요한지 계산합니다. 인수는 거리입니다.
    /// </summary>
    /// <param name="xDistance"></param>
    /// <returns></returns>
    public int GetFoodRequiredBetweenNodes(int xDistance)
    {
        if (xDistance < 0) xDistance *= -1;

        AircraftManager aircraftManager = GameManager.Aircraft;
        int baseValue = xDistance * _foodPerDistance;
        int stateFactor = 2 * aircraftManager.MaxAircraftState - aircraftManager.CurrentAircraftState;
        int foodRequired = baseValue * (_maxWeight + aircraftManager.CurrentWeight) * stateFactor /
            _maxWeight / _maxAircraftState;

        return Mathf.Max(foodRequired, 0);
    }

    /// <summary>
    /// 패러미터의 자원 갯수가 플레이어에게 추가되었을 때의 무게를 정수로 반환하는 함수입니다. 수량 조절을 통해 무게를 실시간으로 확인해야하는 경우 사용하면 됩니다.
    /// </summary>
    /// <param name="food"></param>
    /// <param name="bolt"></param>
    /// <param name="nut"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public int GetWeightByAddResource(int addedFood, int addedBolt, int addedNut, int addedFuel)
    {
        AircraftManager aircraftManager;
        aircraftManager = GameManager.Aircraft;
        return (addedFood + aircraftManager.Food) * _weightPerFood + (addedBolt + aircraftManager.Bolt) * _weightPerBolt + 
            (addedNut + aircraftManager.Nut) * _weightPerNut + (addedFuel + aircraftManager.Fuel) * _weightPerFuel;
    }

    /// <summary>
    /// "현재 플레이어의 무게 계산값"을 정수형으로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentWeight()
    {
        AircraftManager aircraftManager;
        aircraftManager = GameManager.Aircraft;
        return aircraftManager.Food * _weightPerFood + aircraftManager.Bolt * _weightPerBolt + aircraftManager.Nut * _weightPerNut + aircraftManager.Fuel * _weightPerFuel;
    }

    public int GetWeightByResource(int food, int bolt, int nut, int fuel)
    {
        return food * _weightPerFood + bolt * _weightPerBolt + nut * _weightPerNut + fuel * _weightPerFuel;
    }

    /// <summary>
    /// 네 자원을 모두 입력해 이 자원이 플레이어에게 추가되었을 때 가능한 무게값인지 bool값을 반환합니다.
    /// </summary>
    /// <param name="food"></param>
    /// <param name="bolt"></param>
    /// <param name="nut"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public bool IsPossibleWeight(int addedFood, int addedBolt, int addedNut, int addedFuel)
    {
        return GetWeightByAddResource(addedFood, addedBolt, addedNut, addedFuel) <= _maxWeight;
    }

    /// <summary>
    /// 자원을 입력해 소지 가능한지 검사합니다.
    /// </summary>
    /// <param name="food"></param>
    /// <param name="bolt"></param>
    /// <param name="nut"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public bool IsPossibleWeightByResource(int food, int bolt, int nut, int fuel)
    {
        return GetWeightByResource(food, bolt, nut, fuel) <= _maxWeight;
    }

    /// <summary>
    /// 현재 플레이어의 자원량이 가능한 값인지 bool값으로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public bool IsPossibleWeight()
    {
        return GetCurrentWeight() < _maxWeight;
    }

    public float GetCurrentMultiplierOfRequiredFoodInMove()
    {
        AircraftManager aircraftManager;
        aircraftManager = GameManager.Aircraft;

        float currentMultiplier = (1 + ((float)aircraftManager.CurrentWeight / aircraftManager.MaxWeight)) * ((float)(2 * _maxAircraftState - aircraftManager.CurrentAircraftState) / aircraftManager.MaxAircraftState);
        Debug.Log(currentMultiplier + " " + aircraftManager.CurrentWeight + " " + aircraftManager.CurrentAircraftState);
        return currentMultiplier;
    }

    /// <summary>
    /// 볼트와 너트 값을 기반으로 수리되는 기체의 상태의 수치를 반환합니다. 최대치를 넘어가는 수리의 경우 인수가 늘어나도 그 이상의 값이 반환되지 않습니다.
    /// 실제로 수리 행동을 진행하는 경우 반환되는 값을 기반으로 액션을 트리거 해주세요.
    /// </summary>
    /// <param name="bolt_count"></param>
    /// <param name="nut_count"></param>
    /// <returns></returns>
    public int GetRepairValue(int bolt_count, int nut_count)
    {
        int unlimitedRepairValue = bolt_count * _boltRepairValue + nut_count * _nutRepairValue;
        int currentAircraftState = GameManager.Aircraft.CurrentAircraftState;
        return currentAircraftState + unlimitedRepairValue > _maxAircraftState ? _maxAircraftState - currentAircraftState : unlimitedRepairValue;
    }
}
