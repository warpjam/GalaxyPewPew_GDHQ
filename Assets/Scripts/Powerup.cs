using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _powerupSpeed = 3;

    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

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
                player.TripleShotActive();
            }
            
            Destroy(this.gameObject);
        }
    }
}
