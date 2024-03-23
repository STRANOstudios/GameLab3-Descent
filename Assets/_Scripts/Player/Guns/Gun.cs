using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Sprite sprite;

    [Tooltip("Projectile force")]
    [SerializeField] float muzzleVelocity = 700f;
    [Tooltip("Use spherecast to shoot")]
    [SerializeField] bool spherecast = false;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField] List<Transform> muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] float cooldownWindow = 0.1f;

    private IObjectPool<Projectile> objectPool;

    [SerializeField] bool collectionCheck = true;

    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;

    [SerializeField] float bulletMagazine = 0;

    private float nextTimeToShoot;

    public delegate void Laser(int value);
    public static event Laser shoot = null;

    private void Awake()
    {
        if (!spherecast)
            objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    private void Start()
    {
        if (this.name == "Laser")
        {
            shoot?.Invoke(Mathf.CeilToInt(bulletMagazine));
        }
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

    public void Shoot()
    {
        if (!spherecast)
        {
            if (Time.time > nextTimeToShoot && objectPool != null && bulletMagazine > 0)
            {
                Vector3 cameraForward = Camera.main.transform.forward;

                for (int i = 0; i < muzzlePosition.Count; i++)
                {
                    Projectile bulletObject = objectPool.Get();

                    if (bulletObject == null) return;

                    bulletObject.transform.position = muzzlePosition[0].transform.position;

                    bulletObject.transform.rotation = Quaternion.LookRotation(cameraForward);

                    bulletObject.GetComponent<Rigidbody>().AddForce(cameraForward * muzzleVelocity, ForceMode.Acceleration);
                    bulletObject.Deactivate();
                }

                nextTimeToShoot = Time.time + cooldownWindow;

                if (this.name == "Laser")
                {
                    bulletMagazine -= 0.25f;
                    shoot?.Invoke(Mathf.CeilToInt(bulletMagazine));
                }
                else
                {
                    bulletMagazine--;
                }
            }
        }
        else
        {
            if (Time.time > nextTimeToShoot && bulletMagazine > 0)
            {
                nextTimeToShoot = Time.time + cooldownWindow;
                bulletMagazine--;

                StartCoroutine(ShootSphereCast());
            }
        }
    }

    IEnumerator ShootSphereCast()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        float radius = 0.5f;
        RaycastHit hit;

        if (Physics.SphereCast(origin, radius, direction, out hit, muzzleVelocity))
        {
            int hitLayer = hit.transform.gameObject.layer;

            switch (hitLayer)
            {
                case 0:
                    break;
                case 12:
                    break;
                default:
                    StopCoroutine(ShootSphereCast());
                    break;
            }
        }

        yield return new WaitForSeconds(projectilePrefab.GetRange);
    }

    public void UpdateMonitor()
    {
        shoot?.Invoke(Mathf.CeilToInt(bulletMagazine));
    }

    public int BulletMagazine
    {
        get { return (int)bulletMagazine; }
        set { bulletMagazine = value; }
    }

    public Sprite GetSprite => sprite;

    public int MagazineBullet => (int)bulletMagazine;
}
