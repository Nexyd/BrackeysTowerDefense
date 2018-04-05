using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIData data;
    private bool gameOver = false;
    
    private void Start()
    {
        data = UIData.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.Lives <= 0 && !gameOver)
            EndGame();
    }

    void EndGame()
    {
        data.GameLoggerText = "Game Over!";
        gameOver = true;
    }
}