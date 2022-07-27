using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class Enemies {
    public float quantity;
    public GameObject type;
}
[System.Serializable]
public class Wave {

    public string waveName;
    public int nOfEnemies;
    public Enemies[] enemies;
    public float spawnInterval;

}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public Animator animator;
    public GameObject status_bar;
    public GameObject endgameUI;
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
        } else if (currentWave.nOfEnemies == 0 && totalEnemies.Length == 0 && currentWaveNumber + 1 == waves.Length)
        {
            endgameUI.SetActive(true);
            Time.timeScale = 0f;
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
            Enemies randomEnemy = currentWave.enemies[Random.Range(0,currentWave.enemies.Length)];
            if(randomEnemy.quantity > 0)
            {
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(randomEnemy.type, randomPoint.position, Quaternion.identity);
                GameObject bar = Instantiate(status_bar, transform.position, transform.rotation, enemy.transform);
                //bar.GetComponent<StatusBarNPC>().target = enemy.gameObject;
                currentWave.nOfEnemies--;
                nextSpawnTime = Time.time + currentWave.spawnInterval;
            } 
            randomEnemy.quantity--;
            if (currentWave.nOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }


}
