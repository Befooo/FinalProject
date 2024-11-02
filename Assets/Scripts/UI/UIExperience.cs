using System;
using TMPro;
using UnityEngine;

public class UIExperience : MonoBehaviour
{
    private Experience _experience;
    private TextMeshProUGUI _experienceValueTMP;

    private void Awake()
    {
        _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        _experienceValueTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _experienceValueTMP.text = String.Format("{0}", _experience.ExperiencePoints);
    }
}