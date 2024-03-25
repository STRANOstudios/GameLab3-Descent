using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStats : MonoBehaviour
{
    [SerializeField] float timerToExplode;
    [SerializeField] Transform explodeParticles;
    [SerializeField] float damage;
    [SerializeField, Range(0, 5)] float radiusExplosion;
    [SerializeField] LayerMask layersHitByExplosion;
    List<Collider> hitObjects = new List<Collider>();

    private void Start()
    {
        StartCoroutine(ExplodeOnNothing());
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 6) || (other.gameObject.layer == 8) || (other.gameObject.layer == 13) || (other.gameObject.layer == 14))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explodeParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator ExplodeOnNothing()
    {
        yield return new WaitForSeconds(timerToExplode);
        Explode();
    }

    private void OnDestroy()
    {
        hitObjects.AddRange(Physics.OverlapSphere(transform.position, radiusExplosion, layersHitByExplosion));
        
        foreach (Collider c in hitObjects)
        {
            if (c.TryGetComponent<HP> (out  HP hp))
            {
                hp.myHP -= damage;

                if (hp.myHP <= 0)
                {
                    hp.Death();
                }
            }

            else if(c.TryGetComponent<HealthManager> (out HealthManager manager))
            {
                manager.Damage(damage);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }

#endif
}
