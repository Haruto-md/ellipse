using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour,IHasTrigger
{
    public void Triggerred(Transform transform)
    {
        transform.parent.GetComponent<ObjectManager>().Score+= Mathf.FloorToInt(transform.GetComponent<Rigidbody>().velocity.magnitude);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
