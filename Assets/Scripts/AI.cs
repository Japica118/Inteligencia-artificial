using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,

        Chasing,

        Waiting,

        Attacking
    }

    State currentState;

    NavMeshAgent agent;

    public Transform[] destinationPoints;
    
    int destinationIndex = 0;

    public float visionRange;

    public Transform player;

    [SerializeField] private int sec;

    private float remaining = 5f;

    [SerializeField] private float attackRange;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
            default:
                Chase();
            break;
        
            case State.Waiting:
                Wait();
            break;
            case State.Attacking:
                Attack();
            break;
        }
    }

    
    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1)
        {
            if(destinationIndex < destinationPoints.Length)
            {
                destinationIndex += 1;
            }
            
            if(destinationIndex == destinationPoints.Length)
            {
                destinationIndex = 0;
            }
            
            currentState = State.Waiting;
        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }

         if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            Debug.Log("Estoy atacando");

            currentState = State.Attacking;
        }
    }

    void Wait()
        {

            remaining -= Time.deltaTime;

            if(remaining <= 0)
            {
                currentState = State.Patrolling;

                remaining = 5;
            }

            
        }
    
    void Attack()
    {
        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = State.Chasing;
        }
    }
    

    void OnDrawGizmos()
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    
}
