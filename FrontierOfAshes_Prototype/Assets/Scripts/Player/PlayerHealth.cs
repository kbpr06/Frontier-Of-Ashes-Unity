using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Cantidad máxima de vida del jugador.
    [SerializeField] private int maxHealth = 5;

    [Header("Configuración de reaparición")]

    // Punto inicial donde reaparecerá el jugador
    // antes de activar otro checkpoint.
    [SerializeField] private Transform initialSpawnPoint;

    [Header("Referencias")]

    // Controla la pantalla y transición de Game Over.
    [SerializeField] private GameOverUI gameOverUI;

    // Componente responsable del movimiento del jugador.
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Tiempos de muerte")]

    // Tiempo que permanece visible el mensaje Game Over.
    [SerializeField] private float gameOverDisplayTime = 1.2f;

    // Vida actual del jugador.
    private int currentHealth;

    // Último punto de reaparición activado.
    private Transform currentSpawnPoint;

    // Evita recibir daño o morir varias veces simultáneamente.
    private bool isDead;

    // Permiten consultar la vida desde otros scripts
    // sin modificarla directamente.
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        // El jugador comienza con toda su vida.
        currentHealth = maxHealth;

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

        // Si no asignamos PlayerMovement manualmente,
        // intentamos encontrarlo en el mismo GameObject.
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        UpdateHealthUI();

        Debug.Log(
            "Vida inicial del jugador: " +
            currentHealth + "/" + maxHealth
        );
    }

   
    /// Reduce la vida del jugador según el daño recibido.
  
    public void TakeDamage(int damageAmount)
    {
        // Un jugador muerto no puede seguir recibiendo daño.
        if (isDead)
        {
            return;
        }

        // Ignoramos valores de daño inválidos.
        if (damageAmount <= 0)
        {
            return;
        }

        // Reducimos la vida y evitamos que quede bajo cero.
        currentHealth = Mathf.Clamp(
            currentHealth - damageAmount,
            0,
            maxHealth
        );

        Debug.Log(
            "El jugador recibió " + damageAmount +
            " de daño. Vida actual: " +
            currentHealth + "/" + maxHealth
        );

        UpdateHealthUI();

        // Si la vida llegó a cero, iniciamos la muerte.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

   
    /// Restaura completamente la vida del jugador.
   
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();

        Debug.Log(
            "La vida del jugador fue restaurada: " +
            currentHealth + "/" + maxHealth
        );
    }

    
    /// Guarda un nuevo checkpoint como punto de reaparición.
 
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


    /// Inicia la secuencia de muerte del jugador.

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("El jugador ha muerto.");

        // Bloqueamos el movimiento y eliminamos la dirección guardada.
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        StartCoroutine(DeathSequence());
    }

    /// Controla la transición de Game Over y la reaparición.

    private IEnumerator DeathSequence()
    {
        // Oscurecemos la pantalla.
        if (gameOverUI != null)
        {
            yield return gameOverUI.FadeIn();
        }

        // Dejamos visible el mensaje durante un breve momento.
        yield return new WaitForSeconds(gameOverDisplayTime);

        // Movemos al jugador mientras la pantalla está oscura.
        RespawnPlayer();

        // Aclaramos nuevamente la pantalla.
        if (gameOverUI != null)
        {
            yield return gameOverUI.FadeOut();
        }

        // Devolvemos el control al jugador.
        if (playerMovement != null)
        {
            if (playerMovement != null)
            {
                playerMovement.SetMovementEnabled(true);
            }
        }

        isDead = false;

        Debug.Log("El jugador volvió a estar activo.");
    }

    /// Traslada al jugador al último checkpoint
    /// y restaura completamente su vida.
    private void RespawnPlayer()
    {
        if (currentSpawnPoint != null)
        {
            transform.position = currentSpawnPoint.position;

            // Sincronizamos la nueva posición con la física.
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