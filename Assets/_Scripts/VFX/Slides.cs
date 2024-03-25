using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slides : MonoBehaviour
{
    [Header("Slides")]
    [SerializeField] string _newGameLevel;
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
    [SerializeField] CanvasGroup enemyPanel;

    [Header("Settings")]
    [SerializeField, Min(0)] float delayEndWrting = 1f;

    [Header("VFX")]
    [SerializeField] Transform announcer;
    [SerializeField] Transform target;

    [Space]

    [SerializeField] List<Sketch> sketch = new();

    private void Start()
    {
        if (sketch == null || sketch.Count == 0)
        {
            Debug.LogWarning("SketchItems list is empty. Exiting.");
            return;
        }

        StartCoroutine(ShowNextSketchItem());
    }

    private IEnumerator ShowNextSketchItem()
    {
        foreach (Sketch item in sketch)
        {
            if (item.enemy != null) StartCoroutine(EnemyVfx(item));

            image.sprite = item.image;

            StartCoroutine(WriteText(item));

            yield return new WaitForSeconds(item.delay);
        }
        LoadNextScene();
    }

    private IEnumerator EnemyVfx(Sketch item)
    {
        item.enemy.SetActive(true);

        float duration = item.delay * 0.1f;
        float endTime = Time.time + item.delay - duration;

        Vector3 startPosition = announcer.position;
        Vector3 endPosition = target.position;

        // Animazione di comparsa e movimento
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float percentageComplete = (Time.time - startTime) / duration;
            announcer.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            enemyPanel.alpha += percentageComplete;
            yield return null;
        }

        yield return new WaitForSeconds(duration * 7.5f);

        while (Time.time < endTime)
        {
            float reverseStartTime = Time.time;
            float reverseEndTime = Time.time + duration;

            while (Time.time < reverseEndTime)
            {
                float percentageComplete = (Time.time - reverseStartTime) / duration;
                announcer.position = Vector3.Lerp(endPosition, startPosition, percentageComplete);
                enemyPanel.alpha -= percentageComplete;
                yield return null;
            }
        }

        item.enemy.SetActive(false);
    }


    private void LoadNextScene()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(_newGameLevel);
#else
        SceneManager.LoadScene(2); // Change to the appropriate scene index in the build settings
#endif
    }

    private IEnumerator WriteText(Sketch item)
    {
        float timePerCharacter = (item.delay - delayEndWrting) / item.text.Length;

        text.text = "";

        foreach (char c in item.text)
        {
            text.text += "" + c;
            yield return new WaitForSeconds(timePerCharacter);
        }
    }
}

[Serializable]
public class Sketch
{
    public Sprite image;
    public string text;
    public float delay;
    public GameObject enemy = null;
}