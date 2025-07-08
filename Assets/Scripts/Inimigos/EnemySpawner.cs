using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array com os prefabs dos inimigos
    public float spawnInterval = 2f;   // Intervalo entre os spawns (segundos)

    private float timer = 0f;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float randomX = Random.Range(topLeft.x, topRight.x);
        Vector3 spawnPos = new Vector3(randomX, topLeft.y + 1f, 0);

        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }
}
