using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private Transform _equippedWeaponPrefab;
    [SerializeField] private AnimatorOverrideController _overrideAnimatorController;
    [SerializeField] private float _weaponRange, _weaponDamage;
    [SerializeField] private EHandEquipWeapon _handToEquipWeapon;

    public float WeaponRange => _weaponRange;
    public float WeaponDamage => _weaponDamage;

    public void SpawnWeapon(Transform leftHand, Transform rightHand, Animator animator)
    {
        Transform handTransform = _handToEquipWeapon == EHandEquipWeapon.RIGHT_HAND ? rightHand : leftHand;

        Instantiate(_equippedWeaponPrefab, handTransform);
        animator.runtimeAnimatorController = _overrideAnimatorController;
    }
}