using UnityEngine;

public class AutoUpdateOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform customPivot;
    [SerializeField] private float offsetZposition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        var targetPivot = customPivot ? customPivot : transform;

        spriteRenderer.sortingOrder = Mathf.RoundToInt(targetPivot.position.y) * -1;

        var cachedPosition = spriteRenderer.transform.position;

        spriteRenderer.transform.position = new Vector3(cachedPosition.x, cachedPosition.y, offsetZposition);
    }
}
