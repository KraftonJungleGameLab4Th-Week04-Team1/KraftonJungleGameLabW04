using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 생성시 자동으로 지역의 정보와 플레이어의 정보를 모두 가져옵니다. Instantiate 해주세요.
/// </summary>
public class ReportManager : MonoBehaviour
{
    private int _currentNodeIndex;
    private int _boltToUse;
    private int _nutToUse;
    private int _currentAircraftFood;
    private int _currentAircraftBolt;
    private int _currentAircraftNut;
    private int _currentAircraftFuel;
    private int _currentNodeFood;
    private int _currentNodeBolt;
    private int _currentNodeNut;
    private int _currentNodeFuel;
    private int _aircraftRepairValue;
    private int _currentAircraftWeight;
    private bool isRepairable;
    private bool isCraftable;
    private Node currentNode;
    private AircraftManager aircraftManager;
    private InfoManager Info;
    
    //UI References.
    [SerializeField] private TextMeshProUGUI CityNameText;
    [SerializeField] private GameObject RepairBlockPanel;
    [SerializeField] private GameObject CraftBlockPanel;
    [SerializeField] private GameObject RepairPanel;
    [SerializeField] private GameObject CraftPanel;
    [SerializeField] private TextMeshProUGUI CurrentWeightText;
    [SerializeField] private TextMeshProUGUI CurrentAircraftFoodText;
    [SerializeField] private TextMeshProUGUI CurrentAircraftBoltText;
    [SerializeField] private TextMeshProUGUI CurrentAircraftNutText;
    [SerializeField] private TextMeshProUGUI CurrentAircraftFuelText;
    [SerializeField] private TextMeshProUGUI CurrentNodeFoodText;
    [SerializeField] private TextMeshProUGUI CurrentNodeBoltText;
    [SerializeField] private TextMeshProUGUI CurrentNodeNutText;
    [SerializeField] private TextMeshProUGUI CurrentNodeFuelText;
    [SerializeField] private TextMeshProUGUI CurrentBoltsToUseText;
    [SerializeField] private TextMeshProUGUI CurrentNutsToUseText;
    [SerializeField] private TextMeshProUGUI RepairValueText;


    void Awake()
    {


    }

    void Start()
    {
        Info = GameManager.Info;
        currentNode = NodeManager.NodeDic[3];
        aircraftManager = GameManager.Aircraft;
        _boltToUse = 0;
        _nutToUse = 0;
        _aircraftRepairValue = 0;
        _currentAircraftWeight = aircraftManager.CurrentWeight;
        _currentAircraftFood = aircraftManager.Food;
        _currentAircraftBolt = aircraftManager.Bolt;
        _currentAircraftNut = aircraftManager.Nut;
        _currentAircraftFuel = aircraftManager.Fuel;
        _currentNodeFood = currentNode.Food;
        _currentNodeBolt = currentNode.Bolt;
        _currentNodeNut = currentNode.Nut;
        _currentNodeFuel = currentNode.Fuel;

        if(currentNode.NodeType == NodeType.Normal)
        {
            RepairPanel.SetActive(false);
            CraftPanel.SetActive(false);
            RepairBlockPanel.SetActive(true);
            CraftBlockPanel.SetActive(true);
        }
        else if(currentNode.NodeType == NodeType.RepairNode)
        {
            CraftPanel.SetActive(false);
            CraftBlockPanel.SetActive(true);
        }
        else if(currentNode.NodeType == NodeType.SpaceNode)
        {
            RepairPanel.SetActive(false);
            RepairBlockPanel.SetActive(true);
        }

        CityNameText.text = currentNode.name;
        UpdateAllUI();
    }

    //Repair functions.
    public void AddBoltToUse()
    {
        if(_currentAircraftBolt == 0 && _currentNodeBolt == 0) return;
        if(_currentNodeBolt <= 0)
        {
            _currentAircraftBolt -= 1;
        }
        else
        {
            _currentNodeBolt -= 1;
        }
        _boltToUse += 1;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateAllUI();
    }

    public void DecreaseBoltToUse()
    {
        if(_boltToUse <= 0) return;
        _boltToUse -= 1;
        _currentNodeBolt += 1;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateAllUI();
    }

