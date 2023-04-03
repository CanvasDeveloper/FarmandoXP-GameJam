using UnityEngine;
using UnityEngine.InputSystem;

public class UiDetectGamepad : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    [SerializeField] private GameObject keyboard;
    [SerializeField] private GameObject xbox;
    [SerializeField] private GameObject ps;

    private InputDevice _lastDevice;

    private void Start()
    {
        Desactive();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                if (lastDevice == _lastDevice)
                    return;

                DisableAll();

                var alo = lastDevice.displayName.ToLower();

                if(alo.Contains("keyboard") || alo.Contains("mouse"))
                {
                    keyboard.SetActive(true);
                }

                if(alo.Contains("xbox"))
                {
                    xbox.SetActive(true);
                }

                if(alo.Contains("playstation") || alo.Contains("ps"))
                {
                    ps.SetActive(true);
                }

                _lastDevice = lastDevice;
            }
        };
    }

    private void DisableAll()
    {
        keyboard.SetActive(false);
        xbox.SetActive(false); 
        ps.SetActive(false);
    }

    public void Active()
    {
       canvas.SetActive(true);
    }

    public void Desactive()
    {
        canvas.SetActive(false);
    }
}
