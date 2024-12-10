using System.Collections.Generic;

public interface IModifierProvider
{
    public IEnumerable<float> GetAdditiveModifiers(EStat eStat);
    public IEnumerable<float> GetPercentageModifiers(EStat eStat);
}