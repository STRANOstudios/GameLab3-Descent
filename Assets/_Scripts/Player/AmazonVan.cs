using UnityEngine;

public class AmazonVan : MonoBehaviour
{
    [Header("Ontrigger Setting")]
    [SerializeField] string targetTag = "Box";

    public delegate void StateOfDelivery();
    public static event StateOfDelivery delivered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            other.gameObject.SetActive(false);
            delivered();
        }
    }
}
