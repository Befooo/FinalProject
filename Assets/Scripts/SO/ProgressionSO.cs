using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Progression", fileName = "ProgressionSO")]
public class ProgressionSO : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClasses;

    public float GetStat(EStat stat, ECharacterClass characterClass, int level)
    {
        foreach (ProgressionCharacterClass progressionClass in _progressionCharacterClasses)
        {
            if (progressionClass.CharacterClass != characterClass) continue;

            foreach (ProgressionStat progressionStat in progressionClass.Stats)
            {
                if (progressionStat.Stat != stat) continue;
                if (progressionStat.Values.Length < level) continue;

                return progressionStat.Values[level - 1];
            }
        }

        return 0;
    }

    [Serializable]
    class ProgressionCharacterClass
    {
        [SerializeField] private ECharacterClass _characterClass;
        [SerializeField] private ProgressionStat[] _stats;

        public ECharacterClass CharacterClass => _characterClass;
        public ProgressionStat[] Stats => _stats;
    }


    [Serializable]
    class ProgressionStat
    {
        [SerializeField] private EStat _stat;
        [SerializeField] private float[] _values;

        public float[] Values => _values;
        public EStat Stat => _stat;
    }
}