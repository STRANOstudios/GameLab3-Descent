using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTextColorOnButtonSelect : MonoBehaviour
{
    [Header("Change Text Color On Button Select")]
    [SerializeField] Color selectedColor = Color.red;
    [SerializeField] TMP_Text buttonText;
    private Color originalColor;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (!button && !buttonText) return;
        originalColor = buttonText.color;




    }

    void OnEnable()
    {
        button.onClick.AddListener(OnSelect);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(OnDeselect);
    }

    void OnSelect()
    {
        buttonText.color = selectedColor;
    }

    void OnDeselect()
    {
        buttonText.color = originalColor;
    }
}
