using Infrastructure.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameBootsraper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        [Inject]
        public void Constructor(Game game)
        {
            _game = game;
        }
        
        private void Awake()
        {
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}