namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        public GameLoopState(GameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }
        public void Exit()
        {
            
        }

        public void Enter()
        {
            
        }
    }
}