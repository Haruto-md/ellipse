using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour,IHasTrigger
{
    AudioSource audioSource;
    private void Start()
    {
    }
    public void Triggerred()
    {
        transform.parent.GetComponent<ObjectManager>().Score--;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(Random.Range(-5f,5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }
}