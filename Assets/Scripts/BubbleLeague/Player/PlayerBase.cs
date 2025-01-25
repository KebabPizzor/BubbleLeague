using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] protected InputReader m_inputReader;
    [SerializeField] protected float m_jumpForce = 10000.0f;
    protected Rigidbody m_rigidbody;
    
    protected bool m_isBoosting = false;
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

    private void OnJump(bool input)
    {
        m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
    }

    protected virtual void OnMove(Vector2 input)
    {
    }
}