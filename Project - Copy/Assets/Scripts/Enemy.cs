using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Player player;
    public PlayerAnimationAttackScript playerAttack;
    public EnemyDetectionFight detectFight;
    public Animator EnemAnim;

    LayerMask obstacleMask;

    //Enemy Movement
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;
    private bool isDirSafe = false;
    private float vRotation = 0;

    //Enemy Health
    private float enemyhp = 100;
    public Slider enemyHealthBar;
    public GameObject bloodEffect;

    //EnemyAggro
    private Transform playerTransform;
    private float dist;
    private float howClose = 15;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        float z = agent.transform.position.z;
        float x = agent.transform.position.x;

        EnemAnim.SetFloat("forward", z);
        EnemAnim.SetFloat("strafe", x);

        
        enemyHealthBar.value = enemyhp;
        if (enemyhp <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Enemy dead!");
            return;
        }

       EnemyCower();

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
            }
        }

        if(enemyhp > 30 && !detectFight.playerInDetectionRange)
        {
            agent.speed = 7.5f;
            dist = Vector3.Distance(playerTransform.position, transform.position);

            if(dist <= howClose)
            {
                transform.LookAt(player.transform);
                agent.destination = player.transform.position;
            }
        }

        if(detectFight.playerInDetectionRange && dist<=0.5)
        {
            detectFight.FightInDetectionRange();
        }
    }

    bool RandPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        { 
            result = hit.position;
            return true;
        }


        result = Vector3.zero;
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Katana" && playerAttack.isLightAttacking)
        {
            enemyhp -= 10;
            if (enemyhp <= 25)
            {
                BloodSplat(bloodEffect, 2);
            }
        }
    }

    IEnumerator BloodSplat(GameObject effect, float delay)
    {
        Instantiate(bloodEffect, new Vector3(transform.position.x,transform.position.y,transform.position.z),transform.rotation);
        yield return new WaitForSeconds(delay);
        bloodEffect.SetActive(false);
    }

    void EnemyCower()
    {
        if (enemyhp <=30)
        {
            while (!isDirSafe)
            {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPosition = transform.position + dirToPlayer;
            newPosition = Quaternion.Euler(0, vRotation, 0) * newPosition;

            bool isHit = Physics.Raycast(transform.position, newPosition, out RaycastHit hit, 9, obstacleMask);

            if (hit.transform == null)
            {
                agent.SetDestination(newPosition);
            }
            if (isHit && hit.transform.CompareTag("Obstacle"))
            {
                vRotation += 20;
                isDirSafe = false;
            }
            else
            {
                agent.SetDestination(newPosition);
                isDirSafe = true;
            }
            Debug.Log("Enemy is low! Their Running to cower!");
            }
            detectFight.playerInDetectionRange = false;
        }
    }

    public void AttackPlayer()
    {
        if (player.isBlocking == true)
        {
            EnemAnim.SetTrigger("lightAttack");
            agent.SetDestination(transform.position);
            player.playerhp -= 10 * 0.55f;
            player.TakeStamina(10);
            Debug.Log("Player successfully blocked 45% of damage by enemy!");
        }
        else
        {
        EnemAnim.SetTrigger("lightAttack");
        agent.SetDestination(transform.position);
        player.playerhp -= 10;
        }
    }
}
