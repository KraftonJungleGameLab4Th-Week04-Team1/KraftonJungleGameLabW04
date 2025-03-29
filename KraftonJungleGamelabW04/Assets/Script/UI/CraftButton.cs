using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] private ReportManager reportManager;
    [SerializeField] private GameObject craftButton;
    [SerializeField] private GameObject donePanel;
    [SerializeField] private GameObject checkedButton;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private int craftIndex;

    [SerializeField] private CraftData craftData;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI resourceText;

    private void Start()
    {
        if (reportManager == null)
        {
            reportManager = FindAnyObjectByType<ReportManager>(); // 필요 시 유지, 권장하지 않음
        }

        craftButton.GetComponent<Button>().onClick.AddListener(CheckThisCraftButton);
        checkedButton.GetComponent<Button>().onClick.AddListener(UnCheckThisCraftButton);

        craftData = reportManager.GetCraftDataByNodeIndex(craftIndex);
        nameText.text = craftData.partName;
        resourceText.text = craftData.requiredBolts + "/" + craftData.requiredNuts;



        reportManager.OnCraftCheckAction += SetCraftButtonState;


        // 초기 상태 설정 (ReportManager의 필드가 public이어야 함)
        // SetCraftButtonState(reportManager.finalCheckedIndex, reportManager.finalConfirmedIndex);
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
        //reportManager.OnCraftCheckAction -= SetCraftButtonState; // 이벤트 구독 해제
    }
}