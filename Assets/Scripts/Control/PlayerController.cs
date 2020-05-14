using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0)) _fighter.Attack(target.gameObject);

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);

            if (!hasHit) return false;
            if (Input.GetMouseButton(0))
            {
                _mover.StartMoveAction(hit.point);
            }

            return true;

        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}