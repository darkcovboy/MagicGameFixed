using System;
using DefaultNamespace.Data;
using DefaultNamespace.Enemy;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHeroHealthChanged, IHealth
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        
        private State _state;
        public event Action<float, float> OnHealthChanged;

        public float Current
        {
            get => _state.CurrentHealth;
            set => _state.CurrentHealth = value;
        }

        public float MAX
        {
            get => _state.MaxHealth;
            set => _state.MaxHealth = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            OnHealthChanged?.Invoke(Current,MAX);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHealth = Current;
            progress.HeroState.MaxHealth = MAX;
        }

        public void TakeDamage(float damage)
        {
            if(damage < 0)
                throw new ArgumentException(nameof(damage));

            Debug.Log(damage);
            Current -= damage;
            OnHealthChanged?.Invoke(Current, MAX);
            _heroAnimator.PlayHit();
        }
    }
}