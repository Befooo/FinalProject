using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 99)]
    [SerializeField] private int _startingLevel = 1;
    [SerializeField] private ECharacterClass _characterClass;
    [SerializeField] private ProgressionSO _progressionSO;

    public float GetStat(EStat stat) => _progressionSO.GetStat(stat, _characterClass, _startingLevel);

    public int GetLevel()
    {
        if (!TryGetComponent(out Experience experience)) return _startingLevel;

        float currentXP = experience.ExperiencePoints;
        int penultimateLevel = _progressionSO.GetLevels(EStat.EXPERIENCE_TO_LEVEL_UP, _characterClass);

        for (int i = 1; i <= penultimateLevel; i++)
        {
            float XPToLevelUp = _progressionSO.GetStat(EStat.EXPERIENCE_TO_LEVEL_UP, _characterClass, i);

            if (XPToLevelUp > currentXP) return i;
        }

        return penultimateLevel;
    }
}