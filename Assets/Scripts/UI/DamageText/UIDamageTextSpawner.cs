using UnityEngine;

public class UIDamageTextSpawner : MonoBehaviour
{
    [SerializeField] private UIDamageText _damageTextPrefab;

    public void Spawn(float damageAmount)
    {
        UIDamageText damageTextClone = Instantiate(_damageTextPrefab, transform);
        damageTextClone.SetValue(damageAmount);
    }
}