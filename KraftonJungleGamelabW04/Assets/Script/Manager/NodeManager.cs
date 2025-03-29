using System.Collections.Generic;
using UnityEngine;

public class NodeManager
{
    // 노드들을 관리하는 딕셔너리
    public static Dictionary<int, Node> NodeDic = new Dictionary<int, Node>();
    public bool[] spaceStationParts = new bool[5]; //6 -> 5

    public void Init()
    {
        NodeDic.Clear();

        //22개 노드 경로 추가
        for (int i = 1; i <= 22; i++)
        {
            AddDataToNodeInfo(i, "Nodes/Node" + i.ToString());
        }

        GameManager.Instance.OnConfirmAction += (nodeResources, _) => UpdateCurrentNodeAndAircraft(nodeResources);
        GameManager.Instance.OnArriveAction += VisitNodeFirstTime;
        GameManager.Instance.OnArriveAction += GetDamageOnAircraft;

        // Move중 가장 마지막에 호출될 함수. 가변변수 초기화
        GameManager.Instance.OnArriveAction += SetNodeRisk;
    }

    private void AddDataToNodeInfo(int key, string scriptableObjectPath)
    {
        if (!NodeDic.ContainsKey(key))
        {
            Node original = (Node)Resources.Load(scriptableObjectPath);
            Node copy = Object.Instantiate(original); // 원본 복사
            NodeDic.Add(key, copy);
        }
    }

    //컨펌 메서드
    private void UpdateCurrentNodeAndAircraft(ResourceDto changedValue)
    {
        int idx = GameManager.Instance.CurrentNodeIndex;
        NodeDic[idx].Food = changedValue.food;
        NodeDic[idx].Bolt = changedValue.bolt;
        NodeDic[idx].Nut = changedValue.nut;
        NodeDic[idx].Fuel = changedValue.fuel;
    }

    public void VisitNodeFirstTime(int nextNodeIdx)
    {
        if (NodeDic[nextNodeIdx].IsVisited)
        {
            return;
        }
        // 노드 방문 시 처음 방문했을 때의 이벤트를 적용합니다.
        NodeDic[nextNodeIdx].IsVisited = true;
    }

    public void GetDamageOnAircraft(int nextNodeIdx)
    {
        Debug.Log("GetDamageOnAircraft : " + NodeDic[nextNodeIdx].Risk);
        GameManager.Aircraft.DamageAircraft(NodeDic[nextNodeIdx].Risk);
    }

    // 이동 확정 시 노드들의 기본 리스크(이벤트 적용 전)를 재설정합니다.    
    public void SetNodeRisk(int nextNodeIdx = 0)
    {
        foreach (var node in NodeDic)
        {
            // 리스크를 같은 확률로 할지는 고민해봐야함 => 이벤트 고려할때 같이 고려
            node.Value.Risk = Random.Range(0, 5);
        }
    }
}


