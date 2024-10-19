using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private AnimatorOverrideController _weaponOverrideAnimator;
    [SerializeField] private GameObject _equippedPrefab;
    [SerializeField] private float _weaponDamage;
    [SerializeField] private float _weaponRange;

    public float WeaponDamage => _weaponDamage;
    public float WeaponRange => _weaponRange;

    public void Spawn(Transform handTransform, Animator animator)
    {
        if (_equippedPrefab != null)
        {
            Instantiate(_equippedPrefab, handTransform);
        }

        if (_weaponOverrideAnimator != null)
        {
            animator.runtimeAnimatorController = _weaponOverrideAnimator;
        }
    }
}
