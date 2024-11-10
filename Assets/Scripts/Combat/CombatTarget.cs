using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable
    {
        public ECursorType eCursorType => ECursorType.COMBAT;

        public bool HandleRayCast(PlayerController playerController)
        {
            if (!playerController.GetComponent<Fighter>().CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(1)) playerController.GetComponent<Fighter>().Attack(gameObject);

            return true;
        }
    }
}
