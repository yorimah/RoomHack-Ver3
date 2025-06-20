using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerViewMesh : MonoBehaviour
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
    float rayOffset = 0;

    [SerializeField]
    LayerMask targetLm;
    List<Vector2> Items = new List<Vector2>();

    private void Awake()
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

    public void GeneradeTriangle(Vector3 _playerOrigin, Vector3 _hitPointFirst, Vector3 _hitpointSecond, int index)
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
        Vector3 startPos = this.transform.position;
        float partRot = rayRot / (rayValue - 1);
        float nowRot = rayOffset;

        Profiler.BeginSample("ray");
        for (int i = 0; i < rayValue; i++)
        {
            Vector3 rayPos = new Vector3(rayLen * Mathf.Cos(nowRot * Mathf.Deg2Rad), rayLen * Mathf.Sin(nowRot * Mathf.Deg2Rad), 0);
            RaycastHit2D result = Physics2D.Linecast(startPos, startPos + rayPos, targetLm);
            //Debug.DrawLine(startPos, startPos + rayPos, color: Color.red);

            if (result.collider != null)
            {
                Items.Add(result.point);
            }
            nowRot += partRot;
        }
        Profiler.EndSample();

        Profiler.BeginSample("generade");
        for (int i = 1; i < Items.Count ; i++)
        {
            GeneradeTriangle(Items[i-1], startPos, Items[i], i - 1);
        }
        Profiler.EndSample();

        Items.Clear();
    }
}