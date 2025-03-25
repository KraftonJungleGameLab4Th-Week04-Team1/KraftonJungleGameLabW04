using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // 노드들을 관리하는 딕셔너리
    public Dictionary<int, Node> NodeDic = new Dictionary<int, Node>();

    public void Init()
    {
        GameManager.Instance.OnConfirmNodeAction += GainResource;
        GameManager.Instance.OnConfirmNodeAction += BuildUpSpaceStation;
        GameManager.Instance.OnMoveNodeAction += GetDamageOnAircraft;
        GameManager.Instance.OnMoveNodeAction += SetNodeRisk;
    }

    // Node 클래스는 구조체처럼 변수 저장용으로 사용합니다.
    public void GainResource(Node report)
    {
        int currentIdx = GameManager.Instance.CurrentNodeIndex;
        Node ToAircraft = new Node();

        ToAircraft.Food = report.Food;
        ToAircraft.Bolt = report.Bolt;
        ToAircraft.Fuel = report.Fuel;
        ToAircraft.Nut = report.Nut;
        //GameManager.Aircraft.AddResources(ToAircraft);

        NodeDic[currentIdx].Food -= report.Food;
        NodeDic[currentIdx].Bolt -= report.Bolt;
        NodeDic[currentIdx].Fuel -= report.Fuel;
        NodeDic[currentIdx].Nut -= report.Nut;
    }

    public void GetDamageOnAircraft(int nextNodeIdx)
    {
        // 에어 크래프트 변수 혹은 함수 나오면 추가
        // Aircraft.Hp -= NodeDic[nextNodeIdx].Risk;
    }

    public void BuildUpSpaceStation(Node report)
    {
        // 원래는 UI단에서 해야하는 예외처리를 일단 여기서 처리합니다.
        int currentIdx = GameManager.Instance.CurrentNodeIndex;
        if (NodeDic[currentIdx].NodeType == NodeType.SpaceNode)
        {
            return;
        }

        //Aircraft의 자원소모에 대한 기능 구독은 AircraftManager 혹은 InputManager에서 계산하여 추가 구독합니다.
        //이 함수에서 한번에 처리한다면 아래와 같이 구현합니다.
        //Node ToAircraft = new Node();
        //ToAircraft.Bolt = -report.Bolt;
        //ToAircraft.Nut = -report.Nut;
        
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
