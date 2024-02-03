using DefaultNamespace.CameraLogic;
using Hero;
using Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform _playerPosition;
        [SerializeField] private CameraFollow _cameraCinemachine;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Constructor(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        public override void InstallBindings()
        {
            BindAndCreateHero();
            BindAndCreateHUD();
        }

        private void BindAndCreateHUD()
        {
            _gameFactory.CreateHud(Container);
        }

        private void BindAndCreateHero()
        {
            HeroMove hero = _gameFactory.CreateHero(_playerPosition.gameObject).GetComponent<HeroMove>();
            hero.BindCamera(_cameraCinemachine);
            hero.transform.parent = null;
            Container.Bind<HeroTransform>().FromInstance(hero.GetComponent<HeroTransform>()).AsSingle();
            HeroHealth health = hero.GetComponent<HeroHealth>();
            Container.Bind<IHeroHealthChanged>().To<HeroHealth>().FromInstance(health).AsSingle();
            Container.Bind<HeroMove>().FromInstance(hero).AsSingle();
        }
    }
}