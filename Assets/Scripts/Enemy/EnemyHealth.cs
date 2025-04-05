using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Max health of the enemy
    private int currentHealth;   // Current health

    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer for flashing red

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // If the enemy is already dead, don't apply damage

        currentHealth -= damage; // Decrease current health by the damage value
        Debug.Log("Enemy took damage. Current Health: " + currentHealth);  // Debug log for damage

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();  // Enemy dies when health reaches 0
        }
        else
        {
            StartCoroutine(FlashRed());  // Flash the enemy red when it takes damage
        }
    }

    // Flash the enemy red when hit
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;  // Change color to red
        yield return new WaitForSeconds(0.1f);  // Flash duration
        spriteRenderer.color = Color.white;  // Change back to normal color
    }

    // When enemy dies
    void Die()
    {
        Debug.Log("Enemy died!");
        // Optionally trigger death animations, effects, etc.
        Destroy(gameObject);  // Destroy the enemy GameObject
    }
}
