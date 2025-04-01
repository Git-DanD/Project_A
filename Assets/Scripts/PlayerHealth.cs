using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100; // MaxHP
    private int currentHP; // PlayerHP

    public HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        healthBar.setMaxHP(maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        Debug.Log("Player HP: " + currentHP); // Debug
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

    // Debug for losing HP
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            healthBar.setHP(currentHP);
        }
    }

}
