using UnityEngine;

//[System.Serializable] 혹시 json으로 쓸수있으니..
public class ResourceDto
{
    // 잠시 데이터 운반용으로만 쓰이기 때문에 게터세터활용하지 않음.
    public int food;
    public int bolt;
    public int nut;
    public int fuel;
    public int repairValue;
    // state 변화량
    public int stateValue;

    public ResourceDto(int food, int bolt, int nut, int fuel, int repairValue = 0, int stateValue = 0)
    {
        this.food = food;
        this.bolt = bolt;
        this.nut = nut;
        this.fuel = fuel;
        this.repairValue = repairValue;
        this.stateValue = stateValue;
    }
}
