using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0.0f)] float health;
    [SerializeField, Min(0.0f)] float maxHealth = 200f;

    [Header("Boosts")]
    [SerializeField, Min(0.0f)] float boostshield = 50f;

    [Header("Damages")]
    [SerializeField] float coreDamage = 10.0f;
    [SerializeField] float lavaDamage = 10.0f;

    private bool direIsEvile = true;

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
            case 15:
                direIsEvile = true;
                StartCoroutine(Lava(0.5f));
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 15:
                direIsEvile = false;
                StopCoroutine("Lava");
                break;
        }
    }

    IEnumerator Lava(float delay)
    {
        while (direIsEvile) 
        {
            Damage(lavaDamage);
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Damage(coreDamage);
    }

    public void Damage(float damage)
    {
        health -= damage;
        healt?.Invoke((int)health);

        if (health < 0.0f) dead?.Invoke();
    }
}
