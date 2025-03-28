using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    ReportManager reportManager;

    [SerializeField] GameObject craftButton;
    [SerializeField] GameObject donePanel;
    [SerializeField] GameObject checkedButton;
    [SerializeField] GameObject lockedPanel;
    //[SerializeField] TextMeshProUGUI requireText;

    [SerializeField] int craftIndex;

    private void Start()
    {
        reportManager = FindAnyObjectByType<ReportManager>(); //Find ReportManager on Canvas.
        craftButton.SetActive(false);
        donePanel.SetActive(false);
        checkedButton.SetActive(false);
        lockedPanel.SetActive(false);

        craftButton.GetComponent<Button>().onClick.AddListener(CheckThisCraftButton);
        checkedButton.GetComponent<Button>().onClick.AddListener(UnCheckThisCraftButton);
        //reportManager.OnCraftCheckAction += SetCraftButtonState
    }

    void SetCraftButtonState(int finalCheckedIndex, int finalConfirmedIndex)
    {
        if(craftIndex <= finalConfirmedIndex)
        {
            donePanel.SetActive(true);
        }
        else if(craftIndex <= finalCheckedIndex)
        {
            checkedButton.SetActive(true);
        }
        else if(craftIndex == finalCheckedIndex + 1)
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

        checkedButton.SetActive(false);
        lockedPanel.SetActive(false);
        craftButton.SetActive(false);
        checkedButton.SetActive(true);
    }

    void UnCheckThisCraftButton()
    {
        reportManager.UnCheckCraftPart(craftIndex);

        checkedButton.SetActive(false);
        lockedPanel.SetActive(false);
        checkedButton.SetActive(false);
        craftButton.SetActive(true);
    }
}
