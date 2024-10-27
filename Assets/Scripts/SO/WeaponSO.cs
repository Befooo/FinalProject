using RPG.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
public class WeaponSO : ScriptableObject
{
    private const string WEAPON_NAME = "Weapon";

    [SerializeField] private AnimatorOverrideController _weaponOverrideAnimator;
    [SerializeField] private GameObject _equippedPrefab;
    [SerializeField] private float _weaponDamage;
    [SerializeField] private float _weaponRange;
    [SerializeField] private bool _isRightHanded;
    [SerializeField] private Projectile _projectile = null;

    public float WeaponDamage => _weaponDamage;
    public float WeaponRange => _weaponRange;

    public void Spawn(Transform leftHand, Transform rightHand, Animator animator)
    {
        DestroyOldWeapon(rightHand, leftHand);

        if (_equippedPrefab != null)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);
            GameObject weaponClone = Instantiate(_equippedPrefab, handTransform);
            weaponClone.name = WEAPON_NAME;
        }

        if (_weaponOverrideAnimator != null)
        {
            animator.runtimeAnimatorController = _weaponOverrideAnimator;
        }
    }

    private void DestroyOldWeapon(Transform leftHand, Transform rightHand)
    {
        Transform oldWeaponLeftHand = leftHand.Find(WEAPON_NAME);
        Transform oldWeaponRightHand = rightHand.Find(WEAPON_NAME);

        if (oldWeaponRightHand != null) Destroy(oldWeaponRightHand.gameObject);
        if (oldWeaponLeftHand != null) Destroy(oldWeaponLeftHand.gameObject);
    }

    private Transform GetTransform(Transform rightHand, Transform leftHand)
    {
        return _isRightHanded ? rightHand : leftHand;
    }

    public bool HasProjectile() => _projectile != null;

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
    {
        Projectile projectileClone = Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
        projectileClone.SetTarget(target, _weaponDamage);
    }
}
