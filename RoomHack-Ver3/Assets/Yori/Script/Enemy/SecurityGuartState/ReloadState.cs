﻿using UnityEngine;

public class ReloadState : IState
{
    private Enemy enemy;
    public ReloadState(Enemy _enemy)
    {
        enemy = _enemy;        
    }

    private float timer = 0;
    public void Enter()
    {
        enemy.nowMagazine = enemy.MaxMagazine;
        timer = 0;
    }

    public void Execute()
    {
        timer += GameTimer.Instance.ScaledDeltaTime;
        if (timer >= 1)
        {
            enemy.ChangeState(Enemy.StateType.Idle);
            timer = 0;
        }
    }

    public void Exit()
    {
        enemy.nowMagazine = enemy.gundata.MaxMagazine;
    }
}