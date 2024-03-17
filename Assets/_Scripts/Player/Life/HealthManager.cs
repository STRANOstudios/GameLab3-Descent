using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField, Min(0.0f)] float maxHealth;

    public delegate void Healt();
    public static event Healt Death = null;

    public void Damage(float damage)
    {
        maxHealth -= damage;

        if (maxHealth <= 0.0f) Death?.Invoke();
    }
}
