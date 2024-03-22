using UnityEngine;

public class AmazonVan : MonoBehaviour
{
    [Header("Ontrigger Setting")]
    [SerializeField] string targetTag = "Box";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            other.gameObject.SetActive(false);
        }
    }
}
