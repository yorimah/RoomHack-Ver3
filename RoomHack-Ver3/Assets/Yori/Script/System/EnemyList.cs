using System.Collections.Generic;
using UnityEngine;

public class EnemyList : ISetEnemeyList, IGetEnemyList
{
    private List<Enemy> Enemies;

    public EnemyList()
    {
        Enemies = new();
    }
    public void EnemyListAdd(Enemy enemies)
    {
        Enemies.Add(enemies);
    }
    public List<Enemy> GetEnemies()
    {
        return Enemies;
    }

    public void EnemyListRemove(Enemy enemy)
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
