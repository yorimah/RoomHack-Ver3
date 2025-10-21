using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class PlayerDieState : IPlayerState
{
    public PlayerDieState()
    {

    }
    public void Enter()
    {
        
    }

    public async UniTask Execute()
    {
        await UniTask.Yield();
    }

    public void Exit()
    {
    }
}

