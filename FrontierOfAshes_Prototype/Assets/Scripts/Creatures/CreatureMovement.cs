using System.Collections;
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]

    // Velocidad de desplazamiento de la criatura.
    [SerializeField] private float moveSpeed = 1.2f;

    // Tiempo mínimo que la criatura permanece caminando.
    [SerializeField] private float minMoveTime = 1f;

    // Tiempo máximo que la criatura permanece caminando.
    [SerializeField] private float maxMoveTime = 3f;

    [Header("Configuración de espera")]

    // Tiempo mínimo que la criatura permanece quieta.
    [SerializeField] private float minIdleTime = 2f;

    // Tiempo máximo que la criatura permanece quieta.
    [SerializeField] private float maxIdleTime = 5f;

    [Header("Referencias")]

    // Rigidbody utilizado para mover la criatura.
    private Rigidbody2D rb;

    // Animator utilizado para controlar las animaciones.
    private Animator animator;

    // Dirección actual de movimiento.
    private Vector2 moveDirection;

    // Última dirección válida de la criatura.
    // Comienza mirando hacia abajo.
    private Vector2 lastDirection = Vector2.down;

    // Indica si la criatura está caminando.
    private bool isMoving;

    private void Awake()
    {
        // Obtenemos los componentes necesarios.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Iniciamos el ciclo de comportamiento de la criatura.
        StartCoroutine(MovementRoutine());
    }

    private void FixedUpdate()
    {
        // Solo movemos la criatura cuando está en estado de movimiento.
        if (!isMoving)
        {
            return;
        }

        // Movemos la criatura utilizando el sistema de física 2D.
        rb.MovePosition(
            rb.position +
            moveDirection * moveSpeed * Time.fixedDeltaTime
        );
    }

    /// Controla el ciclo de espera y movimiento de la criatura.
    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            // La criatura comienza quieta.
            StopMoving();

            // Elegimos cuánto tiempo permanecerá quieta.
            float idleTime = Random.Range(
                minIdleTime,
                maxIdleTime
            );

            yield return new WaitForSeconds(idleTime);

            // Elegimos una nueva dirección aleatoria.
            ChooseRandomDirection();

            // Comenzamos el movimiento.
            StartMoving();

            // Elegimos cuánto tiempo caminará.
            float moveTime = Random.Range(
                minMoveTime,
                maxMoveTime
            );

            yield return new WaitForSeconds(moveTime);
        }
    }

    /// Elige aleatoriamente una de las cuatro direcciones principales.
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

        // Guardamos la última dirección elegida.
        lastDirection = moveDirection;
    }

    /// Activa el movimiento y actualiza la animación.
    private void StartMoving()
    {
        isMoving = true;

        UpdateAnimator();
    }

    /// Detiene el movimiento y mantiene la última dirección.
    private void StopMoving()
    {
        isMoving = false;

        // Detenemos cualquier velocidad residual.
        rb.linearVelocity = Vector2.zero;

        UpdateAnimator();
    }

    /// Envía al Animator el estado y la dirección de la criatura.
    private void UpdateAnimator()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(
            "IsMoving",
            isMoving
        );

        animator.SetFloat(
            "MoveX",
            lastDirection.x
        );

        animator.SetFloat(
            "MoveY",
            lastDirection.y
        );
    }
}