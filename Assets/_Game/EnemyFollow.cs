using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private static readonly int DirectionXParam = Animator.StringToHash("directionX");
    private static readonly int DirectionYParam = Animator.StringToHash("directionY");
    private static readonly int AttackParam = Animator.StringToHash("attack");
    private static readonly int isTakeDamage = Animator.StringToHash("isTakeDamage");

    public Vector3 currentVelocity;
    public NavMeshAgent agent;
    
    public PlayerController player;
    public HealthSystem playerHealth;
    public Transform target;

    [SerializeField] BoxCollider2D colDamage; // collider que dá dano no player
    [SerializeField] private float timeToAttack = 1;

    [SerializeField] private float stunnedTime = 2f;

    [SerializeField] private float maxDistance = 2;

    [SerializeField]
    private Animator anim;

    private bool hasHitted;
    private IDamageable health;

    private bool _isStunned;
    private int _index;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        player = FindObjectOfType<PlayerController>();
        playerHealth = player.GetComponent<HealthSystem>();

        health = GetComponent<IDamageable>();

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
        colDamage.enabled = false;
        anim.SetBool(isTakeDamage, _isStunned);

        yield return new WaitForSeconds(stunnedTime);

        agent.isStopped = false;
        _isStunned = false;
        colDamage.enabled = true;
        anim.SetBool(isTakeDamage, _isStunned);
    }


    // Update is called once per frame
    void Update()
    {
        if (_isStunned)
            return;

        if (target)
            return;

        agent.SetDestination(target.position);
        currentVelocity = agent.velocity;
        
        if (currentVelocity == Vector3.zero)
            return;

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

    public void SetSpawnIndex(int index)
    {
        _index = index;
    }

    public void Visible()
    {
        target = player.transform;
    }

    public void Invisible()
    {
        target = null;
        EnemySpawnManager.Instance.AddToAvaliable(this, _index);
    }
}
