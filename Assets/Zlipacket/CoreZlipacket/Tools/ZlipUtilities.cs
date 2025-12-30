using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Zlipacket.CoreZlipacket.Tools
{
    public static class ZlipUtilities
    {
        public static bool CastMouseCickRaycast(Camera cam, Vector2 mousePosition, out RaycastHit raycastHit)
        {
            raycastHit = new RaycastHit();
            
            Vector3 sceneMousePositionNear = new Vector3(
                mousePosition.x,
                mousePosition.y,
                cam.nearClipPlane);
            Vector3 sceneMousePositionFar = new Vector3(
                mousePosition.x,
                mousePosition.y,
                cam.farClipPlane);
            
            Vector3 worldMousePositionNear = cam.ScreenToWorldPoint(sceneMousePositionNear);
            Vector3 worldMousePositionFar = cam.ScreenToWorldPoint(sceneMousePositionFar);

            //Debug.DrawRay(worldMousePositionNear, worldMousePositionFar - worldMousePositionNear, Color.green, 1f);
            if (Physics.Raycast(worldMousePositionNear, worldMousePositionFar - worldMousePositionNear, out RaycastHit hit, float.PositiveInfinity))
            {
                raycastHit = hit;
                return true;
            }
            
            return false;
        }
        
        public static bool ApproximatelyWithMargin(float a, float b, float margin)
        {
            return Mathf.Abs(a - b) < margin;
        }
        
        /// <summary>
        /// Remap Distance of a vector3 between 2 vectors, then return interpolation between 0-1.
        /// </summary>
        /// <param name="inputPos"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        /// <returns></returns>
        public static float RemapVector3Distance(Vector3 inputPos, Vector3 near, Vector3 far)
        {
            //No idea how dot product of these vectors work, but it works, so just leave it.
            return Vector3.Dot(inputPos - near, far - near) / Vector3.Dot(far - near, far - near);
        }
    }
}