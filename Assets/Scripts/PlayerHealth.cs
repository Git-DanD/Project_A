using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100; // MaxHP
    private int currentHP; // PlayerHP

    private SpriteRenderer spriteRenderer;

    public HealthBar healthBar;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHP(maxHP);

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignores damage if invincible

        currentHP -= damage;
        if (currentHP <= 0) 
        {
            currentHP = 0;
            ResetGame();
        }
        else
        {
            StartCoroutine(FlashRed()); // Trigger flash effect
            StartCoroutine(invincibility()); // Start invincibility
        }
        Debug.Log("Player HP: " + currentHP); // Debug
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; // Make red
        yield return new WaitForSeconds(0.1f); // Flash time
        spriteRenderer.color = Color.white; // Revert to normal
    }

    IEnumerator invincibility()
    {
        isInvincible = true; 
        yield return new WaitForSeconds(1f); // invincibility time
        isInvincible = false;
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP) currentHP = maxHP;
        Debug.Log("Player Healed: " + currentHP); // Debug
    }

    public int GetHealth()
    {
        return currentHP;
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload Game
    }

    // Debug for losing HP
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            healthBar.SetHP(currentHP);
        }
    }

}
