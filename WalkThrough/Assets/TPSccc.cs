using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSccc : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform mLookAtPoint;


    public float mFollowDistance=3.0f;
    public float mMaxFollowDistance = 5.0f;
    public float mMinFollowDistance = 2.0f;

    





    private float mVerticalDegree;
    public float mVerticalLimitUp = 50.0f;
    public float mVerticalLimitDown = 50.0f;

    private Vector3 mHorizontalV;
    public float rotateSensitivity=6.0f;
    public LayerMask mCheckLayer;
    void Start()
    {
        Vector3 vDir = mLookAtPoint.position - transform.position;
        mHorizontalV = vDir;
        mHorizontalV.y = 0.0f;
        mHorizontalV.Normalize();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {   
        float xMov = Input.GetAxis("Mouse X");
        float yMov = Input.GetAxis("Mouse Y");
        mHorizontalV = Quaternion.AngleAxis(xMov * rotateSensitivity, Vector3.up) * mHorizontalV;
        Vector3 rotationAxis = Vector3.Cross(mHorizontalV, Vector3.up);
       

        mVerticalDegree -= yMov * rotateSensitivity;
        if (mVerticalDegree < -mVerticalLimitUp)
        {
            mVerticalDegree = -mVerticalLimitUp;
        }
        else if (mVerticalDegree > mVerticalLimitDown)
        {
            mVerticalDegree = mVerticalLimitDown;
        }
        Vector3 vFinalDir = Quaternion.AngleAxis(mVerticalDegree, rotationAxis) * mHorizontalV;//最後攝影機朝向的方向，正確的攝影機方位
        vFinalDir.Normalize();
        Vector3 vFinalPos = mLookAtPoint.position + vFinalDir * mFollowDistance;
        Vector3 vDir = mLookAtPoint.position - vFinalPos;
        
        vDir.Normalize();
        //HandleTpsCameraSphereCast(vDir, vFinalPos);
        RaycastHit rh;
        Ray r = new Ray(mLookAtPoint.position, -vDir);



        if (Physics.SphereCast(r, 0.1f, out rh, mFollowDistance, mCheckLayer))
        {
            Vector3 vHit;



            if (rh.distance < mMinFollowDistance)
            {
                Vector3 point = new Vector3(rh.point.x +rh.normal.x * 0.5f, transform.position.y+ transform.localPosition.y*0.21f, rh.point.z +rh.normal.z * 0.5f);
                float dd = Vector3.Distance(mLookAtPoint.position, point);
                vHit= mLookAtPoint.position - vDir * (dd - 0.1f);

            }else if(rh.distance>mMaxFollowDistance) 
            {
                
                vHit = mLookAtPoint.position - rh.point + vDir * 0.1f;

            }
            else 
            { vHit = mLookAtPoint.position - vDir * (rh.distance - 0.1f); }
            

            vFinalPos = vHit;
        }

        //if (vLimitD < mMinFollowDistance)
        //{
        //    Debug.Log("2222");
        //    //transform.position = vFinalPos.normalized * mMinFollowDistance+new Vector3(0,3.0f,0);
        //    mFollowDistance = mMinFollowDistance;

        //}
        transform.position = vFinalPos;

        

        transform.forward = vDir;












        //Vector3 vRayDir = -vDir;
        //RaycastHit rh;
        //if (Physics.Linecast(mLookAtPoint.position, vFinalPos, out rh, mCheckLayer)) 
        //{
        //    //if (rh.distance < 2.0f)//傷，慢慢移比較不傷
        //    //{

        //    //}
        //    Vector3 vHit = rh.point + vDir * 0.2f;//0.1可調
        //    vFinalPos = vHit;

        //};//兩端點直線碰撞的計算//基本版



    }
    //private void HandleTpsCameraSphereCast(Vector3 vDir, Vector3 vFinalPos)
    //{   

    //    RaycastHit rh;
    //    Ray r = new Ray(mLookAtPoint.position, -vDir);

        

    //    if (Physics.SphereCast(r, 0.1f, out rh, mFollowDistance, mCheckLayer))
    //    {
    //        vFinalPos = mLookAtPoint.position - vDir * (rh.distance - 0.1f);

    //    }
    //    float vLimitD = vFinalPos.magnitude;
    //    if (vLimitD < mMinFollowDistance)
    //    {
    //        Debug.Log("2222");
    //        transform.position = vFinalPos.normalized * mMinFollowDistance;
    //    }
        
    //    transform.position = vFinalPos;
        
    //    transform.forward = vDir;
    //}
}
