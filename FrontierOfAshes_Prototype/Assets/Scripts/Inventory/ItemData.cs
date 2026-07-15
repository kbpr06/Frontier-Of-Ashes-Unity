using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItemData",
    menuName = "Frontier of Ashes/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("Información del objeto")]

    // Nombre que se mostrará en el inventario.
    [SerializeField] private string itemName;

    // Icono utilizado en la interfaz.
    [SerializeField] private Sprite itemIcon;

    // Permite consultar el nombre desde otros scripts.
    public string ItemName => itemName;

    // Permite consultar el icono desde otros scripts.
    public Sprite ItemIcon => itemIcon;
}