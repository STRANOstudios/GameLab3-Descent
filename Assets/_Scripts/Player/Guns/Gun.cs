using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] Projectile projectilePrefab;

    [Tooltip("Projectile force")]
    [SerializeField] float muzzleVelocity = 700f;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField] List<Transform> muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] float cooldownWindow = 0.1f;

    private IObjectPool<Projectile> objectPool;

    [SerializeField] bool collectionCheck = true;

    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;

    private float nextTimeToShoot;

    private void Awake()
    {
        objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.ObjectPool = objectPool;
        return projectileInstance;
    }

    public void shoot()
    {
        if (Time.time > nextTimeToShoot && objectPool != null)
        {
            Projectile bulletObejct = objectPool.Get();

            if (bulletObejct == null) return;

            bulletObejct.transform.position = muzzlePosition[0].transform.position;
            bulletObejct.transform.rotation = muzzlePosition[0].transform.rotation;

            bulletObejct.GetComponent<Rigidbody>().AddForce(bulletObejct.transform.forward * muzzleVelocity, ForceMode.Acceleration);
            bulletObejct.Deactivate();

            nextTimeToShoot = Time.time + cooldownWindow;
        }
    }
}
