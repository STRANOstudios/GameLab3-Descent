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
    private int index = 0;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        anouncerStartPos = announcer.position;

        if (sketch == null || sketch.Count == 0)
        {
            Debug.LogWarning("SketchItems list is empty. Exiting.");
            return;
        }

        StartCoroutine(ShowNextSketchItem());
    }

    private IEnumerator ShowNextSketchItem(int index = 0)
    {
        for (int i = index; i < sketch.Count; i++)
        {
            if(audioSource) audioSource.Stop();
            Sketch item = sketch[i];
            if (i != 0 && item.enemy && sketch[i - 1].enemy) sketch[i - 1].enemy.SetActive(false);
            index = i;

            float duration = 1.5f;
            if (item.enemy != null)
            {
                item.enemy.SetActive(true);
                StartCoroutine(ResetPosition(Time.time, duration, false, target.position));
            }
            else
            {
                StartCoroutine(ResetPosition(Time.time, duration, true, anouncerStartPos));
            }

            image.sprite = item.image;

            if (item.AudioClip && audioSource) audioSource.PlayOneShot(item.AudioClip);

            StartCoroutine(WriteText(item));

            yield return new WaitForSeconds(item.AudioClip ? item.AudioClip.length : item.delay);
        }
        LoadNextScene();
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

    public void Skip()
    {

        StopAllCoroutines();
        index++;
        StartCoroutine(ShowNextSketchItem(index));
    }
}

[Serializable]
public class Sketch
{
    public Sprite image;
    public string text;
    public float delay;
    public GameObject enemy = null;
    public AudioClip AudioClip = null;
}