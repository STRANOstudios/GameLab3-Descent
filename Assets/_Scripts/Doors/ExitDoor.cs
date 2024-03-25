using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    Animator anim;
    bool canOpen;
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
        canOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canOpen)
        {
            anim.SetBool("OpenDoor", true);
        }
    }
}
