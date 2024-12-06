using UnityEngine;
using UnityEngine.Serialization;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70f;
        [SerializeField] private TakeDamageEvent _takeDamage;
        [SerializeField] private UnityEvent _onDie;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        [FormerlySerializedAs("health")] private LazyValue<float> _healthPoints;
        private bool isDead = false;
        public bool IsDead => isDead;

        public float HealthPoints => _healthPoints.value;
        public float MaxHealthPoints => GetComponent<BaseStats>().GetStat(EStat.HEALTH);

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(EStat.HEALTH);
        }

        private void Start()
        {
            _healthPoints.ForceInit();

            if (_healthPoints.value < 0)
            {
                _healthPoints.value = GetComponent<BaseStats>().GetStat(EStat.HEALTH);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regeHealthPoints = MaxHealthPoints * (regenerationPercentage / 100);
            _healthPoints.value = Mathf.Max(_healthPoints.value, regeHealthPoints);
        }

        public float GetPercentage()
        {
            return _healthPoints.value / MaxHealthPoints;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            if (_healthPoints.value == 0)
            {
                _onDie?.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                _takeDamage?.Invoke(damage);
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
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;
            if (_healthPoints.value == 0)
            {
                Die();
            }
        }

        public void Heal(float heal)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value + heal, MaxHealthPoints);
        }
    }
}
