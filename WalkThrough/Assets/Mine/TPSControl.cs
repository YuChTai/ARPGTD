using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mine {

    public class TPSControl : MonoBehaviour {

        public Transform tpsCam;
        public float moveSpeed;

        private void Update() {
            float xSig = Input.GetAxis("Horizontal");
            float ySig = Input.GetAxis("Vertical");
   
            if(0 != xSig || 0 != ySig) {
                Vector3 tpsCamDirZ = tpsCam.forward;
                tpsCamDirZ.y = 0;
                Vector3 tpsCamDirX = tpsCam.right;

                Vector3 zMovm = tpsCamDirZ * ySig;
                Vector3 xMovm = tpsCamDirX * xSig;
                Vector3 lastMovPos = zMovm + xMovm;
                transform.forward = lastMovPos;
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

}