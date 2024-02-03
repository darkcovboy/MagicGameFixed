using Infrastructure;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Services.Input;
using StaticData;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            Container.Bind<IInputSevice>().To<InputSevice>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ISavedLoadService>().To<SaveLoadService>().AsSingle();
        }
    }
}