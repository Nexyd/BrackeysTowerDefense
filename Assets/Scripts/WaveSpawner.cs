using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float timeBetweenWaves;

    private UIData data;
    private float countdown;
    private int waveNumber;
    private int numEnemies;

    private void Start()
    {
        data = UIData.GetInstance();
        countdown = 10f;
        waveNumber = 0;
        numEnemies = 1;
    }

    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown,
            0f, Mathf.Infinity);

        data.WaveCountdown = countdown;
    }

    IEnumerator SpawnWave()
    {
        numEnemies++;
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab, spawnPoint
                .position, spawnPoint.rotation);

            yield return new WaitForSeconds(0.5f);
        }

        data.ActualWave = waveNumber++;
    }
}