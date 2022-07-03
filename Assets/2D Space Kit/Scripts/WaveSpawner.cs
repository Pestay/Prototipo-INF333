using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class Wave {

    public string waveName;
    public int nOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;

}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public Animator animator;
    public GameObject status_bar;
    public Text waveName;
    private Wave currentWave;

    // De 0 hasta n-1 número de oleadas
    private int currentWaveNumber;

    private bool canSpawn = true;
    private bool canAnimate = false;
    private float nextSpawnTime;

    private void Update() {

        currentWave = waves[currentWaveNumber];
        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (totalEnemies.Length == 0 && currentWaveNumber + 1 != waves.Length && canAnimate)
        {
            waveName.text = waves[currentWaveNumber + 1].waveName;
            animator.SetTrigger("WaveComplete");
            canAnimate = false;
        }
    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }

    void SpawnWave()
    {
        if(canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0,currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            GameObject bar = Instantiate(status_bar, transform.position, transform.rotation, enemy.transform);
            //bar.GetComponent<StatusBarNPC>().target = enemy.gameObject;
            currentWave.nOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval; 
            if (currentWave.nOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }


}
