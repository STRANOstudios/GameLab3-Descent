using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyHolder : MonoBehaviour
{
    [HideInInspector] public List<float> keyIDs = new List<float>();

    public delegate void KeyPickUp(int key_id);
    public static event KeyPickUp OnKeyPickUp;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "KeyRed":
                Push(1, other);
                break;
            case "KeyYellow":
                Push(2, other);
                break;
            case "KeyBlue":
                Push(3, other);
                break;
            default:
                break;
        }
    }

    void Push(int index, Collider other)
    {
        keyIDs.Add(index);
        OnKeyPickUp?.Invoke(index);
        other.gameObject.SetActive(false);
    }
}
