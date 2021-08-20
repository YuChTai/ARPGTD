using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSccc : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform mLookAtPoint;


    public float mFollowDistance;
    public float mMixFollowDistance;
    public float mMinFollowDistance;
    


    
    private float mVerticalDegree;
    public float mVerticalLimitUp;
    public float mVerticalLimitDown;

    private Vector3 mHorizontalV;
    public float rotateSensitivity;
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
    private void LateUpdate()
    {
        float xMov = Input.GetAxis("Mouse X");
        float yMov = Input.GetAxis("Mouse Y");
        mHorizontalV = Quaternion.AngleAxis(xMov * rotateSensitivity, Vector3.up) * mHorizontalV;
        Vector3 rotationAxis = Vector3.Cross(mHorizontalV, Vector3.up);
        mVerticalDegree += -yMov;
        if (mVerticalDegree < -mVerticalLimitUp)
        {
            mVerticalDegree = -mVerticalLimitUp;
        }
        else if (mVerticalDegree > mVerticalLimitDown)
        {
            mVerticalDegree = mVerticalLimitDown;
        }
        Vector3 vFinalDir = Quaternion.AngleAxis(mVerticalDegree, rotationAxis) * mHorizontalV;
        vFinalDir.Normalize();
        Vector3 vFinalPos = mLookAtPoint.position + vFinalDir * mFollowDistance;
        transform.position = vFinalPos;



        Vector3 vDir = mLookAtPoint.position - transform.position;
        transform.forward = vDir;
    }
}
