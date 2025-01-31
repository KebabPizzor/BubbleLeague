using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleLeague.Player
{
    public class BottomGroundCheckJumpMovement : MonoBehaviour
    {
        private Rigidbody m_rigidbody;
        private bool m_isGrounded;
        [SerializeField] private GameObject m_sphere;
        [SerializeField] protected float m_jumpForce = 1000.0f;
        [SerializeField] private float m_jumpFastFallDelay = 1f;
        [SerializeField] private Vector3 m_fastFallGravity = new Vector3(0f, -10f, 0f);
        private float m_timeWithoutFastFall;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_groundCheckRadius = 0.2f;

        protected void Awake()
        {
            m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        }

        public void OnJump()
        {
			if(!m_isGrounded || !gameObject.activeInHierarchy) return;
            Debug.Log("Jump!");
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Acceleration);
            m_timeWithoutFastFall = m_jumpFastFallDelay;
        }

        private void FixedUpdate()
        {
            
            m_isGrounded = Physics.CheckSphere(GetGroundCheckPosition(), m_groundCheckRadius, m_groundLayer);
            if (m_timeWithoutFastFall > 0f)
            {
                m_timeWithoutFastFall -= Time.fixedDeltaTime;
                m_rigidbody.AddForce(m_fastFallGravity, ForceMode.Acceleration);
            }
        }

        Vector3 GetGroundCheckPosition()
        {
            return m_rigidbody.GetComponent<Collider>().bounds.center - new Vector3(0f, m_rigidbody.GetComponent<Collider>().bounds.extents.y, 0f);
        }
    }
}