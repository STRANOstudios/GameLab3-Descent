using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float valueID;

    [SerializeField] bool needsKey;
    
    List<float> playerKeys = new List<float>();

    Animator anim;

    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (needsKey)
        {
            playerKeys = other.GetComponent<PlayerKeyHolder>().keyIDs;
            foreach (var key in playerKeys)
            {
                if (key == valueID)
                {
                    anim.SetBool("OpenDoor", true);
                    return;
                }
            }
        }

        else
        {
            anim.SetBool("OpenDoor", true);
            return;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!needsKey)
        {
            anim.SetBool("OpenDoor", false);
            anim.SetBool("CloseDoor", true);
        }
    }
}
