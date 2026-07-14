using System.Collections;
using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Vida máxima de la criatura.
    [SerializeField] private int maxHealth = 2;

    [Header("Configuración de muerte")]

    // Tiempo antes de eliminar la criatura.
    // Debe coincidir aproximadamente con la animación Dead.
    [SerializeField] private float deathDelay = 1f;

    // Vida actual.
    private int currentHealth;

    // Evita daño y muerte repetida.
    private bool isDead;

    // Referencias opcionales.
    private Animator animator;
    private EnemyAI enemyAI;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        Debug.Log(
            gameObject.name +
            " inició con " +
            currentHealth + "/" + maxHealth +
            " puntos de vida."
        );
    }

    /// Reduce la vida de la criatura.
    public void TakeDamage(int damageAmount)
    {
        if (isDead || damageAmount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth - damageAmount,
            0,
            maxHealth
        );

        Debug.Log(
            gameObject.name +
            " recibió " +
            damageAmount +
            " de daño. Vida actual: " +
            currentHealth + "/" + maxHealth
        );

        // Si todavía tiene vida,
        // reproducimos la reacción de daño.
        if (currentHealth > 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }

            return;
        }

        Die();
    }

    /// Inicia la secuencia de muerte.
    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log(
            gameObject.name +
            " ha muerto."
        );

        // Si es un enemigo con IA,
        // detenemos completamente su comportamiento.
        if (enemyAI != null)
        {
            enemyAI.SetDead();
        }

        // Reproducimos la animación de muerte.
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        StartCoroutine(DeathRoutine());
    }

    /// Espera la animación, genera el loot y elimina la criatura.
    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);

        // Generamos el loot si existe este componente.
        CreatureLoot creatureLoot =
            GetComponent<CreatureLoot>();

        if (creatureLoot != null)
        {
            creatureLoot.DropLoot();
        }

        Destroy(gameObject);
    }
}