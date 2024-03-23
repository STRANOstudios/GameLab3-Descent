using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyHolder : MonoBehaviour
{
    [HideInInspector]public List<float> keyIDs = new List<float>();
    public delegate void KeyPickUp(float key_id);
    public event KeyPickUp OnKeyPickUp;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Key>(out var key))
        {
            keyIDs.Add(key.KeyID);
            OnKeyPickUp(key.KeyID);
        }
    }
}
