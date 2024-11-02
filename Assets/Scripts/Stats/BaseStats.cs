using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 99)]
    [SerializeField] private int _startingLevel = 1;
    [SerializeField] private ECharacterClass _characterClass;
    [SerializeField] private ProgressionSO _progressionSO;

    public float GetStat(EStat stat) => _progressionSO.GetStat(stat, _characterClass, _startingLevel);

    public float GetExperienceReward()
    {
        return 10;
    }
}