using System;
using GameDevTV.Saving;
using UnityEngine;

public class Experience : MonoBehaviour, ISaveable
{
    [SerializeField] private float _experiencePoints = 0;
    public float ExperiencePoints => _experiencePoints;

    public event Action OnExperienceGained;

    public void GainExperience(float experience)
    {
        _experiencePoints += experience;
        OnExperienceGained?.Invoke();
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