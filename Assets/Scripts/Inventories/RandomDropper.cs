using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.AI;

public class RandomDropper : ItemDropper
{
    [Tooltip("How far can the pickups be scattered from the dropper")]
    [SerializeField] private float scatterDistance = 1;
    [SerializeField] private DropLibrarySO dropLibrary;
    [SerializeField] private int numberOfDrops = 2;

    const int ATTEMPTS = 30;

    public void RandomDrop()
    {
        BaseStats baseStats = GetComponent<BaseStats>();

        var drops = dropLibrary.GetRandomDrops(baseStats.CurrentLevel);

        foreach (var drop in drops)
        {
            DropItem(drop.item, drop.number);
        }

    }

    protected override Vector3 GetDropLocation()
    {
        for (int i = 0; i < ATTEMPTS; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return transform.position;
    }
}