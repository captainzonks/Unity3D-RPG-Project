using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] public float weaponRange = 2f;
        [SerializeField] public float timeBetweenAttacks = 1f;
        [SerializeField] public float weaponDamage = 5f;

        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private Animator _animator;
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;

            if (_target.IsDead()) return;

            if (!IsInRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if (!(_timeSinceLastAttack > timeBetweenAttacks)) return;
            // trigger Hit() event
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(StopAttack);
            _animator.SetTrigger(Attack1);
        }

        // Animation Event
        private void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            return !combatTarget.GetComponent<Health>().IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            _animator.ResetTrigger(Attack1);
            _animator.SetTrigger(StopAttack);
            _target = null;
        }
    }

}