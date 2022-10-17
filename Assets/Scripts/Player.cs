using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5;
    [SerializeField] private GameObject _mainEnginePrefab;
    [SerializeField] private float _speedBoostMultiplier = 2.5f;
    [SerializeField] private bool _speedBoostActive = false;

    [Header("Thrusters")] 
    [SerializeField] private float _thrusterSpeed = 8;
    [SerializeField] private GameObject _thrustersPrefab;
    [SerializeField] private int _thrustPower = 100;
    [SerializeField] private bool _canThrust = true;
    
    [Header("Weapons")]
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _uniBeamPrefab;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _uniBeamActive;
    [SerializeField] private AudioClip _basicLaserSound;
    [SerializeField] private AudioClip _emptyLaserSound;
    [SerializeField] private AudioClip _uniBeamSound; 
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;

    
    [Header("Lives-Shields & Damage")]
    [SerializeField] private int _playerLives = 3;
    [SerializeField] private bool _shieldsActive = false;
    [SerializeField] private int _shieldHits;
    private SpriteRenderer _shieldHitColor;
    [SerializeField] private GameObject _playerShieldPrefab;
    [SerializeField] private GameObject _leftDamage;
    [SerializeField] private GameObject _rightDamage;
    

    [Header("Misc")]
    private int _score;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private CameraShake _cameraShake;
    
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.Log("The spawn manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UIManager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.Log("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _basicLaserSound;
        }

        if (_cameraShake == null)
        {
            Debug.Log("The Camera Shake is NULL");
        }
    }
    
    void Update()
    {
        PlayerMovement();
        

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            if (_ammoCount == 0 && _tripleShotActive == !true)
            {
                AudioSource.PlayClipAtPoint(_emptyLaserSound, transform.position);
            }
            else
            {
                Firelaser();
            }
    }

    IEnumerator RechargeThrusters()
    {
        yield return new WaitForSeconds(5f);
        while (_thrustPower < 100 && _canThrust == false)
        {
            _thrustPower = 100;
            _uiManager.UpdateThrustSlider(_thrustPower);
            _canThrust = true;
        }
    }
    private void Firelaser()
    {
        if (_uniBeamActive != true)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                _ammoCount--;
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                _uiManager.UpdateAmmoCount(_ammoCount);
            }

            _audioSource.Play();
        }
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.RightShift) && _speedBoostActive != true)
        {
            transform.Translate(direction * _thrusterSpeed * Time.deltaTime);
            _thrustPower--;
            _uiManager.UpdateThrustSlider(_thrustPower);
            if (_thrustPower == 0)
            {
                _canThrust = false;
                StartCoroutine(RechargeThrusters());
            }
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
            
        
        
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
            _shieldHits--;
            _cameraShake.StartShaking();

            switch (_shieldHits)
            {
                case 0:
                    _shieldsActive = false;
                    _playerShieldPrefab.SetActive(false);
                    return;
                case 1:
                    _shieldHitColor.color = Color.red;
                    return;
                case 2:
                    _shieldHitColor.color = Color.green;
                    return;
                default:
                    break;
            }
            
            _shieldsActive = false;
            _playerShieldPrefab.SetActive(false);
            return;
        }
        _playerLives -= 1;
        _uiManager.UpdateLives(_playerLives);
        _cameraShake.StartShaking();

        if (_playerLives == 2)
        {
            _rightDamage.SetActive(true);
        }
        
        else if (_playerLives == 1)
        {
            _leftDamage.SetActive(true);
        }

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
        _shieldHitColor = _playerShieldPrefab.GetComponent<SpriteRenderer>();
        _shieldHitColor.color = Color.cyan;
        _shieldHits = 3;
        _shieldsActive = true;
        _playerShieldPrefab.SetActive(true);
        
    }

    public void ScoreCalculator(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoPickup()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    public void HealthPickup()
    {
        if (_playerLives == 1)
        {
            _playerLives++;
            _leftDamage.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
        else if (_playerLives == 2)
        {
            _playerLives++;
            _rightDamage.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
        
        
        
    }

    public void UniBeamPickup()
    {
        _uniBeamActive = true;
        AudioSource.PlayClipAtPoint(_uniBeamSound, transform.position);
        _uniBeamPrefab.SetActive(true);
        StartCoroutine(UniBeamPowerDownRoutine());
    }

    IEnumerator UniBeamPowerDownRoutine()
    {
        yield return new WaitForSeconds(6.0f);
        _uniBeamPrefab.SetActive(false);
        _uniBeamActive = false;
    }

    public void JangoMinePickup()
    {
        Debug.Log("Jago Mine Whomp!");
        Damage();
    }
}
