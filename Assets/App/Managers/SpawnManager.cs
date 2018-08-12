using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<SpawnableObject> Enemies;
}

[System.Serializable]
public class SpawnableObject
{
    public Enemy Prefab;
    public int Quantity = 1;
}

public class SpawnManager : MonoBehaviour {

    public List<GameObject> SpawnPoints;
    public List<Wave> Waves;
    public GameObject EnemyGoal;


    Wave currentWave;
    int currentWaveNumber = -1;
    List<Enemy> currentEnemies = new List<Enemy>();

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var stillAlive = currentEnemies.Find(enemy => enemy != null);
            if (stillAlive) { return; }

            if(currentWaveNumber < Waves.Count - 1)
            {
                currentWaveNumber++;
            }else
            {
                Debug.Log("YOU WIN!!!");
                return;
            }
            currentWave = Waves[currentWaveNumber];
            StartSpawn();
        }
	}

    void StartSpawn()
    {
        currentEnemies = new List<Enemy>();
        currentWave.Enemies.ForEach(SpawnEnemies);
    }

    void SpawnEnemy(Enemy enemy)
    {
        var spawnLocationNumber = Random.Range(0, SpawnPoints.Count);
        enemy.SpawnLocation = SpawnPoints[spawnLocationNumber].transform;
        var go = Instantiate(
                enemy,
                enemy.SpawnLocation.position,
                enemy.SpawnLocation.rotation
                );
        var motor = go.GetComponent<AIMotor>();
        motor.EnemyGoal = EnemyGoal;
        currentEnemies.Add(go);
    }

    void SpawnEnemies(SpawnableObject enemy)
    {
       
        for(
            var numberOfEnemiesSpawned = 0; 
            numberOfEnemiesSpawned < enemy.Quantity;
            numberOfEnemiesSpawned += 1)
        {
            SpawnEnemy(enemy.Prefab);
        }
    }

}
