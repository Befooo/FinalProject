using System;
using TMPro;
using UnityEngine;

public class UIDamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void DestroyText()
    {
        Destroy(this.gameObject);
    }

    public void SetValue(float amount)
    {
        _textMeshProUGUI.text = String.Format("{0}", amount); ;
    }
}