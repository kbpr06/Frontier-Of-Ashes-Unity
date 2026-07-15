using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Configuración del ataque")]

    // Punto desde donde se calcula el ataque.
    [SerializeField] private Transform attackPoint;

    // Distancia entre el Player y el punto de ataque.
    [SerializeField] private float attackPointDistance = 0.55f;

    // Radio del golpe.
    [SerializeField] private float attackRadius = 0.6f;

    // Daño realizado por ataque.
    [SerializeField] private int attackDamage = 1;

    // Capa que contiene las criaturas que pueden recibir daño.
    [SerializeField] private LayerMask creatureLayer;

    [Header("Referencias")]

    // Sistema de movimiento del Player.
    [SerializeField] private PlayerMovement playerMovement;

    // Animator del Player.
    private Animator animator;

    // Controles generados por Input System.
    private PlayerControls playerControls;

    private void Awake()
    {
        // Creamos los controles.
        playerControls = new PlayerControls();

        // Buscamos las referencias necesarias.
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();

        playerControls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        playerControls.Player.Attack.performed -= OnAttack;

        playerControls.Player.Disable();
    }

    private void Update()
    {
        // Mantiene el punto de ataque delante del Player
        // según la última dirección de movimiento.
        UpdateAttackPointPosition();
    }

    /// Coloca el punto de ataque delante del Player.
    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null || playerMovement == null)
        {
            return;
        }

        Vector2 direction =
            playerMovement.LastMoveDirection;

        attackPoint.localPosition =
            direction * attackPointDistance;
    }

    /// Se ejecuta cuando el Player presiona la tecla de ataque.
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (attackPoint == null)
        {
            Debug.LogWarning(
                "No se asignó AttackPoint en PlayerCombat."
            );

            return;
        }

        // Reproducimos la animación de ataque.
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log(
            "El jugador atacó hacia: " +
            playerMovement.LastMoveDirection
        );

        // Detectamos todas las criaturas
        // dentro del radio del ataque.
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            creatureLayer
        );

        Debug.Log(
            "Criaturas detectadas: " +
            hits.Length
        );

        foreach (Collider2D hit in hits)
        {
            // Buscamos CreatureHealth directamente.
            CreatureHealth creatureHealth =
                hit.GetComponent<CreatureHealth>();

            // Si el collider está en un hijo,
            // buscamos el componente en el padre.
            if (creatureHealth == null)
            {
                creatureHealth =
                    hit.GetComponentInParent<CreatureHealth>();
            }

            // Aplicamos daño si encontramos
            // una criatura válida.
            if (creatureHealth != null)
            {
                creatureHealth.TakeDamage(
                    attackDamage
                );
            }
        }
    }

    /// Dibuja el radio de ataque en la ventana Scene.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRadius
        );
    }
}