using System;
using UnityEngine;
namespace GameFramework
{
    public class CameraController : MonoBehaviour
    {
        #region Private Declarations

        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _rotationRange = Vector2.zero;
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private float _followSpeed = 1f;

        private Vector3 _followAngles;
        private Quaternion _originalRotation;
        private Vector3 _followVelocity;

        #endregion

        #region MonoBehaviour

        // Use this for initialization
        private void Start () {
            _originalRotation = transform.localRotation;
        }

        private void FixedUpdate () {
            if (_target == null) {
                _target = PlayerManager.Instance.GetPlayer(0).transform;
            }

            FollowTarget(Time.deltaTime);
        }

        #endregion


        #region Private Methods

        private void FollowTarget (float deltaTime) {
            // we make initial calculations from the original local rotation
            transform.localRotation = _originalRotation;

            // tackle rotation around Y first
            Vector3 localTarget = transform.InverseTransformPoint(_target.position);
            float yAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            yAngle = Mathf.Clamp(yAngle, -_rotationRange.y * 0.5f, _rotationRange.y * 0.5f);
            transform.localRotation = _originalRotation * Quaternion.Euler(0, yAngle, 0);

            // then recalculate new local target position for rotation around X
            localTarget = transform.InverseTransformPoint(_target.position);
            float xAngle = Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -_rotationRange.x * 0.5f, _rotationRange.x * 0.5f);
            Vector3 targetAngles = new Vector3(_followAngles.x + Mathf.DeltaAngle(_followAngles.x, xAngle),
                                                _followAngles.y + Mathf.DeltaAngle(_followAngles.y, yAngle));

            // smoothly interpolate the current angles to the target angles
            _followAngles = Vector3.SmoothDamp(_followAngles, targetAngles, ref _followVelocity, _followSpeed);

            // and update the gameobject itself
            transform.localRotation = _originalRotation * Quaternion.Euler(-_followAngles.x, _followAngles.y, 0);

            // Update the position
            Vector3 desiredPos = _target.position + (transform.localRotation * _offset);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, _followSpeed * deltaTime);
            transform.position = smoothedPos;
        }

        #endregion
    }

}
