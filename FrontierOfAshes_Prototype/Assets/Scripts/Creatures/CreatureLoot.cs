using UnityEngine;

public class CreatureLoot : MonoBehaviour
{
    [System.Serializable]
    public class LootEntry
    {
        [Header("Objeto a soltar")]

        // Prefab del objeto que aparecerá al morir la criatura.
        public GameObject lootPrefab;

        [Header("Cantidad")]

        // Cantidad mínima que puede aparecer.
        [Min(1)]
        public int minAmount = 1;

        // Cantidad máxima que puede aparecer.
        [Min(1)]
        public int maxAmount = 1;

        [Header("Probabilidad")]

        // Probabilidad de aparición entre 0 y 100.
        [Range(0f, 100f)]
        public float dropChance = 100f;
    }

    [Header("Lista de Loot")]

    // Lista de todos los objetos que esta criatura puede soltar.
    [SerializeField] private LootEntry[] lootTable;

    /// Genera los objetos configurados en la tabla de loot.
    public void DropLoot()
    {
        // Recorremos todos los posibles drops.
        foreach (LootEntry lootEntry in lootTable)
        {
            // Si no hay un Prefab asignado, ignoramos esta entrada.
            if (lootEntry.lootPrefab == null)
            {
                continue;
            }

            // Generamos un número aleatorio entre 0 y 100.
            float randomValue = Random.Range(0f, 100f);

            // Si el valor aleatorio supera la probabilidad,
            // este objeto no aparecerá.
            if (randomValue > lootEntry.dropChance)
            {
                continue;
            }

            // Elegimos una cantidad aleatoria entre el mínimo y máximo.
            // Sumamos 1 al máximo porque Random.Range con int
            // no incluye el valor máximo.
            int amount = Random.Range(
                lootEntry.minAmount,
                lootEntry.maxAmount + 1
            );

            // Creamos cada objeto de loot.
            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    lootEntry.lootPrefab,
                    GetRandomDropPosition(),
                    Quaternion.identity
                );
            }

            Debug.Log(
                gameObject.name +
                " soltó " +
                amount +
                " objeto(s) de tipo " +
                lootEntry.lootPrefab.name
            );
        }
    }

    /// Devuelve una posición ligeramente aleatoria
    /// alrededor de la criatura para evitar que todos
    /// los objetos aparezcan exactamente uno sobre otro.
    private Vector3 GetRandomDropPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * 0.25f;

        return transform.position + new Vector3(
            randomOffset.x,
            randomOffset.y,
            0f
        );
    }
}