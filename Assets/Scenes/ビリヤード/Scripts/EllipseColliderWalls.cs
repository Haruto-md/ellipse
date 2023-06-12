//  EllipseCollider2D.cs
//  http://kan-kikuchi.hatenablog.com/entry/EllipseCollider2D
//
//  Created by kan.kikuchi on 2023.05.13.
//  Modified
using UnityEngine;

public class EllipseColliderWalls : MonoBehaviour, IHasTrigger
{

    private Mesh mesh;
    private MeshCollider meshCollider;

    //XとYの大きさ
    [SerializeField]
    private float _xSize = 1.0f, _ySize = 1.0f;
    public float XSize
    {
        get => _xSize;
        set
        {
            _xSize = value;
            UpdateMesh();
        }
    }
    public float YSize
    {
        get => _ySize;
        set
        {
            _ySize = value;
            UpdateMesh();
        }
    }

    //点の数(多いほど滑らかに)
    [SerializeField]
    private int _points = 64;
    public int Points
    {
        get => _points;
        set
        {
            _points = value;
            UpdateMesh();
        }
    }

    private void OnValidate()
    {
        //設定値が変更されたらコライダーの形を更新
        UpdateMesh();
    }

    //=================================================================================
    //更新
    //=================================================================================

    /// <summary>
    /// 現在のスケールからコライダーの形更新
    /// </summary>
    public void UpdateMesh()
    {
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();
        var points = new Vector3[4 * _points];
        var angle = 0f;
        var deltaAngle = 2f * Mathf.PI / _points;

        for (var i = 0; i < _points; i++)
        {
            var x = Mathf.Cos(angle) * _xSize * 0.5f;
            var z = Mathf.Sin(angle) * _ySize * 0.5f;

            points[4 * i] = new Vector3(x, transform.position.y + 5f, z);
            points[4 * i + 1] = new Vector3(x, transform.position.y - 1f, z);
            points[4 * i + 2] = new Vector3(x*1.1f, transform.position.y - 1f, z * 1.1f);
            points[4 * i + 3] = new Vector3(x * 1.1f, transform.position.y + 5f, z * 1.1f);

            angle += deltaAngle;
        }

        mesh.vertices = points;

        for (var i = 0; i < _points; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                int[] newTriangles = new int[] { 4 * i + j, 4 * i + (1 + j) % 4, 4 * ((i + 1) % _points) + j, 4 * i + (1 + j) % 4, 4 * ((i + 1) % _points) + (1 + j) % 4, 4 * ((i + 1) % _points) + j };
                int[] combinedTriangles = new int[mesh.triangles.Length + newTriangles.Length];
                mesh.triangles.CopyTo(combinedTriangles, 0);
                newTriangles.CopyTo(combinedTriangles, mesh.triangles.Length);
                mesh.triangles = combinedTriangles;
            }
        }
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshCollider.sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    public void Triggerred(Transform transform)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}