using System;
using RPG.Core;
using TMPro;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    private Health _health;
    private TextMeshProUGUI _healthValueTMP;

    private void Awake()
    {
        _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        _healthValueTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _healthValueTMP.text = String.Format("{0:0}/{1:0}", _health.HealthPoints, _health.MaxHealthPoints);
    }
}