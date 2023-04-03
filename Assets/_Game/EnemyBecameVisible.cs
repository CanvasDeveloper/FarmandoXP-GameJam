using UnityEngine;

public class EnemyBecameVisible : MonoBehaviour
{
    private EnemyFollow enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyFollow>();
    }

    private void OnBecameInvisible()
    {
        enemy.Invisible();
    }

    private void OnBecameVisible()
    {
        enemy.Visible();
    }
}
