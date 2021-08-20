using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByTeacher {

    public class TPSCamera : MonoBehaviour {
        public Transform mLookAtPoint;
        public float mFollowDistance;
        public float mMinFollowDistance;
        public float mMaxFollowDistance;

        private float mVerticalDegree;
        public float mVerticalLimitUp;
        public float mVerticalLimitDown;

        private Vector3 mHorizontalVector;
        public float mMouseRotateSensitivity = 1.0f;
        private Vector3 mCurrentVel = Vector3.zero;
        public LayerMask mCheckLayer;


        void Start() {
            Vector3 vDir = transform.position - mLookAtPoint.position;
            mHorizontalVector = vDir;
            mHorizontalVector.y = 0.0f;
            mHorizontalVector.Normalize();
        }

        private void LateUpdate() {
            float fMX = Input.GetAxis("Mouse X");
            float fMY = Input.GetAxis("Mouse Y");
            Debug.Log(fMX);
            mHorizontalVector = Quaternion.AngleAxis(fMX * mMouseRotateSensitivity, Vector3.up) * mHorizontalVector;
            Vector3 rotationAxis = Vector3.Cross(mHorizontalVector, Vector3.up);
            mVerticalDegree -= fMY * mMouseRotateSensitivity;
            if(mVerticalDegree < -mVerticalLimitUp) {
                mVerticalDegree = -mVerticalLimitUp;
            } else if(mVerticalDegree > mVerticalLimitDown) {
                mVerticalDegree = mVerticalLimitDown;
            }
            Vector3 vFinalDir = Quaternion.AngleAxis(mVerticalDegree, rotationAxis) * mHorizontalVector;
            vFinalDir.Normalize();
            Vector3 vFinalPosition = mLookAtPoint.position + vFinalDir * mFollowDistance;
            Vector3 vDir = mLookAtPoint.position - vFinalPosition;

            vDir.Normalize();
            RaycastHit rh;
            Ray r = new Ray(mLookAtPoint.position, -vDir);

            if(Physics.SphereCast(r, 0.1f, out rh, mFollowDistance, mCheckLayer)) {
                vFinalPosition = mLookAtPoint.position - vDir * (rh.distance - 0.1f);
            }

            //if(Physics.Linecast(mLookAtPoint.position, vFinalPosition, out rh, mCheckLayer))
            //{
            //    //if(rh.distance < 2.0f)
            //    //{

            //    //}
            //    Vector3 vHit = rh.point + vDir * 0.1f;
            //    vFinalPosition = vHit;
            //} 
            //transform.position = Vector3.Lerp(transform.position, vFinalPosition, 0.3f);
            // transform.position = Vector3.SmoothDamp(transform.position, vFinalPosition, ref mCurrentVel, 0.01f, 10.0f);
            transform.position = vFinalPosition;
            transform.forward = vDir;

        }
    }

}