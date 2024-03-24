using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] float rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
