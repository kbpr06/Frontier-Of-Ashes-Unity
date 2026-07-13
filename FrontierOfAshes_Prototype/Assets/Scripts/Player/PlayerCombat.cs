using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Configuración del ataque")]

    // Punto desde donde se calcula el ataque.
    [SerializeField] private Transform attackPoint;

    // Radio de alcance del ataque.
    [SerializeField] private float attackRadius = 0.6f;

    // Cantidad de daño realizado con cada golpe.
    [SerializeField] private int attackDamage = 1;

    // Capa que contiene las criaturas que pueden recibir daño.
    [SerializeField] private LayerMask creatureLayer;

    // Controles generados mediante el nuevo Input System.
    private PlayerControls playerControls;

    private void Awake()
    {
        // Creamos una instancia de los controles del jugador.
        playerControls = new PlayerControls();
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
        // Dejamos de escuchar la acción para evitar duplicaciones.
        playerControls.Player.Attack.performed -= OnAttack;

        // Desactivamos los controles.
        playerControls.Player.Disable();
    }

    /// Se ejecuta cuando el jugador presiona la tecla de ataque.
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("El jugador realizó un ataque.");

        // Detectamos todos los colliders de la capa Creature
        // que se encuentren dentro del radio del ataque.
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            creatureLayer
        );

        Debug.Log(
            "Criaturas detectadas: " +
            hits.Length
        );

        // Recorremos todos los colliders encontrados.
        foreach (Collider2D hit in hits)
        {
            // Intentamos obtener el componente de vida de la criatura.
            CreatureHealth creatureHealth =
                hit.GetComponent<CreatureHealth>();

            // Si el componente no está directamente en el collider,
            // también lo buscamos en el objeto padre.
            if (creatureHealth == null)
            {
                creatureHealth =
                    hit.GetComponentInParent<CreatureHealth>();
            }

            // Si encontramos el sistema de vida, aplicamos daño.
            if (creatureHealth != null)
            {
                creatureHealth.TakeDamage(attackDamage);
            }
        }
    }

    /// Dibuja el alcance del ataque en la ventana Scene.
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