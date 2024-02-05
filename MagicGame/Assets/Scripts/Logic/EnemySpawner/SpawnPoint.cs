using DefaultNamespace.Data;
using DefaultNamespace.Enemy;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Logic.EnemySpawner
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;

        private string _id;

        [SerializeField] private bool _slain;
        
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public bool Slain => _slain;
        
        public void Constructor(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }

        public void Initialize(string id, MonsterTypeId monsterTypeId)
        {
            _id = id;
            _monsterTypeId = monsterTypeId;
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
}