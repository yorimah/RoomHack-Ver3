using System.Collections.Generic;
using UnityEngine;

public class TestTriangle : MonoBehaviour
{
    List<Mesh> mesh = new List<Mesh>();

    [SerializeField]
    GameObject triangle;

    List<GameObject> triangles = new List<GameObject>();

    [SerializeField]
    float rayRot = 360;
    [SerializeField]
    int rayValue = 36;
    [SerializeField]
    float rayLen = 5;

    [SerializeField]
    LayerMask targetLm;
    List<Vector2> Items = new List<Vector2>();

    private void Awake()
    {
        Instriangle();
    }
    void Instriangle()
    {
        for (int i = 0; i < rayValue; i++)
        {
            mesh.Add(new Mesh());
            triangles.Add(Instantiate(triangle, Vector2.zero, Quaternion.identity));
            triangles[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }
    }
    private void Update()
    {
        PlayeViewer();
    }

    public void GeneradeTriangle(Vector2 _playerOrigin, Vector2 _hitPointFirst, Vector2 _hitpointSecond, int index)
    {
        Vector3[] vertices = new Vector3[] {
            _playerOrigin,
            _hitPointFirst,
            _hitpointSecond
        };
        int[] triangles = new int[] { 0, 1, 2 };

        mesh[index].vertices = vertices;
        mesh[index].triangles = triangles;
        mesh[index].RecalculateNormals();
    }

    public void PlayeViewer()
    {
        Vector2 startPos = this.transform.position;
        float partRot = rayRot / (rayValue - 1);
        float nowRot = 0;

        // Žü‚è‚ÉƒŒƒC‚ð”ò‚Î‚·
        for (int i = 0; i < rayValue; i++)
        {
            Vector2 rayPos = new Vector2(rayLen * Mathf.Cos(nowRot * Mathf.Deg2Rad), rayLen * Mathf.Sin(nowRot * Mathf.Deg2Rad));
            RaycastHit2D result = Physics2D.Linecast(startPos, startPos + rayPos, targetLm);

            if (result.collider != null)
            {
                Items.Add(result.point);
            }
            nowRot -= partRot;
        }

        for (int i = 1; i < Items.Count ; i++)
        {
            GeneradeTriangle(Items[i], startPos, Items[i - 1], i - 1);
        }
        Items.Clear();
    }
}