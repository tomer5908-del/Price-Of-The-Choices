using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform king;
    private float range = 5f;
    private float attack_range = 1.7f;
    private Rigidbody2D rb;
    private float movement_speed = 3f;
    private Animator animator;
    public int hp = 3;
    private float attackCooldown = 0.5f;  // Cooldown duration (0.5 seconds)
    private float lastAttackTime = 0f;  // Last attack time
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        king = Movement.king_public.transform;
    }

    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (CheckNearPlayers()) MoveTowardsPlayer();
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("walking", false);
            animator.SetInteger("direction", 0);
        }
    }
    private bool CheckNearPlayers()
    {
        float distance = Vector2.Distance(transform.position, king.position);
        if (distance < range) return true;
        else return false;
    }
    public bool CheckAttackRange()
    {
        if (!CheckNearPlayers()) return false;
        float distance = Vector2.Distance(transform.position, king.position);
        if (distance < attack_range) return true;
        else return false;
    }

    public void DealDamage()
    {
        hp -= 1;
        Debug.Log(hp);
        if (hp <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = king.position - transform.position;
        Vector2 direction_norm = direction.normalized;
        float dx = direction_norm.x;
        float dy = direction_norm.y;
        string movement_direction = "na";
        if (dy > 0 && Mathf.Abs(dx) < Mathf.Abs(dy))
        {
            
            movement_direction = "up";
        }
        else if (dy < 0 && Mathf.Abs(dx) < Mathf.Abs(dy))
        {
            
            movement_direction = "down";
        }
        else if (dx < 0 && Mathf.Abs(dy) < Mathf.Abs(dx))
        {
            
            movement_direction = "left";
        }
        else if (dx > 0 && Mathf.Abs(dy) < Mathf.Abs(dx))
        {
            
            movement_direction = "right";
        }
        else
        {
            animator.SetBool("walking", false);
            movement_direction = "na";

        }
        bool should_attack = CheckAttackRange();
        if (!should_attack)
        {
            animator.SetBool("attack", false);
            animator.SetInteger("attack_direction", 0);
        }
        if (should_attack && Time.time >= lastAttackTime + attackCooldown)
        {
            Movement.DealDamage();  // Call damage dealing method here
            lastAttackTime = Time.time;  // Update the last attack time
        }

        switch (movement_direction)
        {
            case "up":
                if (should_attack)
                {
                    animator.SetBool("attack", true);
                    animator.SetInteger("attack_direction", 1);
                }
                    animator.SetBool("walking", true);
                animator.SetInteger("direction", 1);
                break;
            case "down":
                if (should_attack)
                {
                    animator.SetBool("attack", true);
                    animator.SetInteger("attack_direction", 3);
                }
                animator.SetBool("walking", true);
                animator.SetInteger("direction", 3);
                break;
            case "right":
                if (should_attack)
                {
                    animator.SetBool("attack", true);
                    animator.SetInteger("attack_direction", 2);
                }
                animator.SetBool("walking", true);
                animator.SetInteger("direction", 2);
                break;
            case "left":
                if (should_attack)
                {
                    animator.SetBool("attack", true);
                    animator.SetInteger("attack_direction", 4);
                }
                animator.SetBool("walking", true);
                animator.SetInteger("direction", 4);
                break;
            case "na":
                animator.SetBool("walking", false);
                animator.SetInteger("direction", 0);
                break;
            default:
                animator.SetBool("walking", false);
                animator.SetInteger("direction", 0);
                break;
        }
        if (!should_attack)
        {
            rb.linearVelocity = direction_norm * movement_speed;
        }
        else rb.linearVelocity = Vector2.zero;
    }
}
