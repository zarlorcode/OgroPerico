using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;      // Lista de enemigos posibles
    public float spawnInterval = 5f;       // Cada cuánto spawnear
    public int maxEnemies = 5;             // Número máximo de enemigos activos en la habitación

    private BoxCollider2D roomArea;
    private Transform player;

    private int currentEnemyCount = 0;

    private void Start()
    {
        roomArea = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(SpawnLoop());
    }

    

    private IEnumerator SpawnLoop()
    {
        SpawnEnemy();
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (player == null) continue;

            // Verificar si el jugador está dentro de la habitación
            if (roomArea.OverlapPoint(player.position))
            {
                if (currentEnemyCount < maxEnemies)
                    SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // Elegir aleatoriamente el tipo de enemigo
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Elegir una posición aleatoria dentro del BoxCollider
        Vector2 spawnPos = new Vector2(
            Random.Range(roomArea.bounds.min.x, roomArea.bounds.max.x),
            Random.Range(roomArea.bounds.min.y, roomArea.bounds.max.y)
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        // Hacer al enemigo hijo del spawner
        enemy.transform.SetParent(this.transform);

        // Pasar el área automáticamente
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.movementArea = roomArea;
        currentEnemyCount++;

        if (enemyBase != null)
            enemyBase.OnDeath += () => currentEnemyCount--;
    }
}
