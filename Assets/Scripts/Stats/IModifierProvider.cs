using System.Collections.Generic;

public interface IModifierProvider
{
    public IEnumerable<float> GetAdditiveModifier(EStat eStat);
    public IEnumerable<float> GetPercentageModifiers(EStat eStat);
}