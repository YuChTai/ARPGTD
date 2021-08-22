using UnityEngine;

public class PlayerController : CharacterBase {

    protected float xSig;
    protected float ySig;

    private Transform tpsCam;
    private Vector3 tpsCamDirZ;
    private Vector3 tpsCamDirX;
    
    protected Vector3 LastMovPos { get; private set; }

    protected override void Awake() {
        base.Awake();

        tpsCam = Camera.main.transform;
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        ySig = Input.GetAxis("Vertical");
        xSig = Input.GetAxis("Horizontal");

        FollowCameraZAxisAlways();

        CalculatePlayerMovement();
    }

    private void FollowCameraZAxisAlways() {
        tpsCamDirZ = tpsCam.forward;
        tpsCamDirZ.y = 0;
        transform.forward = tpsCamDirZ;
    }

    private void CalculatePlayerMovement() {
        tpsCamDirX = tpsCam.right;

        Vector3 zMovm = tpsCamDirZ * ySig;
        Vector3 xMovm = tpsCamDirX * xSig;

        LastMovPos = zMovm + xMovm;
        LastMovPos+=Physics.gravity;

        LastMovPos.Normalize();
    }

}
