using System;
using TMPro;
using UnityEngine;

public class UILevel : MonoBehaviour
{
    private BaseStats _baseStats;
    private TextMeshProUGUI _healthValueTMP;

    private void Awake()
    {
        _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        _healthValueTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _healthValueTMP.text = String.Format("{0}", _baseStats.GetLevel());
    }
}