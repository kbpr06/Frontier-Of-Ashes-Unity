using System.Collections;
using TMPro;
using UnityEngine;

public class ItemNotificationUI : MonoBehaviour
{
    [Header("Referencias de interfaz")]

    // Texto donde se mostrará el objeto recogido.
    [SerializeField] private TMP_Text notificationText;

    [Header("Configuración")]

    // Tiempo que permanece visible la notificación.
    [SerializeField] private float displayTime = 2f;

    // Coroutine actualmente activa.
    private Coroutine notificationCoroutine;

    private void Start()
    {
        // La notificación comienza oculta.
        gameObject.SetActive(false);
    }

    /// Muestra temporalmente el nombre y cantidad del objeto recogido.
    public void ShowNotification(ItemData itemData, int amount)
    {
        if (itemData == null || notificationText == null)
        {
            return;
        }

        // Si ya había una notificación activa,
        // la detenemos para mostrar la nueva.
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }

        // Activamos el panel antes de mostrar el mensaje.
        gameObject.SetActive(true);

        notificationText.text =
            itemData.ItemName +
            " +" +
            amount;

        notificationCoroutine =
            StartCoroutine(NotificationRoutine());
    }

    /// Mantiene visible el mensaje durante unos segundos.
    private IEnumerator NotificationRoutine()
    {
        yield return new WaitForSeconds(displayTime);

        // Ocultamos nuevamente el panel.
        gameObject.SetActive(false);

        notificationCoroutine = null;
    }
}