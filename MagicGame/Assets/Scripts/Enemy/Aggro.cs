using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private AgentMoveToPlayer _follow;
        [SerializeField] private float _cooldown;
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
            
            SwitchFollowOff();
        }

        private void OnTriggerEnter(Collider obj)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopAggroCoroutine();
            
                SwitchFollowOn();
            }
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
            {
                StopCoroutine(_aggroCoroutine);
                _aggroCoroutine = null;
            }
        }

        private void OnTriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;
                _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
            }
        }

        private IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            
            SwitchFollowOff();
        }

        private void SwitchFollowOff() => _follow.enabled = false;

        private void SwitchFollowOn() => _follow.enabled = true;
    }
}