using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStateType
{
    Action,
    Hack,
    Die,
    num
}


public class PlayerStateContoller
{
    IPlayerState currentState;
    public PlayerStateType stateType { get; private set; }
    private Dictionary<PlayerStateType, IPlayerState> states;

    private CancellationTokenSource cancellationTokenSource;

    public PlayerStateContoller(Rigidbody2D playerRigidBody, GunData gunData, Material material,
        GameObject bulletPre, float moveSpeed, GameObject player, IPlayerInput playerInput)
    {
        cancellationTokenSource = new CancellationTokenSource();
        states = new Dictionary<PlayerStateType, IPlayerState>()
    {
        { PlayerStateType.Action, new PlayerActionState(playerRigidBody,gunData,material,
        bulletPre,moveSpeed,player,this,playerInput) },
        { PlayerStateType.Hack, new PlayerHackState(this,playerInput) },
        { PlayerStateType.Die, new PlayerDieState() },
    };
        stateType = PlayerStateType.Action;
        currentState = states[stateType];
        currentState.Enter();
        Update();

        
    }
    public async void Update()
    {
        while (true)
        {
            try
            {
                await currentState.Execute().AttachExternalCancellation(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                cancellationTokenSource = new();
            }

        }

    }
    public void ChangeState(PlayerStateType type)
    {
        Debug.Log("ステートを" + type.ToString() + "に変更！");
        currentState?.Exit();
        currentState = states[type];
        currentState?.Enter();

        stateType = type;
    }

    public void DieChangeState()
    {
        ChangeState(PlayerStateType.Die);
        cancellationTokenSource.Cancel();
    }
}
