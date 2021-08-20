using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team_Seventh {

    public class TPSCamera : MonoBehaviour {

        public Transform lookAtPoint; 
        public float followDistance;
        public float minFollowDistance;
        public float maxFollowDistance;

        public float verticalLimitUp; //俯仰角所能呈現的最大幅度
        public float verticalLimitDown; //俯仰角所能呈現的最小幅度

        private float verticalDegree; //當前的俯仰角

        private Vector3 horizontalVectorDir; //水平向量
        public float mouseRotateSensitivity; //滑鼠轉動的靈敏度
        private Vector3 currentVel = Vector3.zero;
        public LayerMask checkLayer; //是否為指定的圖層

        private void Start() {
            Vector3 newPos = transform.position - lookAtPoint.position;
            horizontalVectorDir = newPos;
            horizontalVectorDir.y = 0.0f;
            horizontalVectorDir.Normalize();
        }

        private void LateUpdate() {
            float rotationY = Input.GetAxis("Mouse X");
            float rotationX = Input.GetAxis("Mosue Y");

            horizontalVectorDir = Quaternion.AngleAxis(rotationY * mouseRotateSensitivity, Vector3.up) * horizontalVectorDir;
            Vector3 rotationAxis = Vector3.Cross(horizontalVectorDir, Vector3.up);
            verticalDegree -= rotationY * mouseRotateSensitivity;
            if(verticalDegree < -verticalLimitDown) {
                verticalDegree = -verticalLimitDown;
            }else if(verticalDegree > verticalLimitUp) {
                verticalDegree = verticalLimitUp;
            }
            Vector3 finalVectorDir = Quaternion.AngleAxis(verticalDegree, rotationAxis) * horizontalVectorDir;
            finalVectorDir.Normalize();
            Vector3 finalPosition = lookAtPoint.position + finalVectorDir * followDistance;
            Vector3 newPos = lookAtPoint.position - finalVectorDir;

            Vector3 dir = newPos;
            dir.Normalize();
            RaycastHit raycastHit;
            Ray ray = new Ray(lookAtPoint.position, -newPos);
            if(Physics.SphereCast(ray, 0.1f, out raycastHit, followDistance, checkLayer)) {
                finalPosition = lookAtPoint.position - newPos * (raycastHit.distance - 0.1f);
            }
            transform.position = finalPosition;
            transform.forward = dir;
        }

    }

}
