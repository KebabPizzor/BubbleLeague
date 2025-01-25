using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_sphere;

    [SerializeField] private float m_speed = 10.0f;
    [SerializeField] private float m_boostSpeed = 10.0f;
    [Range(0.01f, 1.0f)] [SerializeField] private float m_rotationSpeed;

    private Vector2 m_moveInput;
    private Vector2 m_lookInput;
    private float m_sprintFactor;
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = m_sphere.GetComponent<Rigidbody>();
    }

    public void OnSprint(float sprintForce)
    {
        Debug.Log($"Sprinting: {sprintForce}");
        m_sprintFactor = sprintForce;
    }

    public void OnLook(Vector2 value)
    {
        m_lookInput = value;
    }

    public void OnMove(Vector2 value)
    {
        m_moveInput = value;
    }

    private void Update()
    {
        if (m_lookInput.x != 0f)
        {
            Debug.Log($"Looking: {m_lookInput.x}");
        }
        transform.Rotate(Vector3.up * (m_lookInput.x * m_rotationSpeed));
    }

    private void LateUpdate()
    {
        transform.position = m_sphere.transform.position;
    }

    private void FixedUpdate()
    {
        var movementForce = (m_moveInput.y * transform.forward + m_moveInput.x * transform.right).normalized * Mathf.Lerp(m_speed, m_boostSpeed, m_sprintFactor);
        if(movementForce != Vector3.zero) Debug.Log($"Moving: {movementForce} ({movementForce.magnitude})");
        m_rigidbody.AddForce(movementForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}