using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitProfileManager
{
    public void InitializeAllUnitProfile()
    {
        var unitProfiles = LoadAllUnitProfile();

        if (unitProfiles == null || !unitProfiles.Any())
        {
            Debug.LogError("Не удалось загрузить UnitProfile. Проверьте папку Resources/Units.");
            return;
        }

        foreach (var unitProfile in unitProfiles)
        {
            if (unitProfile == null)
            {
                Debug.LogError("Обнаружен null UnitProfile в списке загруженных объектов. Пропускаем...");
                continue;
            }

            UnitData unitData = null;

            if (unitProfile is EnemyProfile)
            {
                var enemyProfile = (EnemyProfile)unitProfile;

                unitData = enemyProfile.Data;
            }

            if (unitProfile is PlayerProfile)
            {
                var playerProfile = (PlayerProfile)unitProfile;

                unitData = playerProfile.Data;

                playerProfile.Data.Exp = 0;
                playerProfile.Data.LevelUpExp = playerProfile.Data.StartLevelUpExp;
            }

            unitData.CurrentLevel = 0;

            unitData.Health.StartValue = unitData.BaseMaxHealth;
            unitData.MaxHealth = (int)unitData.Health.StartValue;
            unitData.Health.CurrentValue = unitData.Health.StartValue;
            unitData.Health.Price = unitData.Health.StartPrice;
            unitData.Health.NumberDevelop = 0;

            unitData.SpeedMovement.CurrentValue = unitData.SpeedMovement.StartValue;
            unitData.SpeedMovement.Price = unitData.SpeedMovement.StartPrice;
            unitData.SpeedMovement.NumberDevelop = 0;

            unitData.Damage.CurrentValue = unitData.Damage.StartValue;
            unitData.Damage.Price = unitData.Damage.StartPrice;
            unitData.Damage.NumberDevelop = 0;
        }
    }

    private List<UnitProfile> LoadAllUnitProfile()
    {
        return Resources.LoadAll<UnitProfile>("Units").ToList();
    }
}
