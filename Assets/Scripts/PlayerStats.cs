using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startingMoney;
    public int startingLives;
    private static int money;
    private static int lives;

    private const int maxMoney = 99999;
    private static UIData data;
    
    private void Start()
    {
        data = UIData.GetInstance();

        Money = startingMoney;
        data.Currency = Money;

        Lives = startingLives;
        data.Lives = Lives;
    }

    public static int Money
    {
        get { return money; }
        set {
            money = value;
            data.Currency = value;
        }
    }

    public static int Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    public static int MaxMoney
    {
        get { return maxMoney; }
    }
}