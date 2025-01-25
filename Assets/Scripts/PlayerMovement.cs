using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody _rbody;
    private bool _disabled;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rbody = GetComponent<Rigidbody>();
    }

    public void Disable()
    {
        _disabled = true;
    }
    private void FixedUpdate()
    {
        if (_disabled) return;
        
        var moveInput = _playerInput.actions["Movement"].ReadValue<Vector2>();
        
        Debug.Log($"Move Input: {moveInput}");
    }

    void OnMovement(InputValue value)
    {
        
    }

    void OnSprint(InputValue value)
    {
        Debug.Log($"Sprint On: {value.Get<float>()}");
    }

    void OnJump(InputValue _)
    {
        Debug.Log("Jump!");
    }

    void OnDash(InputValue _)
    {
        Debug.Log("Dash!");
    }
}
