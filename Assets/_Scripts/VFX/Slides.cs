using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slides : MonoBehaviour
{
    [Header("Slides")]
    [SerializeField] string _newGameLevel;
    [SerializeField] Image image;
    [SerializeField] List<Sprite> sprites = new();
    [SerializeField, Range(0f, 10f)] float delay = 1f;

    private void Start()
    {
        if (sprites.Count <= 0) return;
        for (int i = 0; i < sprites.Count; i++)
        {
            image.sprite = sprites[i];
            Delay(delay);
        }
#if UNITY_EDITOR
        SceneManager.LoadScene(_newGameLevel);
#else
        SceneManager.LoadScene(2); //3
#endif
    }

    private IEnumerator Delay(float value = 1f)
    {
        yield return new WaitForSeconds(delay);
    }
}
