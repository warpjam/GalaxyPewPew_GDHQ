using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _commonEnemy;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private UIManager _uiManager;
    [SerializeField] private int _waveNumber;
    [SerializeField] private int _enemiesKilled;
    [SerializeField] private int _maxEnemies;
    [SerializeField] private int _enemiesLeftToSpawn;
    [SerializeField] private bool _stopSpawning = true;


    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UIManager is NULL!");
        }
    }



    public void StartSpawning(int waveNumber)
    {
        _stopSpawning = false;
        _enemiesKilled = 0;
        _waveNumber = waveNumber;
        _uiManager.DisplayWaveText(_waveNumber);
        _enemiesLeftToSpawn = _waveNumber + 1;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false && _enemiesKilled <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_commonEnemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesLeftToSpawn--;
            if (_enemiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(5.0f);
        }
        StartSpawning(_waveNumber + 1);
    }

    public void EnemyKilled()
    {
        _enemiesKilled ++;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 9, 0);
            int _randomPowerup = Random.Range(0, 6);
            Instantiate(_powerups[_randomPowerup], _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 15));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
