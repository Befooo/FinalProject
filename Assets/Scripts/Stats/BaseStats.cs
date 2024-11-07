using System;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 99)]
    [SerializeField] private int _startingLevel = 1;
    [SerializeField] private ECharacterClass _characterClass;
    [SerializeField] private ProgressionSO _progressionSO;
    [SerializeField] private GameObject _levelUpVFX;
    [SerializeField] private bool _shouldUseModifiers = false;

    public event Action OnLevelUp;

    private int _currentLevel;
    public int CurrentLevel
    {
        get
        {
            if (_currentLevel < 1) _currentLevel = CalculateLevel();
            return _currentLevel;
        }

        private set
        {
            _currentLevel = value;
        }
    }

    private void Start()
    {
        CurrentLevel = CalculateLevel();
        ListenExperienceStat();
    }

    public float GetStat(EStat stat) => GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat) / 100);

    private float GetPercentageModifier(EStat eStat)
    {
        if (!_shouldUseModifiers) return 0;

        float total = 0;

        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float modifiers in provider.GetPercentageModifiers(eStat))
            {
                total += modifiers;
            }
        }

        return total;
    }

    private float GetBaseStat(EStat stat) => _progressionSO.GetStat(stat, _characterClass, CurrentLevel);

    private float GetAdditiveModifier(EStat stat)
    {
        if (!_shouldUseModifiers) return 0;

        float total = 0;

        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float modifiers in provider.GetAdditiveModifier(stat))
            {
                total += modifiers;
            }
        }

        return total;
    }

    public int CalculateLevel()
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

    private void ListenExperienceStat()
    {
        if (!TryGetComponent(out Experience experience)) return;

        experience.OnExperienceGained += UpdateLevel;
    }

    private void UpdateLevel()
    {
        int newLevel = CalculateLevel();
        if (newLevel > CurrentLevel)
        {
            CurrentLevel = newLevel;
            LevelUpEffect();
            OnLevelUp?.Invoke();
        }
    }

    private void LevelUpEffect()
    {
        Instantiate(_levelUpVFX, transform);
    }
}