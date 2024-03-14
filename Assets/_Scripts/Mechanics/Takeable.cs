using UnityEngine;

public class Takeable : MonoBehaviour
{
    [Header("Takeable Settings")]
    [Tooltip("it's the associated gun")]
    [SerializeField] GameObject gun;
    [SerializeField] int bulletMagazine;
    [Tooltip("true, it's a primary weapon | false, it's a secondary weapon")]
    [SerializeField] bool primaryOrSecondary = true;

    public GameObject Gun => gun;

    public int BulletMagazine => bulletMagazine;

    public bool PrimaryOrSecondary => primaryOrSecondary;
}
