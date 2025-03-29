using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 생성시 자동으로 지역의 정보와 플레이어의 정보를 모두 가져옵니다. Instantiate 해주세요.
/// </summary>
public class ReportManager : MonoBehaviour
{
    public enum ResourceType
    {
        Food,
        Bolt,
        Nut,
        Fuel
    }

    //Infos.
    private int _boltToUse;
    private int _nutToUse;
    private int _aircraftRepairValue;
    private int _currentAircraftWeight;

    //amount of moving resource per input.
    private int _amountOfFoodMovingByInput;
    private int _amountOfBoltMovingByInput;
    private int _amountOfNutMovingByInput;
    private int _amountOfFuelMovingByInput;

    //private bool[] _isPartsConfirmed = new bool[6];
    //private bool[] _isPartsChecked = new bool[6];
    private int finalConfirmedIndex;
    private int finalCheckedIndex;

    //References.
    private Node currentNode;
    private AircraftManager aircraftManager;
    private InfoManager Info;

    //DTOs.
    ResourceDto aircraftValue;
    ResourceDto nodeValue;

    private CraftData[] craftDatas; // Array to hold all CraftData ScriptableObjects

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
    //[SerializeField] private List<GameObject> CraftButtons;
    [SerializeField] private GameObject ConfirmButton;
    [SerializeField] private Slider weightSlider;

    public Action<int, int> OnCraftCheckAction;

    void Awake()
    {


    }

    void Start()
    {
        //InitializeData
        InitializeInformation();
        
        //InitializeUI
        InitializeUI();
        UpdateInfoUI();
    }

    private void InitializeInformation()
    {
        Info = GameManager.Info;
        currentNode = NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex];
        aircraftManager = GameManager.Aircraft;


        _boltToUse = 0;
        _nutToUse = 0;
        _aircraftRepairValue = 0;
        _amountOfBoltMovingByInput = 1;
        _amountOfBoltMovingByInput = 1;
        _amountOfNutMovingByInput = 1;
        _amountOfFuelMovingByInput = 5;

        aircraftValue = new ResourceDto(
            food: aircraftManager.Food,
            bolt: aircraftManager.Bolt,
            nut: aircraftManager.Nut,
            fuel: aircraftManager.Fuel,
            repairValue: _aircraftRepairValue
        //stateValue : ???
        );

        nodeValue = new ResourceDto(
            food: currentNode.Food,
            bolt: currentNode.Bolt,
            nut: currentNode.Nut,
            fuel: currentNode.Fuel
        );

        _currentAircraftWeight = Info.GetWeightByResource(aircraftValue.food, aircraftValue.bolt, aircraftValue.nut, aircraftValue.fuel);

