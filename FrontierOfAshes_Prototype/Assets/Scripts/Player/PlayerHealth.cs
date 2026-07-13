using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private int maxHealth = 3;

    [Header("Reaparicion")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 2f;

    private int currentHealth;
    private bool isDead;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;

        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        NotifyHud();
        Debug.Log("Vida inicial: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        NotifyHud();
        Debug.Log("Vida restante: " + currentHealth);

        if (currentHealth == 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    private IEnumerator DieAndRespawn()
    {
        isDead = true;

        Debug.Log("El jugador ha muerto.");

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;
        NotifyHud();

        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (playerCollider != null)
            playerCollider.enabled = true;

        if (rb != null)
            rb.simulated = true;

        isDead = false;

        Debug.Log("Jugador reaparecio.");
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        NotifyHud();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    private void NotifyHud()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
        }
    }
}
