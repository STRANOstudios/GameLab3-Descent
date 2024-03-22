using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slides : MonoBehaviour
{
    [Header("Slides")]
    [SerializeField] List<Sprite> sprites = new();
    [SerializeField, Range(0f, 10f)] float delay = 1f;

    private void Start()
    {

    }

    private IEnumerator DelayedMiniMap(float value = 1f)
    {
        mapIsActive = false;
        yield return new WaitForSeconds(value);
        mapIsActive = true;
    }
}
