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
    [SerializeField] private bool _isRightHanded;

    public float WeaponDamage => _weaponDamage;
    public float WeaponRange => _weaponRange;

    public void Spawn(Transform leftHand, Transform rightHand, Animator animator)
    {
        if (_equippedPrefab != null)
        {
            Transform handTransform = _isRightHanded ? rightHand : leftHand;
            Instantiate(_equippedPrefab, handTransform);
        }

        if (_weaponOverrideAnimator != null)
        {
            animator.runtimeAnimatorController = _weaponOverrideAnimator;
        }
    }
}
