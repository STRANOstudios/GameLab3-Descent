using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] public float KeyID;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

    }
}
