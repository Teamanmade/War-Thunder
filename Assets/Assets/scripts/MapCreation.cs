using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{
    // 0: Base, 1: Wall, 2: Obstacle, 3: SpawnEffect, 4: River, 5: Grass, 6: Border
    public GameObject[] items;

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Awake()
    {
        InitMap();
    }

    private void InitMap()
    {
        // Place base
        CreateItem(items[0], new Vector3(0, -8, 0), Quaternion.identity);

        // Surround base with walls
        CreateItem(items[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(items[1], new Vector3(1, -8, 0), Quaternion.identity);
        for (int i = -1; i < 2; i++)
            CreateItem(items[1], new Vector3(i, -7, 0), Quaternion.identity);

        // Create borders
        for (int i = -11; i < 12; i++)
        {
            CreateItem(items[6], new Vector3(i, 9, 0), Quaternion.identity);
            CreateItem(items[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++)
        {
            CreateItem(items[6], new Vector3(-12, i, 0), Quaternion.identity);
            CreateItem(items[6], new Vector3(12, i, 0), Quaternion.identity);
        }

        // Player spawn
        var playerBorn = Instantiate(items[3], new Vector3(-2, -8, 0), Quaternion.identity);
        //playerBorn.GetComponent<Born>().createPlayer = true;

        // Enemy spawn points
        CreateItem(items[3], new Vector3(-11, 8, 0), Quaternion.identity);
        CreateItem(items[3], new Vector3(0, 8, 0), Quaternion.identity);
        CreateItem(items[3], new Vector3(11, 8, 0), Quaternion.identity);

        InvokeRepeating(nameof(CreateEnemy), 4f, 5f);

        // Map objects
        PlaceRandomItems(items[1], 65); // Walls
        PlaceRandomItems(items[2], 20); // Obstacles
        PlaceRandomItems(items[4], 20); // Rivers
        PlaceRandomItems(items[5], 20); // Grass
        PlaceRandomItems(items[7], 2); // shield
        PlaceRandomItems(items[8], 2); // SpeedUp
        PlaceRandomItems(items[9], 2); //DoubleShot
    }

    private void PlaceRandomItems(GameObject prefab, int count)
    {
        int attempts = 0;
        int placed = 0;
        while (placed < count && attempts < count * 10) // Avoid infinite loop
        {
            Vector3 pos = CreateRandomPosition();
            if (occupiedPositions.Add(pos))
            {
                CreateItem(prefab, pos, Quaternion.identity);
                placed++;
            }
            attempts++;
        }
    }

    private void CreateItem(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(prefab, position, rotation, transform);
        occupiedPositions.Add(position);
    }

    private Vector3 CreateRandomPosition()
    {
        // Exclude x=-10/10, y=-8/8
        return new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
    }

    private void CreateEnemy()
    {
        int spawnIndex = Random.Range(0, 3);
        Vector3[] positions = { new Vector3(-10, 8, 0), new Vector3(0, 8, 0), new Vector3(10, 8, 0) };
        CreateItem(items[3], positions[spawnIndex], Quaternion.identity);
    }
}