using UnityEngine;

public class Regen : MonoBehaviour
{
    [SerializeField] private float lifeRegen = 0.2f;
    [SerializeField] private float timeToFill = 0.2f;

    private float _timer;
    private bool _calculate;

    private HealthSystem _health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out HealthSystem health))
        {
            _health = health;   
            _calculate = true;
        }
    }

    private void Update()
    {
        if (_calculate)
        {
            _timer += Time.deltaTime;

            if(_timer >= timeToFill)
            {
                _timer = 0;
                _health.Heal(lifeRegen);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _calculate = false;
    }
}
