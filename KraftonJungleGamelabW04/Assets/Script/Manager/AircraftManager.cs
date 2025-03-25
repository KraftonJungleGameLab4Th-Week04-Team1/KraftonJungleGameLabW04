using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    //변수
    public int Food => _food; private int _food;
    public int Bolt => _bolt; private int _bolt;
    public int Nut => _nut; private int _nut;
    public int Fuel => _fuel; private int _fuel;
    public int CurrentWeight => _currentWeight; private int _currentWeight;


    /// <summary>
    /// GameManager 초기화 후 초기화. 초기에 플레이어가 가질 자원량 설정.
    /// </summary>
    public void Init()
    {
        //초기 자원 설정.
        _food = 10;
        _bolt = 0;
        _nut = 0;
        _fuel = 15;
    }

    //////////자원 업데이트 함수
    private void AddResources(int foodToAdd, int boltToAdd, int nutToAdd, int fuelToAdd)
    {
           
    }

    private void DecreaseResources(int foodToDecrease, int boltToDecrease, int nutToDecrease, int fuelToDecrease)
    {

    }
}
