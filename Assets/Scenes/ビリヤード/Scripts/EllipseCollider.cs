//  EllipseCollider2D.cs
//  http://kan-kikuchi.hatenablog.com/entry/EllipseCollider2D
//
//  Created by kan.kikuchi on 2023.05.13.
//  Modified
using UnityEngine;

public class EllipseCollider : MonoBehaviour
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

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        UpdateMesh();
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
        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
        }
        if(mesh == null)
        {
            mesh = new Mesh();
        }
        var points = new Vector3[2*_points];
        var angle = 0f;
        var deltaAngle = 2f * Mathf.PI / _points;

        for (var i = 0; i < _points; i++)
        {
            var x = Mathf.Cos(angle) * _xSize * 0.5f;
            var z = Mathf.Sin(angle) * _ySize * 0.5f;

            points[i] = new Vector3(x, transform.position.y + 1f,z);
            points[i+_points] = new Vector3(x, transform.position.y-1f, z);

            angle += deltaAngle;
        }

        mesh.vertices = points;
        mesh.triangles = new int[] { 0, 1, 5};
        meshCollider.sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;

    }

}