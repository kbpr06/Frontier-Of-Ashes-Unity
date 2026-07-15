using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Configuración del objeto")]

    // Información del objeto que será guardado.
    [SerializeField] private ItemData itemData;

    // Cantidad entregada al recogerlo.
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo el Player puede recoger el objeto.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Agregamos el objeto al inventario.
        if (
            InventoryManager.Instance != null &&
            itemData != null
        )
        {
            InventoryManager.Instance.AddItem(
                itemData,
                amount
            );

            Debug.Log(
                "Objeto recogido: " +
                itemData.ItemName +
                " x" +
                amount
            );
        }

        // Eliminamos el pickup de la escena.
        Destroy(gameObject);
    }
}