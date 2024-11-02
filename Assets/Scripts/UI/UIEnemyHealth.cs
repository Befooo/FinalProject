using System;
using RPG.Combat;
using RPG.Core;
using TMPro;
using UnityEngine;

public class UIEnemyHealth : MonoBehaviour
{
    private Fighter _fighter;
    private TextMeshProUGUI _healthValueTMP;

    private void Awake()
    {
        _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        _healthValueTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_fighter.GetTarget() == null) { _healthValueTMP.text = "N/A"; return; }

        Health health = _fighter.GetTarget();
        _healthValueTMP.text = String.Format("{0:0}%", health.GetPercentage() * 100);
    }
}