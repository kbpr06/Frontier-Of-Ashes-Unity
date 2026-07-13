using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Cantidad máxima de vida que puede tener el jugador.
    [SerializeField] private int maxHealth = 5;

    [Header("Configuración de reaparición")]

    // Punto donde reaparecerá el jugador antes de activar un checkpoint.
    [SerializeField] private Transform initialSpawnPoint;

    // Vida actual del jugador.
    private int currentHealth;

    // Último punto de reaparición activado.
    private Transform currentSpawnPoint;

    // Evita ejecutar la muerte más de una vez.
    private bool isDead;

    // Permite consultar la vida desde otros scripts,
    // pero evita que puedan modificarla directamente.
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        // El jugador comienza con toda su vida.
        currentHealth = maxHealth;

        // Al iniciar, utilizamos el punto inicial de la aldea.
        if (initialSpawnPoint != null)
        {
            currentSpawnPoint = initialSpawnPoint;
        }
        else
        {
            // Este mensaje aparecerá si olvidamos asignar
            // VillageSpawnPoint en el Inspector.
            Debug.LogWarning(
                "No se asignó Initial Spawn Point en PlayerHealth."
            );
        }

        // Mostramos la vida inicial en el HUD.
        UpdateHealthUI();

        Debug.Log(
            "Vida inicial del jugador: " +
            currentHealth + "/" + maxHealth
        );
    }

    
    /// Reduce la vida del jugador según el daño recibido.
    
    public void TakeDamage(int damageAmount)
    {
        // No recibimos daño si el jugador ya está muerto.
        if (isDead)
        {
            return;
        }

        // Evitamos aceptar valores iguales o inferiores a cero.
        if (damageAmount <= 0)
        {
            return;
        }

        // Restamos vida y evitamos que el valor quede bajo cero.
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

        // Actualizamos los corazones.
        UpdateHealthUI();

        // Si la vida llega a cero, se ejecuta la muerte.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    /// Restaura completamente la vida del jugador.
    
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;

        // Actualizamos los corazones después de recuperar la vida.
        UpdateHealthUI();

        Debug.Log(
            "La vida del jugador fue restaurada: " +
            currentHealth + "/" + maxHealth
        );
    }

    /// Guarda un nuevo checkpoint como punto de reaparición.
    /// Este método es llamado por el script Checkpoint.
   
    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        // Evitamos guardar una referencia vacía.
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


    /// Se ejecuta cuando la vida del jugador llega a cero.

    private void Die()
    {
        // Evitamos ejecutar la muerte más de una vez.
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("El jugador ha muerto.");

        // Iniciamos una secuencia de muerte con una pequeña espera.
        StartCoroutine(DeathSequence());
    }

    
    /// Controla la espera entre la muerte y la reaparición.

    private IEnumerator DeathSequence()
    {
        // Esperamos 1,5 segundos antes de reaparecer.
        yield return new WaitForSeconds(1.5f);

        Respawn();
    }

    /// Devuelve al jugador al último checkpoint activado
    /// y restaura completamente su vida.
    private void Respawn()
    {
        if (currentSpawnPoint != null)
        {
            // Trasladamos al jugador a un punto seguro.
            transform.position = currentSpawnPoint.position;

            // Sincronizamos inmediatamente la nueva posición
            // con el sistema de físicas de Unity.
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

        // Recuperamos toda la vida después de mover al jugador.
        RestoreFullHealth();

        // El jugador vuelve a estar activo y puede recibir daño.
        isDead = false;
    }


    /// Envía la vida actual al GameManager para actualizar el HUD.
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
