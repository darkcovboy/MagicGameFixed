using System;
using DefaultNamespace.Data;
using DefaultNamespace.Enemy;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;

        private string _id;

        [SerializeField] private bool _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public bool Slain => _slain;

        [Inject]
        public void Constructor(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }

        private void Awake()
        {
            _id = GetComponent<UniqieId>().Id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slain = true;
            else
                Spawn();
        }

        private void Spawn()
        {
           GameObject monster = _factory.CreateMonster(_monsterTypeId, transform);
           _enemyDeath = monster.GetComponent<EnemyDeath>();
           _enemyDeath.Happend += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null) _enemyDeath.Happend -= Slay;
            _slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }

    public enum MonsterTypeId
    {
        Chest = 0,
        Piska = 10
    }
}