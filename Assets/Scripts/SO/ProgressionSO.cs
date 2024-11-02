using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Progression", fileName = "ProgressionSO")]
public class ProgressionSO : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClasses;

    public float GetHealth(ECharacterClass characterClass, int level)
    {
        foreach (ProgressionCharacterClass progressionClass in _progressionCharacterClasses)
        {
            if (progressionClass.CharacterClass == characterClass) return progressionClass.Healths[level - 1];
        }

        return 0;
    }

    [Serializable]
    class ProgressionCharacterClass
    {
        [SerializeField] private ECharacterClass _characterClass;
        [SerializeField] private float[] _healths;

        public ECharacterClass CharacterClass => _characterClass;
        public float[] Healths => _healths;
    }
}