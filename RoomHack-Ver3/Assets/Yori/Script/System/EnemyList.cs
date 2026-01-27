using System.Collections.Generic;
using UnityEngine;

public class EnemyList : ISetEnemeyList, IGetEnemyList
{
    private List<EnemyBase> Enemies;

    public EnemyList()
    {
        Enemies = new();
    }
    public void EnemyListAdd(EnemyBase enemies)
    {
        Enemies.Add(enemies);
    }
    public List<EnemyBase> GetEnemies()
    {
        return Enemies;
    }

    public void EnemyListRemove(EnemyBase enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
        }
        else
        {
            Debug.LogError("リストにない敵を削除しようとしています");
        }
    }
}
