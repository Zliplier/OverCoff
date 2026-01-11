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
        /// Remap Distance of a vector3 between 2 vectors, then return interpolation/extrapolation.
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
        
        /// <summary>
        /// Returns a random unit vector within a cone defined by a direction and an angle.
        /// </summary>
        /// <param name="coneDirection">The central axis direction of the cone (normalized).</param>
        /// <param name="angleDegrees">The maximum angle from the central axis (e.g., 10f for a 20 degree total arc).</param>
        /// <returns>A random unit vector within the cone.</returns>
        public static Vector3 GetRandomDirectionInCone(Vector3 coneDirection, float angleDegrees)
        {
            // 1. Get a random rotation around the cone's forward axis (random spin)
            Quaternion randomSpin = Quaternion.AngleAxis(Random.Range(0f, 360f), coneDirection);

            // 2. Get a random tilt/angle from the forward axis
            // We use Random.Range(0f, angleDegrees) to get an angle within the cone's limit
            Quaternion randomTilt = Quaternion.AngleAxis(Random.Range(0f, angleDegrees), Vector3.Cross(coneDirection, Vector3.up));

            // Note: If coneDirection is Vector3.up, the Cross product is zero. A safer way to get a perpendicular axis:
            Vector3 axis = Vector3.Cross(coneDirection, Vector3.right);
            if (axis == Vector3.zero) axis = Vector3.Cross(coneDirection, Vector3.up); // Fallback for edge case

            randomTilt = Quaternion.AngleAxis(Random.Range(0f, angleDegrees), axis);


            // 3. Combine the rotations and apply to the original direction
            Vector3 result = (randomSpin * randomTilt) * coneDirection;

            // Ensure the result is normalized (should be by quaternion math, but good practice)
            return result.normalized;
        }
    }
}