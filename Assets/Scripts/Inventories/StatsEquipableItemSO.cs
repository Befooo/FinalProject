using GameDevTV.Inventories;
using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
public class StatsEquipableItemSO : EquipableItem, IModifierProvider
{
    [SerializeField]
    Modifier[] additiveModifiers;
    [SerializeField]
    Modifier[] percentageModifiers;

    [Serializable]
    struct Modifier
    {
        public EStat eStat;
        public float value;
    }

    public IEnumerable<float> GetAdditiveModifiers(EStat eStat)
    {
        foreach (Modifier modifier in additiveModifiers)
        {
            if (modifier.eStat == eStat)
            {
                yield return modifier.value;
            }
        }
    }

    public IEnumerable<float> GetPercentageModifiers(EStat eStat)
    {
        foreach (Modifier modifier in percentageModifiers)
        {
            if (modifier.eStat == eStat)
            {
                yield return modifier.value;
            }
        }
    }
}