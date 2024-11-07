using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System.Security.Cryptography;

namespace RPG.Control
{
    public enum ECursorType { NONE, MOVEMENT, COMBAT }

    [Serializable]
    struct CursorMapping
    {
        public ECursorType cursorType;
        public Texture2D texture;
        public Vector2 hotspot;
    }
    public class PlayerController : MonoBehaviour
    {
        Health health;
        [SerializeField] private CursorMapping[] _cursorMappings = null;
        private void Awake()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(ECursorType.NONE);
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetCursor(ECursorType.COMBAT);
                return true;
            }
            return false;
        }

        private void SetCursor(ECursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(ECursorType cursorType)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if (mapping.cursorType == cursorType) return mapping;
            }

            return _cursorMappings[0];
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(ECursorType.MOVEMENT);
                return true;
            }
            return false;
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
