using UnityEngine;

public class BallCharacter : MonoBehaviour
{
    [SerializeField] protected InputReader m_inputReader;
    [SerializeField] private GameObject m_sphere;

    [SerializeField] private float m_speed = 10.0f;
    [SerializeField] private float m_boostSpeed = 10.0f;
    [Range(0.01f, 1.0f)] [SerializeField] private float m_rotationSpeed = 10.0f;

    private Rigidbody m_sphereRigidBody;
    private Vector2 m_moveInput = Vector2.zero;
    private Vector2 m_lookInput = Vector2.zero;
    private bool m_isBoosting = false;

    private void OnEnable()
    {
        m_sphereRigidBody = m_sphere.GetComponent<Rigidbody>();
       if (m_inputReader != null)
       {
           m_inputReader.Initialize();
           m_inputReader.MoveEvent += OnMove;
           m_inputReader.BreakEvent += OnBreak;
           m_inputReader.LookEvent += OnLook;
       }
    }

    private void OnDisable()
    {
        if (m_inputReader != null)
        {
            m_inputReader.MoveEvent -= OnMove;
            m_inputReader.BreakEvent -= OnBreak;
            m_inputReader.LookEvent -= OnLook;
            m_inputReader.Dispose();
        }   
    }

    protected void OnBreak(bool input)
    {
        Debug.Log("PlayerCharacter.OnBreak");
    }

    protected void OnLook(Vector2 input)
    {
        m_lookInput = input;
    }

    protected void OnMove(Vector2 input)
    {
        m_moveInput = input;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (m_lookInput.x * m_rotationSpeed));
    }

    private void LateUpdate()
    {
        transform.position = m_sphere.transform.position;
    }

    private void FixedUpdate()
    {
        var movementForce = m_moveInput.y * transform.forward + m_moveInput.x * transform.right;
        m_sphereRigidBody.AddForce(movementForce * ((m_isBoosting ? m_boostSpeed : m_speed) * Time.fixedDeltaTime),
            ForceMode.Acceleration);
    }
}