using System.Collections.Generic;
using DefaultNamespace.Enemy;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud(DiContainer container);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
        void Register(ISavedProgressReader progressReader);
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform transform);
        LootPiece CreateLoot();
    }
}