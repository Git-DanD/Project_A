using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 10;  // Amount of damage the player deals
    public float attackRange = 1f;  // How far the player can hit
    public LayerMask enemyLayer;  // The layer where enemies are located

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left-click for attack
        {
            Attack();
        }
    }

    void Attack()
    {
        // Get the position of the mouse in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Ensure the attack happens on the 2D plane

        // Create a Ray from the player to the mouse position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePosition - transform.position, attackRange, enemyLayer);

        // If we hit something
        if (hit.collider != null)
        {
            Debug.Log("Hit an enemy!");

            // Make sure the object has an EnemyHealth component
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);  // Apply damage
            }
        }
    }
}
