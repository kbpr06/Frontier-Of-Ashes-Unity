using System.Collections;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("Configuración de transición")]

    // Tiempo que demora el panel en aparecer o desaparecer.
    [SerializeField] private float fadeDuration = 0.5f;

    // CanvasGroup utilizado para controlar la transparencia.
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Buscamos el CanvasGroup en este mismo GameObject.
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError(
                "GameOverPanel no tiene un componente CanvasGroup."
            );

            return;
        }

        // El panel comienza invisible.
        canvasGroup.alpha = 0f;

        // No bloquea clics ni otras interacciones.
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

  
    /// Muestra gradualmente el panel de Game Over.

    public IEnumerator FadeIn()
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        Debug.Log("Mostrando pantalla de Game Over.");

        yield return FadeCanvasGroup(
            canvasGroup.alpha,
            1f
        );
    }

    
    /// Oculta gradualmente el panel de Game Over.
  
    public IEnumerator FadeOut()
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        Debug.Log("Ocultando pantalla de Game Over.");

        yield return FadeCanvasGroup(
            canvasGroup.alpha,
            0f
        );
    }


    /// Cambia progresivamente la transparencia del CanvasGroup.
  
    private IEnumerator FadeCanvasGroup(
        float startAlpha,
        float targetAlpha
    )
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                progress
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}