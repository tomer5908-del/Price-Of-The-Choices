using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private GameObject king;
    public static GameObject king_public;
    public GameObject camera_object;
    private float movement_speed = 5f;
    private float camera_smooth_speed = 0.125f;
    private float max_x = 3f;
    private float max_y = 2f;
    private float camera_offset_x = 0f;
    private float camera_offset_y = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool should_attack = false;
    public static KingProperties parameters;
    public Image bravery_bar;
    public Image loyalty_bar;
    public Image helpfulness_bar;
    public Image health_bar;
    public List<GameObject> all_enemies;
    private float attackCooldown = 0.5f; // Cooldown time in seconds
    private float lastAttackTime = 0f; // Store the time of the last attack
    public static int hp = 20;
    public static bool game_running = false;

    private void Awake()
    {
        king = gameObject;
        rb = king.GetComponent<Rigidbody2D>();
        animator = king.GetComponent<Animator>();
        king_public = king;
        parameters = ScriptableObject.CreateInstance<KingProperties>();
    }
    private void Start()
    {
        parameters.bravery = 50;
        parameters.loyalty = 50;
        parameters.helpfulness = 50;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            should_attack = true;
        }
        bravery_bar.fillAmount = Mathf.Clamp01((float)parameters.bravery / 100f);
        loyalty_bar.fillAmount = Mathf.Clamp01((float)parameters.loyalty / 100f);
        helpfulness_bar.fillAmount = Mathf.Clamp01((float)parameters.helpfulness / 100f);

        health_bar.fillAmount = Mathf.Clamp01((float)hp / 20f);
    }
    void FixedUpdate()
    {
        if (king != null && camera_object != null)
        {
            float ix = Input.GetAxisRaw("Horizontal") * movement_speed;
            float iy = Input.GetAxisRaw("Vertical") * movement_speed;
            if (!game_running)
            {
                ix = 0;
                iy = 0;
            }

            rb.linearVelocity = new Vector2(ix, iy);

            string movement_direction = "";
            if (ix > 0)
            {
                movement_direction = "right";
            }
            else if (ix < 0)
            {
                movement_direction = "left";
            }
            else if (iy < 0)
            {
                movement_direction = "down";
            }
            else if (iy > 0)
            {
                movement_direction = "up";
            }

            // Handle Movement and Attack
            if (ix != 0 || iy != 0)  // Player is moving
            {
                animator.SetBool("walking", true);
            }
            else  // Player is not moving
            {
                animator.SetBool("walking", false);
            }

            // Only trigger attack once when space is pressed
            if (!should_attack)
            {
                animator.SetBool("attack", false);  // Trigger attack animation once
                animator.SetInteger("attack_direction", 0);
            }
            if (should_attack)
            {
                animator.SetBool ("attack", true);  // Trigger attack animation once
                animator.SetInteger("attack_direction", GetAttackDirection(movement_direction));
                AttackNearby();
                should_attack = false;  // Reset the flag after the attack
            }
    

                // Set movement direction
                int direction = 0;
            switch (movement_direction)
            {
                case "up":
                    direction = 1;
                    break;
                case "down":
                    direction = 3;
                    break;
                case "right":
                    direction = 2;
                    break;
                case "left":
                    direction = 4;
                    break;
                default:
                    break;
            }

            animator.SetInteger("direction", direction);
        }

        // Camera follow logic (no changes)
        Vector3 king_pos = king.transform.position;
        Vector3 cam_pos = camera_object.transform.position;

        float dx = king_pos.x - cam_pos.x;
        float dy = king_pos.y - cam_pos.y;

        float cam_x = cam_pos.x;
        float cam_y = cam_pos.y;

        if (Mathf.Abs(dx) > max_x)
        {
            cam_x = king_pos.x - Mathf.Sign(dx) * max_x;
        }
        if (Mathf.Abs(dy) > max_y)
        {
            cam_y = king_pos.y - Mathf.Sign(dy) * max_y;
        }

        Vector3 targetPos = new Vector3(cam_x + camera_offset_x, cam_y + camera_offset_y, camera_object.transform.position.z);
        camera_object.transform.position = Vector3.Lerp(camera_object.transform.position, targetPos, camera_smooth_speed);
    }


    private int GetAttackDirection(string movement_direction)
    {
        switch (movement_direction)
        {
            case "up": return 1;
            case "down": return 3;
            case "right": return 2;
            case "left": return 4;
            default: return 3;
        }
    }

    private void AttackNearby()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Attack logic
            foreach (var enemy in all_enemies)
            {
                if (enemy != null)
                {
                    if (enemy.GetComponent<Enemy>().CheckAttackRange())
                    {
                        enemy.GetComponent<Enemy>().DealDamage();
                    }
                }
            }

            // Update the last attack time
            lastAttackTime = Time.time;
        }
    }

    public static void DealDamage()
    {
        hp -= 1;
    }

}
