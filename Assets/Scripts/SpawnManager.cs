using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _commonEnemy;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool _stopSpawning = false;
    private UIManager _uiManager;
    [SerializeField] private int[] _enemiesPerWave;
    [SerializeField] private float[] _timeToNextWave;
    [SerializeField] public List<GameObject> _enemies = new List<GameObject>();

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UIManager is NULL");
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        
            for (int i = 0; i < _enemiesPerWave.Length; i++)
            {
                _uiManager.NextWave(i + 1);
                for (int j = 0; j < _enemiesPerWave[i]; j++)
                {
                    if (_stopSpawning == true)
                        StopAllCoroutines();
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(_commonEnemy, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _enemies.Add(newEnemy);
                    yield return new WaitForSeconds(_timeToNextWave[i]);
                }
                yield return new WaitUntil(() => _enemies.Count == 0);
            }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 9, 0);
            int _randomPowerup = Random.Range(0, 7);
            Instantiate(_powerups[_randomPowerup], _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 10));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
