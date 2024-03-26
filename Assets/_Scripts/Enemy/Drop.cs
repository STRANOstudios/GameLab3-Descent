
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


[System.Serializable]
public class Drop : MonoBehaviour
{
    [SerializeField, Range(1, 100)] public float[] percentage;
    [SerializeField] public GameObject[]  drops;



    public int GetRandomSpawn()
    {
        float ran = Random.Range(0f, 1f);
        float numForAdding = 0f;
        float total = 0f;

        for (int i = 0; i < percentage.Length; i++)
        {
            total += percentage[i]; 
        }

        for (int i = 0; i < drops.Length; i++)
        {
            if (percentage[i] / total + numForAdding >=ran)
            {
                return i;
            }
            else
            {
                numForAdding += percentage[i] / total;
            }
        }
        return 0;
    }
    //[Header("Drop Chance")]
    //[SerializeField, Range(1, 100)] public float chance;

    //[SerializeField] public Transform powerup;

    //float ran;

    //public void CheckDropChance()
    //{

    //    ran = Random.Range(0f, 100f);

    //    if (ran <= chance)
    //    {
    //        Instantiate(powerup, transform.position, Quaternion.identity);
    //    }

    //}


}
