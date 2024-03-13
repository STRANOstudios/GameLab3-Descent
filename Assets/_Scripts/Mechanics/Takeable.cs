using UnityEngine;

public class Takeable : MonoBehaviour
{
    [Header("Takeable Settings")]
    [Tooltip("it's the associated gun")]
    [SerializeField] GameObject gun;
    [SerializeField] int bulletMagazine;

    public GameObject Gun => gun;

    public int BulletMagazine => bulletMagazine;
}
