using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ReportManager reportManager;
    [SerializeField] private GameObject craftButton;
    [SerializeField] private GameObject donePanel;
    [SerializeField] private GameObject checkedButton;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI requiredResourcesText;


    [Header("CraftData References")]
    [SerializeField] private int craftIndex;
    [SerializeField] CraftData craftData;

    private void Start()
    {
        if (reportManager == null)
        {
            reportManager = FindAnyObjectByType<ReportManager>(); // 필요 시 유지, 권장하지 않음
        }

        craftData = reportManager.GetCraftDataByIndex(craftIndex);
        nameText.text = craftData.partName;
        requiredResourcesText.text = craftData.requiredBolts.ToString() + "/" + craftData.requiredNuts;


        craftButton.GetComponent<Button>().onClick.AddListener(CheckThisCraftButton);
        checkedButton.GetComponent<Button>().onClick.AddListener(UnCheckThisCraftButton);

        reportManager.OnCraftCheckAction += SetCraftButtonState;
        
    }

    void SetCraftButtonState(int finalCheckedIndex, int finalConfirmedIndex)
    {
        // 모든 UI 요소 비활성화
        donePanel.SetActive(false);
        checkedButton.SetActive(false);
        craftButton.SetActive(false);
        lockedPanel.SetActive(false);

        // 조건에 따라 하나만 활성화
        if (craftIndex <= finalConfirmedIndex)
        {
            donePanel.SetActive(true);
        }
        else if (craftIndex <= finalCheckedIndex)
        {
            checkedButton.SetActive(true);
        }
        else if (craftIndex == finalCheckedIndex + 1)
        {
            craftButton.SetActive(true);
        }
        else
        {
            lockedPanel.SetActive(true);
        }
    }

    void CheckThisCraftButton()
    {
        reportManager.CheckCraftPart(craftIndex);
    }

    void UnCheckThisCraftButton()
    {
        reportManager.UnCheckCraftPart(craftIndex);
    }

    private void OnDestroy()
    {
    }
}