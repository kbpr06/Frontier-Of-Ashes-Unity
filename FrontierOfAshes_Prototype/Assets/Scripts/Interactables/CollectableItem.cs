using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private string itemName = "Knife";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Revisamos si el objeto que entró al trigger es el Player.
        if (collision.CompareTag("Player"))
        {
            // Mostramos en consola qué objeto fue recogido.
            Debug.Log("Objeto recogido: " + itemName);

            // Destruimos este objeto de la escena.
            Destroy(gameObject);
        }
    }
}