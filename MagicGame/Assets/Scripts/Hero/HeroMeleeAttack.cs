using System;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Enemy;
using Infrastructure.Services.PersistentProgress;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class HeroMeleeAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private StarterAssetsInputs _playerInput;
        [SerializeField] private GameObject _slashPrefab;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_playerInput.meleeAttack)
            {
                _heroAnimator.PlayMeleeAttack();
                _playerInput.meleeAttack = false;
            }
                
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private int Hit()
        {
            PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 2f);
            return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits,
                _layerMask);
        }

        private Vector3 StartPoint() => new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);
    }
}