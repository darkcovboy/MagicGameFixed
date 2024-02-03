using Hero;
using UnityEngine;
using Zenject;

namespace UI
{
    public class HealthBarHero : HealthBar
    {
        private IHeroHealthChanged _healthChanged;
        
        [Inject]
        public void Constructor(IHeroHealthChanged heroHealthChanged)
        {
            _healthChanged = heroHealthChanged;
        }

        private void OnEnable()
        {
            _healthChanged.OnHealthChanged += HealthChanged;
        }

        private void OnDisable()
        {
            _healthChanged.OnHealthChanged -= HealthChanged;
        }
    }
}