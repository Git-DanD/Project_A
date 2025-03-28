using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; // Movement speed of the enemy
    public float attackRange = 1f; // Range at which the enemy attacks the player
    public int health = 50; // Health of the enemy
    public Transform player; // The player's transform to follow

    private bool isAttacking = false; // To check if the enemy is attacking

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Move towards the player
        MoveTowardsPlayer();

        // Check if the enemy should attack
        if (Vector2.Distance(transform.position, player.position) <= attackRange && !isAttacking)
        {
            AttackPlayer();
        }
    }

    // Move towards the player
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    // Attack the player (triggered when in range)
    void AttackPlayer()
    {
        isAttacking = true;

        // Here you can implement damage to the player or animations, etc.
        // For now, let's just output to the console
        Debug.Log("Enemy attacks the player!");

        // After the attack, wait a short time before allowing the enemy to attack again
        Invoke("ResetAttack", 1f); // Reset attack after 1 second
    }

    // Reset the attack state
    void ResetAttack()
    {
        isAttacking = false;
    }

    // Method to take damage from the player
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health); // Show current health in console for testing
        if (health <= 0)
        {
            Die();
        }
    }

    // Method for the enemy to die
    void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy when health reaches zero
    }
}
