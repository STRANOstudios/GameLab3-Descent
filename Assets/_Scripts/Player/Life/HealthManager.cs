using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0.0f)] float health;

    public delegate void Healt(int value);
    public static event Healt healt = null;

    public delegate void Death();
    public static event Death dead = null;

    private void Start()
    {
        healt?.Invoke((int)health);
    }

    public void Damage(float damage)
    {
        health -= damage;
        healt?.Invoke((int)health);

        if (health < 0.0f) dead?.Invoke();
    }
}
