using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Drop Chance")]
    [SerializeField, Range(1, 100)] public float chance;

    [SerializeField] Transform powerup;

    float ran;

    public void CheckDropChance()
    {

        ran = Random.Range(0f, 100f);

        if (ran <= chance)
        {
            Instantiate(powerup, transform.position, Quaternion.identity);
        }

    }
}
