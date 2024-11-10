using RPG.Core;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Health _healthComponent = null;
    [SerializeField] private RectTransform _foreground = null;
    [SerializeField] private Canvas _root;

    private void Update()
    {
        if (Mathf.Approximately(_healthComponent.GetPercentage(), 0) || Mathf.Approximately(_healthComponent.GetPercentage(), 1)) { _root.enabled = false; return; }

        _foreground.localScale = new Vector3(_healthComponent.GetPercentage(), 1, 1);
        _root.enabled = true;
    }

}