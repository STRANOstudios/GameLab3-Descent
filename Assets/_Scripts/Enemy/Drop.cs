using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


[System.Serializable]
public class Drop : MonoBehaviour
{
    [Header("Drop Chance")]
    [SerializeField, Range(1, 100)] public float chance;

    [SerializeField] public Transform powerup;

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
