using System.Collections.Generic;
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

    void changeGun()
    {
        //to be implemented (?)
    }
}
