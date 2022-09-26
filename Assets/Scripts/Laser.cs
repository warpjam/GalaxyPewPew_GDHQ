using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _laserSpeed = 8;

    void Start()
    {
        
    }
    
    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
    }
}
