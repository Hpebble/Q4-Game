using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemiesToSpawn;
    public List<Enemy> aliveEnemiesSpawned;
    public BoxCollider2D AreaToSpawn;
    public int maxEnemiesAlive;
    public float spawnSpeed;
    [SerializeField]
    private float spawnCD;
    public int spawnBatchSize;
    public bool emitting;

    [Header("Destroy On Use")]
    public bool destroyOnFinalSpawn;
    public int maxEnemiesToSpawn;
    private int enemiesSpawned;
    void Start()
    {
        
    }
    void Update()
    {
        CheckIfEnemyAlive();
        if (emitting)
        {
            Countdown();
        }
    }
    public void SpawnEnemy()
    {
        if (aliveEnemiesSpawned.Count == maxEnemiesAlive) { return; }
        Vector3 spawnPoint;
        float x = Random.Range(AreaToSpawn.bounds.min.x, AreaToSpawn.bounds.max.x);
        float y = Random.Range(AreaToSpawn.bounds.min.y, AreaToSpawn.bounds.max.y);
        spawnPoint = new Vector3(x, y, 0f);
        GameObject enemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)].gameObject, spawnPoint, Quaternion.identity, this.gameObject.transform);
        aliveEnemiesSpawned.Add(enemy.GetComponent<Enemy>()); //Spawn Enemy and add them to the aliveEnemies list
        enemiesSpawned++;
        //Debug.Log("Spawned Enemy");
    }
    public void CheckIfEnemyAlive()
    {
        if (aliveEnemiesSpawned.Count != 0 || aliveEnemiesSpawned != null)
        {
            foreach (Enemy enemy in aliveEnemiesSpawned)
            {
                if (enemy.health <= 0)
                {
                    aliveEnemiesSpawned.Remove(enemy);
                    return;
                }
            }
        }
    }
    public void Countdown()
    {
        if (destroyOnFinalSpawn && enemiesSpawned == maxEnemiesToSpawn)
        {
            Destroy(this.gameObject);
        }
        if(aliveEnemiesSpawned.Count == maxEnemiesAlive)
        {
            spawnCD = spawnSpeed;
            return;
        }
        if(spawnCD > 0)
        {
            spawnCD -= Time.deltaTime;
        }
        if(spawnCD <= 0)
        {
            int i;
            for (i = 0; i < spawnBatchSize; i++)
            {
                SpawnEnemy();
            }
            spawnCD = spawnSpeed;
        }
    }
}
