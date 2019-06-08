using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    public enum MonsterState {idle, trace, attack, die};
    public MonsterState monsterState = MonsterState.idle;
    public float wanderSpeed = 0.2f;
    public float chaseSpeed = 3f;
    public float StateRadius = 10f;
    public Transform[] waypoints;
    // Start is called before the first frame update
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;
    private Vector3 StatePoint;
    private Renderer renderer;
    private int waypointIndex = 0;
    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
        renderer = GetComponent<Renderer>();
        StatePoint = RandomWanderPoint();
       
    }
    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * StateRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, StateRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
    public void Wander()
    {
        nvAgent.SetDestination(playerTr.transform.position);
        if (monsterState == MonsterState.idle)
        {
            if (Vector3.Distance(transform.position, StatePoint) < 2f)
            {
                StatePoint = RandomWanderPoint();
            }
            else
            {
                nvAgent.SetDestination(StatePoint);
            }
        }
        else
        {
            //Waypoint wandering
            if (waypoints.Length >= 2)
            {
                if (Vector3.Distance(waypoints[waypointIndex].position, transform.position) < 7f)
                {
                    if (waypointIndex == waypoints.Length - 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        waypointIndex++;
                    }
                }
                else
                {
                    nvAgent.SetDestination(waypoints[waypointIndex].position);
                }
            }
            else
            {
                Debug.LogWarning("Please assign more than 1 waypoint to the AI: " + gameObject.name);
            }
        }
    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);
            if(dist<=attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else if(animator.GetBool("IsTrace")!=true)
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    animator.SetBool("IsTrace", true);
                    nvAgent.speed = wanderSpeed;
                    Wander();
                    break;

                case MonsterState.trace:
                    nvAgent.speed = chaseSpeed;
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;
                case MonsterState.attack:
                    nvAgent.Stop();
                    animator.SetBool("IsAttack", true);
                    break;

            }
            yield return null;
        }
    }
    

}

