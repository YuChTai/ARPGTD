using UnityEngine;

public class TPSCamera : MonoBehaviour {

    [Header("===== GameObject References ")]
    public Transform lookAt;

    [Header("===== Camera Properties Settings =====")]
    public float maxTargetDistance = 10f;
    public float minTargetDistance = 3f;
    public float targetDistance = 5f;
    public float maxYAngle = 45f;
    public float minYAngle = 20f;
    public float rotateSensitivity = 1f;
    public LayerMask isColliLayer;
    public float suitCoeff = 0.5f;

    [Header("====== Camera's Bounds Settings =====")]
    public CameraBoundary camBounds;

    [Header("===== Camera Damp Settings =====")]
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float maxSpeed = 20f;
    private Vector3 curVel;

    [Header("===== LayerMask's Material Alpha Settings=====")]
    private MeshRenderer meshRend;
    private Color curColor;
    private float oriColorAlpha;
    [SerializeField] [Range(0, 1f)] private float colorAlphaChange;

    private float curYAngle;
    private Vector3 horVecDir;
    private Vector3 horVec;

    private Vector3 camPos;
    private Vector3 xMaxCameraBound;
    private Vector3 xMinCameraBound;

    [System.Serializable]
    public struct CameraBoundary {
        public float xMaxBound;
    }

    public bool isDrawLineCast = false; //只用於「編輯場景」檢視。

    private void Start() {
        Vector3 subVec = lookAt.position-transform.position ;
        horVec = subVec;
        horVec.y = 0;
        horVec.Normalize();
        horVecDir = horVec.normalized;

        camPos = transform.position;
        camPos.x += camBounds.xMaxBound;
        xMaxCameraBound = camPos;
        camPos = transform.position;
        camPos.x -= camBounds.xMaxBound;
        xMinCameraBound = camPos;
        camPos = transform.position;
    }

    private void LateUpdate() {
        float xMov = Input.GetAxis("Mouse X");
        float yMov = Input.GetAxis("Mouse Y");

        horVec = Quaternion.AngleAxis(xMov * rotateSensitivity, Vector3.up) * horVec;

        horVecDir = horVec.normalized;

        Vector3 yRotAxis = Vector3.Cross(horVecDir, Vector3.up);
        curYAngle -= yMov * rotateSensitivity;

        if(curYAngle > maxYAngle) {
            curYAngle = maxYAngle;
        } else if(curYAngle < -minYAngle) {
            curYAngle = -minYAngle;
        }

        Vector3 lastVec = Quaternion.AngleAxis(curYAngle, yRotAxis) * horVecDir;

        Vector3 lastVecDir = lastVec.normalized;

        Vector3 lastVecPos = lookAt.position + lastVecDir * targetDistance;
        Vector3 subVec = lookAt.position - lastVecPos;
        Vector3 rayVecDir = -subVec;

        UseLineCastToDetectObstacle(lastVecPos, subVec);

        transform.position = Vector3.SmoothDamp(transform.position, lastVecPos, ref curVel, smoothTime, maxSpeed);

        transform.forward = subVec;
    }

    private void UseLineCastToDetectObstacle(Vector3 lastVecPos, Vector3 subVec) {
        RaycastHit rcHit;

        if(Physics.Linecast(lookAt.position, lastVecPos, out rcHit, isColliLayer)
            || Physics.Linecast(lookAt.position, xMaxCameraBound, out rcHit, isColliLayer)
            || Physics.Linecast(lookAt.position, xMinCameraBound, out rcHit, isColliLayer)
            ) {
            if(isDrawLineCast) {
                Debug.DrawLine(lookAt.position, xMaxCameraBound, Color.red, 1.0f);
                Debug.DrawLine(lookAt.position, xMinCameraBound, Color.red, 1.0f);
            }

            Vector3 suitCamPos = Vector3.zero;
            if(rcHit.distance < 2.0f && "Plane" != rcHit.transform.gameObject.name) {
                if(null == meshRend) {
                    meshRend = rcHit.transform.gameObject.GetComponent<MeshRenderer>();
                    curColor = meshRend.material.color;
                    oriColorAlpha = meshRend.material.color.a;
                }
                suitCamPos = rcHit.point + subVec.normalized * -2.5f; //TODO: 有想到更好的實作方式，再回來做調整。

                curColor.a = colorAlphaChange;
                meshRend.material.color = curColor;
            } else {
                if(null == meshRend) {
                    meshRend = rcHit.transform.gameObject.GetComponent<MeshRenderer>();
                    curColor = meshRend.material.color;
                    oriColorAlpha = meshRend.material.color.a;
                }
                suitCamPos = rcHit.point + subVec.normalized * suitCoeff;

                curColor.a = oriColorAlpha;
                meshRend.material.color = curColor;
            }   
            lastVecPos = suitCamPos;
        } else {
            if(null != meshRend) {
                curColor.a = oriColorAlpha;
                meshRend.material.color = curColor;
            }

        }

    }

    //private void UseSphereToDetectObstacle(Vector3 rayVecDir, Vector3 lastVecPos) {
    //    Ray ray = new Ray(lookAt.position, rayVecDir);
    //    RaycastHit rcHit;
    //    if(Physics.SphereCast(ray, 0.1f, out rcHit, targetDistance, isColliLayer)) {
    //        lastVecPos = lookAt.position + rayVecDir * (rcHit.distance - suitCoeff);
    //    }
    //}
}
