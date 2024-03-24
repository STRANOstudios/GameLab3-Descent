using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("The amount of damage dealt to the target.")]
    [SerializeField] float damage;

    [Tooltip("The range of the projectile.")]
    [SerializeField] float range;

    [Header("VFX")]
    [SerializeField] ParticleSystem impactVFX = null;

    private IObjectPool<Projectile> objectPool;

    public IObjectPool<Projectile> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(range));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactVFX != null) impactVFX.Play();

        StopAllCoroutines();
        Reset();

        Debug.Log(collision.gameObject.name + " has been hit.");
    }

    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        objectPool.Release(this);
    }

    public float GetDamage => damage;

    public float GetRange => range;
}
