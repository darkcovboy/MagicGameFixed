using System;
using Hero;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace DefaultNamespace.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 0.5f;
        
        [SerializeField] private NavMeshAgent _agent;

        private Transform _heroTransformPosition;
        
        public void Constructor(HeroTransform heroMove)
        {
            _heroTransformPosition = heroMove.transform;
        }
        
        private void Update()
        {
            if (PlayerNotReached())
                _agent.destination = _heroTransformPosition.position;
        }

        private bool PlayerNotReached() => Vector3.Distance(_agent.transform.position, _heroTransformPosition.position) >= MinimalDistance;
    }
}