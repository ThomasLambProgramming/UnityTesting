using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBallTest : MonoBehaviour
{
    [SerializeField] GameObject _baseballObject = null;
    [SerializeField] float _moveSpeed = 5f;
    private float _timer = 0;
    private BezierCurve _curve = null;
    private bool _throwing = false;
    // Start is called before the first frame update
    void Start()
    {
        _curve = GetComponent<BezierCurve>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _throwing = true;
            _timer = 0;
            _baseballObject.SetActive(true);
        }

        if (_throwing)
        {
            _timer += Time.deltaTime * _moveSpeed;

            if (_timer >= 1)
            {
                _throwing = false;
                _baseballObject.SetActive(false);
            }

            _baseballObject.transform.position = _curve.GetPoint(_timer);
        }
    }
}
