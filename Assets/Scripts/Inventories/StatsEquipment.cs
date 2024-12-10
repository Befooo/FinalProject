using System.Collections.Generic;
using GameDevTV.Inventories;

public class StatsEquipment : Equipment, IModifierProvider
{
    public IEnumerable<float> GetAdditiveModifiers(EStat eStat)
    {
        foreach (EquipLocation slot in GetAllPopulatedSlots())
        {
            IModifierProvider item = GetItemInSlot(slot) as IModifierProvider;
            if (item == null) continue;

            foreach (float modifier in item.GetAdditiveModifiers(eStat))
            {
                yield return modifier;
            }
        }
    }

    public IEnumerable<float> GetPercentageModifiers(EStat eStat)
    {
        foreach (EquipLocation slot in GetAllPopulatedSlots())
        {
            IModifierProvider item = GetItemInSlot(slot) as IModifierProvider;
            if (item == null) continue;

            foreach (float modifier in item.GetPercentageModifiers(eStat))
            {
                yield return modifier;
            }
        }
    }
}