using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] public float myHP;

    public virtual void Death()
    {
        Debug.Log("died");
    }
}
