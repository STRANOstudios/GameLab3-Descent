using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [Header("Shooting Manager")]
    [SerializeField] List<Gun> primaryGuns;
    [SerializeField] List<Gun> secondaryGuns;

    [SerializeField] Gun bomb;
    [SerializeField] Gun flare;

    private PlayerInputHadler inputHandler;

    private int primaryGunEnable = 0;
    private int secondaryGunEnable = 0;

    private void Awake()
    {
        inputHandler = PlayerInputHadler.Instance;
    }

    private void FixedUpdate()
    {
        Primary();
        Secondary();

        Bomb();
        Flare();

        changeGunPrimary();
        changeGunSecondary();
    }

    void Primary()
    {
        if (primaryGuns.Count <= 0) return;
        if (inputHandler.fire1Trigger)
        {
            primaryGuns[primaryGunEnable].shoot();
            Debug.Log("shoot primary");
        }
    }

    void Secondary()
    {
        if (secondaryGuns.Count <= 0) return;
        if (inputHandler.fire2Trigger)
        {
            secondaryGuns[secondaryGunEnable].shoot();
            Debug.Log("shoot secondary");
        }
    }

    void Bomb()
    {
        if (bomb == null) return;
        if (inputHandler.bombTrigger)
        {
            bomb.shoot();
            Debug.Log("shoot bomb");
        }
    }

    void Flare()
    {
        if (flare == null) return;
        if (inputHandler.flareTrigger)
        {
            flare.shoot();
            Debug.Log("shoot flare");
        }
    }

    void changeGunPrimary()
    {
        if (primaryGuns.Count <= 1) return;
        if (inputHandler.list1Value != 0)
        {
            primaryGunEnable += (int)inputHandler.list1Value;
            primaryGunEnable = primaryGunEnable >= primaryGuns.Count ? primaryGunEnable = 0 : primaryGunEnable;
            primaryGunEnable = primaryGunEnable < 0 ? primaryGuns.Count - 1 : primaryGunEnable;
            Debug.Log("change primary");
        }
        StartCoroutine(DelayButton(0.1f));
    }

    void changeGunSecondary()
    {
        if (secondaryGuns.Count <= 1) return;
        if (inputHandler.list2Value != 0)
        {
            secondaryGunEnable += (int)inputHandler.list2Value;
            secondaryGunEnable = secondaryGunEnable >= secondaryGuns.Count ? secondaryGunEnable = 0 : secondaryGunEnable;
            secondaryGunEnable = secondaryGunEnable < 0 ? secondaryGuns.Count - 1 : secondaryGunEnable;
            Debug.Log("change primary");
        }
        StartCoroutine(DelayButton(0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletMagazine"))
        {
            Takeable tmp = other.gameObject.GetComponent<Takeable>();
            foreach (Gun gun in primaryGuns)
            {
                if (gun.name == tmp.Gun.name)
                {
                    gun.BulletCharging = tmp.BulletMagazine;
                    break;
                }
            }
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator DelayButton(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}