using TMPro;
using UnityEngine;

public class HighestScore : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] TMP_Text text;

    public void SetScore()
    {
        int tmp = (int)GetSavedInt("Score");
        text.text = tmp.ToString("D5");
    }

    float GetSavedInt(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        return 0;
    }
}
