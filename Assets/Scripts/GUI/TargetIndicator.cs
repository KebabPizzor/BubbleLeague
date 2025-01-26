using UnityEngine;

namespace GUI
{
    public class TargetIndicator : MonoBehaviour
    {
        public Transform target; // The target to follow
        public RectTransform indicator; // The UI element used as the indicator
        public Camera cam; // The camera used for the split screen
        public RectTransform canvasRect; // The RectTransform of the canvas

        void Update()
        {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position);

            if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
            {
                // Target is on screen
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out localPoint);
                indicator.localPosition = localPoint;
                indicator.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.down); // Face the camera
            }
            else
            {
                // Target is off screen
                Vector3 cappedScreenPos = screenPos;

                if (screenPos.z < 0)
                {
                    cappedScreenPos *= -1;
                }

                Rect cameraRect = cam.pixelRect;
                cappedScreenPos.x = Mathf.Clamp(cappedScreenPos.x, Mathf.Lerp(cameraRect.xMin, cameraRect.xMax, 0.05f), Mathf.Lerp(cameraRect.xMin, cameraRect.xMax, 0.95f));
                cappedScreenPos.y = Mathf.Clamp(cappedScreenPos.y, Mathf.Lerp(cameraRect.yMin, cameraRect.yMax, 0.05f), Mathf.Lerp(cameraRect.yMin, cameraRect.yMax, 0.95f));

                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, cappedScreenPos, cam, out localPoint);
                indicator.localPosition = localPoint;
                
                // Calculate the angle to point the indicator in the correct direction
                Vector2 screenCenter = new Vector2(Mathf.Lerp(cameraRect.xMin, cameraRect.xMax, 0.5f), Mathf.Lerp(cameraRect.yMin, cameraRect.yMax, 0.5f));
                Vector2 direction = ((Vector2)cappedScreenPos - screenCenter).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                indicator.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}