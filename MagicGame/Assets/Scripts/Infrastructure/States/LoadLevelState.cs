using System;
using DefaultNamespace.CameraLogic;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Loading;
using Logic;
using UnityEngine;
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
        private IPersistentProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = persistentProgressService;
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
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitSpawners()
        {
            foreach (var spawnerObject in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
            {
                var spawner = spawnerObject.GetComponent<EnemySpawner>();
                _gameFactory.Register(spawner);
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