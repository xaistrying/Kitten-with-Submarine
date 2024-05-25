using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public Transform[] scrapPrefabs;

    public int numberOfSpawner;
    public float radius;

    public float timeBetweenSpawns;
    public float percentScrapAppear;
    private float waveCountdown;

    private List<GameObject> spawners;
    private List<GameObject> destinations;

    void Start()
    {
        spawners = new List<GameObject>();
        for (int i = 0; i < numberOfSpawner; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSpawner;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector2 pos = transform.position + new Vector3(x,y);
            GameObject spawnPoint = new GameObject("spawner" + i);
            spawnPoint.transform.parent = this.transform;
            spawnPoint.transform.localPosition = pos;
            spawners.Add(spawnPoint);
        }

        destinations = new List<GameObject>();
        for (int i = 0; i < numberOfSpawner; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSpawner;
            float x = Mathf.Cos(angle) * 5f;
            float y = Mathf.Sin(angle) * 5f;
            Vector2 pos = transform.position + new Vector3(x,y);
            GameObject spawnPoint = new GameObject("destination" + i);
            spawnPoint.transform.parent = this.transform;
            spawnPoint.transform.localPosition = pos;
            destinations.Add(spawnPoint);
        }

        waveCountdown = timeBetweenSpawns;
    }

    void Update()
    {
        if (waveCountdown <= 0)
        {
            if (Random.value <= percentScrapAppear)
            {
                SpawnEnemy( scrapPrefabs[Random.Range(0, scrapPrefabs.Length)] );
            }
            else 
            {
                SpawnEnemy( enemyPrefabs[Random.Range(0, enemyPrefabs.Length)] );
            }
            waveCountdown = timeBetweenSpawns;
        }
        else 
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void SpawnEnemy(Transform enemyPrefab)
    {
        Transform _sp = spawners[ Random.Range(0, spawners.Count) ].transform;
        Transform _enemy = Instantiate(enemyPrefab, _sp.position, Quaternion.identity) as Transform;
        var enemyMovement = _enemy.gameObject.AddComponent<EnemyMovement>();
        Transform destinatePoint = destinations[Random.Range(0, destinations.Count)].transform;
        enemyMovement.direction = new Vector2(
            destinatePoint.position.x - _sp.position.x,
            destinatePoint.position.y - _sp.position.y).normalized;
        float angle = Mathf.Atan2(destinatePoint.position.x - _sp.position.x, destinatePoint.position.y - _sp.position.y);
        if (angle*Mathf.Rad2Deg < 180 && angle*Mathf.Rad2Deg > 0) 
        {
            _enemy.Rotate(0,0,-90 -angle * Mathf.Rad2Deg);
            _enemy.localScale += new Vector3(0,-2,0) ;
        }
        else 
        {
            _enemy.Rotate(0,0,-90 -angle * Mathf.Rad2Deg);
            
        }
    }
}
