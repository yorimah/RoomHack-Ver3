using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FlySecurityDrone : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new DroneMoveState(this) },
        { StateType.Shot, new DroneShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];
        clack().AttachExternalCancellation(token).Forget();
    }
    async UniTask clack()
    {
        while (true)
        {
            // クラックされるまで待機
            await UniTask.WaitUntil(() => clacked);
            // nowfirewallが下限突破してたら戻す
            if (NowFireWall <= 0)
            {
                NowFireWall = 0;
            }
            // ハックした内容
            
            // リカバリが終わるまでここでループ
            while (NowFireWall <= MaxFireWall)
            {
                currentState = states[StateType.Idle];
                FireWallRecavary();
                await UniTask.Yield();
            }
            // ループが終わったら初期状態に戻す
            clacked = false;
            await UniTask.Yield();
        }
    }
}
