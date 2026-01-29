using UnityEngine;
public class SecurityRobotIdleState : IEnemyState
{
    private EnemyBase enemy;

    private PlayerCheck playerCheck;
    public enum PatrolMoveEnum
    {
        moveStraight,
        turn,
    }
    PatrolMoveEnum patrolMoveEnum;
    // mesh関係
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    // 長さ
    private float viewDistance = 3f;
    // 分割数
    private int segment = 20;

    private Rigidbody2D enemyRigidBody;

    private float checkDistance = 1;
    public SecurityRobotIdleState(EnemyBase _enemy, Material _shotRanageMaterial)
    {
        enemy = _enemy;
        playerCheck = enemy.playerCheck;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
        Material shotRanageMaterial = _shotRanageMaterial;
        GameObject viewRange = new GameObject(enemy.gameObject.name + "ViewRange");
        viewRange.AddComponent<MeshRenderer>();
        viewRange.AddComponent<MeshFilter>();
        viewRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        viewRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = viewRange.GetComponent<MeshRenderer>();

        mr.material = new Material(shotRanageMaterial);
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色(仮)
        mr.sortingOrder = 10;
        vertices = new Vector3[segment + 2];
        triangles = new int[segment * 3];
    }
    public void Enter()
    {
        patrolMoveEnum = PatrolMoveEnum.moveStraight;
    }

    public void Execute()
    {
        EnemyView();
        MoveTypeChange();
    }

    public void Exit()
    {
        mesh.Clear();
    }
    Vector2 targetDir;
    public void MoveTypeChange()
    {
        switch (patrolMoveEnum)
        {
            case PatrolMoveEnum.moveStraight:
                // 壁とぶつかったら左右どちらかに方向転換
                if (!Physics2D.Raycast(enemy.transform.position, enemy.transform.up, checkDistance, enemy.GetObstacleMask()))
                {
                    enemyRigidBody.linearVelocity = enemy.transform.up * enemy.moveSpeed * GameTimer.Instance.GetCustomTimeScale();
                }
                else
                {
                    Vector2 rightDir = enemy.transform.right;
                    Vector2 leftDir = -enemy.transform.right;
                    RaycastHit2D rightRangeRay = Physics2D.Raycast(enemy.transform.position, rightDir, 10, enemy.GetObstacleMask());
                    float rightRange = Vector2.Distance(enemy.transform.position, rightRangeRay.point);
                    RaycastHit2D leftRangeRay = Physics2D.Raycast(enemy.transform.position, leftDir, 10, enemy.GetObstacleMask());
                    float leftRange = Vector2.Distance(enemy.transform.position, leftRangeRay.point);
                    targetDir = rightRange >= leftRange ? rightDir : leftDir;
                    patrolMoveEnum = PatrolMoveEnum.turn;
                }
                break;
            case PatrolMoveEnum.turn:
                float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
                float current = enemy.transform.eulerAngles.z;
                float rotateSpeed = 180f;
                float newAngle = Mathf.MoveTowardsAngle(current, targetAngle, rotateSpeed * GameTimer.Instance.GetScaledDeltaTime());
                enemy.transform.rotation = Quaternion.Euler(0, 0, newAngle);
                float diff = Mathf.DeltaAngle(current, targetAngle);

                if (Mathf.Abs(diff) < 1f)
                {
                    patrolMoveEnum = PatrolMoveEnum.moveStraight;
                    Debug.Log("回転終わり");
                }
                break;
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

            float angle = 60 * 2f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -60 + (angle / segment) * i;

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