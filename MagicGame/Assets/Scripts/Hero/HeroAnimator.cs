using System;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(Animator))]
    public class HeroAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
        private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIDJump = Animator.StringToHash("Jump");
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDHit = Animator.StringToHash("GetHit");
        private readonly int _animIDDeath = Animator.StringToHash("Death");
        private readonly int _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");

        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(_moveSpeed, speed);
        }

        public void SetGrounded(bool isGrounded)
        {
            _animator.SetBool(_animIDGrounded, isGrounded);
        }

        public void SetJump(bool isJumping)
        {
            _animator.SetBool(_animIDJump, isJumping);
        }

        public void SetFreeFall(bool isFreeFalling)
        {
            _animator.SetBool(_animIDFreeFall, isFreeFalling);
        }

        public void PlayHit()
        {
            _animator.SetTrigger(_animIDHit);
        }

        public void PlayDeath()
        {
            _animator.SetTrigger(_animIDDeath);
        }

        public void PlayMeleeAttack()
        {
            _animator.SetTrigger(_animIDMeleeAttack);
        }
    }
}