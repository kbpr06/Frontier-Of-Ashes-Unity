using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 movementInput;

    private void Awake()
    {
        // Obtenemos el Rigidbody2D del Player.
        // Lo usaremos para mover al personaje respetando la física 2D de Unity.
        rb = GetComponent<Rigidbody2D>();

        // Creamos una instancia de la clase PlayerControls,
        // que fue generada automáticamente desde el archivo PlayerControls.inputactions.
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos el mapa de controles del jugador.
        playerControls.Player.Enable();

        // Nos suscribimos al evento Move.
        // Cada vez que el jugador presione una tecla de movimiento,
        // Unity ejecutará el método OnMove.
        playerControls.Player.Move.performed += OnMove;

        // Cuando el jugador suelte la tecla, también ejecutamos OnMove
        // para actualizar el movimiento a cero.
        playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        // Quitamos la suscripción a los eventos para evitar errores
        // si el objeto se desactiva o se destruye.
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;

        // Desactivamos el mapa de controles del jugador.
        playerControls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Leemos el valor Vector2 enviado por la acción Move.
        // Ejemplo:
        // Flecha arriba = (0, 1)
        // Flecha abajo = (0, -1)
        // Flecha derecha = (1, 0)
        // Flecha izquierda = (-1, 0)
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Movemos al jugador usando Rigidbody2D.
        // FixedUpdate se usa porque el movimiento depende de física.
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
