using UnityEngine;
using System;
using GameDevTV.Inventories;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("RPG/Inventory/DropLibrary"))]
public class DropLibrarySO : ScriptableObject
{
    [SerializeField] DropConfig[] potentialDrops;
    [SerializeField] private float[] dropChancePercentage;
    [SerializeField] private int[] minDrops;
    [SerializeField] private int[] maxDrops;

    public struct Dropped
    {
        public InventoryItem item;
        public int number;
    }

    public IEnumerable<Dropped> GetRandomDrops(int level)
    {
        if (!ShouldRanDomDrop(level)) yield break;

        for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
        {
            yield return GetRandomDrop(level);
        }
    }

    private bool ShouldRanDomDrop(int level)
    {
        return UnityEngine.Random.Range(0, 100) < GetByLevel(dropChancePercentage, level);
    }

    private int GetRandomNumberOfDrops(int level)
    {
        int min = GetByLevel(minDrops, level);
        int max = GetByLevel(maxDrops, level);

        return UnityEngine.Random.Range(min, max);
    }

    private Dropped GetRandomDrop(int level)
    {
        DropConfig dropConfig = SelectRandomItem(level);
        Dropped dropped = new Dropped();

        dropped.item = dropConfig.item;
        dropped.number = dropConfig.GetRandomNumber(level);

        return dropped;
    }

    private DropConfig SelectRandomItem(int level)
    {
        float totalChange = GetTotalChange(level);
        float randomRoll = UnityEngine.Random.Range(0, totalChange);
        float changeTotal = 0;

        foreach (DropConfig dropConfig in potentialDrops)
        {
            changeTotal += GetByLevel(dropConfig.relativeChance, level);

            if (totalChange > randomRoll) return dropConfig;
        }

        return null;
    }

    private float GetTotalChange(int level)
    {
        float total = 0;
        foreach (DropConfig dropConfig in potentialDrops)
        {
            total += GetByLevel(dropConfig.relativeChance, level);
        }

        return total;
    }

    static T GetByLevel<T>(T[] values, int level)
    {
        if (values.Length == 0) return default;
        if (level > values.Length) return values[values.Length - 1];
        if (level <= 0) return default;
        return values[level - 1];
    }


    [Serializable]
    class DropConfig
    {
        public EquipableItem item;
        public float[] relativeChance;
        public int[] minNumber;
        public int[] maxNumber;

        public int GetRandomNumber(int level)
        {
            if (!item.IsStackable())
            {
                return 1;
            }

            int min = GetByLevel(minNumber, level);
            int max = GetByLevel(maxNumber, level);

            return UnityEngine.Random.Range(min, max);
        }
    }
}