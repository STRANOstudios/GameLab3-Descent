using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] float valueID;
    [SerializeField] bool needsKey;
    [SerializeField] bool isBossDoor = false;

    [Header("Audio source")]
    [SerializeField] AudioSource audioSource;
    
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
                    Open();
                    return;
                }
            }
        }

        else
        {
            Open();
            return;
        }
    }

    void Open()
    {
        anim.SetBool("OpenDoor", true);
        if(audioSource) audioSource.Play();
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
