using System;
using DefaultNamespace.Enemy;
using UnityEngine;

namespace UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private HealthBarEnemy _healthBarEnemy;

        private void Start()
        {
            _enemyHealth.OnHealthChanged += _healthBarEnemy.HealthChanged;
            _healthBarEnemy.HealthChanged(_enemyHealth.Current, _enemyHealth.MAX);
        }

        private void OnDisable()
        {
            _enemyHealth.OnHealthChanged -= _healthBarEnemy.HealthChanged;
        }
    }
}