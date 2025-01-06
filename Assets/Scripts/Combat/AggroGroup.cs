using UnityEngine;
using UnityEngine.Timeline;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activeOnStart = false;

        private void Start()
        {
            Activate(activeOnStart);
        }

        public void Activate(bool shouldActive)
        {
            foreach (Fighter fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = shouldActive;
                }
                fighter.enabled = shouldActive;
            }
        }
    }
}