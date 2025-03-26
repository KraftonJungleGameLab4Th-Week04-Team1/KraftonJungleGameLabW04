using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeCheck : MonoBehaviour
{
    public GameObject[] PartPrefabs = new GameObject[6];

    private void Start()
    {
        PartPrefabs[0] = gameObject.transform.GetChild(0).gameObject;
        PartPrefabs[1] = gameObject.transform.GetChild(1).gameObject;
        PartPrefabs[2] = gameObject.transform.GetChild(2).gameObject;
        PartPrefabs[3] = gameObject.transform.GetChild(3).gameObject;
        PartPrefabs[4] = gameObject.transform.GetChild(4).gameObject;
        PartPrefabs[5] = gameObject.transform.GetChild(5).gameObject;

        //이건 개오바라 이그노어메서드로 뺐어요
        //GameManager.Instance.OnConfirmAction += (_, __, ___, ____, _____, ______, _______, ________, _________) => AddParts();
        GameManager.Instance.OnConfirmAction += IgnoreParamsEscapeCheck;
    }

    public bool CheckEscape(int food)
    {
        if (GetCurrentPartsCount() < 6)
        {
            return false;
        }

        if (food < 180)
        {
            return false;
        }

        return true;
    }

    private void IgnoreParamsEscapeCheck(int a, int b, int c, int d, int e, int f, int g, int h, int i)
    {
        AddParts();
        EscapeEnding(f+a);
    }

    private void AddParts()
    {
        int partsCount = GetCurrentPartsCount();
        for (int i = 0; i < partsCount; i++)
        {
            if (PartPrefabs[i].activeSelf == false)
            {
                PartPrefabs[i].SetActive(true);
            }
        }
    }

    private int GetCurrentPartsCount()
    {
        int partsCount = 0;
        foreach (var havingPart in GameManager.NodeManager.spaceStationParts)
        {
            if (havingPart)
            {
                partsCount++;
            }
        }
        return partsCount;
    }

    private void EscapeEnding(int food)
    {
        if(CheckEscape(food))
        {
            SceneManager.LoadScene("EndingScene");
        }
    }
}
