using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_sphere;
    [SerializeField] private GameObject m_camera;

    [SerializeField] private float m_speed = 10.0f;
    [SerializeField] private float m_boostSpeed = 10.0f;
    [SerializeField] private float m_energyDrainSpeed = 10.0f;
    [Range(0.01f, 1.0f)] [SerializeField] private float m_rotationSpeed;

    [SerializeField] private AudioSource chargeSFX;
    
    private Vector2 m_moveInput;
    private Vector2 m_lookInput;
    private Rigidbody m_rigidbody;
    private PlayerAttributes m_playerAttributes;

    //Sprint
    private float m_sprintFactor;
    private bool m_isSprinting;

    private void Awake()
    {
        m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        m_playerAttributes = GetComponent<PlayerAttributes>();
    }

    public void OnSprint(float sprintForce)
    {
        m_sprintFactor = sprintForce;
        m_isSprinting = m_sprintFactor > 0f;
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
        if (chargeSFX.isPlaying && !m_isSprinting || m_playerAttributes.GetCurrentEnergy() <= 0f)
        {
            chargeSFX.Stop();
        } else if (!chargeSFX.isPlaying && m_isSprinting && m_playerAttributes.GetCurrentEnergy() > 0f)
        {
            chargeSFX.Play();
        }
        transform.Rotate(Vector3.up * (m_lookInput.x * m_rotationSpeed) + Vector3.left * (m_lookInput.y * m_rotationSpeed));
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        if (m_isSprinting)
            m_playerAttributes.ChangeEnergy(-Time.deltaTime * m_energyDrainSpeed);
    }

    private void LateUpdate()
    {
        transform.position = m_sphere.transform.position;
    }

    private void FixedUpdate()
    {
        var movementForce = (m_moveInput.y * transform.forward + m_moveInput.x * transform.right).normalized *
                            Mathf.Lerp(m_speed, m_boostSpeed, m_playerAttributes.GetCurrentEnergy() > 0f ? m_sprintFactor : 0f);
        m_rigidbody.AddForce(movementForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    public void OnPowerUpCollected()
    {
        GetComponentInParent<Player>().PlayPickUpSound();
        m_playerAttributes.RefillEnergy();
    }

    public void Reset()
    {
        m_playerAttributes?.Initialize();
    }
}