using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState currentState;
    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Start Game");
        currentState = GameState.StartGame;
        LoaderOverlayManager.instance.EndOverlay();
    }

    private void PauseGame()
    {
        if (currentState == GameState.GameOver)
        {
            return;
        }
        Debug.Log("Pause Game");
        currentState = GameState.Paused;
    }

    public void GameOver()
    {
        if (currentState == GameState.GameOver)
        {
            return;
        }
        Debug.Log("Game Over");
        currentState = GameState.GameOver;
    }
}

public enum GameState
{
    // MainMenu,
    StartGame,
    Paused,
    GameOver
}



