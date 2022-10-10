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
    [SerializeField] public Slider _thrustSlider;
    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/15";
        _gameOverTxt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL");
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



}
