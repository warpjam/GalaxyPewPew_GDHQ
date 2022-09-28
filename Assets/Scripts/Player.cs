using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedBoostMultiplier = 2;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _playerLives = 3;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private bool _shieldsActive = false;
    [SerializeField] private GameObject _playerShieldPrefab;
    [SerializeField] private int _score;
    private UIManager _uiManager;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UIManager is NULL");
        }
    }
    
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Firelaser();
        }
    }

    private void Firelaser()
    { 
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5.79f),0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_shieldsActive == true)
        {
            _shieldsActive = false;
            _playerShieldPrefab.SetActive(false);
            return;
        }
        _playerLives -= 1;
        _uiManager.UpdateLives(_playerLives);
        
        if (_playerLives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _speedBoostActive = false;
            _speed /= _speedBoostMultiplier;
        }
    }

    public void ShieldsUp()
    {
        _shieldsActive = true;
        _playerShieldPrefab.SetActive(true);
        
    }

    public void ScoreCalculator(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
