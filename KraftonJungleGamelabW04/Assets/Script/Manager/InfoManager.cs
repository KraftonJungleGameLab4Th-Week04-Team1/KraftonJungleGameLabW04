using UnityEngine;

public class InfoManager : MonoBehaviour
{
    //각 자원의 단위무게 설정.
    public int WeightPerFood => _weightPerFood; private int _weightPerFood;
    public int WeightPerBolt => _weightPerBolt; private int _weightPerBolt;
    public int WeightPerNut => _weightPerNut; private int _weightPerNut;
    public int WeightPerFuel => _weightPerFuel; private int _weightPerFuel;

    void Init()
    {
        //Now For Debugging.
        _weightPerFood = 1;
        _weightPerBolt = 2;
        _weightPerNut = 3;
        _weightPerFuel = 2;
    }
 
}
