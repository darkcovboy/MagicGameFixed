using DefaultNamespace.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        public ISavedLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService, ISavedLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState,string>(_persistentProgressService.Progress.WorldData.PositionOnLevel.Level);
        }
        
        private void LoadProgressOrInitNew()
        {
            _persistentProgressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress("Main");

            progress.WorldData.LootData.Collected = 0;
            progress.HeroState.MaxHealth = 50;
            progress.HeroStats.Damage = 20f;
            progress.HeroStats.DamageRadius = 1f;
            progress.HeroState.ResetHealth();
            
            return progress;
        }
    }
}