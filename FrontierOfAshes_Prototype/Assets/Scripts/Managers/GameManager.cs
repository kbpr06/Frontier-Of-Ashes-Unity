using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instancia global del GameManager.
    // Esto permite que otros scripts puedan acceder a él fácilmente.
    public static GameManager Instance { get; private set; }

    [Header("Game Data")]
    [SerializeField] private int collectedItems = 0;

    private void Awake()
    {
        // Si no existe otro GameManager, este se convierte en la instancia principal.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si ya existe un GameManager, destruimos este duplicado.
            Destroy(gameObject);
        }
    }

    public void AddCollectedItem(string itemName)
    {
        // Aumentamos el contador de objetos recogidos.
        collectedItems++;

        // Mostramos el resultado en consola para comprobar que funciona.
        Debug.Log("Objeto recogido: " + itemName + " | Total: " + collectedItems);
    }

    public int GetCollectedItems()
    {
        // Devuelve la cantidad actual de objetos recogidos.
        return collectedItems;
    }
}