using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] Projectile projectilePrefab;

    [Tooltip("Projectile force")]
    [SerializeField] float muzzleVelocity = 700f;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField] Transform muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] float cooldownWindow = 0.1f;

    private PlayerInputHadler inputHandler;
    private IObjectPool<Projectile> objectPool;

    [SerializeField] bool collectionCheck = true;

    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;

    private float nextTimeToShoot;

    private void Awake()
    {
        inputHandler = PlayerInputHadler.Instance;

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

    private void FixedUpdate()
    {
        Primary();
        Secondary();
    }

    void Primary()
    {
        if (inputHandler.fire1Trigger && Time.time > nextTimeToShoot && objectPool != null)
        {
            Projectile bulletObejct = objectPool.Get();

            if (bulletObejct == null) return;

            bulletObejct.GetComponent<Rigidbody>().AddForce(bulletObejct.transform.forward * muzzleVelocity, ForceMode.Acceleration);
            bulletObejct.Deactivate();

            nextTimeToShoot = Time.time + cooldownWindow;
        }
    }

    void Secondary()
    {
        if (inputHandler.fire2Trigger) { }
    }
}
