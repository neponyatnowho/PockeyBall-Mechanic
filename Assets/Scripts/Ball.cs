using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector3 _forceAngle;
    [SerializeField] private float _force;
    [SerializeField] private float _stickSpawnDelay;
    [SerializeField] private float _tensionSensity;
    [SerializeField] private GameObject _stick;

    private Quaternion _startRotation;
    private float _tension;
    private float lastMousePosition;
    private Rigidbody _rigitbody;
    private bool _isInStick;
    private Animator _stickAnimator;
    private Coroutine _stickCorutine;
    private float _elapsetTimeBeforeSpawn;

    private void OnEnable()
    {
        _rigitbody = GetComponent<Rigidbody>();
        _isInStick = true;
        _startRotation = transform.rotation;
    }
    private void Update()
    {
        _elapsetTimeBeforeSpawn += Time.deltaTime;

        if (Input.GetMouseButtonUp(0))
        {
            if(_tension > 0.1f && transform.parent)
            {
                _stickCorutine = StartCoroutine(StickBase(transform.parent.gameObject, _tension));
                
                transform.parent = null;
                transform.rotation = _startRotation;
                _rigitbody.isKinematic = false;
                _isInStick = false;
                _rigitbody.AddForce(_forceAngle * _tension * _force, ForceMode.Impulse);
                _tension = 0;
            }
            
        }

        if (Input.GetMouseButtonDown(0) && _elapsetTimeBeforeSpawn > _stickSpawnDelay)
        {
            StopAllCoroutines();
            lastMousePosition = Input.mousePosition.y;
            if (!_isInStick)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.TryGetComponent(out Block block))
                    {
                        _rigitbody.isKinematic = true;
                        var stick = Instantiate(_stick, GetStickSpawnPoint(_stick), _stick.gameObject.transform.rotation);
                        transform.parent = stick.GetComponentInChildren<LastBone>().transform;
                        transform.position = transform.parent.position;
                        _isInStick = true;

                    }
                }
            }
            _elapsetTimeBeforeSpawn = 0f;
        }

        if(Input.GetMouseButton(0))
        {

            _tension = (lastMousePosition - Input.mousePosition.y )/ _tensionSensity;
            _tension = Mathf.Clamp(_tension, 0f, 1.0f);

            if(transform.parent)
            {
                _stickAnimator = GetComponentInParent<Animator>();
                _stickAnimator.SetFloat("Blend", _tension);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Floor flor))
        {
            SceneManager.LoadScene(0);
        }
    }

    private Vector3 GetStickSpawnPoint(GameObject stick)
    {
        return new Vector3(stick.transform.position.x, transform.position.y, 0f);
    }

    private IEnumerator StickBase(GameObject stick, float tension)
    {
        var animator = stick.gameObject.GetComponentInParent<Animator>();
        while (tension > 0.01)
        {
            tension = Mathf.Lerp(tension, 0f, _force * Time.deltaTime);
            animator.SetFloat("Blend", tension);
            yield return null;
        }
        yield return null;

    }
}
