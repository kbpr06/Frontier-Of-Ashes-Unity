using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text itemsText;
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        if (healthText == null && itemsText != null)
        {
            GameObject healthObject = new GameObject("Health_Text");
            healthObject.transform.SetParent(itemsText.transform.parent, false);

            RectTransform rectTransform = healthObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = itemsText.rectTransform.anchorMin;
            rectTransform.anchorMax = itemsText.rectTransform.anchorMax;
            rectTransform.pivot = itemsText.rectTransform.pivot;
            rectTransform.anchoredPosition = itemsText.rectTransform.anchoredPosition + new Vector2(0f, -40f);
            rectTransform.sizeDelta = itemsText.rectTransform.sizeDelta;

            healthText = healthObject.AddComponent<TextMeshProUGUI>();
            healthText.font = itemsText.font;
            healthText.fontSize = itemsText.fontSize;
            healthText.color = itemsText.color;
            healthText.alignment = itemsText.alignment;
            healthText.text = "Vida: -/-";
        }
    }

    public void UpdateCollectedItems(int amount)
    {
        // Actualiza el contador de objetos en pantalla.
        itemsText.text = "Objetos: " + amount;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText == null)
            return;

        healthText.text = "Vida: " + currentHealth + "/" + maxHealth;
    }
}
