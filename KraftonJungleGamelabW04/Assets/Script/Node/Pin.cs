using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private List<GameObject> _pins = new List<GameObject>();
    private int _currentNode;
    
    public void Init(int index)
    {
        _currentNode = index;

        _pins.Add(transform.GetChild(0).gameObject);
        _pins.Add(transform.GetChild(1).gameObject);
        
        _pins[0].gameObject.SetActive(true);
        _pins[1].gameObject.SetActive(false);
        
        GameManager.Instance.OnArriveAction += ChangeToVisitedPin;
    }

    private void ChangeToVisitedPin(int nodeNum)
    {
        if (nodeNum != NodeManager.NodeDic[_currentNode].NodeNum)
        {
            return;
        }
        
        _pins[0].gameObject.SetActive(false);
        _pins[1].gameObject.SetActive(true);
    }
}
