using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        if (muzzleVelocity != 0)
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
        if (muzzleVelocity != 0)
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
        else
        {
            if (Time.time > nextTimeToShoot && bulletMagazine > 0)
            {
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
            int hitLayer = hit.transform.gameObject.layer; // Recupera il layer dell'oggetto colpito

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

    public int BulletCharging
    {
        get { return bulletMagazine; }
        set { bulletMagazine += value; }
    }
}
