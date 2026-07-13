using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Referencias de interfaz")]
    [SerializeField] private TMP_Text itemsText;
    [SerializeField] private HealthUI healthUI;

    
    /// Actualiza temporalmente el contador general de objetos.
    /// Más adelante será reemplazado por el inventario.
    public void UpdateCollectedItems(int amount)
    {
        if (itemsText == null)
            return;

        itemsText.text = "Objetos: " + amount;
    }

  
    /// Actualiza visualmente la vida del jugador.
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthUI == null)
            return;

        healthUI.UpdateHealth(currentHealth, maxHealth);
    }
}