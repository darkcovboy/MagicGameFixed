using System;
using Unity.Mathematics;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(HeroHealth))]
    [RequireComponent(typeof(HeroMove))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth _health;
        [SerializeField] private HeroMove _move;
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private HeroMeleeAttack _heroMeleeAttack;
        [SerializeField] private GameObject _deathFx;
        private bool _isDead = false;

        private void Start() => _health.OnHealthChanged += HealthChanged;

        private void OnDestroy() => _health.OnHealthChanged -= HealthChanged;

        private void HealthChanged(float current, float max)
        {
            if (current <= 0)
                Die();
        }

        private void Die()
        {
            if (!_isDead)
            {
                _isDead = true;
                _move.enabled = false;
                _heroMeleeAttack.enabled = false;
                _heroAnimator.PlayDeath();
                Instantiate(_deathFx, transform.position, quaternion.identity);
            }
            
        }
    }
}