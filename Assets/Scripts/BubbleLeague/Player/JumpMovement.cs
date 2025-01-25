using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleLeague.Player
{
    public class JumpMovement : MonoBehaviour
    {
        private Rigidbody m_rigidbody;
        private bool m_isGrounded;
        [SerializeField] private GameObject m_sphere;
        [SerializeField] private float m_jumpForce = 10000.0f;
        [SerializeField] private LayerMask m_groundLayer;
        [Range(0.01f, 1.0f)][SerializeField] private float m_groundCheckRadius = 0.2f;

        protected void Awake()
        {
            m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            m_isGrounded = Physics.CheckSphere(transform.position, 0.7f, m_groundLayer);
        }

        public void OnJump()
        {
            if (m_isGrounded)
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }
        }
    }
}