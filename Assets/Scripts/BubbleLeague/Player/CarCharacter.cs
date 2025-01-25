using UnityEngine;
using UnityEngine.Serialization;

public class CarCharacter : MonoBehaviour
{
    [SerializeField] private InputReader m_inputReader;
    [SerializeField] private float m_motorTorque = 2000.0f;
    [SerializeField] private float m_brakeTorque = 2000.0f;
    [SerializeField] private float m_maxSpeed = 20.0f;
    [SerializeField] private float m_steeringRange = 30.0f;
    [SerializeField] private float m_steeringRangeAtMaxSpeed = 10.0f;
    [SerializeField] private float m_centreOfGravityOffset = -1.0f;

    private Vector2 m_moveInput = Vector2.zero;
    private WheelControl[] m_wheels;
    private Rigidbody m_rigidBody;

    private void OnEnable()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.centerOfMass += Vector3.up * m_centreOfGravityOffset;
        m_wheels = GetComponentsInChildren<WheelControl>();

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

    private void OnLook(Vector2 input)
    {
    }

    private void OnBreak(bool input)
    {
    }

    private void OnMove(Vector2 input)
    {
        m_moveInput = input;
    }

    private void Update()
    {
        var forwardSpeed = Vector3.Dot(transform.forward, m_rigidBody.linearVelocity);
        var speedFactor = Mathf.InverseLerp(0, m_maxSpeed, forwardSpeed);
        var currentMotorTorque = Mathf.Lerp(m_motorTorque, 0, speedFactor);
        var currentSteerRange = Mathf.Lerp(m_steeringRange, m_steeringRangeAtMaxSpeed, speedFactor);
        var isAccelerating = Mathf.Sign(m_moveInput.y) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in m_wheels)
        {
            if (wheel.steerable)
            {
                Debug.Log(m_moveInput.x);
                wheel.WheelCollider.steerAngle = m_moveInput.x * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = m_moveInput.y * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = Mathf.Abs(m_moveInput.y) * m_brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}