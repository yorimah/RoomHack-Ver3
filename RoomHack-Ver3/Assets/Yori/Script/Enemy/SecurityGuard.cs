using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class SecurityGuard : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];
        clack().AttachExternalCancellation(token).Forget();
    }
    async UniTask clack()
    {
        await UniTask.WaitUntil(() => clacked);
        shotIntervalTime = 0.5f;
        while (true)
        {
            FireWallRecavary();
            await UniTask.Yield();
        }

        
    }
}
