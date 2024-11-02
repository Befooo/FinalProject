using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    private const string WEAPON_NAME = "Weapon";
    [SerializeField] private Transform _equippedWeaponPrefab;
    [SerializeField] private AnimatorOverrideController _overrideAnimatorController;
    [SerializeField] private float _weaponRange, _weaponDamage;
    [SerializeField] private EHandEquipWeapon _handToEquipWeapon;
    [SerializeField] private Projectile _projectilePrefab = null;

    public float WeaponRange => _weaponRange;
    public float WeaponDamage => _weaponDamage;

    public void SpawnWeapon(Transform leftHand, Transform rightHand, Animator animator)
    {
        DestroyOldWeapon(leftHand, rightHand);

        Transform weaponClone = Instantiate(_equippedWeaponPrefab, GetHandTransform(leftHand, rightHand));


        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (_overrideAnimatorController != null)
        {
            animator.runtimeAnimatorController = _overrideAnimatorController;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        weaponClone.name = WEAPON_NAME;
    }

    private void DestroyOldWeapon(Transform leftHand, Transform rightHand)
    {
        Transform oldWeapon = rightHand.Find(WEAPON_NAME);
        if (oldWeapon == null) oldWeapon = leftHand.Find(WEAPON_NAME);
        if (oldWeapon == null) return;

        Destroy(oldWeapon.gameObject);
    }

    private Transform GetHandTransform(Transform leftHand, Transform rightHand)
    {
        Transform handTransform = _handToEquipWeapon == EHandEquipWeapon.RIGHT_HAND ? rightHand : leftHand;
        return handTransform;
    }

    public bool HasProjectile() => _projectilePrefab != null;

    public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target, GameObject instigator)
    {
        Projectile projectileClone = Instantiate(_projectilePrefab, GetHandTransform(leftHand, rightHand).position, Quaternion.identity);
        projectileClone.SetTargetHealth(target, instigator, _weaponDamage);
    }
}