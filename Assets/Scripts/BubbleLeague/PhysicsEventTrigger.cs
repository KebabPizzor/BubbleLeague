using System;
using UnityEngine;
using UnityEngine.Events;

namespace BubbleLeague
{
    public class PhysicsEventTrigger : MonoBehaviour
    {
        public UnityEvent<bool> OnContact;

        private void OnTriggerEnter(Collider _)
        {
            OnContact?.Invoke(true);
        }

        private void OnTriggerExit(Collider _)
        {
            OnContact?.Invoke(false);
        }

        private void OnCollisionEnter(Collision _)
        {
            OnContact?.Invoke(true);
        }

        private void OnCollisionExit(Collision _)
        {
            OnContact?.Invoke(false);
        }
    }
}
