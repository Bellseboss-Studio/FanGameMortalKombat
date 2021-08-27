using System.Collections.Generic;
using UnityEngine.Assertions;

public class GameStatesConfiguration
{
    private int InitialState;
    private readonly Dictionary<int, IGameState> _states;
        
    public const int IdleState = 0;
    public const int CombatState = 1;

    public GameStatesConfiguration()
    {
        _states = new Dictionary<int, IGameState>();
    }



    public void AddInitialState(int id, IGameState state)
    {
        _states.Add(id, state);
        InitialState = id;
    }

    public void AddState(int id, IGameState state)
    {
        _states.Add(id, state);
    }

    public IGameState GetState(int stateId)
    {
        Assert.IsTrue(_states.ContainsKey(stateId), $"State with id {stateId} do not exit");
        return _states[stateId];
    }

    public IGameState GetInitialState()
    {
        return GetState(InitialState);
    }
}