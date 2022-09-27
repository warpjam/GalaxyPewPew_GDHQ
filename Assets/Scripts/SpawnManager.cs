using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _commonEnemy;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _tripleShotPowerupPrefab;

    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_commonEnemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 9, 0);
            Instantiate(_tripleShotPowerupPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 15));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
