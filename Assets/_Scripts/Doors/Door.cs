using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] float valueID;
    [SerializeField] bool needsKey;
    [SerializeField] bool isBossDoor = false;

    [Header("Audio source")]
    [SerializeField] AudioClip sound;
    
    List<float> playerKeys = new List<float>();

    Animator anim;
    private AudioSource audioSource;

    public delegate void StartBossFight();
    public static event StartBossFight bossfight;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Debug.Log(playerKeys.Count);
        Debug.Log("value id: " + valueID);
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
        anim.SetBool("CloseDoor", false);
        anim.SetBool("OpenDoor", true);
        SetClip();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isBossDoor) return;
        if (!needsKey)
        {
            anim.SetBool("OpenDoor", false);
            anim.SetBool("CloseDoor", true);
            SetClip();
        }
    }

    void SetClip()
    {
        if (audioSource)
        {
            audioSource.Stop();
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
}
