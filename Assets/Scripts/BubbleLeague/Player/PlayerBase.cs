using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected InputReader m_inputReader;
    
    protected void Initialize()
    {
        if (m_inputReader != null)
        {
            m_inputReader.Initialize();
            m_inputReader.MoveEvent += OnMove;
            m_inputReader.JumpEvent += OnJump;
            m_inputReader.LookEvent += OnLook;
        }
    }

    protected void Dispose()
    {
        if (m_inputReader != null)
        {
            m_inputReader.MoveEvent -= OnMove;
            m_inputReader.JumpEvent -= OnJump;
            m_inputReader.LookEvent -= OnLook;
            m_inputReader.Dispose();
        }
    }
    
    protected virtual void OnLook(Vector2 input)
    {
    }

    protected virtual void OnJump(bool input)
    {
    }

    protected virtual void OnMove(Vector2 input)
    {
    }
}