using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float _experiencePoints = 0;

    public void GainExperience(float experience)
    {
        _experiencePoints += experience;
    }
}