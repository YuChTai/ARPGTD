using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Work {

    public class PlayerNoWeapon : PlayerController {   
        public float moveSpeed;

        protected override void Update() {
            base.Update();

            if(Vector3.zero != LastMovPos) {
                charCtrler.Move(LastMovPos * Time.deltaTime * moveSpeed);
            }
            AnimateAnimation(ySig, xSig);
        }

        private void AnimateAnimation(float ySig, float xSig) {
            anim.SetFloat("walk_Forward", ySig);
            anim.SetFloat("walk_Left", xSig);
            anim.SetFloat("walk_Right", xSig);
            anim.SetFloat("walk_Backward", ySig);
        }

    }

}
