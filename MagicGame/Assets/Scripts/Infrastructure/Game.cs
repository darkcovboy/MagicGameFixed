using Infrastructure.States;
using Services.Input;

namespace Infrastructure
{
    public class Game
    {
        public static IInputSevice InputService;
        public GameStateMachine StateMachine { get;}

        public Game(GameStateMachine gameStateMachine, IInputSevice inputService)
        {
            StateMachine = gameStateMachine;
            InputService = inputService;
        }
    }
}