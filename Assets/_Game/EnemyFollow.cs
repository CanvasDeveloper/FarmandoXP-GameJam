using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private static readonly int DirectionXParam = Animator.StringToHash("directionX");
    private static readonly int DirectionYParam = Animator.StringToHash("directionY");

    public Vector3 currentVelocity;
    public NavMeshAgent agent;
    public Transform target;

    [SerializeField]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>().transform;

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
        UpdatePlayerScale();
        UpdateAnimator();
    }


    private void UpdatePlayerScale()
    {
        transform.localScale = new Vector3(Mathf.Sign(currentVelocity.x), 1, 1);
    }

    private void UpdateAnimator()
    {
        anim.SetFloat(DirectionXParam, currentVelocity.x);
        anim.SetFloat(DirectionYParam, currentVelocity.y);
    }
}
