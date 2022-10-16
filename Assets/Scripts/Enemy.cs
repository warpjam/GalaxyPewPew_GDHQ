using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 4.0f;
    private int _movementType; // 0 = straight, 1 = wave
    private Player _player;
    private Animator _enemyExplosion;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canfire = -1f;
    private bool _lasersActive;
    private SpawnManager _spawnManager;
    




    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _movementType = Random.Range(0, 2);
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

        if (Time.time > _canfire && _lasersActive == true) 
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject _enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

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
                transform.Translate(new Vector3(Mathf.Cos(Time.time * 4) * 2, -1, 0) * _enemySpeed * Time.deltaTime);
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
    
}
