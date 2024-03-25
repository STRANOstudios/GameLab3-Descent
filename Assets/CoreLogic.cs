using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreLogic : MonoBehaviour
{
    [Header("Core Specs")]
    [SerializeField] float health = 100f;

    [Header("Fire")]
    [SerializeField] List<ParticleSystem> nozzles = new();
    [SerializeField] float fireRatio = 1f;

    [Header("VFX")]
    [SerializeField] float rotationSpeed = 1f;

    public delegate void Core();
    public static event Core death = null;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        Door.bossfight += Fire;
    }

    private void OnDisable()
    {
        Door.bossfight -= Fire;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Damage(collision.transform.GetComponent<Projectile>().GetDamage);
        }
    }

    void Damage(float value)
    {
        health -= value;
        if (health <= 0)
        {
            death?.Invoke();
            gameObject.SetActive(false);
        }
    }

    void Fire()
    {
        foreach (var nozzle in nozzles)
        {
            nozzle.gameObject.SetActive(true);
        }
        StartCoroutine(Wait(fireRatio));
    }

    IEnumerator Wait(float delay = 0.3f)
    {
        yield return new WaitForSeconds(delay);
        Fire();
    }
}
