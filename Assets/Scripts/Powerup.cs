using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _powerupSpeed = 3;
    [SerializeField] private int _powerupID; //ID for Powerups 0 = Tripleshot -> 1 = Speed -> 2 = Shields -> 3 == Ammo -> 4 == HealthPickup -> 5 == UniBeam => 6 == JangoMine
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _powerUpSound;
    //private int _rotationSpeed = 60;
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        // if (_powerupID == 6)
        // {
        //     transform.Rotate(new Vector3(0, 0, _rotationSpeed) * Time.deltaTime);
        // }

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsUp();
                        break;
                    case 3:
                        player.AmmoPickup();
                        break;
                    case 4:
                        player.HealthPickup();
                        break;
                    case 5:
                        player.UniBeamPickup();
                        break;
                    case 6:
                        player.JangoMinePickup();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
