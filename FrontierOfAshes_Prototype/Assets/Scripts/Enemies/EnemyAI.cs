using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detección del jugador")]

    // Distancia desde la que el enemigo detecta al Player.
    [SerializeField] private float detectionRange = 5f;

    // Distancia en la que deja de avanzar y queda listo para atacar.
    [SerializeField] private float attackRange = 1f;

    [Header("Movimiento")]

    // Velocidad utilizada durante el patrullaje.
    [SerializeField] private float walkSpeed = 1f;

    // Velocidad utilizada cuando persigue al Player.
    [SerializeField] private float runSpeed = 2.2f;

    [Header("Tiempos de patrullaje")]

    // Tiempo mínimo y máximo que permanece quieto.
    [SerializeField] private float minIdleTime = 2f;
    [SerializeField] private float maxIdleTime = 4f;

    // Tiempo mínimo y máximo que camina patrullando.
    [SerializeField] private float minWalkTime = 1f;
    [SerializeField] private float maxWalkTime = 3f;

    [Header("Referencias")]

    // Referencia al Player.
    [SerializeField] private Transform player;

    // Componentes del enemigo.
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Dirección actual de movimiento.
    private Vector2 moveDirection;

    // Estado de comportamiento actual.
    private bool isPatrolling;
    private bool isChasing;
    private bool isAttacking;
    private bool isDead;

    // Coroutine actual de patrullaje.
    private Coroutine patrolCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Buscamos automáticamente al Player si no fue asignado.
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Iniciamos el comportamiento de patrullaje.
        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        // Si el enemigo está muerto o no encuentra al Player,
        // no actualizamos su comportamiento.
        if (isDead || player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(
            transform.position,
            player.position
        );

        // Si el Player entra en rango,
        // interrumpimos el patrullaje y comenzamos la persecución.
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isPatrolling = false;

            moveDirection =
                ((Vector2)player.position - rb.position).normalized;

            // Si está suficientemente cerca,
            // dejamos de correr para poder atacar.
            if (distanceToPlayer <= attackRange)
            {
                isChasing = false;
            }
        }
        else
        {
            // Cuando el Player se aleja,
            // el enemigo vuelve a su comportamiento normal.
            isChasing = false;
        }

        UpdateAnimator();
        UpdateVisualDirection();
    }

    private void FixedUpdate()
    {
        if (isDead || isAttacking)
        {
            return;
        }

        // Persecución.
        if (isChasing)
        {
            rb.MovePosition(
                rb.position +
                moveDirection * runSpeed * Time.fixedDeltaTime
            );

            return;
        }

        // Patrullaje.
        if (isPatrolling)
        {
            rb.MovePosition(
                rb.position +
                moveDirection * walkSpeed * Time.fixedDeltaTime
            );
        }
    }

    /// Alterna entre espera y caminata cuando el Player está lejos.
    private IEnumerator PatrolRoutine()
    {
        while (!isDead)
        {
            // No patrullamos mientras perseguimos o atacamos.
            if (isChasing || isAttacking)
            {
                yield return null;
                continue;
            }

            // Estado Idle.
            isPatrolling = false;
            moveDirection = Vector2.zero;

            float idleTime = Random.Range(
                minIdleTime,
                maxIdleTime
            );

            yield return new WaitForSeconds(idleTime);

            // Si durante la espera detectó al Player,
            // no iniciamos el paseo.
            if (isChasing || isAttacking || isDead)
            {
                continue;
            }

            // Elegimos una dirección aleatoria.
            ChooseRandomDirection();

            // Comenzamos a caminar.
            isPatrolling = true;

            float walkTime = Random.Range(
                minWalkTime,
                maxWalkTime
            );

            yield return new WaitForSeconds(walkTime);

            isPatrolling = false;
        }
    }

    /// Elige una dirección aleatoria de patrullaje.
    private void ChooseRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                moveDirection = Vector2.up;
                break;

            case 1:
                moveDirection = Vector2.down;
                break;

            case 2:
                moveDirection = Vector2.left;
                break;

            case 3:
                moveDirection = Vector2.right;
                break;
        }
    }

    /// Actualiza los parámetros del Animator.
    private void UpdateAnimator()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(
            "IsMoving",
            isPatrolling
        );

        animator.SetBool(
            "IsRunning",
            isChasing
        );
    }

    /// Gira horizontalmente el sprite según la dirección.
    private void UpdateVisualDirection()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (moveDirection.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    /// Permite que otros scripts indiquen si el enemigo está atacando.
    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    /// Detiene completamente la IA cuando el enemigo muere.
    public void SetDead()
    {
        isDead = true;
        isPatrolling = false;
        isChasing = false;
        isAttacking = false;

        moveDirection = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }
    }

    /// Permite saber si el Player está dentro del rango de ataque.
    public bool IsPlayerInAttackRange()
    {
        if (player == null)
        {
            return false;
        }

        return Vector2.Distance(
            transform.position,
            player.position
        ) <= attackRange;
    }

    /// Devuelve la referencia actual al Player.
    public Transform GetPlayer()
    {
        return player;
    }

    /// Muestra los rangos de detección y ataque en Scene.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}