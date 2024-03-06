using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] GameObject defaultSelectedObject;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultSelectedObject);
    }

    private void Update()
    {
        if(Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedObject);
        }
    }
}
