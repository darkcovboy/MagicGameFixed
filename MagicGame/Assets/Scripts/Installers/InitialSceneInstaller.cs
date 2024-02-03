using Infrastructure;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.States;
using Loading;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class InitialSceneInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindGameStateMachine();
            BindGame();
            
        }
        
        private void RegisterServices()
        {
            Container.Bind<IInputSevice>().To<InputSevice>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            
        }
        
        private void BindGameStateMachine()
        {
            Container.Bind<GameStateMachine>().AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }

        private void BindGame()
        {
            LoadingCurtain curtain = Container.InstantiatePrefabForComponent<LoadingCurtain>(_loadingCurtainPrefab);
            Container.BindInterfacesAndSelfTo<LoadingCurtain>().FromInstance(curtain).AsSingle();
            Container.Bind<Game>().FromNew().AsSingle().NonLazy();
        }
    }
}
