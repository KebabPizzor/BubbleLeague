using UnityEngine;
public class CarCharacter : PlayerBase
{
    //[SerializeField] private InputReader m_inputReader;
    [SerializeField] private float m_motorTorque = 2000.0f;
    [SerializeField] private float m_boostMotorTorque = 2000.0f;
    [SerializeField] private float m_brakeTorque = 2000.0f;
    [SerializeField] private float m_maxSpeed = 20.0f;
    [SerializeField] private float m_steeringRange = 30.0f;
    [SerializeField] private float m_steeringRangeAtMaxSpeed = 10.0f;
    [SerializeField] private float m_centreOfGravityOffset = -1.0f;
    
    
    private Vector2 m_moveInput = Vector2.zero;
    private WheelControl[] m_wheels;

    protected void OnEnable()
    {
        Initialize();
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.centerOfMass += Vector3.up * m_centreOfGravityOffset;
        m_wheels = GetComponentsInChildren<WheelControl>();
    }

    private void OnDisable()
    {
        Dispose();
    }

    protected override void OnLook(Vector2 input)
    {
    }

    protected override void OnMove(Vector2 input)
    {
        m_moveInput = input;
    }

    private void Update()
    {
        var forwardSpeed = Vector3.Dot(transform.forward, m_rigidbody.linearVelocity);
        var speedFactor = Mathf.InverseLerp(0, m_maxSpeed, forwardSpeed);
        var currentMotorTorque = Mathf.Lerp(m_isBoosting ? m_motorTorque : m_boostMotorTorque, 0, speedFactor);
        var currentSteerRange = Mathf.Lerp(m_steeringRange, m_steeringRangeAtMaxSpeed, speedFactor);
        var isAccelerating = Mathf.Sign(m_moveInput.y) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in m_wheels)
        {
            if (wheel.steerable)
            {
                
                wheel.WheelCollider.steerAngle = wheel.isLeftWheel ? m_moveInput.x * currentSteerRange : -m_moveInput.x * currentSteerRange;
                
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