using System.Collections;
using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    [Header("Configuración de vida")]

    // Vida máxima de la criatura.
    [SerializeField] private int maxHealth = 2;

    [Header("Configuración de muerte")]

    // Tiempo que esperamos antes de eliminar la criatura.
    [SerializeField] private float deathDelay = 1.5f;

    // Vida actual.
    private int currentHealth;

    // Evita recibir daño después de morir.
    private bool isDead;

    // Referencias opcionales.
    private Animator animator;
    private EnemyAI enemyAI;

    // Indican si el Animator posee estos parámetros.
    private bool hasHurtParameter;
    private bool hasDeadParameter;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        // La criatura comienza con toda su vida.
        currentHealth = maxHealth;

        // Buscamos componentes opcionales.
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        // Comprobamos qué parámetros existen realmente
        // en el Animator Controller de esta criatura.
        if (animator != null)
        {
            hasHurtParameter = HasAnimatorParameter("Hurt");
            hasDeadParameter = HasAnimatorParameter("Dead");
        }

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
        // Una criatura muerta no puede seguir recibiendo daño.
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

        // Si todavía tiene vida, reproducimos Hurt
        // solamente si este Animator posee ese parámetro.
        if (currentHealth > 0)
        {
            if (animator != null && hasHurtParameter)
            {
                animator.SetTrigger("Hurt");
            }

            return;
        }

        Die();
    }

    /// Inicia la muerte de la criatura.
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

        // Si la criatura posee IA enemiga,
        // detenemos completamente su comportamiento.
        if (enemyAI != null)
        {
            enemyAI.SetDead();
        }

        // Reproducimos Dead solamente si
        // el Animator Controller posee ese parámetro.
        if (animator != null && hasDeadParameter)
        {
            animator.SetTrigger("Dead");
        }

        StartCoroutine(DeathRoutine());
    }

    /// Espera la animación, genera el loot y elimina la criatura.
    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);

        CreatureLoot creatureLoot =
            GetComponent<CreatureLoot>();

        if (creatureLoot != null)
        {
            creatureLoot.DropLoot();
        }

        Destroy(gameObject);
    }

    /// Comprueba si el Animator Controller
    /// contiene un parámetro con el nombre indicado.
    private bool HasAnimatorParameter(string parameterName)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName)
            {
                return true;
            }
        }

        return false;
    }
}