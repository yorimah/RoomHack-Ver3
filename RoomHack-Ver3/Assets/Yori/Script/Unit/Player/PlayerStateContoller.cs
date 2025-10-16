using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum PlayerStateType
{
    Action,
    Hack,
    num
}

public class PlayerStateContoller
{
    IState currentState;
    public PlayerStateType stateType { get; private set; }
    private Dictionary<PlayerStateType, IState> states;

    private CancellationTokenSource cancellationTokenSource;
    private PlayerInput playerInput;

    public PlayerStateContoller(Rigidbody2D playerRigidBody, GunData gunData, Material material,
        GameObject bulletPre, float moveSpeed, PlayerInput _playerInput, GameObject player)
    {
        playerInput = _playerInput;
        cancellationTokenSource = new CancellationTokenSource();
        states = new Dictionary<PlayerStateType, IState>()
    {
        { PlayerStateType.Action, new PlayerActionState(playerRigidBody,gunData,material,
        bulletPre,moveSpeed,playerInput,player,this) },
        { PlayerStateType.Hack, new PlayerHackState(playerInput,this) },
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
                Debug.Log(currentState.ToString() + "を実行中");
            }
            catch (OperationCanceledException)
            {

                throw;
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
}
