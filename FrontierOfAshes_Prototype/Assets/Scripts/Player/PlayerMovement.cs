using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerControls playerControls;
    private Vector2 movementInput;

    private void Awake()
    {
        // Obtenemos los componentes necesarios del Player.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Creamos la clase generada desde PlayerControls.inputactions.
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos el mapa de controles Player.
        playerControls.Player.Enable();

        // Escuchamos el evento Move.
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar el evento Move para evitar errores.
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;

        // Desactivamos los controles.
        playerControls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Leemos la dirección de movimiento.
        movementInput = context.ReadValue<Vector2>();

        // Cambiamos entre Idle y Walk.
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);

        // Giramos el sprite según la dirección horizontal.
        // Si se mueve a la izquierda, mira a la izquierda.
        // Si se mueve a la derecha, se voltea visualmente.
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        // Movemos al jugador usando física 2D.
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}