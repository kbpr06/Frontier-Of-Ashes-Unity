using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    [SerializeField] private float moveSpeed = 4f;

    // Componentes utilizados por el jugador.
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Clase generada por el nuevo Input System.
    private PlayerControls playerControls;

    // Dirección recibida desde las teclas.
    private Vector2 movementInput;

    // Indica si el jugador tiene permitido moverse.
    private bool canMove = true;

    private void Awake()
    {
        // Obtenemos los componentes del mismo GameObject.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Creamos una instancia de los controles.
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos el mapa de controles del jugador.
        playerControls.Player.Enable();

        // Escuchamos cuando se presiona o se suelta una dirección.
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar los eventos para evitar duplicaciones.
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;

        playerControls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Si el movimiento está bloqueado, ignoramos cualquier entrada.
        if (!canMove)
        {
            movementInput = Vector2.zero;
            return;
        }

        // Leemos la dirección enviada por el Input System.
        movementInput = context.ReadValue<Vector2>();

        UpdateAnimation();
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        // Si el jugador no puede moverse, no actualizamos su posición.
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
    /// Cambia entre la animación de reposo y movimiento.
   
    private void UpdateAnimation()
    {
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
    }

    /// Cambia la dirección visual del personaje.

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

        // Al bloquear el movimiento eliminamos la última dirección guardada.
        if (!canMove)
        {
            movementInput = Vector2.zero;

            // Detenemos cualquier desplazamiento pendiente del Rigidbody.
            rb.linearVelocity = Vector2.zero;

            // Dejamos al personaje en la animación de reposo.
            animator.SetBool("IsMoving", false);
        }
    }
}