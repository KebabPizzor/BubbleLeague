using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleLeague.Player
{
    public class JumpMovement : MonoBehaviour
    {
        private Rigidbody m_rigidbody;
        [SerializeField] private GameObject m_sphere;
        [SerializeField] protected float m_jumpForce = 10000.0f;

        protected void Awake()
        {
            m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        }

        public void OnJump()
        {
            Debug.Log("Jump!");
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }
}