using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [FormerlySerializedAs("health")] float healthPoints = -1f;
        private bool isDead = false;
        public bool IsDead => isDead;

        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(EStat.HEALTH);
            }
        }

        public float GetPercentage()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(EStat.HEALTH);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
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
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
