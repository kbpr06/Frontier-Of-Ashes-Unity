using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Referencias de interfaz")]

    // Imagen donde se mostrará el icono del objeto.
    [SerializeField] private Image itemIcon;

    // Texto donde se mostrará la cantidad.
    [SerializeField] private TMP_Text amountText;

    /// Configura visualmente el contenido de la casilla.
    public void Setup(ItemData itemData, int amount)
    {
        if (itemData == null)
        {
            return;
        }

        // Mostramos el icono correspondiente.
        itemIcon.sprite = itemData.ItemIcon;

        // Mostramos la cantidad almacenada.
        amountText.text = "x" + amount;
    }
}