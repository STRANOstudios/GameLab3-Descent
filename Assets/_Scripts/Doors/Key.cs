using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] public float KeyID;

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
