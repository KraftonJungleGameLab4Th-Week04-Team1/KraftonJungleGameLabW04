using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // 노드들을 관리하는 딕셔너리
    public Dictionary<int, Node> NodeDic = new Dictionary<int, Node>();
    public Node SelectedNode;
    
    public void Init()
    {
        Node[] foundNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node node in foundNodes)
        {
            if (!NodeDic.ContainsKey(node.NodeIdx))
            {
                NodeDic.Add(node.NodeIdx, node);
            }
            else
            {
                Debug.LogWarning($"중복된 인덱스: {node.NodeIdx}를 가진 Node가 이미 존재합니다.");
            }
        }
        GameManager.Instance.OnConfirmGainAction += GainResource;
        GameManager.Instance.OnConfirmUseAction += BuildUpSpaceStation;
        GameManager.Instance.OnMoveNodeAction += GetDamageOnAircraft;
        GameManager.Instance.OnMoveNodeAction += SetNodeRisk;
    }

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

    public void GetDamageOnAircraft(int nextNodeIdx)
    {
        GameManager.Aircraft.DamageAircraft(NodeDic[nextNodeIdx].Risk);
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

    // 이동 확정 시 노드들의 기본 리스크(이벤트 적용 전)를 재설정합니다.    
    public void SetNodeRisk(int nextNodeIdx = 0)
    {
        foreach (var node in NodeDic)
        {
            node.Value.Risk = Random.Range(0, 5);
        }
    }
}
