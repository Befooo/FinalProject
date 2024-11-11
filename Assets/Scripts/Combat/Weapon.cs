using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHit;

    public void OnHit()
    {
        _onHit?.Invoke();
    }
}