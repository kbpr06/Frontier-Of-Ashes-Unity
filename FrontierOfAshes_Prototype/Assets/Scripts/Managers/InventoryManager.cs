using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Referencias de interfaz")]

    // Interfaz encargada de mostrar la notificación
    // temporal cuando se recoge un objeto.
    [SerializeField] private ItemNotificationUI itemNotificationUI;

    // Guarda cada tipo de objeto junto a su cantidad.
    private Dictionary<ItemData, int> inventory =
        new Dictionary<ItemData, int>();

    private void Awake()
    {
        // Permitimos una sola instancia del inventario.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Agrega un objeto al inventario.
    public void AddItem(ItemData itemData, int amount = 1)
    {
        // Evitamos agregar objetos sin información
        // o cantidades inválidas.
        if (itemData == null || amount <= 0)
        {
            return;
        }

        // Si el objeto ya existe en el inventario,
        // aumentamos su cantidad.
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] += amount;
        }
        else
        {
            // Si es la primera vez que se recoge,
            // lo agregamos al inventario.
            inventory.Add(itemData, amount);
        }

        // Mostramos información en la consola
        // para comprobar que el objeto fue guardado.
        Debug.Log(
            itemData.ItemName +
            " agregado al inventario. Cantidad actual: " +
            inventory[itemData]
        );

        // Mostramos una notificación temporal
        // indicando qué objeto se recogió.
        if (itemNotificationUI != null)
        {
            itemNotificationUI.ShowNotification(
                itemData,
                amount
            );
        }
    }

    /// Devuelve todos los objetos almacenados
    /// para que otros sistemas puedan consultarlos.
    public Dictionary<ItemData, int> GetInventory()
    {
        return inventory;
    }

    /// Devuelve la cantidad almacenada
    /// de un objeto específico.
    public int GetItemAmount(ItemData itemData)
    {
        if (
            itemData != null &&
            inventory.ContainsKey(itemData)
        )
        {
            return inventory[itemData];
        }

        return 0;
    }
}