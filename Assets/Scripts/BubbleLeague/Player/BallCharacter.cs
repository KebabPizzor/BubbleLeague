using UnityEngine;

public class BallCharacter : PlayerBase
{
    [SerializeField] private GameObject m_sphere;

    [SerializeField] private float m_speed = 10.0f;
    [SerializeField] private float m_boostSpeed = 10.0f;
    [Range(0.01f, 1.0f)] [SerializeField] private float m_rotationSpeed = 10.0f;

    private Vector2 m_moveInput = Vector2.zero;
    private Vector2 m_lookInput = Vector2.zero;

    private void OnEnable()
    {
        Initialize();
        m_rigidbody = m_sphere.GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        Dispose();
    }

    protected override void OnLook(Vector2 input)
    {
        m_lookInput = input;
    }

    protected override void OnMove(Vector2 input)
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
        m_rigidbody.AddForce(movementForce * ((m_isBoosting ? m_boostSpeed : m_speed) * Time.fixedDeltaTime),
            ForceMode.Acceleration);
    }
}