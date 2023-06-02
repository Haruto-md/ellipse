using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour,IHasTrigger
{
    public void Triggerred()
    {
        transform.parent.GetComponent<ObjectManager>().Score++;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
