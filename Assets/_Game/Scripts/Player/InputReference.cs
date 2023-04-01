using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// Simula um botao, para evitar que seja trigado mais de uma vez no update
/// </summary>
public class InputButton
{
    public bool IsPressed;
}

/// <summary>
/// Usado pelo player
/// </summary>
public class InputReference : MonoBehaviour, PlayerInputMap.IGameplayActions
{
    //quero pegar o valor, mas nao deixo ninguem de fora atualizar
    public Vector2 Movement { get; private set; } = Vector2.zero;
    public Vector2 MousePosition { get; private set; } = Vector2.zero;
    public InputButton PauseButton { get; private set; } = new InputButton();
    public InputButton ShootButton { get; private set; } = new InputButton();
    public InputButton DashButton { get; private set; } = new InputButton();

    private PlayerInputMap playerInputs;

    private void Start()
    {
        playerInputs = new PlayerInputMap();

        playerInputs.Gameplay.SetCallbacks(this);
        playerInputs.Enable();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        MousePosition = new Vector2(input.x, input.y);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        Movement = new Vector2(input.x, input.y).normalized;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        ShootButton.IsPressed = context.ReadValueAsButton();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseButton.IsPressed = context.ReadValueAsButton();
        StartCoroutine(ResetButton(PauseButton));
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        DashButton.IsPressed = context.ReadValueAsButton();
        StartCoroutine(ResetButton(DashButton));
    }

    private IEnumerator ResetButton(InputButton button)
    {
        yield return new WaitForEndOfFrame();

        if (button.IsPressed)
            button.IsPressed = false;
    }

    
}
