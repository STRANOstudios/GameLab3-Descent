using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] Projectile projectilePrefab;

    [Tooltip("Projectile force, if it be 0, it will be circlecast")]
    [SerializeField] float muzzleVelocity = 700f;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField] List<Transform> muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] float cooldownWindow = 0.1f;

    private IObjectPool<Projectile> objectPool;

    [SerializeField] bool collectionCheck = true;

    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;

    [SerializeField] int bulletMagazine = 0;

    private float nextTimeToShoot;

    private void Awake()
    {
        if (muzzleVelocity == 0)
        {
            //circlecast to be implement
        }
        else { objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize); }
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
        if (Time.time > nextTimeToShoot && objectPool != null && bulletMagazine > 0)
        {
            Projectile bulletObject = objectPool.Get();

            if (bulletObject == null) return;

            bulletObject.transform.position = muzzlePosition[0].transform.position;

            Vector3 cameraForward = Camera.main.transform.forward;
            bulletObject.transform.rotation = Quaternion.LookRotation(cameraForward);

            bulletObject.GetComponent<Rigidbody>().AddForce(cameraForward * muzzleVelocity, ForceMode.Acceleration);
            bulletObject.Deactivate();

            nextTimeToShoot = Time.time + cooldownWindow;

            bulletMagazine--;
        }
    }

    public int BulletCharging
    {
        get { return bulletMagazine; }
        set { bulletMagazine += value; }
    }
}
