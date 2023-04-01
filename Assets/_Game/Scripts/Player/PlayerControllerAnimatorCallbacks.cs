using UnityEngine;

public class PlayerControllerAnimatorCallbacks : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void UpdateToDown() => playerController.UpdateToDown();
    public void UpdateToUp() => playerController.UpdateToUp();
    public void UpdateToSide() => playerController.UpdateToSide();

}
