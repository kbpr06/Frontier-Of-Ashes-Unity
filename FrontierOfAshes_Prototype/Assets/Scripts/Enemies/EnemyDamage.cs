using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración del ataque")]

    // Daño que realiza cada ataque.
    [SerializeField] private int damageAmount = 1;

    // Tiempo mínimo entre un ataque y el siguiente.
    [SerializeField] private float attackCooldown = 1.5f;

    // Momento aproximado dentro de la animación
    // en que realmente se aplica el daño.
    [SerializeField] private float damageDelay = 0.4f;

    // Referencias necesarias.
    private EnemyAI enemyAI;
    private Animator animator;

    // Evita iniciar varios ataques simultáneamente.
    private bool canAttack = true;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Esperamos hasta que el Player entre en el rango.
        if (
            canAttack &&
            enemyAI != null &&
            enemyAI.IsPlayerInAttackRange()
        )
        {
            StartCoroutine(AttackRoutine());
        }
    }

    /// Ejecuta un ataque completo con animación y cooldown.
    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        // Informamos a la IA que el enemigo está atacando.
        enemyAI.SetAttacking(true);

        // Activamos la animación Attack.
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log(
            gameObject.name +
            " inició un ataque."
        );

        // Esperamos hasta el momento visual del golpe.
        yield return new WaitForSeconds(damageDelay);

        // Comprobamos nuevamente que el Player
        // siga suficientemente cerca.
        if (enemyAI.IsPlayerInAttackRange())
        {
            Transform player = enemyAI.GetPlayer();

            if (player != null)
            {
                PlayerHealth playerHealth =
                    player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);

                    Debug.Log(
                        gameObject.name +
                        " hizo " +
                        damageAmount +
                        " de daño al Player."
                    );
                }
            }
        }

        // Dejamos terminar el tiempo de espera del ataque.
        yield return new WaitForSeconds(
            Mathf.Max(
                0f,
                attackCooldown - damageDelay
            )
        );

        enemyAI.SetAttacking(false);
        canAttack = true;
    }
}