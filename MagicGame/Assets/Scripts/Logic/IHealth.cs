using System;

namespace DefaultNamespace.Enemy
{
    public interface IHealth
    {
        float Current { get; set; }
        float MAX { get; set; }
        event Action<float, float> OnHealthChanged;
        void TakeDamage(float damage);
    }
}