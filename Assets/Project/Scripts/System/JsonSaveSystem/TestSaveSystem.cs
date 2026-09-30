using UnityEngine;
using Zenject;

public class TestSaveSystem : MonoBehaviour
{
    private JsonSaveSystem<PlayerProfile> _saveSystem;
    private Game _game;

    [Inject]
    public void Construct(JsonSaveSystem<PlayerProfile> saveSystem, Game game)
    {
        _saveSystem = saveSystem;
        _game = game;

        DontDestroyOnLoad(gameObject);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        _saveSystem.Save(_game.GameData.SelectedPlayer.Value);
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        var playerProfile = _saveSystem.Load();
    //        Debug.Log($"playerProfile = {playerProfile.Data.Name}, Health = {playerProfile.Data.Health.StartValue}");
    //        _game.GameData.SelectedPlayer.Value = playerProfile;
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        string savePath = Application.persistentDataPath + "/Saves";
    //        Debug.Log(savePath);

    //        if (!Directory.Exists(savePath))
    //        {
    //            Debug.LogWarning("Папка сохранений не найдена: " + savePath);
    //            return;
    //        }

    //        try
    //        {
    //            // Удаляем все файлы в папке
    //            string[] files = Directory.GetFiles(savePath);
    //            foreach (string file in files)
    //            {
    //                File.Delete(file);
    //                Debug.Log("Удален файл: " + file);
    //            }

    //            Debug.Log("Все файлы сохранений успешно удалены!");
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.LogError("Ошибка при удалении файлов: " + e.Message);
    //        }
    //    }
    //}
}
