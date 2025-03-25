using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // 노드들을 관리하는 딕셔너리
    public Dictionary<int, Node> NodeDic = new Dictionary<int, Node>();

    public void Init()
    {
        GameManager.Instance.OnConfirmNodeAction += GainResourece;
    }

    // Node 클래스는 구조체처럼 변수 저장용으로 사용합니다.
    public void GainResourece(Node report)
    {
        int idx = GameManager.Instance.CurrentNodeIndex;
        Node ToAircraft = new Node();

        ToAircraft.Food = report.Food;
        ToAircraft.Bolt = report.Bolt;
        ToAircraft.Fuel = report.Fuel;
        ToAircraft.Not = report.Not;
        //GameManager.Aircraft.AddResources(ToAircraft);

        NodeDic[idx].Food -= report.Food;
        NodeDic[idx].Bolt -= report.Bolt;
        NodeDic[idx].Fuel -= report.Fuel;
        NodeDic[idx].Not -= report.Not;
    }

}
