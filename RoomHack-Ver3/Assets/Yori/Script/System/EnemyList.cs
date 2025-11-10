using System.Collections.Generic;

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
}
