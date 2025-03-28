using System.Collections.Generic;
using UnityEngine;

public class NodeManager
{
    // 노드들을 관리하는 딕셔너리
    public static Dictionary<int, Node> NodeDic = new Dictionary<int, Node>();
    public Node SelectedNode;
    public bool[] spaceStationParts = new bool[6];

    public void Init()
    {
        //22개 노드 경로 추가
        for (int i = 1; i <= 22; i++)
        {
            AddDataToBossInfo(i, "Nodes/Node" + i.ToString());
        }

        GameManager.Instance.OnConfirmAction += (nodeResources, _) => UpdateCurrentNodeAndAircraft(nodeResources);
        GameManager.Instance.OnMoveNodeAction += GetDamageOnAircraft;
        GameManager.Instance.OnMoveNodeAction += VisitNodeFirstTime;

        // Move중 가장 마지막에 호출될 함수. 가변변수 초기화
        GameManager.Instance.OnMoveNodeAction += SetNodeRisk;
    }

    private void AddDataToBossInfo(int key, string scriptableObjectPath)
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

    /* [Legacy] 격차가 컨펌일때 사용하던 메서드들
    // Node 클래스는 구조체처럼 변수 저장용으로 사용합니다.
    public void GainResource(Node report)
    {
        //현재 AirCraft가 위치한 노드
        int currentIdx = GameManager.Instance.CurrentNodeIndex;

        //현재 노드에서 자원을 획득하고, 노드의 자원을 감소시킵니다.
        Node ToAircraft = new Node();
        ToAircraft.Food = report.Food;
        ToAircraft.Bolt = report.Bolt;
        ToAircraft.Fuel = report.Fuel;
        ToAircraft.Nut = report.Nut;
        GameManager.Aircraft.GainResources(ToAircraft);

        NodeDic[currentIdx].Food -= report.Food;
        NodeDic[currentIdx].Bolt -= report.Bolt;
        NodeDic[currentIdx].Fuel -= report.Fuel;
        NodeDic[currentIdx].Nut -= report.Nut;
    }

    public void BuildUpSpaceStation(Node report)
    {
        // 원래는 UI단에서 해야하는 예외처리를 일단 여기서 처리합니다.
        int currentIdx = GameManager.Instance.CurrentNodeIndex;
        if (NodeDic[currentIdx].NodeType == NodeType.SpaceNode)
        {
            return;
        }
        //Aircraft의 자원소모에 대한 기능은 AircraftManager에서 계산하여 추가 구독합니다.
        NodeDic[GameManager.Instance.CurrentNodeIndex].SpaceStationLevel++;
    }
    
    */

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
