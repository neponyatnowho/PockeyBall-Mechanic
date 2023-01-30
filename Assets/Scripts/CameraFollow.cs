using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _ball;

    private float _lastBallPositionY;
    private float _fixFloreY;

    private void Start()
    {
        _fixFloreY = transform.position.y;
        _lastBallPositionY = _fixFloreY;
    }
    private void Update()
    {
        if(_ball.position.y > _lastBallPositionY)
        {
            _lastBallPositionY = _ball.position.y;
            transform.position = new Vector3(transform.position.x, _lastBallPositionY, transform.position.z);
        }

        if(_ball.position.y < _lastBallPositionY - 2f)
        {
            if(_ball.position.y > _fixFloreY)
            {
                _lastBallPositionY = _ball.position.y + 2f;
                transform.position = new Vector3(transform.position.x, _lastBallPositionY, transform.position.z);
            }
        }
    }
}
