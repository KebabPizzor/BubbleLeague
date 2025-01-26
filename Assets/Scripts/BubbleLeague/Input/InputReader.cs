using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public UnityEvent<Vector2> MoveEvent;
    public UnityEvent JumpEvent;
    public UnityEvent PauseEvent;
    public UnityEvent<Vector2> LookEvent; 
    public UnityEvent<float> SprintEvent; 

    public void OnMovement(InputValue value)
    {
        MoveEvent?.Invoke(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookEvent?.Invoke(value.Get<Vector2>());
    }

    public void OnSprint(InputValue value)
    {
        SprintEvent?.Invoke(value.Get<float>());
    }
    
    public void OnJump(InputValue _)
    {
        Debug.Log("Trying to Jump.");
        JumpEvent?.Invoke();
    }
    
    public void OnPause(InputValue _)
    {
        PauseEvent?.Invoke();
    }
}