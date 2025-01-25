using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    private GameInput m_gameInput;
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<bool> JumpEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { }; 

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke(true);
        }

        if (context.canceled)
        {
            JumpEvent?.Invoke(false);
        }
    }

    public void Initialize()
    {
        if (m_gameInput == null)
        {
            m_gameInput = new GameInput();
            m_gameInput.Gameplay.SetCallbacks(this);
            m_gameInput.Gameplay.Enable();
        }
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Dispose()
    {
        if (m_gameInput != null)
        {
            m_gameInput.Gameplay.RemoveCallbacks(this);
            m_gameInput.Gameplay.Disable();
            m_gameInput = null;
        }
    }
}