using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace BubbleLeague
{
    public class Hoop : MonoBehaviour
    {
        public UnityEvent OnHoopTriggered;
        public Collider bottomCollider;

        private bool _touchingTop;
        private bool _touchingMiddle;
        private bool _touchingBottom;
        
        [UsedImplicitly]
        public void OnTopTriggerTouch(bool isTouching)
        {
            Debug.Log($"Top: {isTouching}");
            if (isTouching)
            {
                Debug.Log($"Top: Activate!");
                this.bottomCollider.gameObject.layer = LayerMask.NameToLayer("Attacker");
                _touchingTop = true;
            }
            else
            {
                OnTopTriggerTouchEnd();
            }
        }

        void OnTopTriggerTouchEnd()
        {
            if (!_touchingMiddle)
            {
                Debug.Log($"Top: Failed!");
                this.bottomCollider.gameObject.layer = LayerMask.NameToLayer("DefenderAndAttacker");
                _touchingTop = false;
            }
        }

        [UsedImplicitly]
        public void OnMiddleTriggerTouch(bool isTouching)
        {
            Debug.Log($"Middle: {isTouching}");
            if (isTouching)
            {
                if (!_touchingTop) return;
                Debug.Log($"Middle: Activate!");
                _touchingMiddle = true;
            }
            else
            {
                OnMiddleTriggerTouchEnd();
            }
        }

        void OnMiddleTriggerTouchEnd()
        {
            if (!_touchingBottom)
            {
                Debug.Log($"Middle: Failed!");
                _touchingMiddle = false;
                _touchingTop = false;this.bottomCollider.gameObject.layer = LayerMask.NameToLayer("DefenderAndAttacker");
            }
        }

        [UsedImplicitly]
        public void OnBottomTriggerTouch(bool isTouching)
        {
            Debug.Log($"Bottom: {isTouching}");
            if (isTouching)
            {
                if (!_touchingMiddle) return;
                Debug.Log($"Bottom: Activate!");
                _touchingBottom = true;
            }
            else
            {
                OnBottomTriggerTouchEnd();
            }
        }

        public void OnBottomTriggerTouchEnd()
        {
            if (!_touchingMiddle)
            {
                Debug.Log($"Bottom: Failed!");
                this.bottomCollider.gameObject.layer = LayerMask.NameToLayer("DefenderAndAttacker");
            }
            else
            {
                Debug.Log("Full Hoop!");
                OnHoopTriggered?.Invoke();
            }
            _touchingBottom = false;
            _touchingMiddle = false;
            _touchingTop = false;
            
        }

        public void Reset()
        {
            _touchingBottom = false;
            _touchingMiddle = false;
            _touchingTop = false;
            this.bottomCollider.gameObject.layer = LayerMask.NameToLayer("DefenderAndAttacker");
        }
    }
}
