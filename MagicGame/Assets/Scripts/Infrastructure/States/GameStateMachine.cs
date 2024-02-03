using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Loading;
using StaticData;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService persistentProgressService, ISavedLoadService savedLoadService, IStaticDataService staticDataService)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, staticDataService),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, gameFactory, persistentProgressService),
                [typeof(LoadProgressState)] = new LoadProgressState(this, persistentProgressService, savedLoadService),
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }
        
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}