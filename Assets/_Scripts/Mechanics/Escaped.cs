using UnityEngine;

public class Escaped : MonoBehaviour
{
    public delegate void Free();
    public static event Free escaped;

    private void OnTriggerEnter(Collider other)
    {
        escaped?.Invoke();
    }
}
