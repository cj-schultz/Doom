using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject easyEnemyPrefab;
    public GameObject hardEnemyPrefab;
    public float timeBetweenWaves = 3f;

    private BoxCollider col;
    private Vector3 min;
    private Vector3 max;

    private List<Wave> waves;
    private int waveIndex;

    private int currentWaveEnemyCount;

    void Awake()
    {
        col = GetComponent<BoxCollider>();

        min = col.bounds.min;
        max = col.bounds.max;

        InitWavesAndStartSpawning();    
    }

    void OnEnable()
    {
        Enemy.OnEnemyDie += HandleEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDie -= HandleEnemyDeath;
    }

    private void InitWavesAndStartSpawning()
    {
        Wave w1 = new Wave();
        w1.easyEnemies = 1;
        w1.hardEnemies = 0;
        Wave w2 = new Wave();
        w2.easyEnemies = 3;
        w2.hardEnemies = 0;
        Wave w3 = new Wave();
        w3.easyEnemies = 4;
        w3.hardEnemies = 2;
        Wave w4 = new Wave();
        w4.easyEnemies = 4;
        w4.hardEnemies = 4;
        Wave w5 = new Wave();
        w5.easyEnemies = 14;
        w5.hardEnemies = 0;
        Wave w6 = new Wave();
        w6.easyEnemies = 5;
        w6.hardEnemies = 8;

        waves = new List<Wave>();
        waves.Add(w1);
        waves.Add(w2);
        waves.Add(w3);
        waves.Add(w4);
        waves.Add(w5);
        waves.Add(w6);
        
        waveIndex = 0;
        currentWaveEnemyCount = waves[waveIndex].easyEnemies + waves[waveIndex].hardEnemies;

        SpawnWave(waves[waveIndex]);        
    }        
    
    private void HandleEnemyDeath()
    {
        currentWaveEnemyCount--;

        if(currentWaveEnemyCount <= 0)
        {
            waveIndex++;
            currentWaveEnemyCount = waves[waveIndex].easyEnemies + waves[waveIndex].hardEnemies;

            StartCoroutine(SpawnWaveInSeconds(waves[waveIndex], timeBetweenWaves));
        }
    }

    private IEnumerator SpawnWaveInSeconds(Wave wave, float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);

        SpawnWave(wave);
    }

    private void SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.easyEnemies; i++)
        {
            Vector3 spawnPoint = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z));

            Instantiate(easyEnemyPrefab, spawnPoint, Quaternion.identity);
        }

        for (int i = 0; i < wave.hardEnemies; i++)
        {
            Vector3 spawnPoint = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z));

            Instantiate(hardEnemyPrefab, spawnPoint, Quaternion.identity);
        }
    }

    private struct Wave
    {
        public int easyEnemies;
        public int hardEnemies;
    }                
}
