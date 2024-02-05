using System;
using DG.Tweening;
using UnityEngine;

namespace Logic
{
    public class PopupAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        
        private void OnEnable()
        {
            StartAnimation();
        }

        private void StartAnimation()
        {
            transform.DOMove(transform.position + Vector3.up, _duration);
        }
    }
}