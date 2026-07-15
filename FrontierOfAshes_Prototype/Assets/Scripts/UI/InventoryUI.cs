using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias de interfaz")]

    // Panel completo que se abre y se cierra.
    [SerializeField] private GameObject inventoryPanel;

    // Contenedor donde se crearán las casillas.
    [SerializeField] private Transform inventoryContent;

    // Prefab utilizado para crear cada casilla.
    [SerializeField] private GameObject inventorySlotPrefab;

    // Controles del jugador.
    private PlayerControls playerControls;

    // Indica si el inventario está abierto.
    private bool isOpen;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos las acciones del Player.
        playerControls.Player.Enable();

        // Escuchamos la acción Inventory.
        playerControls.Player.Inventory.performed +=
            ToggleInventory;
    }

    private void OnDisable()
    {
        // Dejamos de escuchar la acción.
        playerControls.Player.Inventory.performed -=
            ToggleInventory;

        playerControls.Player.Disable();
    }

    private void Start()
    {
        // El inventario comienza cerrado.
        inventoryPanel.SetActive(false);
        isOpen = false;
    }

    /// Abre o cierra el inventario.
    private void ToggleInventory(
        InputAction.CallbackContext context
    )
    {
        isOpen = !isOpen;

        inventoryPanel.SetActive(isOpen);

        // Reconstruimos la interfaz cada vez que se abre.
        if (isOpen)
        {
            RefreshInventory();
        }
    }

    /// Actualiza visualmente todos los objetos guardados.
    private void RefreshInventory()
    {
        if (
            InventoryManager.Instance == null ||
            inventoryContent == null ||
            inventorySlotPrefab == null
        )
        {
            return;
        }

        // Eliminamos las casillas visuales anteriores.
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<ItemData, int> inventory =
            InventoryManager.Instance.GetInventory();

        // Creamos una nueva casilla por cada tipo de objeto.
        foreach (
            KeyValuePair<ItemData, int> item
            in inventory
        )
        {
            GameObject newSlot = Instantiate(
                inventorySlotPrefab,
                inventoryContent
            );

            InventorySlotUI slotUI =
                newSlot.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Setup(
                    item.Key,
                    item.Value
                );
            }
        }
    }
}