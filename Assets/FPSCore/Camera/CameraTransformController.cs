using UnityEngine;

namespace FPSCore.Camera
{
    /// <summary>
    /// Simple camera follower. Attach to the main camera.
    /// Set 'anchor' to the transform the camera should follow (e.g. CameraAnchor inside PlayerView).
    /// Since PlayerView already interpolates, camera just copies the anchor transform in LateUpdate.
    /// </summary>
    public class CameraTransformController : MonoBehaviour
    {
        [Tooltip("The transform to follow. If null, will try to find CameraAnchorAuthoring.")]
        [SerializeField] private Transform _anchor;

        private void LateUpdate()
        {
            if (_anchor == null)
            {
                // Try to find anchor by component
                var anchorAuth = FindAnyObjectByType<CameraAnchorAuthoring>();
                if (anchorAuth != null)
                    _anchor = anchorAuth.transform;
            }

            if (_anchor == null)
                return;

            // PlayerView already interpolates, so just copy position/rotation
            transform.position = _anchor.position;
            transform.rotation = _anchor.rotation;
        }
    }
}
