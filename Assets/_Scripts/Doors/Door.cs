using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float valueID;
    [SerializeField] bool needsKey;
    [SerializeField] bool isBossDoor = false;
    
    List<float> playerKeys = new List<float>();

    Animator anim;

    public delegate void StartBossFight();
    public static event StartBossFight bossfight;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isBossDoor) bossfight?.Invoke();

        //anim.SetBool("CloseDoor", false);
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

    private void OnTriggerExit(Collider other)
    {
        if (isBossDoor) return;
        if (!needsKey)
        {
            anim.SetBool("OpenDoor", false);
            anim.SetBool("CloseDoor", true);
        }
    }
}
