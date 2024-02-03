using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private AgentMoveToPlayer _move;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private GameObject _deathFX;

        public event Action Happend;

        private void Start()
        {
            _enemyHealth.OnHealthChanged += DeathCheck;
        }

        private void OnDestroy()
        {
            _enemyHealth.OnHealthChanged -= DeathCheck;
        }

        private void DeathCheck(float current, float max)
        {
            if (current <= 0)
                Die();
        }

        private void Die()
        {
            _move.enabled = false;
            _enemyHealth.OnHealthChanged -= DeathCheck;
            _enemyAnimator.PlayDeath();
            SpawnDeathFx();
            StartCoroutine(DestroyTimer());
            Happend?.Invoke();
        }

        private void SpawnDeathFx()
        {
            Instantiate(_deathFX, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}