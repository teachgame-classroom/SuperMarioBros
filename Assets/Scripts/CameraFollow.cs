using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowMode { SimpleFollowX, Mario }

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public FollowMode followMode;
    private float cameraOffsetX;
    private float cameraOffsetY;

    private float cameraHalfWidth;
    private float marioStartFollowDistance;

    private Vector3 lastTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        if(followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }

        cameraOffsetX = (transform.position - followTarget.position).x;
        cameraOffsetY = transform.position.y;

        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        marioStartFollowDistance = cameraHalfWidth * 0.33f;
        Debug.Log(cameraHalfWidth);
    }

    // Update is called once per frame
    void Update()
    {
        switch(followMode)
        {
            case FollowMode.SimpleFollowX:
                SimpleFollowX();
                break;
            case FollowMode.Mario:
                MarioFollow();
                break;
            default:
                break;
        }

        lastTargetPos = followTarget.position;
    }

    private void SimpleFollowX()
    {
        transform.position = Vector3.right * (followTarget.position.x + cameraOffsetX) + Vector3.up * cameraOffsetY + Vector3.forward * -10;
    }

    private void MarioFollow()
    {
        if (lastTargetPos.x > followTarget.position.x + 0.01f) return;

        // 摄像机到马里奥之间的X方向距离
        float targetDistanceX = (transform.position - followTarget.position).x;

        // 摄像机和马里奥之间的X距离已经小于开始跟随的距离
        if(targetDistanceX < marioStartFollowDistance)
        {
            // 归一化X距离：0 - 摄像机和马里奥的X坐标相同，1 - 马里奥刚进入跟随范围
            float nomalizedDistance = targetDistanceX / marioStartFollowDistance;

            float offsetX = Mathf.Lerp(0, cameraHalfWidth - marioStartFollowDistance, nomalizedDistance);

            float destinationX = followTarget.position.x + offsetX;

            Vector3 destination = Vector3.right * destinationX + Vector3.up * cameraOffsetY + Vector3.forward * -10;

            Debug.DrawLine(transform.position, destination, Color.yellow);

            //transform.position = destination;
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2.25f);
        }
        else
        {
            //Debug.Log("在左:" + targetDistanceX);
        }
    }
}
