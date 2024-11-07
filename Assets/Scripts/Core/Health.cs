using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70f;
        [FormerlySerializedAs("health")] private float _healthPoints = -1f;
        private bool isDead = false;
        public bool IsDead => isDead;

        public float HealthPoints => _healthPoints;
        public float MaxHealthPoints => GetComponent<BaseStats>().GetStat(EStat.HEALTH);

        private void Start()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
            if (_healthPoints < 0)
            {
                _healthPoints = GetComponent<BaseStats>().GetStat(EStat.HEALTH);
            }
        }

        private void RegenerateHealth()
        {
            float regeHealthPoints = MaxHealthPoints * (regenerationPercentage / 100);
            _healthPoints = Mathf.Max(_healthPoints, regeHealthPoints);
        }

        public float GetPercentage()
        {
            return _healthPoints / MaxHealthPoints;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            if (!instigator.TryGetComponent(out Experience experience)) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(EStat.EXPERIENCE_REWARD));
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;
            if (_healthPoints == 0)
            {
                Die();
            }
        }
    }
}
