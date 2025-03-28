using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackCooldown = 1f; // Time between attacks
    public int damage = 10; // Amount of damage the player deals
    public float attackRange = 1f; // Attack range
    public float attackConeAngle = 45f; // Angle of the attack cone
    private float lastAttackTime = 0f;

    void Update()
    {
        // Detect mouse click to perform attack
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Check if the cooldown has passed
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Perform the attack logic (you can add animations or effects here)
            Debug.Log("Player attacked! Damage: " + damage);

            // Call method to deal damage to enemies within the cone
            DealDamageToEnemies();

            // Update the last attack time
            lastAttackTime = Time.time;
        }
    }

    void DealDamageToEnemies()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate the direction from the player to the mouse position
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Loop through all the colliders within attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange); // Range for attack detection

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy")) // If the object is tagged as "Enemy"
            {
                Vector2 enemyDirection = (hit.transform.position - transform.position).normalized;
                
                // Calculate the angle between the attack direction and the direction to the enemy
                float angle = Vector2.Angle(attackDirection, enemyDirection);

                // Check if the enemy is within the attack cone (half of attackConeAngle on each side)
                if (angle <= attackConeAngle / 2f)
                {
                    // If the enemy is within the cone, apply damage
                    EnemyAI enemy = hit.GetComponent<EnemyAI>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage); // Call the TakeDamage method on the enemy
                        Debug.Log("Enemy hit! Damage dealt: " + damage);
                    }
                }
            }
        }
    }
}
