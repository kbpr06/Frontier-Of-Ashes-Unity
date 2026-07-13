using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Cantidad máxima de vida de la criatura.
    [SerializeField] private int maxHealth = 2;

    // Vida actual de la criatura.
    private int currentHealth;

    // Evita ejecutar la muerte más de una vez.
    private bool isDead;

    // Permite consultar la vida desde otros scripts
    // sin modificarla directamente.
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        // Al crearse la criatura, comienza con toda su vida.
        currentHealth = maxHealth;

        Debug.Log(
            gameObject.name +
            " inició con " +
            currentHealth + "/" + maxHealth +
            " puntos de vida."
        );
    }

    /// Recibe una cantidad determinada de daño.
    public void TakeDamage(int damageAmount)
    {
        // Una criatura muerta no puede recibir más daño.
        if (isDead)
        {
            return;
        }

        // Ignoramos cantidades de daño inválidas.
        if (damageAmount <= 0)
        {
            return;
        }

        // Restamos la vida y evitamos que quede bajo cero.
        currentHealth = Mathf.Clamp(
            currentHealth - damageAmount,
            0,
            maxHealth
        );

        Debug.Log(
            gameObject.name +
            " recibió " + damageAmount +
            " de daño. Vida actual: " +
            currentHealth + "/" + maxHealth
        );

        // Si la vida llega a cero, la criatura muere.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// Ejecuta la muerte de la criatura.
    private void Die()
    {
        // Evitamos ejecutar este proceso más de una vez.
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log(
            gameObject.name +
            " ha muerto."
        );

        // Por ahora eliminamos la criatura inmediatamente.
        // Más adelante aquí agregaremos animación, sonido y loot.
        Destroy(gameObject);
    }
}