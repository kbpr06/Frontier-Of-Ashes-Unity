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
            // Avisamos al GameManager que este objeto fue recogido.
            GameManager.Instance.AddCollectedItem(itemName);

            // Destruimos el objeto recogido de la escena.
            Destroy(gameObject);
        }
    }
}