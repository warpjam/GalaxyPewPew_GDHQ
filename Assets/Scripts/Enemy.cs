using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyID; //type0 = commonEnemy, type 1 = enemy2, type 2 = backfire enemy 
    [SerializeField] private float _enemySpeed = 4.0f;
    private int _movementType; // 0 = straight, 1 = wave, 2 = rammer;
    private Player _player;
    private Animator _enemyExplosion;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _backShotLaserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _lasersActive;
    private SpawnManager _spawnManager;
    private bool _isShieldActive;
    [SerializeField] private GameObject _enemyShield;
    private float _detectZone = 10f;
    private int _ramSpeed = 3;
    




    private void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            _isShieldActive = true;
            _enemyShield.SetActive(true);
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _movementType = Random.Range(0, 3);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The player is NULL");
        }

        _lasersActive = true;
        _enemyExplosion = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_enemyExplosion == null)
        {
            Debug.Log("The Animation is NULL");
        }

        if (_audioSource == null)
        {
            Debug.Log("The enemy AudioSource is NULL");
        }
    }

    void Update()
    {
        EnemyMovement();
        EnemyFire();
        CheckPlayerBehindEnemy();

    }

    public void EnemyFire()
    {
        if (Time.time > _canFire && _enemyID == 0 && _lasersActive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject _enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

        if (Time.time > _canFire && _enemyID == 1)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_missilePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,180)));
        }

    }

    private void EnemyMovement()
    {
        switch (_movementType)
        {
            case 0:
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(new Vector3(Mathf.Cos(Time.time * 2) * 1, -1, 0) * Time.deltaTime);
                break;
            case 2:
                if (Vector3.Distance(_player.transform.position, transform.position) < _detectZone)
                    transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
                        _ramSpeed * Time.deltaTime);
                else if(Vector3.Distance(_player.transform.position, transform.position) > _detectZone)
                    transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
                break;
            default:
                return;
        }

        if (transform.position.y < -5.3f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _enemyShield.SetActive(false);
            //Destroy(other.gameObject);
            return;
        }
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0.0f;
            _lasersActive = false;
            Destroy(GetComponent<Collider2D>());
            _spawnManager._enemies.Remove(this.gameObject);
            Destroy(this.gameObject, 2.8f);
        }
        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                
                _player.ScoreCalculator(10);
            }
            //trigger anim
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0.0f;
            _lasersActive = false;
            Destroy(GetComponent<Collider2D>());
            _spawnManager._enemies.Remove(this.gameObject);
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("UniBeam"))
        {
            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0.0f;
            _lasersActive = false;
            Destroy(GetComponent<Collider2D>());
            _spawnManager._enemies.Remove(this.gameObject);
            Destroy(this.gameObject, 2.8f);
        }
    }

    private void CheckPlayerBehindEnemy()
    {
        float playerX = _player.transform.position.x;
        float enemyX = gameObject.transform.position.x;
        float playerY = _player.transform.position.y;
        float enemyY = gameObject.transform.position.y;
        float xRange = 15f;
        float yRange = 5f;

        if (playerX < enemyX && playerX >= enemyX - xRange && playerY < 
            enemyY + yRange && playerY > enemyY - yRange)
        {
            Debug.Log("Player is behind...Fire");
            BackShotFire();
            return;
        } else {
            return;
        }

    }
    
    
    IEnumerator BackShotFire()
    {
        yield return new WaitForSeconds(1.0f);
        Instantiate(_backShotLaserPrefab, transform.position, Quaternion.identity);
    }


}
