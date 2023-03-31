using UnityEngine;

public class FMODHelthSystemIntegration : MonoBehaviour
{
    private HealthSystem healthSystem;

    public string eventName;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDie += DieTrigger;
    }

    private void OnDisable()
    {
        healthSystem.OnDie -= DieTrigger;
    }

    private void DieTrigger(IDamageable value)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName, transform.position);
    }
}
