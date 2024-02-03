using Data;
using DefaultNamespace.Data;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISavedLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
            
            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() => PlayerPrefs.GetString(ProgressKey)?
            .ToDeserialized<PlayerProgress>();
    }
}