using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0.0f)] float health;
    [SerializeField, Min(0.0f)] float maxHealth = 200f;

    [Header("Boosts")]
    [SerializeField, Min(0.0f)] float boostshield = 50f;

    [Header("Core Damage")]
    [SerializeField] float damage = 10.0f;

    public delegate void Healt(int value);
    public static event Healt healt = null;

    public delegate void Death();
    public static event Death dead = null;

    private void Start()
    {
        healt?.Invoke((int)health);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 11:
                if (health >= maxHealth) return;
                health = Mathf.Clamp(health + boostshield, 0.0f, maxHealth);
                healt?.Invoke((int)health);
                other.gameObject.SetActive(false);
                break;
            case 14:
                Damage(other.gameObject.GetComponent<Bullet>().Damage);
                break;
            default:
                break;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Damage(damage);
    }

    public void Damage(float damage)
    {
        health -= damage;
        healt?.Invoke((int)health);

        if (health < 0.0f) dead?.Invoke();
    }
}
