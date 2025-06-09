using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestTriangle : MonoBehaviour
{
    [SerializeField]
    float[] a = new float[3];

    Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];
        for (int i = 0; i < 3; i++)
        {
            //    float angle = i * 120f * Mathf.Deg2Rad;

            float angle = a[i] * Mathf.Deg2Rad;
            vertices[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        }
        triangles = new int[3] { 0, 1, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}