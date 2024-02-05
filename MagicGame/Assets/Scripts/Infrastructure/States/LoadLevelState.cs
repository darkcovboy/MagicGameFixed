using System;
using Data;
using DefaultNamespace.CameraLogic;
using DefaultNamespace.Data;
using DefaultNamespace.Enemy;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Loading;
using Logic;
using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private const string InitialPoint = "InitialPoint";
        private const string EnemySpawnerTag = "EnemySpawner";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = persistentProgressService;
            _staticData = staticDataService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }
        
        public void Exit() =>
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            InitSpawners();
            InitUncollectedLoot();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUncollectedLoot()
        {
            if (_progressService.Progress.UncollectedLoot.UntakedLoot.Count > 0)
            {
                foreach (LootSaveData lootSaveData in _progressService.Progress.UncollectedLoot.UntakedLoot)
                {
                    LootPiece loot = _gameFactory.CreateLoot();
                    loot.Initialize(lootSaveData.Loot);
                    loot.transform.position = lootSaveData.Vector3.AsUnityVector();
                }
            }
            
        }

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneKey);

            foreach (EnemySpawnerData enemySpawner in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(enemySpawner.Position, enemySpawner.ID, enemySpawner.MonsterType);
            }
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }
    }
}