using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackShotLaser : MonoBehaviour
{
    [SerializeField] private int _backshotLaserSpeed = 5;


    void Update()
    {
        MoveUp();
    } 
    
    private void MoveUp()
        {
            transform.Translate(Vector3.up * _backshotLaserSpeed * Time.deltaTime);
    
            if (transform.position.y > 8.0f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
    
                Destroy(this.gameObject);
            }
        }
}
