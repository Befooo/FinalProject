using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private Transform _equippedWeaponPrefab;
    [SerializeField] private AnimatorOverrideController _overrideAnimatorController;
    [SerializeField] private float _weaponRange, _weaponDamage;

    public float WeaponRange => _weaponRange;
    public float WeaponDamage => _weaponDamage;

    public void SpawnWeapon(Transform transform, Animator animator)
    {
        Instantiate(_equippedWeaponPrefab, transform);
        animator.runtimeAnimatorController = _overrideAnimatorController;
    }
}