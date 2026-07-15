using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Vida máxima del Player.
    [SerializeField] private int maxHealth = 5;

    [Header("Configuración de reaparición")]

    // Punto inicial de reaparición.
    [SerializeField] private Transform initialSpawnPoint;

    [Header("Referencias")]

    // Interfaz de Game Over.
    [SerializeField] private GameOverUI gameOverUI;

    // Movimiento del Player.
    [SerializeField] private PlayerMovement playerMovement;

    // Animator del Player.
    private Animator animator;

    [Header("Tiempos de muerte")]

    // Tiempo que dejamos visible la animación de muerte
    // antes de mostrar la pantalla de Game Over.
    [SerializeField] private float deathAnimationTime = 1.2f;

    // Tiempo que permanece visible el Game Over.
    [SerializeField] private float gameOverDisplayTime = 1.5f;

    // Vida actual.
    private int currentHealth;

    // Último punto de reaparición activado.
    private Transform currentSpawnPoint;

    // Evita recibir daño durante la muerte.
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        // El Player comienza con toda su vida.
        currentHealth = maxHealth;

        // Buscamos componentes necesarios.
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        animator = GetComponent<Animator>();

        // Establecemos el punto inicial de reaparición.
        if (initialSpawnPoint != null)
        {
            currentSpawnPoint = initialSpawnPoint;
        }
        else
        {
            Debug.LogWarning(
                "No se asignó Initial Spawn Point en PlayerHealth."
            );
        }

        UpdateHealthUI();

        Debug.Log(
            "Vida inicial del jugador: " +
            currentHealth + "/" + maxHealth
        );
    }

    /// Reduce la vida del Player.
    public void TakeDamage(int damageAmount)
    {
        // No recibimos daño si el Player ya está muerto.
        if (isDead)
        {
            return;
        }

        // Ignoramos cantidades inválidas.
        if (damageAmount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth - damageAmount,
            0,
            maxHealth
        );

        Debug.Log(
            "El jugador recibió " +
            damageAmount +
            " de daño. Vida actual: " +
            currentHealth + "/" + maxHealth
        );

        UpdateHealthUI();

        // Si la vida llega a cero, iniciamos la muerte.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// Restaura toda la vida del Player.
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();

        Debug.Log(
            "La vida del jugador fue restaurada: " +
            currentHealth + "/" + maxHealth
        );
    }

    /// Guarda un nuevo punto de reaparición.
    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        if (newSpawnPoint == null)
        {
            return;
        }

        currentSpawnPoint = newSpawnPoint;

        Debug.Log(
            "Nuevo punto de reaparición establecido: " +
            newSpawnPoint.gameObject.name
        );
    }

    /// Inicia la secuencia de muerte.
    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("El jugador ha muerto.");

        // Bloqueamos el movimiento.
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        // Reproducimos la animación de muerte.
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        StartCoroutine(DeathSequence());
    }

    /// Controla la animación de muerte,
    /// Game Over y reaparición.
    private IEnumerator DeathSequence()
    {
        // Primero dejamos que se vea la animación de muerte.
        yield return new WaitForSeconds(
            deathAnimationTime
        );

        // Después mostramos el Game Over.
        if (gameOverUI != null)
        {
            yield return gameOverUI.FadeIn();
        }
        else
        {
            Debug.LogWarning(
                "No se asignó GameOverUI en PlayerHealth."
            );
        }

        // Dejamos visible el mensaje.
        yield return new WaitForSeconds(
            gameOverDisplayTime
        );

        // Reaparecemos mientras la pantalla está oscura.
        RespawnPlayer();

        // Reiniciamos el Animator.
        if (animator != null)
        {
            animator.ResetTrigger("Dead");

            // Cambia este nombre si tu estado Idle
            // tiene otro nombre exacto en el Animator.
            animator.Play("Idle");
        }

        // Ocultamos nuevamente la pantalla.
        if (gameOverUI != null)
        {
            yield return gameOverUI.FadeOut();
        }

        // Recuperamos el movimiento.
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }

        isDead = false;

        Debug.Log(
            "El jugador volvió a estar activo."
        );
    }

    /// Mueve al Player al último checkpoint
    /// y restaura su vida.
    private void RespawnPlayer()
    {
        if (currentSpawnPoint != null)
        {
            transform.position =
                currentSpawnPoint.position;

            Physics2D.SyncTransforms();

            Debug.Log(
                "El jugador reapareció en: " +
                currentSpawnPoint.gameObject.name
            );
        }
        else
        {
            Debug.LogWarning(
                "No existe un punto de reaparición asignado."
            );
        }

        RestoreFullHealth();
    }

    /// Envía la vida actual al HUD.
    private void UpdateHealthUI()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning(
                "No se encontró una instancia de GameManager."
            );

            return;
        }

        GameManager.Instance.UpdatePlayerHealth(
            currentHealth,
            maxHealth
        );
    }
}