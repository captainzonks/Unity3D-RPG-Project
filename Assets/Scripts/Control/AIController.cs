using Combat;
using Core;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] public float chaseDistance = 5f;
        private Fighter _fighter;
        private Health _health;
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if(InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            var distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}