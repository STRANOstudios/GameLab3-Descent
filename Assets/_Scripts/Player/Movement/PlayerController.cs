using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Player movement speed
    [SerializeField] private float rotationSpeed = 3f; // Player rotation speed
    [SerializeField] private float acceleration = 5f; // Player acceleration
    [SerializeField] private float deceleration = 0.5f; // Player deceleration

    void Update()
    {
        // Forward/backward movement
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * verticalInput;

        // Sideways movement
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection += transform.right * horizontalInput;

        // Applying acceleration/deceleration
        if (verticalInput != 0)
        {
            moveSpeed += acceleration * Time.deltaTime * Mathf.Sign(verticalInput);
        }
        else
        {
            // Deceleration when W or S is not pressed
            moveSpeed -= deceleration * Time.deltaTime;
        }

        // Clamp speed to minimum and maximum
        moveSpeed = Mathf.Clamp(moveSpeed, 0f, 20f);

        // Applying movement
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Rotation based on mouse input
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);

        // Rotation based on mouse input
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.right * mouseY * rotationSpeed);

        // Input for primary and secondary fire
        if (Input.GetMouseButtonDown(0))
        {
            PrimaryFire();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SecondaryFire();
        }
    }

    void PrimaryFire()
    {
        // Implementation of primary fire
        // Add your desired logic here
        Debug.Log("Primary Fire");
    }

    void SecondaryFire()
    {
        // Implementation of secondary fire
        // Add your desired logic here
        Debug.Log("Secondary Fire");
    }
}
