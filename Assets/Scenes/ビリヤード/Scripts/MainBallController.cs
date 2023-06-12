using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBallController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool mouseDraggingFlag;
    private float forcePower = 0;
    private Vector3 forceDirection;
    private float angle;

    public float angleDuration=1;
    public float angleRange = 1/4f;
    public GameObject array;
    public GameObject startPositionIndicator;
    public Vector3 focusPosition;
    public float startDislocationStrength;
    public float activeFriction;

    [SerializeField] private float maxForcePower = 10;
    [SerializeField] private float forceWeight = 1;
    [SerializeField] private bool forceDisorientationFlag;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouseDraggingFlag = false;
        transform.position = getNoisedVector3(focusPosition, startDislocationStrength);
    }
    private Vector3 getNoisedVector3(Vector3 position,float magnitude) 
    {
        position += new Vector3(Random.Range(-magnitude, magnitude), 0, Random.Range(-magnitude, magnitude));
        return position;
    }
    private void Update()
    {
        // マウスの左クリックを検知
        if (Input.GetMouseButtonDown(0))
        {
            if (rb.velocity.magnitude < 5)
            {
                array.SetActive(true);
                startPositionIndicator.SetActive(true);
                startPosition = getMousePoint();
                startPositionIndicator.transform.position = startPosition;
                mouseDraggingFlag = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (mouseDraggingFlag == true)
            {
                if (forceDirection.magnitude / maxForcePower > 0.2)
                {
                    GetComponent<AudioSource>().Play();
                    rb.AddForce(forceDirection.normalized * forcePower, ForceMode.Impulse);
                    transform.parent.GetComponent<ObjectManager>().AttemptTimes++;
                }
            }
            mouseDraggingFlag = false;
            array.SetActive(false);
            startPositionIndicator.SetActive(false);
        }

        if (transform.position.y < 0)
        {
            transform.position = new Vector3(-30f, 1.6f, 0);
            rb.velocity = Vector3.zero;
        }
        
        if (mouseDraggingFlag)
        {
            endPosition = getMousePoint();
            // クリックした地点に向けて力を加える
            forceDirection = (startPosition-endPosition) * forceWeight;
            forcePower = Mathf.Min(forceDirection.magnitude, maxForcePower);
            array.transform.localScale = new Vector3(1, 1, 2*forcePower / maxForcePower);

            if (forceDisorientationFlag)
            {
                angle += 2 * Mathf.PI * Time.deltaTime / angleDuration;
                float rotationAngle = Mathf.Sin(angle) * 180f * angleRange;
                // Quaternionを作成
                Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);

                // Vector3を回転させる
                forceDirection = rotation * forceDirection;
            }
            array.transform.LookAt(array.transform.position+forceDirection.normalized, Vector3.up);
        }
        else
        {
            forcePower = 0;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (rb.velocity.magnitude > 5)
            {
                rb.velocity = rb.velocity * activeFriction;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IHasTrigger>().Triggerred(transform);
        transform.position = getNoisedVector3(focusPosition, startDislocationStrength);
        rb.velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "wall")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private Vector3 getMousePoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            hit.point =new Vector3(hit.point.x,0, hit.point.z);
            return hit.point;
        }
        else
        {
            // マウスの位置が何かしらのオブジェクトに当たらなかった場合の処理
            return Vector3.zero; // 仮の戻り値として原点を返す
        }
    }
}