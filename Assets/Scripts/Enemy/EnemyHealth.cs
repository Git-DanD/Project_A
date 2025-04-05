using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    private SpriteRenderer spriteRenderer;
    private bool inCombat = false;

    public EnemyHealthBar healthBar; // Reference to the health bar script

    private float combatTimer = 0f;
    private float combatDuration = 3f; // Time in seconds to stay visible after last hit

    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHP(maxHP);
        healthBar.Hide();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        if (!inCombat)
        {
            inCombat = true;
            healthBar.Show();
        }

        healthBar.SetHP(currentHP);

        StartCoroutine(FlashRed());

        combatTimer = combatDuration; // Reset the fade timer on taking damage

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Update()
    {
        if (inCombat)
        {
            combatTimer -= Time.deltaTime;
            if (combatTimer <= 0f)
            {
                inCombat = false;
                healthBar.Hide();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
