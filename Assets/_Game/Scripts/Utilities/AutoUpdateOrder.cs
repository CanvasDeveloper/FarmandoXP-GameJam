using UnityEngine;

public class AutoUpdateOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform customPivot;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        var targetPivot = customPivot ? customPivot : transform;

        spriteRenderer.sortingOrder = Mathf.RoundToInt(targetPivot.position.y) * -1;
    }
}
