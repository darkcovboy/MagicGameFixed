using System;
using ExternalPropertyAttributes;
using Logic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;
        [SerializeField, Min(1)] private int _health = 1;
        [SerializeField, Min(1f)] private float _damage = 1f;
        [SerializeField, Min(1)] private int _minLoot;
        [SerializeField, Min(1)] private int _maxLoot;
        [SerializeField, Range(0.5f, 1f)] private float _effectiveDistance = 0.5f;
        [SerializeField, Range(0.5f, 1f)] private float _cleavage = 0.5f;
        [SerializeField] private GameObject _prefab;

        private void OnValidate()
        {
            if(_minLoot > _maxLoot)
                _maxLoot = (_minLoot+1);
        }

        public int Health => _health;

        public float Damage => _damage;

        public float EffectiveDistance => _effectiveDistance;

        public float Cleavage => _cleavage;

        public GameObject Prefab => _prefab;
        
        public MonsterTypeId MonsterTypeId => _monsterTypeId;

        public int MAXLoot => _maxLoot;

        public int MINLoot => _minLoot;
    }
}