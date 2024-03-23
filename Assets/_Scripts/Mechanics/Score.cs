using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] int score = 0;

    public delegate void ObjectDeactivated(int value);
    public static event ObjectDeactivated OnObjectDeactivated;

    void OnDisable()
    {
        OnObjectDeactivated?.Invoke(score);
    }
}
