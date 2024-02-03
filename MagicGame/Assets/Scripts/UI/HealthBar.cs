using System;
using Hero;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void HealthChanged(float current, float max) => _image.fillAmount = (current / max);
    }
}