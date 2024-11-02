using RPG.Saving;
using UnityEngine;

public class Experience : MonoBehaviour, ISaveable
{
    [SerializeField] private float _experiencePoints = 0;

    public void GainExperience(float experience)
    {
        _experiencePoints += experience;
    }

    public object CaptureState()
    {
        return _experiencePoints;
    }

    public void RestoreState(object state)
    {
        _experiencePoints = (float)state;
    }
}