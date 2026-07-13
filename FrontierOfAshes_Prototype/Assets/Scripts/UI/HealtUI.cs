using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referencias de corazones")]
    [SerializeField] private Image[] hearts;

    [Header("Sprites de vida")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    /// <summary>
    /// Actualiza visualmente los corazones según la vida actual del jugador.
    /// </summary>
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Muestra un corazón lleno si corresponde a la vida actual.
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }

            // Oculta los corazones que superen la vida máxima configurada.
            hearts[i].gameObject.SetActive(i < maxHealth);
        }
    }
}