        int i = 0;
        while (i < GameManager.NodeManager.spaceStationParts.Length && GameManager.NodeManager.spaceStationParts[i])
        {
            i++; 
        }
        finalCheckedIndex = i - 1;
        finalConfirmedIndex = i - 1;
        Invoke("UpdateCraftButton", 0.15f);
        // Load all CraftData ScriptableObjects from Resources/CraftDatas/
        craftDatas = Resources.LoadAll<CraftData>("CraftDatas");
    }

    private void InitializeUI()
    {
        //Panel 설정.
        if (currentNode.NodeType == NodeType.Normal)
        {
            RepairPanel.SetActive(false);
            CraftPanel.SetActive(false);
            RepairBlockPanel.SetActive(true);
            CraftBlockPanel.SetActive(true);
        }
        else if (currentNode.NodeType == NodeType.RepairNode)
        {
            CraftPanel.SetActive(false);
            CraftBlockPanel.SetActive(true);
        }
        else if (currentNode.NodeType == NodeType.SpaceNode)
        {
            RepairPanel.SetActive(false);
            RepairBlockPanel.SetActive(true);

            UpdateCraftButton();
        }

        //상단 도시이름 설정.
        CityNameText.text = currentNode.NodeName;
    }
   

    //Repair functions.
    public void AddBoltToUse()
    {
        if(aircraftValue.bolt == 0 && nodeValue.bolt == 0) return;
        if(nodeValue.bolt <= 0)
        {
            aircraftValue.bolt -= _amountOfBoltMovingByInput;
        }
        else
        {
            nodeValue.bolt -= _amountOfBoltMovingByInput;
        }
        _boltToUse += _amountOfBoltMovingByInput;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateInfoUI();
    }

    public void DecreaseBoltToUse()
    {
        if(_boltToUse <= 0) return;
        _boltToUse -= _amountOfBoltMovingByInput;
        nodeValue.bolt += _amountOfBoltMovingByInput;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateInfoUI();
    }

    public void AddNutToUse()
    {
        if(aircraftValue.nut == 0 && nodeValue.nut == 0) return;
        if(nodeValue.nut <= 0)
        {
            aircraftValue.nut -= _amountOfNutMovingByInput;
        }
        else
        {
            nodeValue.nut -= _amountOfNutMovingByInput;
        }
        _nutToUse += _amountOfNutMovingByInput;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        UpdateInfoUI();
    }

    public void DecreaseNutToUse()
    {
        if(_nutToUse <= 0) return;
        _nutToUse -= _amountOfNutMovingByInput;
        nodeValue.nut += _amountOfNutMovingByInput;
        _aircraftRepairValue = Info.GetRepairValue(_boltToUse, _nutToUse);
        
        UpdateInfoUI();
    }

    #region Resource Funtions
    public void TakeFood() => TransferResource(ResourceType.Food, isTaking : true);
    public void ReleaseFood() => TransferResource(ResourceType.Food, isTaking : false);
    public void TakeBolt() => TransferResource(ResourceType.Bolt, isTaking: true);
    public void ReleaseBolt() => TransferResource(ResourceType.Bolt, isTaking: false);
    public void TakeNut() => TransferResource(ResourceType.Nut, isTaking: true);
    public void ReleaseNut() => TransferResource(ResourceType.Nut, isTaking: false);

    public void TakeFuel()
    {
        int amount = Mathf.Min(nodeValue.fuel, 5);
        while (amount > 0 && !TransferResource(ResourceType.Fuel, true, amount))
        {
            amount--;
        }
    }

    public void ReleaseFuel()
    {
        int amount = Mathf.Min(aircraftValue.fuel, 5);
        TransferResource(ResourceType.Fuel, false, amount);
    }

    private bool TransferResource(ResourceType type, bool isTaking, int amount = 1)
    {
        // Get current values based on resource type
        int currentNodeAmount = type switch
        {
            ResourceType.Food => nodeValue.food,
            ResourceType.Bolt => nodeValue.bolt,
            ResourceType.Nut => nodeValue.nut,
            ResourceType.Fuel => nodeValue.fuel,
            _ => 0
        };

        int currentAircraftAmount = type switch
        {
            ResourceType.Food => aircraftValue.food,
            ResourceType.Bolt => aircraftValue.bolt,
            ResourceType.Nut => aircraftValue.nut,
            ResourceType.Fuel => aircraftValue.fuel,
            _ => 0
        };

        // Check conditions
        if (isTaking)
        {
            if (currentNodeAmount < amount) return false;
            var tempAircraftValues = new ResourceDto(
                food: type == ResourceType.Food ? aircraftValue.food + amount : aircraftValue.food,
                bolt: type == ResourceType.Bolt ? aircraftValue.bolt + amount : aircraftValue.bolt,
                nut: type == ResourceType.Nut ? aircraftValue.nut + amount : aircraftValue.nut,
                fuel: type == ResourceType.Fuel ? aircraftValue.fuel + amount : aircraftValue.fuel
            );
            if (!Info.IsPossibleWeightByResource(tempAircraftValues.food, tempAircraftValues.bolt, tempAircraftValues.nut, tempAircraftValues.fuel))
                return false;
        }
        else if (currentAircraftAmount < amount)
        {
            return false;
        }

        // Update values
        int change = isTaking ? amount : -amount;

        switch (type)
        {
            case ResourceType.Food:
                nodeValue.food -= change;
                aircraftValue.food += change;
                break;
            case ResourceType.Bolt:
                nodeValue.bolt -= change;
                aircraftValue.bolt += change;
                break;
            case ResourceType.Nut:
                nodeValue.nut -= change;
                aircraftValue.nut += change;
                break;
            case ResourceType.Fuel:
                nodeValue.fuel -= change;
                aircraftValue.fuel += change;
                break;
        }

        _currentAircraftWeight = Info.GetWeightByResource(aircraftValue.food, aircraftValue.bolt, aircraftValue.nut, aircraftValue.fuel);
        UpdateInfoUI();
        return true;
    }

    #endregion



    #region Crafts
    //Craft Functions.

    public void CheckCraftPart(int partIndex)
    {
        if (partIndex < 0 || partIndex >= craftDatas.Length || partIndex >= GameManager.NodeManager.spaceStationParts.Length) return;

        CraftData craftData = craftDatas[partIndex];
        int totalBoltsAvailable = nodeValue.bolt + aircraftValue.bolt;
        int totalNutsAvailable = nodeValue.nut + aircraftValue.nut;

        if (totalBoltsAvailable < craftData.requiredBolts || totalNutsAvailable < craftData.requiredNuts) return;

        int boltsFromNode = Math.Min(nodeValue.bolt, craftData.requiredBolts);
        nodeValue.bolt -= boltsFromNode;
        aircraftValue.bolt -= (craftData.requiredBolts - boltsFromNode);

        int nutsFromNode = Math.Min(nodeValue.nut, craftData.requiredNuts);
        nodeValue.nut -= nutsFromNode;
        aircraftValue.nut -= (craftData.requiredNuts - nutsFromNode);

        finalCheckedIndex = partIndex;
        UpdateCraftButton();
        UpdateInfoUI();
    }

    public void UnCheckCraftPart(int partIndex)
    {
        if (partIndex < 0 || partIndex >= craftDatas.Length || partIndex >= GameManager.NodeManager.spaceStationParts.Length) return;

        for (int i = partIndex; i <= finalCheckedIndex; i++)
        {
            CraftData craftData = craftDatas[i];
            nodeValue.bolt += craftData.requiredBolts;
            nodeValue.nut += craftData.requiredNuts;
        }

        finalCheckedIndex = partIndex - 1;
        UpdateCraftButton();
        UpdateInfoUI();
    }

    public void ConfirmReport()
    {
        for (int i = finalConfirmedIndex + 1; i <= finalCheckedIndex; i++)
        {
            GameManager.NodeManager.spaceStationParts[i] = true;
        }
        finalConfirmedIndex = finalCheckedIndex;

        GameManager.Instance.OnConfirmAction?.Invoke(nodeValue, aircraftValue);
        Destroy(gameObject);
    }
    public void UpdateCraftButton()
    {
        OnCraftCheckAction?.Invoke(finalCheckedIndex, finalConfirmedIndex);
    }

    #endregion

    private void UpdateInfoUI()
    {
        CurrentWeightText.text = "무게 : " + _currentAircraftWeight + " / " + Info.MaxWeight;
        weightSlider.value = _currentAircraftWeight;
        CurrentAircraftFoodText.text = aircraftValue.food.ToString();
        CurrentAircraftBoltText.text = aircraftValue.bolt.ToString();
        CurrentAircraftNutText.text = aircraftValue.nut.ToString();
        CurrentAircraftFuelText.text = aircraftValue.fuel.ToString();
        CurrentNodeFoodText.text = nodeValue.food.ToString();
        CurrentNodeBoltText.text = nodeValue.bolt.ToString();
        CurrentNodeNutText.text = nodeValue.nut.ToString();
        CurrentNodeFuelText.text = nodeValue.fuel.ToString();
        CurrentBoltsToUseText.text = _boltToUse.ToString();
        CurrentNutsToUseText.text = _nutToUse.ToString();
        RepairValueText.text = "항공기가 " + _aircraftRepairValue + "% 만큼 수리됩니다.";
    }

    //public void ConfirmReport()
    //{
    //    Debug.Log($"Confirming - Node: Food={nodeValue.food}, Bolt={nodeValue.bolt}, Nut={nodeValue.nut}, Fuel={nodeValue.fuel}");
    //    Debug.Log($"Confirming - Aircraft: Food={aircraftValue.food}, Bolt={aircraftValue.bolt}, Nut={aircraftValue.nut}, Fuel={aircraftValue.fuel}");
    //    Debug.Log($"Repair: BoltsToUse={_boltToUse}, NutsToUse={_nutToUse}, RepairValue={_aircraftRepairValue}");

    //    int i = 0;
    //    while (i < GameManager.NodeManager.spaceStationParts.Length && i <= finalCheckedIndex)
    //    {
    //        GameManager.NodeManager.spaceStationParts[i] = true;
    //        i++;
    //    }



    //    GameManager.Instance.OnConfirmAction?.Invoke(nodeValue, aircraftValue);
    //    Debug.Log("after repair : " + GameManager.Aircraft.CurrentAircraftState);
    //    Destroy(gameObject);
    //}

    public void CancelReport()
    {
        Destroy(gameObject);
    }
}
 