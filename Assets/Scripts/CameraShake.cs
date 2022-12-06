using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shakeTime = 0.2f;
    private float _shakePower = 0.2f;

    public void StartShaking()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    public IEnumerator CameraShakeRoutine()
    {
        Vector3 _defaultPosition = transform.position;
        float _timer = 0f;

        while (_timer < _shakeTime)
        {
            float _xPosition = Random.Range(-1f, 1f) * _shakePower;
            float _yPosition = Random.Range(-1f, 1f) * _shakePower;
            transform.position = new Vector3(_xPosition, _yPosition, -10f);
            _timer += Time.deltaTime;
            yield return null;
        }

        transform.position = _defaultPosition;
    }

}
