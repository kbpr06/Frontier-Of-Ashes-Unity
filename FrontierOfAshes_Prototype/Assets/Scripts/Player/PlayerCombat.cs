using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Configuración del ataque")]

    // Punto desde donde se calcula el ataque.
    [SerializeField] private Transform attackPoint;

    // Distancia entre el centro del jugador y el punto de ataque.
    [SerializeField] private float attackPointDistance = 0.55f;

    // Radio de alcance del golpe.
    [SerializeField] private float attackRadius = 0.6f;

    // Cantidad de daño realizado con cada golpe.
    [SerializeField] private int attackDamage = 1;

    // Capa que contiene las criaturas que pueden recibir daño.
    [SerializeField] private LayerMask creatureLayer;

    [Header("Referencias")]

    // Sistema de movimiento utilizado para consultar
    // la última dirección del jugador.
    [SerializeField] private PlayerMovement playerMovement;

    // Controles generados mediante el nuevo Input System.
    private PlayerControls playerControls;

    private void Awake()
    {
        // Creamos una instancia de los controles.
        playerControls = new PlayerControls();

        // Si no se asignó PlayerMovement desde el Inspector,
        // lo buscamos en el mismo GameObject.
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
    }

    private void OnEnable()
    {
        // Activamos el mapa de controles Player.
        playerControls.Player.Enable();

        // Escuchamos la acción Attack.
        playerControls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar la acción Attack.
        playerControls.Player.Attack.performed -= OnAttack;

        // Desactivamos los controles.
        playerControls.Player.Disable();
    }

    private void Update()
    {
        // Actualizamos constantemente la posición del punto de ataque
        // según la dirección hacia la que mira el jugador.
        UpdateAttackPointPosition();
    }

    /// Coloca el punto de ataque delante del jugador.
    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null || playerMovement == null)
        {
            return;
        }

        Vector2 direction = playerMovement.LastMoveDirection;

        // Como AttackPoint es hijo del Player,
        // usamos localPosition para moverlo respecto del jugador.
        attackPoint.localPosition =
            direction * attackPointDistance;
    }

    /// Se ejecuta cuando el jugador presiona la tecla de ataque.
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (attackPoint == null)
        {
            Debug.LogWarning(
                "No se asignó AttackPoint en PlayerCombat."
            );

            return;
        }

        Debug.Log(
            "El jugador atacó hacia: " +
            playerMovement.LastMoveDirection
        );

        // Detectamos los colliders de la capa Creature
        // que se encuentran dentro del radio del ataque.
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            creatureLayer
        );

        Debug.Log(
            "Criaturas detectadas: " +
            hits.Length
        );

        // Recorremos todas las criaturas detectadas.
        foreach (Collider2D hit in hits)
        {
            // Buscamos el sistema de vida en el objeto golpeado.
            CreatureHealth creatureHealth =
                hit.GetComponent<CreatureHealth>();

            // Si el collider está en un objeto hijo,
            // buscamos la vida en el objeto padre.
            if (creatureHealth == null)
            {
                creatureHealth =
                    hit.GetComponentInParent<CreatureHealth>();
            }

            // Aplicamos daño si encontramos una criatura válida.
            if (creatureHealth != null)
            {
                creatureHealth.TakeDamage(attackDamage);
            }
        }
    }

    /// Dibuja el radio del ataque en la ventana Scene.
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