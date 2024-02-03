using UnityEngine;

namespace DefaultNamespace.Enemy
{
    [RequireComponent(typeof(SimpleEnemyAttack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private SimpleEnemyAttack _simpleEnemyAttack;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            _simpleEnemyAttack.DisableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            _simpleEnemyAttack.DisableAttack();
        }

        private void TriggerEnter(Collider obj)
        {
            _simpleEnemyAttack.EnableAttack();
        }
    }
}