using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    Animator anim;
    private void OnEnable()
    {
        CoreLogic.death += Timer;
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        CoreLogic.death -= Timer;
    }
    private void Timer()
    {
        anim.SetBool("OpenDoor", true);
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
