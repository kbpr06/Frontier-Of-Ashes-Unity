using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]

    // Velocidad de desplazamiento del jugador.
    [SerializeField] private float moveSpeed = 4f;

    // Componentes utilizados por el jugador.
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Clase generada por el nuevo Input System.
    private PlayerControls playerControls;

    // Dirección actual recibida desde las teclas.
    private Vector2 movementInput;

    // Última dirección válida hacia la que miró el jugador.
    // Comienza mirando hacia abajo.
    private Vector2 lastMoveDirection = Vector2.down;

    // Indica si el jugador tiene permitido moverse.
    private bool canMove = true;

    // Permite que otros scripts consulten la última dirección
    // sin modificarla directamente.
    public Vector2 LastMoveDirection => lastMoveDirection;

    private void Awake()
    {
        // Obtenemos los componentes necesarios del Player.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Creamos una instancia de los controles.
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos el mapa de controles Player.
        playerControls.Player.Enable();

        // Escuchamos los eventos de movimiento.
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar los eventos para evitar duplicaciones.
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;

        // Desactivamos los controles.
        playerControls.Player.Disable();
    }

    /// Recibe la dirección enviada por el Input System.
    private void OnMove(InputAction.CallbackContext context)
    {
        // Si el movimiento está bloqueado, eliminamos cualquier entrada.
        if (!canMove)
        {
            movementInput = Vector2.zero;
            return;
        }

        // Leemos la dirección actual.
        movementInput = context.ReadValue<Vector2>();

        // Solo actualizamos la última dirección cuando existe movimiento.
        // Así el jugador conserva la dirección al quedarse quieto.
        if (movementInput != Vector2.zero)
        {
            lastMoveDirection = GetCardinalDirection(movementInput);
        }

        UpdateAnimation();
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        // Si el movimiento está bloqueado, no desplazamos al jugador.
        if (!canMove)
        {
            return;
        }

        // Movemos al jugador utilizando el sistema de física 2D.
        rb.MovePosition(
            rb.position +
            movementInput * moveSpeed * Time.fixedDeltaTime
        );
    }

    /// Convierte la dirección recibida en una de las cuatro
    /// direcciones principales: arriba, abajo, izquierda o derecha.
    private Vector2 GetCardinalDirection(Vector2 direction)
    {
        // Si el movimiento horizontal es mayor,
        // elegimos izquierda o derecha.
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0
                ? Vector2.right
                : Vector2.left;
        }

        // En caso contrario, elegimos arriba o abajo.
        return direction.y > 0
            ? Vector2.up
            : Vector2.down;
    }

    /// Cambia entre la animación de reposo y movimiento.
    private void UpdateAnimation()
    {
        bool isMoving = movementInput != Vector2.zero;

        animator.SetBool("IsMoving", isMoving);
    }

    /// Cambia la orientación horizontal del sprite.
    private void UpdateDirection()
    {
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    /// Permite bloquear o habilitar el movimiento del jugador.
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        // Al bloquear el movimiento eliminamos
        // la última entrada activa.
        if (!canMove)
        {
            movementInput = Vector2.zero;

            // Detenemos cualquier velocidad pendiente.
            rb.linearVelocity = Vector2.zero;

            // Dejamos al jugador en estado de reposo.
            animator.SetBool("IsMoving", false);
        }
    }
}