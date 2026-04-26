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
        // kiểm tra nếu có dữ liệu lưu trước đó thì load nó lên, nếu không thì bắt đầu một game mới

    }

    private void PauseGame()
    {
        // end game rồi thì không cần pause nữa
        if (currentState == GameState.GameOver)
        {
            return;
        }
        Debug.Log("Pause Game");
        currentState = GameState.Paused;
        // tạm dừng game, có thể bằng cách set Time.timeScale = 0
        // SaveGame(); // lưu trạng thái hiện tại của game
    }

    public void GameOver()
    {
        // end game rồi thì không cần xử lý game over nữa
        if (currentState == GameState.GameOver)
        {
            return;
        }
        Debug.Log("Game Over");
        currentState = GameState.GameOver;
        // xử lý khi game kết thúc, có thể hiển thị màn hình
        // Xóa dữ liệu lưu nếu cần thiết hoặc giữ lại để có thể tiếp tục sau này
    }
}

public enum GameState
{
    // MainMenu,
    StartGame,
    Paused,
    GameOver
}



