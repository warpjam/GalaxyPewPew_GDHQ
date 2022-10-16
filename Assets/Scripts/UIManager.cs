using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private TMP_Text _gameOverTxt;
    [SerializeField] private TMP_Text _restartTxt;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] public Slider _thrustSlider;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/15";
        _gameOverTxt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL!");
        }
    }

    public void UpdateScore(int scoreCount)
    {
        _scoreText.text = "Score: " + scoreCount.ToString();
    }
    
    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoText.text = "Ammo:" + ammoCount.ToString() + "/15";
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateThrustSlider(float charge)
    {
        _thrustSlider.value = charge;
    }
    

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverTxt.gameObject.SetActive(true);
        _restartTxt.gameObject.SetActive(true);
        
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverTxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DisplayWaveText(int waveNumber)
    {
        _waveText.text = "Wave " + waveNumber;
        _waveText.gameObject.SetActive(true);
        StartCoroutine(WaveTextRoutine());
    }

    IEnumerator WaveTextRoutine()
    {
        while (_waveText == true)
        {
            yield return new WaitForSeconds(3.0f);
            _waveText.gameObject.SetActive(false);
        }
    }
    
}
