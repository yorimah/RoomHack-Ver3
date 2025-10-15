using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class PlayerStateContoller
{
    IState currentState;
    StateType stateType;
    Dictionary<StateType, IState> states;

    CancellationTokenSource cts;
    public enum StateType
    {
        Action,
        Hack,
        num
    }

    public PlayerStateContoller(Rigidbody2D playerRigidBody, GunData gunData, Material material, GameObject bulletPre, float moveSpeed, PlayerInput playerInput, GameObject player)
    {
        cts = new();
        states = new Dictionary<StateType, IState>()
    {
        { StateType.Action, new PlayerActionState(playerRigidBody,gunData,material,bulletPre,moveSpeed,playerInput,player) },
        //{ StateType.Hack, new PlayerHackState() },
    };
        stateType = StateType.Action;
        currentState = states[stateType];
        Update();
    }
    public async void Update()
    {
        while (true)
        {
            try
            {
                await currentState.Execute().AttachExternalCancellation(cts.Token);
            }
            catch (OperationCanceledException)
            {

                throw;
            }
            finally
            {
                cts = new();
            }

        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeState(StateType type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState?.Enter();

        stateType = type;
    }
}
