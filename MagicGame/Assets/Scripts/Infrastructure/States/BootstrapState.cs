using Services.Input;
using StaticData;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _staticDataService.LoadMonsters();
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() =>            
            _stateMachine.Enter<LoadProgressState>();
        public void Exit()
        {
        }
    }
}