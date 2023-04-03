using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private static readonly int DirectionXParam = Animator.StringToHash("directionX");
    private static readonly int DirectionYParam = Animator.StringToHash("directionY");
    private static readonly int AttackParam = Animator.StringToHash("attack");

    public Vector3 currentVelocity;
    public NavMeshAgent agent;
    
    public PlayerController player;
    public HealthSystem playerHealth;
    public Transform target;

    [SerializeField] private float damage = 1;
    [SerializeField] private float timeToAttack = 1;

    [SerializeField] private float stunnedTime = 2f;

    [SerializeField] private float maxDistance = 2;

    [SerializeField]
    private Animator anim;

    private bool hasHitted;
    private IDamageable health;

    private bool _isStunned;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = FindObjectOfType<PlayerController>();
        playerHealth = player.GetComponent<HealthSystem>();

        health = GetComponent<IDamageable>();

        target = player.transform;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        health.OnTakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= TakeDamage;
    }

    private void TakeDamage(Vector3 value)
    {
        StopAllCoroutines();
        StartCoroutine(Stunned());
    }

    private IEnumerator Stunned()
    {
        _isStunned = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(stunnedTime);

        agent.isStopped = false;
        _isStunned = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (_isStunned)
            return; 

        //if()

        agent.SetDestination(target.position);
        currentVelocity = agent.velocity;
        
        if (currentVelocity == Vector3.zero)
            return;

        if (Vector2.Distance(transform.position, target.position) < agent.stoppingDistance)
        {
            //perto da arvore
            if (!hasHitted)
            {
                anim.SetTrigger(AttackParam);

                playerHealth.TakeDamage(transform.position, damage);

                hasHitted = true;

                StartCoroutine(WaitForHit());
            }

            return;
        }

        UpdatePlayerScale();
        UpdateAnimator();
    }

    private void UpdatePlayerScale()
    {
        transform.localScale = new Vector3(Mathf.Sign(currentVelocity.x), 1, 1);
    }

    private IEnumerator WaitForHit()
    {
        yield return new WaitForSeconds(timeToAttack);
        hasHitted = false;
    }

    private void UpdateAnimator()
    {
        anim.SetFloat(DirectionXParam, currentVelocity.x);
        anim.SetFloat(DirectionYParam, currentVelocity.y);
    }
}
