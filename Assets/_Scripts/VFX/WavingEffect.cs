using UnityEngine;

public class WavingEffect : MonoBehaviour
{
    public float waveSpeed = 1.0f;   // Speed of the wave
    public float waveAmplitude = 0.1f;   // Amplitude of the wave

    private Vector3 initialPosition;
    private float randomOffset;
    private float randomDirection;

    void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;

        // Generate random offset for starting position
        randomOffset = Random.Range(-1.0f, 1.0f);

        // Generate random direction for movement
        randomDirection = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
    }

    void Update()
    {
        // Calculate the new position on the Y-axis based on time and the sine function
        float waveMovement = Mathf.Sin((Time.time + randomOffset) * waveSpeed) * waveAmplitude;
        Vector3 newPosition = initialPosition + Vector3.up * (waveMovement * randomDirection);

        // Apply the new position to the object
        transform.position = newPosition;
    }
}
