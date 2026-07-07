using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Data")]
    [SerializeField] private int collectedItems = 0;

    [Header("UI References")]
    [SerializeField] private HUDController hudController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Al iniciar el juego, actualizamos la UI con el contador inicial.
        hudController.UpdateCollectedItems(collectedItems);
    }

    public void AddCollectedItem(string itemName)
    {
        // Sumamos un objeto recogido.
        collectedItems++;

        // Mostramos el resultado en consola para comprobar.
        Debug.Log("Objeto recogido: " + itemName + " | Total: " + collectedItems);

        // Actualizamos el texto visible en pantalla.
        hudController.UpdateCollectedItems(collectedItems);
    }

    public int GetCollectedItems()
    {
        return collectedItems;
    }
}