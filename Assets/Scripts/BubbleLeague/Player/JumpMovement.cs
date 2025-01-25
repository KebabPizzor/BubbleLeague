using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleLeague.Player
{
    public class JumpMovement : MonoBehaviour
    {
        private Rigidbody m_rigidbody;
        [SerializeField] private GameObject m_sphere;
        [SerializeField] protected float m_jumpForce = 10000.0f;
        [SerializeField] private float m_jumpFastFallDelay = 1f;
        [SerializeField] private Vector3 m_fastFallGravity = new Vector3(0f, -10f, 0f);
        private float m_timeWithoutFastFall;

        protected void Awake()
        {
            m_rigidbody = m_sphere.GetComponent<Rigidbody>();
        }

        public void OnJump()
        {
            Debug.Log("Jump!");
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Acceleration);
            m_timeWithoutFastFall = m_jumpFastFallDelay;
        }

        private void FixedUpdate()
        {
            if (m_timeWithoutFastFall > 0f)
            {
                m_timeWithoutFastFall -= Time.fixedDeltaTime;
                m_rigidbody.AddForce(m_fastFallGravity, ForceMode.Acceleration);
            }
        }
    }
}