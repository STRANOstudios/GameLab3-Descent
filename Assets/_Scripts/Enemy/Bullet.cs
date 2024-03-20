using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    private void Update()
    {
        transform.position += (transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HP>(out var otherHP)) 
        {
            otherHP.myHP -= damage;
            if (otherHP.myHP <= 0)
            {
                otherHP.Death();
            }
        }
        Destroy(gameObject);

    }
}
