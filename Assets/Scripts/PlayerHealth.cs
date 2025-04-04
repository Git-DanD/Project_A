using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100; // MaxHP
    public int maxMana = 100; // MaxMana
    private int currentHP; // PlayerHP
    private int currentMana; // PlayerMana

    private SpriteRenderer spriteRenderer;

    public HealthBar healthBar;
    public ManaBar manaBar;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHP(maxHP);

        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);

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

    public void Attack(int attDamage, int mana)
    {
        currentMana -= mana;
        if (currentMana <= 0) 
        {
            currentMana = 0;
        }
        Debug.Log("Player Mana: " + currentMana);
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

    public int GetMana()
    {
        return currentMana;
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack(10, 10);
            manaBar.SetMana(currentMana);
        }
    }

}
