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
    int currentWaveNumber = 0;


	// Use this for initialization
	void Start () {
        currentWave = Waves[currentWaveNumber];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartSpawn();
        }
	}

    void StartSpawn()
    {
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
