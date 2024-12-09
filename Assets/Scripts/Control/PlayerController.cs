using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;

namespace RPG.Control
{
    public enum ECursorType { NONE, MOVEMENT, COMBAT, UI, PICK_UP, FULL_PICK_UP }

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
        [SerializeField] private float _maxNavMeshProjectionDistance = 1.0f;
        [SerializeField] private float _raycastRadius = 1.0f;

        bool isDraggingUI = false;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead) { SetCursor(ECursorType.NONE); return; }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(ECursorType.NONE);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                IRayCastable[] raycastables = hit.transform.GetComponents<IRayCastable>();

                foreach (IRayCastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.eCursorType);
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), _raycastRadius);
            float[] distances = new float[hits.Length];
            distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0)) isDraggingUI = false;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) isDraggingUI = true;

                SetCursor(ECursorType.UI);
                return true;
            }

            if (isDraggingUI) return true;

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
            bool hasHit = RayCastNavMesh(out Vector3 hit);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(hit)) return false;

                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().StartMoveAction(hit, 1f);
                }
                SetCursor(ECursorType.MOVEMENT);
                return true;
            }
            return false;
        }

        private bool RayCastNavMesh(out Vector3 target)
        {
            target = Vector3.zero;

            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
