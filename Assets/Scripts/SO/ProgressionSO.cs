using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Progression", fileName = "ProgressionSO")]
public class ProgressionSO : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClasses;
    private Dictionary<ECharacterClass, Dictionary<EStat, float[]>> _lookupTable = null;

    public float GetStat(EStat stat, ECharacterClass characterClass, int level)
    {
        BuildLookUp();

        float[] values = _lookupTable[characterClass][stat];

        if (values.Length < level) return 0;

        return values[level - 1];
    }

    private void BuildLookUp()
    {
        if (_lookupTable != null) return;

        _lookupTable = new Dictionary<ECharacterClass, Dictionary<EStat, float[]>>();

        foreach (ProgressionCharacterClass progressionClass in _progressionCharacterClasses)
        {
            Dictionary<EStat, float[]> statLookUpTable = new Dictionary<EStat, float[]>();

            foreach (ProgressionStat progressionStat in progressionClass.Stats)
            {
                statLookUpTable[progressionStat.Stat] = progressionStat.Values;
            }

            _lookupTable[progressionClass.CharacterClass] = statLookUpTable;
        }
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