using System;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonSaveSystem<T>
{
    private readonly string _filePath;
    private const string BackupExtension = ".backup";

    public JsonSaveSystem(string fileName = "Save.json")
    {
        string directory = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        _filePath = Path.Combine(directory, fileName);
    }

    /// <summary>Сохраняет данные в JSON-файл с резервным копированием</summary>
    public bool Save(T data)
    {
        if (data == null)
        {
            Debug.LogError("Попытка сохранить null-данные!");
            return false;
        }

        try
        {
            CreateBackup();

            var json = JsonUtility.ToJson(data, true);
            using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                writer.Write(json);
            }

            Debug.Log($"Данные типа {typeof(T).Name} успешно сохранены: {_filePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка сохранения данных типа {typeof(T).Name}: {e.Message}");
            RestoreFromBackup();
            return false;
        }
    }

    /// <summary>Загружает данные из JSON-файла</summary>
    public T Load()
    {
        if (!File.Exists(_filePath))
        {
            Debug.LogWarning($"Файл сохранения не найден: {_filePath}. Используется значение по умолчанию.");
            return default;
        }

        try
        {
            string json = File.ReadAllText(_filePath, Encoding.UTF8);
            T result = JsonUtility.FromJson<T>(json);

            if (result == null)
            {
                Debug.LogError("Ошибка десериализации: получен null после загрузки.");
                return default;
            }

            Debug.Log($"Данные типа {typeof(T).Name} успешно загружены: {_filePath}");
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка загрузки данных типа {typeof(T).Name}: {e.Message}. Попытка восстановления из бэкапа...");
            return RestoreFromBackup() ?? default;
        }
    }

    /// <summary>Создаёт резервную копию текущего файла сохранения</summary>
    private void CreateBackup()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string backupPath = _filePath + BackupExtension;
                File.Copy(_filePath, backupPath, true);
                Debug.Log("Создана резервная копия сохранения.");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Не удалось создать резервную копию: {e.Message}");
        }
    }

    /// <summary>Восстанавливает данные из резервной копии</summary>
    private T RestoreFromBackup()
    {
        string backupPath = _filePath + BackupExtension;
        if (!File.Exists(backupPath))
        {
            Debug.LogWarning("Резервная копия не найдена.");
            return default;
        }

        try
        {
            string json = File.ReadAllText(backupPath, Encoding.UTF8);
            T restored = JsonUtility.FromJson<T>(json);
            if (restored != null)
            {
                File.Copy(backupPath, _filePath, true);
                Debug.Log("Восстановление из резервной копии успешно.");
                return restored;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка восстановления из бэкапа: {e.Message}");
        }
        return default;
    }
}