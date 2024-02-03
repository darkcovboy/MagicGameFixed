using System;
using UI;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;

        [SerializeField] private float _current;
        [SerializeField] private float _max;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float MAX
        {
            get => _max;
            set => _max = value;
        }

        public event Action<float, float> OnHealthChanged;

        public void TakeDamage(float damage)
        {
            Current -= damage;
            _enemyAnimator.PlayHit();
            OnHealthChanged?.Invoke(Current, MAX);
        }
    }
}