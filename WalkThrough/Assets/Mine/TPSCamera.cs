using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mine {

    public class TPSCamera : MonoBehaviour { 
        public Transform lookAt;
        public float maxTargetDistance;
        public float minTargetDistance;
        public float targetDistance;
        public float maxYAngle;
        public float minYAngle;
        public float rotSensitivity;
        public LayerMask isCollLayer;
        public float suitCoeff;


        private float curYAngle;
        private Vector3 horVecDir;
        private Vector3 horVec;
        private Vector3 curVel; //保留給 SmoothDamp() 計算算用。

        private void Start() {
            Vector3 subVec = transform.position - lookAt.position;
            Vector3 horVec = subVec;
            horVec.y = 0;
            horVecDir = horVec.normalized;
        }

        private void LateUpdate() {
            float xMov = Input.GetAxis("Mouse X");
            float yMov = Input.GetAxis("Mouse Y");
            horVec = Quaternion.AngleAxis(xMov * rotSensitivity, Vector3.up) * horVecDir; //對著 horVecDir，以 Vector3.up 為軸進行旋轉。
            horVecDir = horVec.normalized;
            Vector3 yRotAxis = Vector3.Cross(horVecDir, Vector3.up); //找出與 horVecDir 及 Vector3.up 相互垂直的第三軸。
            curYAngle -= yMov * rotSensitivity;

            if(curYAngle > maxYAngle) {
                curYAngle = maxYAngle;
            }else if(curYAngle < -minYAngle) {
                curYAngle = -minYAngle;
            }

            //if(curYAngle < -maxYAngle) {
            //    curYAngle = -maxYAngle;
            //} else if(curYAngle > minYAngle) {
            //    curYAngle = minYAngle;
            //}

            Vector3 lastVec = Quaternion.AngleAxis(curYAngle, yRotAxis) * horVecDir;
            Vector3 lastVecDir = lastVec.normalized;
            Vector3 lastVecPos = lookAt.position + lastVecDir * targetDistance; //根據 lookAt.position 的方向，反向延伸一個距離。
            Vector3 subVec = lookAt.position - lastVecPos;

            
            RaycastHit rcHit;
            //Ray ray = new Ray(lookAt.position, -subVec.normalized);
            //if(Physics.SphereCast(ray, 0.1f, out rcHit, targetDistance, isCollLayer)) {
            //    lastVecPos = lookAt.position + rayVecDir * (rcHit.distance - suitCoeff);
            //}
            float xBounds = lookAt.parent.GetComponent<CharacterController>().bounds.extents.x;
            float yBounds = lookAt.parent.GetComponent<CharacterController>().bounds.extents.y;

            if(Physics.Linecast(lookAt.position, lastVecPos, out rcHit, isCollLayer)
                || Physics.Linecast(lookAt.position + new Vector3(20.0f, 0, 0), lastVecPos, out rcHit, isCollLayer)
                || Physics.Linecast(lookAt.position + new Vector3(20.0f, 0, 0), lastVecPos, out rcHit, isCollLayer)
                ) {
                //if(rcHit.distance < 2.0f) {

                //}
                
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
                Vector3 suitCamPos = rcHit.point + subVec.normalized * suitCoeff;
                lastVecPos = suitCamPos;
            }
            
            transform.position = Vector3.SmoothDamp(transform.position, lastVecPos, ref curVel, 0.1f, 15.0f);
            //transform.position = Vector3.Lerp(transform.position, lastVecPos, 0.3f);
            //transform.position = lastVecPos;

            //Vector3 subVec = lookAt.position - transform.position;
            transform.forward = subVec;
        }

    }

}

