using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private float movement;

    private bool facingRight = true;
    private bool isGrounded = true;

    public int maxHealth = 10;
    public int currentCoint = 0;

    [SerializeField] private Text health;
    [SerializeField] private Text coin;
    [SerializeField] private Transform attackPoint;


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private LayerMask attacklayer;
    [SerializeField] private int maxJumps = 2;
   

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
        }

        health.text = maxHealth.ToString();
        coin.text = currentCoint.ToString();

        movement = Input.GetAxis("Horizontal");

        FlipPlayer();

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                jump();
                isGrounded = false;
                animator.SetBool("Jump", true);
            }

        }

        if (Mathf.Abs(movement) > 0.1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < 0.1f)
        {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime 
            * moveSpeed;
    }

    private void FlipPlayer()
    {
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }

    private void jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }

    public void PlayerAttack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, 
                                                        attackRadius, attacklayer);

        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PatrolEnemy>() != null)
            {
                collInfo.gameObject.GetComponent<PatrolEnemy>().EnemyTakeDamage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            currentCoint++;
            collision.gameObject.transform.GetChild(0).GetComponent<Animator>()
                                                        .SetTrigger("Collected");
            Destroy(collision.gameObject, 0.5f);
        }
    }

    private void Die()
    {
        Debug.Log("Die");
        FindObjectOfType<GameManager>().isGameActive = false;
        Destroy(this.gameObject);
    }

}
