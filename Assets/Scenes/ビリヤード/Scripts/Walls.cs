using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour,IHasTrigger
{
    [SerializeField] private GameObject _wall_prefab;

    [SerializeField]
    private float x_scale;
    [SerializeField]
    private float y_scale;
    [SerializeField]
    private float width;
    //点の数(多いほど滑らかに)
    [SerializeField]
    private int _points = 64;
    public int Points
    {
        get => _points;
        set
        {
            _points = value;
            UpdateWalls();
        }
    }


    private void OnValidate()
    {
        //設定値が変更されたらコライダーの形を更新
        UpdateWalls();
    }

    //=================================================================================
    //更新
    //=================================================================================

    /// <summary>
    /// 現在のスケールからコライダーの形更新
    /// </summary>
    public void UpdateWalls()
    {

        var points = new Vector3[_points];
        var angle = 0f;
        var deltaAngle = 2f * Mathf.PI / _points;
        angle -= deltaAngle/2;

        for (var i = 0; i < _points; i++)
        {
            var x = Mathf.Cos(angle)*0.5f* x_scale;
            var z = Mathf.Sin(angle)*0.5f* y_scale;
            points[i] = new Vector3(x, transform.position.y, z);
            angle += deltaAngle;
        }

        for (var i = 0; i < _points; i++)
        {
            // 現在の点の座標と次の点の座標を取得します
            var currentPoint = points[i];
            var nextPoint = points[(i + 1) % _points]; // 最後の点から最初の点への接続を実現します

            // 中点の座標を計算します
            var midPoint = (currentPoint + nextPoint)*0.5f;

            // 壁オブジェクトの向き（Quaternion）を計算します
            var direction = nextPoint - currentPoint;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            // 壁オブジェクトをInstantiateします
            var wall = Instantiate(_wall_prefab, midPoint, rotation, transform);
            var distance = Vector3.Distance(currentPoint, nextPoint);
            wall.transform.Translate(new Vector3(_wall_prefab.transform.localScale.x / 2,0,0), Space.Self);
            // 壁オブジェクトのXスケールを設定します
            wall.transform.localScale = new Vector3(_wall_prefab.transform.localScale.x/transform.lossyScale.x, _wall_prefab.transform.lossyScale.y / transform.localScale.y, distance / transform.lossyScale.z);
        }
    }
    public void Triggerred(Transform transform)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}