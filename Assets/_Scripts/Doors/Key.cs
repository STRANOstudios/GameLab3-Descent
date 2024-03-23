using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] public float KeyID;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
