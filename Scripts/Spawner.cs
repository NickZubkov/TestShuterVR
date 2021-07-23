using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPosition = null;
    [SerializeField] private GameObject _enemyPref = null;
    [SerializeField] private int _enemyLimit = 20;
    [SerializeField] private int _spawnDelay = 2;

    private int _enemyCounter = 0;

    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }
    private IEnumerator EnemySpawn()
    {
        while (_enemyCounter < _enemyLimit)
        {
            var spawnID = Random.Range(0, _spawnPosition.Length);
            Instantiate(_enemyPref, _spawnPosition[spawnID].position, Quaternion.identity);
            _enemyCounter++;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}

