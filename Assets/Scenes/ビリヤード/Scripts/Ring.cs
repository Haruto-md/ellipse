using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private GameObject ringPrefab;
    private GameObject[] rings = new GameObject[2];
    // Start is called before the first frame update

    [SerializeField]
    private float x_scale;
    [SerializeField]
    private float z_scale;
    [SerializeField]
    private float angleVelocity;
    private float phase;
    private void Start()
    {
        phase = 0;
        rings[0] = Instantiate(ringPrefab, new Vector3(x_scale, 2f, z_scale), Quaternion.identity, transform);
        rings[1] = Instantiate(ringPrefab, new Vector3(-x_scale, 2f, -z_scale), Quaternion.identity, transform);
    }

    private void Update()
    {
        //設定値が変更されたらコライダーの形を更新
        UpdateRing();
    }

    //=================================================================================
    //更新
    //=================================================================================

    /// <summary>
    /// 現在のスケールからコライダーの形更新
    /// </summary>
    public void UpdateRing()
    {
        phase += Time.deltaTime * angleVelocity;
        var x = Mathf.Cos(phase) * 0.5f * x_scale;
        var z = Mathf.Sin(phase) * 0.5f * z_scale;

        rings[0].transform.position = new Vector3(x, 2f, z);
        rings[1].transform.position = new Vector3(-x, 2f, -z);
    }

}