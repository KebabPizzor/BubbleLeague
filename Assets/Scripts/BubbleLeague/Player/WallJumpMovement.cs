using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleLeague.Player
{
    public class WallJumpMovement : MonoBehaviour
    {
        private Rigidbody m_rigidbody;
        private bool m_isGrounded;
        [SerializeField] private GameObject m_sphere;
        [SerializeField] protected float m_jumpForce = 1000.0f;
        [SerializeField] private float m_jumpFastFallDelay = 1f;
        [SerializeField] private Vector3 m_fastFallGravity = new Vector3(0f, -10f, 0f);
        private float m_timeWithoutFastFall;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private float m_groundCheckMultiplier = 1.01f;
        
        protected void Awake()
        {
            m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        }

        public void OnJump()
        {
			if(!m_isGrounded || !gameObject.activeInHierarchy) return;
            Debug.Log("Jump!");
            
            m_rigidbody.AddForce((transform.position-m_sphere.GetComponent<GroundContactSaver>().LatestGroundContact).normalized * m_jumpForce, ForceMode.Acceleration);
            m_timeWithoutFastFall = m_jumpFastFallDelay;
        }

        private void FixedUpdate()
        {
            m_isGrounded = Physics.CheckSphere(GetGroundCheckPosition(), GetGroundCheckRadius(), m_groundLayer);
            if (m_timeWithoutFastFall > 0f)
            {
                m_timeWithoutFastFall -= Time.fixedDeltaTime;
                m_rigidbody.AddForce(m_fastFallGravity, ForceMode.Acceleration);
            }
        }

        Vector3 GetGroundCheckPosition()
        {
            return m_rigidbody.GetComponent<Collider>().bounds.center;
        }

        float GetGroundCheckRadius()
        {
            return m_rigidbody.GetComponent<SphereCollider>().radius * m_groundCheckMultiplier * m_rigidbody.transform.lossyScale.y;
        }
    }
}