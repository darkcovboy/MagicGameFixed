using System;
using Logic;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        [SerializeField] private string _id;
        [SerializeField] private MonsterTypeId _monsterType;
        [SerializeField] private Vector3 _position;

        public EnemySpawnerData(string id, MonsterTypeId monsterType, Vector3 position)
        {
            _id = id;
            _monsterType = monsterType;
            _position = position;
        }

        public string ID => _id;

        public MonsterTypeId MonsterType => _monsterType;

        public Vector3 Position => _position;
    }
}