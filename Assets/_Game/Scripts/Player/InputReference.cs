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

    private PlayerInputMap playerInputs;

    private void Start()
    {
        playerInputs = new PlayerInputMap();

        playerInputs.Gameplay.SetCallbacks(this);
        playerInputs.Enable();
    }

    private void Update()
    {
        //Old input
        //UpdateMovementValue();
        //UpdatePauseValue();
    }

    #region OLD INPUT
    private void UpdateMovementValue()
    {
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");

        Movement = new Vector2(inputX, inputY).normalized;
    }

    private void UpdatePauseValue()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            PauseButton.IsPressed = true;
            StartCoroutine(ResetButton(PauseButton));
        }

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
            PauseButton.IsPressed = false;
    }
    #endregion
   

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
        //StartCoroutine(ResetButton(ShootButton));
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseButton.IsPressed = context.ReadValueAsButton();
        StartCoroutine(ResetButton(PauseButton));
    }

    private IEnumerator ResetButton(InputButton button)
    {
        yield return new WaitForEndOfFrame();

        if (button.IsPressed)
            button.IsPressed = false;
    }
}
