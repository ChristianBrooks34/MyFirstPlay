using UnityEngine.SceneManagement;

public sealed class GameLauncer
{
    public void StartPlay()
    {
        SceneManager.LoadScene("Lobby");
    }
}