    public void AddNutToUse()
    {
        if(_currentAircraftNut == 0 && _currentNodeNut == 0) return;
        if(_currentNodeNut <= 0)
        {
            _currentAircraftNut -= 1;
        }
        else
        {
            _currentNodeNut -= 1;
        }
        _nutToUse += 1;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateAllUI();
    }

    public void DecreaseNutToUse()
    {
        if(_nutToUse <= 0) return;
        _nutToUse -= 1;
        _currentNodeNut += 1;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        
        UpdateAllUI();
    }

    //Resource Functions.
    public void TakeFood()
    {
        if(_currentNodeFood <= 0) return;
        if(!Info.IsPossibleWeight(_currentAircraftFood + 1, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel)) return;

        _currentNodeFood -= 1;
        _currentAircraftFood += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    public void ReleaseFood()
    {
        if(_currentAircraftFood <= 0) return;
        _currentAircraftFood -= 1;
        _currentNodeFood += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    //Resource Functions.
    public void TakeBolt()
    {
        if(_currentNodeBolt <= 0) return;
        if(!Info.IsPossibleWeight(_currentAircraftFood, _currentAircraftBolt + 1, _currentAircraftNut, _currentAircraftFuel)) return;

        _currentNodeBolt -= 1;
        _currentAircraftBolt += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    public void ReleaseBolt()
    {
        if(_currentAircraftBolt <= 0) return;
        _currentAircraftBolt -= 1;
        _currentNodeBolt += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

        //Resource Functions.
    public void TakeNut()
    {
        if(_currentNodeNut <= 0) return;
        if(!Info.IsPossibleWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut + 1, _currentAircraftFuel)) return;

        _currentNodeNut -= 1;
        _currentAircraftNut += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    public void ReleaseNut()
    {
        if(_currentAircraftNut <= 0) return;
        _currentAircraftNut -= 1;
        _currentNodeNut += 1;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

        //Resource Functions.
    public void TakeFuel()
    {
        int finalValue;
        if(_currentNodeFuel <= 0) return;
        else if(_currentNodeFuel <= 5)
        {
            finalValue = 5;
            while(!Info.IsPossibleWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel + finalValue))
            {
                finalValue--;
            }
            if(finalValue > _currentNodeFuel)
            {
                finalValue = _currentNodeFuel;
            }
        }
        else
        {
            finalValue = 5;
            while(!Info.IsPossibleWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel + finalValue))
            {
                finalValue--;
            }
        }

        _currentNodeFuel -= finalValue;
        _currentAircraftFuel += finalValue;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    public void ReleaseFuel()
    {
        int finalValue;
        if(_currentAircraftFuel <= 0) return;
        else if(_currentAircraftFuel <= 5)
        {
            finalValue = _currentAircraftFuel;
        }
        else
        {
            finalValue = 5;
        }

        _currentAircraftFuel -= finalValue;
        _currentNodeFuel += finalValue;
        _currentAircraftWeight = Info.GetCurrentWeight(_currentAircraftFood, _currentAircraftBolt, _currentAircraftNut, _currentAircraftFuel);
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        CurrentWeightText.text = "Current Weight : " + _currentAircraftWeight + " / " + Info.MaxWeight;
        CurrentAircraftFoodText.text = _currentAircraftFood.ToString();
        CurrentAircraftBoltText.text = _currentAircraftBolt.ToString();
        CurrentAircraftNutText.text = _currentAircraftNut.ToString();
        CurrentAircraftFuelText.text = _currentAircraftFuel.ToString();
        CurrentNodeFoodText.text = _currentNodeFood.ToString();
        CurrentNodeBoltText.text = _currentNodeBolt.ToString();
        CurrentNodeNutText.text = _currentNodeNut.ToString();
        CurrentNodeFuelText.text = _currentNodeFuel.ToString();
        CurrentBoltsToUseText.text = _boltToUse.ToString();
        CurrentNutsToUseText.text = _nutToUse.ToString();
        RepairValueText.text = "Your Aircraft Will Be Repaired : +" + _aircraftRepairValue + "%";
    }
}
