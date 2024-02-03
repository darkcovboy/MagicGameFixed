using System;

namespace Hero
{
    public interface IHeroHealthChanged
    {
        event Action<float, float> OnHealthChanged;
    }
}