using UnityEngine;

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
    }

    private void Awake()
    {
        GameManager.Instance.OnConfirmUseAction += _ => AddParts();
        GameManager.Instance.OnConfirmUseAction += _ => EscapeEnding();
        GameManager.Instance.OnConfirmGainAction += _ => EscapeEnding();
    }

    public bool CheckEscape()
    {
        if (GetCurrentPartsCount() < 6)
        {
            return false;
        }

        if (GameManager.Aircraft.Food < 180)
        {
            return false;
        }

        return true;
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

    private void EscapeEnding()
    {
        if(CheckEscape())
        {
            Debug.Log("Escape Success!");
        }
    }
}
