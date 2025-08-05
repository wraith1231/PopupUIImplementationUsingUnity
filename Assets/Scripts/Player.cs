using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector3 _moveDir;
    private SphereButton _button;

    private bool _isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_moveDir != null && _isMoving)
            transform.position += _moveDir * Time.deltaTime;
    }

    public void MovingChange(bool moving)
    {
        _isMoving = moving;
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if(input != null)
        {
            _moveDir = new Vector3(input.x, 0, input.y) * 3;
        }
    }

    public void SetButton(SphereButton button)
    {
        _button = button;
    }

    private void OnJump()
    {
        if (_button != null)
        {
            _button.OpenPopup();
        }
    }
}
