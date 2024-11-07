using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Saving;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform _leftHandTransform, _rightHandTransform;
        [SerializeField] private WeaponSO _defaultWeaponSO;
        [SerializeField] private string _defaultWeaponName = "Unarmed";
        private WeaponSO _currentWeaponSO;

        private Health _target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            if (_currentWeaponSO == null)
            {
                EquipWeapon(_defaultWeaponSO);
            }
        }

        public Health GetTarget() => _target;

        public void EquipWeapon(WeaponSO weaponSO)
        {
            _currentWeaponSO = weaponSO;

            Animator animator = GetComponent<Animator>();

            weaponSO.SpawnWeapon(_leftHandTransform, _rightHandTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_target.IsDead) return;
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // Trigger Hit()
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (_target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(EStat.DAMAGE);
            if (_currentWeaponSO.HasProjectile())
            {
                _currentWeaponSO.LaunchProjectile(_leftHandTransform, _rightHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }
        public void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeaponSO.WeaponRange;
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            StopAttack();
            _target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            return _currentWeaponSO.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponSO weaponSO = Resources.Load<WeaponSO>(weaponName);
            EquipWeapon(weaponSO);
        }

        public IEnumerable<float> GetAdditiveModifier(EStat eStat)
        {
            if (eStat == EStat.DAMAGE)
            {
                yield return _currentWeaponSO.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(EStat eStat)
        {
            if (eStat == EStat.DAMAGE)
            {
                yield return _currentWeaponSO.PercentageBonus;
            }
        }
    }
}
