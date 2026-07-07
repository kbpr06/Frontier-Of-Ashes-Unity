using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text itemsText;

    public void UpdateCollectedItems(int amount)
    {
        // Actualiza el contador de objetos en pantalla.
        itemsText.text = "Objetos: " + amount;
    }
}