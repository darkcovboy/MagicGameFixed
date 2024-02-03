using System.Linq;
using Hero;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class SimpleEnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _attackCooldown = 3f;
        [SerializeField] private float _cleavage = 0.5f;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private float _damage = 5f;

        private Transform _heroTransform;
        private float _currentAttackCooldown;
        private bool _isAttacking;
        private Collider[] _hits = new Collider[1];
        private int _layerMask;
        private bool _attackIsActive;
        public void Constructor(HeroTransform heroTransform, float damage, float cleaveage, float effectiveDistance)
        {
            _heroTransform = heroTransform.transform;
            _damage = damage;
            _cleavage = cleaveage;
            _effectiveDistance = effectiveDistance;
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            
            if(CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1f);
                hit.transform.GetComponent<HeroHealth>().TakeDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _currentAttackCooldown = _attackCooldown;
            _isAttacking = false;
        }

        public void DisableAttack() => _attackIsActive = false;

        public void EnableAttack() => _attackIsActive = true;

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * _effectiveDistance;
        }

        private bool CooldownIsUp() => _currentAttackCooldown <= 0;

        private bool CanAttack() => _attackIsActive && !_isAttacking && CooldownIsUp();

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _isAttacking = true;
        }
    }
}