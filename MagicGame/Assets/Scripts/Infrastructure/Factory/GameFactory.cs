using System.Collections.Generic;
using DefaultNamespace.Enemy;
using Hero;
using Infrastructure.AssetManagement;
using Infrastructure.Services.PersistentProgress;
using Logic;
using Logic.EnemySpawner;
using StaticData;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private IAssetProvider _assets;
        private DiContainer _container;
        private IStaticDataService _staticDataService;
        private GameObject _hero;
        private IPersistentProgressService _progressService;


        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameFactory(IAssetProvider assets, IStaticDataService staticDataService, IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _progressService = persistentProgressService;
        }

        public GameObject CreateHero(GameObject at)
        {
            _hero = InstantiateRegistered(Paths.HeroPath, at.transform.position);
            return _hero;
        }


        public void CreateHud(DiContainer container)
        {
            GameObject hud =InstantiateRegistered(container, Paths.HudPath);
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void CreateSpawner(Vector3 at, string enemySpawnerID, MonsterTypeId monsterTypeId)
        {
            var spawner = InstantiateRegistered(Paths.Spawner, at)
                .GetComponent<SpawnPoint>();
            
            spawner.Constructor(this);
            spawner.Initialize(enemySpawnerID, monsterTypeId);
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData data = _staticDataService.ForMonster(typeId);
            GameObject monster = Object.Instantiate(data.Prefab, parent.position, Quaternion.identity, parent);
            IHealth health = monster.GetComponent<IHealth>();
            Debug.Log(data.Health);
            health.Current = data.Health;
            health.MAX = data.Health;
            HeroTransform heroTransform = _hero.GetComponent<HeroTransform>();
            monster.GetComponent<AgentMoveToPlayer>().Constructor(heroTransform);
            var attack = monster.GetComponent<SimpleEnemyAttack>();
            attack.Constructor(heroTransform, data.Damage, data.Cleavage, data.EffectiveDistance);
            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this);
            lootSpawner.SetLoot(data.MINLoot, data.MAXLoot);
            return monster;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(Paths.Loot).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData, this);
            return lootPiece;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(DiContainer container,string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(container,prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject prefab)
        {
            foreach (ISavedProgressReader progressReader in prefab.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if(progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }
    }
}