using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeCheck : MonoBehaviour
{
    public int EscapeFoodCount = 180;
    public int PartCount = 5;
    private GameObject[] _partPrefabs;

    private void Start()
    {
        _partPrefabs = new GameObject[PartCount];

        for (int i = 0; i < PartCount; i++)
        {
            _partPrefabs[i] = gameObject.transform.GetChild(i).gameObject;
        }

        //이건 개오바라 이그노어메서드로 뺐어요
        //GameManager.Instance.OnConfirmAction += (_, __, ___, ____, _____, ______, _______, ________, _________) => AddParts();
        GameManager.Instance.OnConfirmAction += IgnoreParamsEscapeCheck;
    }

    public bool CheckEscape(int food)
    {
        if (GetCurrentPartsCount() < PartCount)
        {
            return false;
        }

        if (food < EscapeFoodCount)
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
            if (_partPrefabs[i].activeSelf == false)
            {
                _partPrefabs[i].SetActive(true);
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
