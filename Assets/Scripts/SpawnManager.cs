using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemiesToSpawn;
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
                _uiManager.NextWave(i + 2);
                for (int j = 0; j < _enemiesPerWave[i]; j++)
                {
                    if (_stopSpawning == true)
                        StopAllCoroutines();
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    int _randomEnemy = Random.Range(0,2);
                    GameObject newEnemy = Instantiate(_enemiesToSpawn[_randomEnemy], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _enemies.Add(newEnemy);
                    yield return new WaitForSeconds(_timeToNextWave[i]);
                }
                yield return new WaitUntil(() => _enemies.Count == 0);
            }
    }

    /*private void PowerUpProbabilityChecker(Vector3 randomXposition){
    
        var randomValue = Random.value;

        if (randomValue > 0f && randomValue <= .05f)
            Instantiate(_powerups[0], randomXposition, Quaternion.identity);
        else if (randomValue > 0.05f && randomValue <= 0.2f) //SHIELDS
            Instantiate(_powerups[1], randomXposition, Quaternion.identity);
        else if (randomValue > 0.2f && randomValue <= 0.4f) //health
            Instantiate(_powerups[2], randomXposition, Quaternion.identity);
        else if (randomValue > 0.4f && randomValue <= 0.6f) //TripleShot
            Instantiate(_powerups[3], randomXposition, Quaternion.identity);
        else if (randomValue > 0.6f && randomValue <= 1f) //Ammo
            Instantiate(_powerups[4], randomXposition, Quaternion.identity);
    }*/

    private int PowerUpChooser()
    {
        var random = Random.Range(0, 101);
        if (random >= 75) //Ammo
        {
            return 3;
        }
        if (random >= 60 && random < 75) //Speedboost
        {
            return 1;
        }
        if (random >= 45 && random < 60) //JangoMine
        {
            return 6;
        }
        if (random >= 30 && random < 45) //shields
        {
            return 2;
        }
        if (random >= 15 && random < 30) //tripleshot 
        {
            return 0;
        }
        if (random >= 5 && random < 15) //health
        {
            return 4;
        }

        if (random >= 0 && random < 5)//unibeam
        {
            return 5;
        }

        return 3;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 9, 0);
            //int _randomPowerup = Random.Range(0, 7);
            Instantiate(_powerups[PowerUpChooser()], _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 10));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
