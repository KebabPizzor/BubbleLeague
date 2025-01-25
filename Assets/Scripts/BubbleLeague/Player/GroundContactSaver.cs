using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace BubbleLeague.Player
{
    public class GroundContactSaver : MonoBehaviour
    {
        [SerializeField] private LayerMask m_groundLayer;
        public Vector3 LatestGroundContact;
        
        private void OnCollisionStay(Collision other)
        {
            if (IsInLayerMask(other.gameObject, m_groundLayer))
            {
                LatestGroundContact = other.contacts.First().point;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsInLayerMask(other.gameObject, m_groundLayer))
            {
                LatestGroundContact = other.contacts.First().point;
            }
        }
        
        private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }
    }
}