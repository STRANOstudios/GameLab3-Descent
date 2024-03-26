using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    private IObjectPool<Projectile> objectPool;

    [Header("Debug")]
    [SerializeField] bool collectionCheck = true;
    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;
    [SerializeField] float bulletMagazine = 50;

    private float nextTimeToShoot;

    public delegate void Laser(int value);
    public static event Laser shoot = null;

    public delegate void LaserGoliath(float value);
    public static event LaserGoliath setMagazine = null;

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
            setMagazine?.Invoke(bulletMagazine);
        }
    }

    private void OnEnable()
    {
        Gun.setMagazine += SetBullet;
    }

    private void OnDisable()
    {
        Gun.setMagazine -= SetBullet;
    }

    void SetBullet(float value)
    {
        if (this.name == "Laser" || this.name == "Goliath") bulletMagazine = value;
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
                bulletMagazine -= 1f;
                StartCoroutine(Fire());
            }
            else if (this.name == "Goliath")
            {
                if (bulletMagazine - 5f >= 0)
                {
                    bulletMagazine -= 5f;
                    StartCoroutine(Fire());
                }
            }
            else
            {
                bulletMagazine--;
            }
        }

        if (audioSource != null) audioSource.Play();
    }

    IEnumerator Fire(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        shoot?.Invoke(Mathf.CeilToInt(bulletMagazine));
        setMagazine?.Invoke(bulletMagazine);
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

    public int MagazineBullet
    {
        get { return (int)bulletMagazine; }
        set { bulletMagazine = value; }
    }
}
