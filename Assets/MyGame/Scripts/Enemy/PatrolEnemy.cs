using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public Transform checkPoint;
    public Transform player;
    public Transform AttackPoint;
    
    public LayerMask layerMask;
    public LayerMask attacklayer;

    private Animator animator;

    private bool facingLeft = true;
    private bool inRange = false;

    public float distance = 1f;
    public float moveSpeed = 2f;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public float attackRadius = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if (inRange)
        {
            patrolAttack();
        }
        else
        {
            patrolEnemy();
        }
    }

    private void patrolAttack()
    {
        if (player.position.x > transform.position.x && facingLeft == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (player.position.x < transform.position.x && facingLeft == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
        
        if (Vector2.Distance(transform.position, player.position) >
                                                                retrieveDistance)
        {
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position,
                                    player.position, chaseSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("Attack1", true);
        }
    }

    private void patrolEnemy()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down,
                                                              distance, layerMask);

        if (hit == false && facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (hit == false && facingLeft == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    private void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(AttackPoint.position, 
                                                            attackRadius, attacklayer);
        
        if (collInfo)
        {
            Debug.Log(collInfo.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (AttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRadius);
    }





}
