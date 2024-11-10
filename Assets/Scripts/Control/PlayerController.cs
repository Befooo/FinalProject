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
    public enum ECursorType { NONE, MOVEMENT, COMBAT, UI, PICK_UP }

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
        [SerializeField] private float _maxPathLength = 40.0f;
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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(ECursorType.UI);
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
            bool hasHit = RayCastNavMesh(out Vector3 hit);
            if (hasHit)
            {
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

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > _maxPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;

            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
