using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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
            shotIntervalTime = 0.5f;
            // リカバリが終わるまでここでループ
            while (NowFireWall <= MaxFireWall)
            {
                FireWallRecavary();
                await UniTask.Yield();
            }
            // ループが終わったら初期状態に戻す
            shotIntervalTime = 1f / shotRate;
            clacked = false;
            await UniTask.Yield();
        }
    }
}
