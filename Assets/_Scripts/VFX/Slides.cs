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

    private Vector3 anouncerStartPos;

    private void Start()
    {
        anouncerStartPos = announcer.position;

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
            float duration = item.delay * 0.1f;
            if (item.enemy != null)
            {
                //StartCoroutine(EnemyVfx(item));
                item.enemy.SetActive(true);
                StartCoroutine(ResetPosition(Time.time, duration, false, target.position));
            }
            else
            {
                StartCoroutine(ResetPosition(Time.time, duration, true, anouncerStartPos));
            }

            image.sprite = item.image;

            StartCoroutine(WriteText(item));

            yield return new WaitForSeconds(item.delay);
            if (item.enemy != null) item.enemy.SetActive(false);
        }
        LoadNextScene();
    }

    private IEnumerator EnemyVfx(Sketch item)
    {
        item.enemy.SetActive(true);

        float duration = item.delay * 0.1f;

        StartCoroutine(ResetPosition(Time.time, duration, false, target.position));

        yield return new WaitForSeconds(duration * 7.5f);

        StartCoroutine(ResetPosition(Time.time, duration, true, anouncerStartPos));

        item.enemy.SetActive(false);
    }

    IEnumerator ResetPosition(float startTime, float duration, bool reverse, Vector3 endPosition)
    {
        while (Time.time - startTime < duration)
        {
            float percentageComplete = (Time.time - startTime) / duration;
            announcer.position = Vector3.Lerp(announcer.position, endPosition, percentageComplete);
            enemyPanel.alpha += reverse ? -percentageComplete : percentageComplete;
            yield return null;
        }
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