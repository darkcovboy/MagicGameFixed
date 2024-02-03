using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MinVelocity = 0.1f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _enemyAnimator;

        private void OnValidate()
        {
            if (!_agent)
                _agent = GetComponent<NavMeshAgent>();

            if (!_enemyAnimator)
                _enemyAnimator = GetComponent<EnemyAnimator>();
        }

        private void Update()
        {
            if(ShouldMove())
                _enemyAnimator.Move(_agent.velocity.magnitude);
            else
                _enemyAnimator.StopMoving();
        }

        private bool ShouldMove() => _agent.velocity.magnitude > MinVelocity && _agent.remainingDistance > _agent.radius;
    }
}