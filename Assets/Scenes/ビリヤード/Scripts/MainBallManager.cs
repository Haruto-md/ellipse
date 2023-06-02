using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBallManager : MonoBehaviour
{
    [SerializeField] private float interval;
    private float acumulatedTime;
    private int ballNum;

    public GameObject mainBall;
    public Vector3 launchPosition = new Vector3();
    public int maxBallNum;
    public float forcePower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        acumulatedTime += Time.deltaTime;
        if (acumulatedTime > interval)
        {
            if (ballNum < maxBallNum)
            {
                LaunchBall();
            }
            acumulatedTime = 0;
        }
        
    }
    private void LaunchBall()
    {
        ballNum++;
        var ball = Instantiate(mainBall, launchPosition, Quaternion.identity);
        var ballRigidBody =  ball.GetComponent<Rigidbody>();
        ballRigidBody.AddForce(new Vector3(Random.Range(-forcePower, 3*forcePower), 0, Random.Range(-2*forcePower, 2*forcePower)), ForceMode.Impulse);
    }
}
