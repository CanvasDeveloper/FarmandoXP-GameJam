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

    [SerializeField]
    private Animator anim;

    private bool hasHitted;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = FindObjectOfType<PlayerController>();
        playerHealth = player.GetComponent<HealthSystem>();

        target = player.transform;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
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
