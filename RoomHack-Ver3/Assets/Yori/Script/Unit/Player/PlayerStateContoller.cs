using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    IPlayerState globalState;
    public PlayerStateType stateType { get; private set; }

    private Dictionary<PlayerStateType, IPlayerState> states;

    private CancellationTokenSource cancellationTokenSource;

    public PlayerStateContoller(Rigidbody2D playerRigidBody, GunData gunData, Material material,
        GameObject bulletPre, float moveSpeed, GameObject player, IPlayerInput playerInput,IHaveGun haveGun)
    {
        cancellationTokenSource = new CancellationTokenSource();
        states = new Dictionary<PlayerStateType, IPlayerState>()
    {
        { PlayerStateType.Action, new PlayerActionState(playerRigidBody,moveSpeed,this,playerInput) },
        { PlayerStateType.Hack, new PlayerHackState(this,playerInput) },
        { PlayerStateType.Die, new PlayerDieState() },
    };
        globalState = new PlayerGlobalState(gunData, material, playerInput, player, bulletPre,haveGun);
        stateType = PlayerStateType.Action;
        currentState = states[stateType];
        currentState.Enter();
        globalState.Enter();
        Update();
    }
    public async void Update()
    {
        while (true)
        {
            try
            {
                await currentState.Execute().AttachExternalCancellation(cancellationTokenSource.Token);
                await globalState.Execute().AttachExternalCancellation(cancellationTokenSource.Token);
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
        cancellationTokenSource.Cancel();
        ChangeState(PlayerStateType.Die);
    }
}
