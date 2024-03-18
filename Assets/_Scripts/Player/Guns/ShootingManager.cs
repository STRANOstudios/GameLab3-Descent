using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [Header("Shooting Manager")]
    [SerializeField] GameObject inventary;

    [SerializeField] List<Gun> primaryGuns;
    [SerializeField] List<Gun> secondaryGuns;

    [SerializeField] Gun bomb;
    [SerializeField] Gun flare;

    private PlayerInputHadler inputHandler;

    private int primaryGunEnable = 0;
    private int secondaryGunEnable = 0;

    private bool DelayP = true;
    private bool DelayS = true;

    public delegate void GunEdit(bool isPrimary, Sprite sprite, string name, int bulletMagazine);
    public static event GunEdit Gun = null;

    private void Awake()
    {
        inputHandler = PlayerInputHadler.Instance;
    }

    private void Start()
    {
        UpdateMonitors();
    }

    private void FixedUpdate()
    {
        Primary();
        Secondary();

        Bomb();
        //Flare();

        changeGunPrimary();
        changeGunSecondary();
    }

    void Primary()
    {
        if (primaryGuns.Count <= 0) return;
        if (inputHandler.fire1Trigger)
        {
            primaryGuns[primaryGunEnable].Shoot();
        }
    }

    void Secondary()
    {
        if (secondaryGuns.Count <= 0) return;
        if (inputHandler.fire2Trigger)
        {
            secondaryGuns[secondaryGunEnable].Shoot();
        }
    }

    void Bomb()
    {
        if (bomb == null) return;
        if (inputHandler.bombTrigger)
        {
            bomb.Shoot();
        }
    }

    void Flare()
    {
        if (flare == null) return;
        if (inputHandler.flareTrigger)
        {
            flare.Shoot();
        }
    }

    void changeGunPrimary()
    {
        if (primaryGuns.Count <= 1) return;
        if (inputHandler.list1Value != 0 && DelayP)
        {
            StartCoroutine(DelayChangeP());
            primaryGunEnable += (int)inputHandler.list1Value;
            primaryGunEnable = primaryGunEnable >= primaryGuns.Count ? primaryGunEnable = 0 : primaryGunEnable;
            primaryGunEnable = primaryGunEnable < 0 ? primaryGuns.Count - 1 : primaryGunEnable;
            Debug.Log("change primary");
        }
        UpdateMonitors();
        StartCoroutine(DelayButton(0.1f));
    }

    IEnumerator DelayChangeP(float delay = 0.3f)
    {
        DelayP = false;
        yield return new WaitForSeconds(delay);
        DelayP = true;
    }

    void changeGunSecondary()
    {
        if (secondaryGuns.Count <= 1) return;
        if (inputHandler.list2Value != 0 && DelayS)
        {
            StartCoroutine(DelayChangeS());
            secondaryGunEnable += (int)inputHandler.list2Value;
            secondaryGunEnable = secondaryGunEnable >= secondaryGuns.Count ? secondaryGunEnable = 0 : secondaryGunEnable;
            secondaryGunEnable = secondaryGunEnable < 0 ? secondaryGuns.Count - 1 : secondaryGunEnable;
            Debug.Log("change primary");
        }
        UpdateMonitors();
    }

    IEnumerator DelayChangeS(float delay = 0.3f)
    {
        DelayS = false;
        yield return new WaitForSeconds(delay);
        DelayS = true;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "BulletMagazine":
                GetBullets(other.gameObject);
                break;
            case "Gun":
                GetGun(other.gameObject);
                break;
            default:
                break;
        }
    }

    void GetBullets(GameObject other)
    {
        Takeable tmp = other.GetComponent<Takeable>();

        if (tmp.PrimaryOrSecondary ? primaryGuns.Count <= 0 : secondaryGuns.Count <= 0) return;

        foreach (Gun gun in tmp.PrimaryOrSecondary ? primaryGuns : secondaryGuns)
        {
            if (gun.name == tmp.Gun.name)
            {
                gun.BulletCharging = tmp.BulletMagazine;
                break;
            }
        }
        other.SetActive(false);
    }

    void GetGun(GameObject other)
    {
        Takeable tmp = other.GetComponent<Takeable>();

        GameObject gun = Instantiate(tmp.Gun, inventary.transform);

        if (tmp.PrimaryOrSecondary) primaryGuns.Add(gun.GetComponent<Gun>());
        if (!tmp.PrimaryOrSecondary) secondaryGuns.Add(gun.GetComponent<Gun>());

        gun.name = tmp.Gun.name;

        other.SetActive(false);
    }

    IEnumerator DelayButton(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void UpdateMonitors()
    {
        if (primaryGuns.Count > 0) Gun?.Invoke(true, primaryGuns[primaryGunEnable].GetSprite, primaryGuns[primaryGunEnable].name, primaryGuns[primaryGunEnable].MagazineBullet);
        if (secondaryGuns.Count > 0) Gun?.Invoke(false, secondaryGuns[primaryGunEnable].GetSprite, secondaryGuns[primaryGunEnable].name, secondaryGuns[primaryGunEnable].MagazineBullet);
    }
}