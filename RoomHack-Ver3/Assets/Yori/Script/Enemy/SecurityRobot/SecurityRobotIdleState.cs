using UnityEngine;
public class SecurityRobotIdleState : IEnemyState
{
    private EnemyBase enemy;

    private PlayerCheck playerCheck;

    // mesh関係
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    private GameObject shotRange;

    // 長さ
    private float viewDistance = 3f;
    // 分割数
    private int segment = 20;
    private Material shotRanageMaterial;

    private Rigidbody2D enemyRigidBody;

    private float checkDistance = 1;
    public SecurityRobotIdleState(EnemyBase _enemy, Material _shotRanageMaterial)
    {
        enemy = _enemy;
        playerCheck = enemy.playerCheck;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
        shotRanageMaterial = _shotRanageMaterial;
        shotRange = new GameObject(enemy.gameObject.name + "shotRangge");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        shotRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        shotRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = shotRange.GetComponent<MeshRenderer>();

        mr.material = new Material(shotRanageMaterial);
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色(仮)
        mr.sortingOrder = 10;
        vertices = new Vector3[segment + 2];
        triangles = new int[segment * 3];
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        EnemyView();
        PatrolMove();
    }

    public void Exit()
    {
        mesh.Clear();
    }

    public void PatrolMove()
    {
        if (Physics2D.Raycast(enemy.transform.position, enemy.transform.up, checkDistance, enemy.GetObstacleMask()))
        {
            Debug.Log("壁に激突");
        }
    }

    public void EnemyView()
    {
        ViewMesh();
        if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
        {
            Vector2 forward = enemy.transform.up;
            Vector2 toTarget = enemy.PlayerPosition - enemy.transform.position;
            float angle = Vector2.SignedAngle(forward, toTarget);
            if (Mathf.Abs(angle) <= 60 && toTarget.sqrMagnitude <= viewDistance * viewDistance)
            {
                enemy.ChangeState(EnemyStateType.Move);
            }
        }
    }
    public void ViewMesh()
    {
        if (mesh != null)
        {
            mesh.Clear();

            vertices[0] = enemy.transform.position;

            float angle = 30 * 2f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -30 + (angle / segment) * i;

                Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
                Vector3 dir = rot * enemy.transform.up;

                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, dir, viewDistance, enemy.GetObstacleMask());
                if (hit.collider != null)
                {
                    // 障害物に当たったらその地点を頂点にする
                    vertices[i + 1] = hit.point;
                }
                else
                {
                    // 何もなければ円周上の点
                    vertices[i + 1] = enemy.transform.position + dir * viewDistance;
                }

                if (i < segment)
                {
                    int start = i * 3;
                    // 中心
                    triangles[start] = 0;
                    // 左上
                    triangles[start + 1] = i + 2;
                    // 右上
                    triangles[start + 2] = i + 1;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }
}