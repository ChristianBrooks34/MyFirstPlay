using UnityEngine;

public static class UnitDataInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeGameStart()
    {
        var allProfiles = Resources.LoadAll<UnitProfile>("Units");

        if (allProfiles == null || allProfiles.Length == 0)
        {
            Debug.LogWarning("UnitDataInitializer: Не найдено UnitProfile в папке Resources/Units. Инициализация пропущена.");
            return;
        }

        foreach (var profile in allProfiles)
        {
            if (profile == null)
            {
                Debug.LogError("UnitDataInitializer: Обнаружен null-профиль в списке. Пропускаем...");
                continue;
            }

            // Принудительно вызываем инициализацию данных
            profile.InitializeUnitData();
        }
    }
}
