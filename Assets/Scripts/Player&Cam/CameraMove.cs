using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public enum CamViewMode
    {
        FREE, //自由视角
        TOP,  //俯视角
        LIMIT //固定视角
    }

    //相机物体
    private Transform camTrans;
    private Transform playerTrans; // 玩家对象的Transform

    /// <summary>
    /// 相机视角模式
    /// </summary>
    public CamViewMode viewMode = CamViewMode.FREE;

    [SerializeField]
    private Vector3 _resetTrans; //相机重置位置
    [SerializeField]
    private Vector3 _resetAngles; //相机重置角度

    [Header("键盘移动速度")]
    public float m_speed = 3f;
    [Header("鼠标中键移动速度")]
    public float m_mSpeed = 0.5f;
    [Header("旋转速度")]
    public float m_rSpeed = 5f;
    [Header("缩放速度")]
    public float m_sSpeed = 5f;
    [Header("最大缩放距离")]
    public float m_maxDistance = 10f;
    [Header("中键移动的缓动值")]
    public float moveSmoothing = 0.2f;

    private float m_deltX = 0f; //计算右键旋转
    private float m_deltY = 0f; //计算右键旋转

    void Start()
    {
        _resetTrans = transform.position;
        camTrans = GameObject.Find("Main Camera").GetComponent<Transform>();
        playerTrans = transform; // 玩家对象就是该脚本所在物体（通常是Player）
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 在UI上时不执行
        // if (EventSystem.current.IsPointerOverGameObject()) return;

        if (viewMode != CamViewMode.LIMIT)
        {
            CameraRotate(); // 摄像机旋转
            CameraScale();  // 缩放
        }

        // 不同视角
        CameraMode();
    }

    private void CameraMode()
    {
        switch (viewMode)
        {
            case CamViewMode.TOP:
                camTrans.localRotation = Quaternion.Euler(90, camTrans.localRotation.eulerAngles.y, camTrans.localRotation.eulerAngles.z);
                break;
            default:
                break;
        }
    }

    void CameraScale()
    {
        // 鼠标滚轮场景缩放;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float m_distance = Input.GetAxis("Mouse ScrollWheel") * m_sSpeed;
            Vector3 newPos = camTrans.localPosition + camTrans.forward * m_distance;
            Debug.Log(newPos.magnitude);
            if (newPos.magnitude >= m_maxDistance) return;
            camTrans.localPosition = newPos;
        }
    }

    void CameraRotate()
    {
        // 获取鼠标输入并调整旋转
        m_deltX += Input.GetAxis("Mouse X") * m_rSpeed; // 水平旋转（控制玩家对象）
        m_deltY -= Input.GetAxis("Mouse Y") * m_rSpeed; // 垂直旋转（控制摄像机）

        m_deltX = ClampAngle(m_deltX, -360, 360);
        m_deltY = ClampAngle(m_deltY, -90, 90);

        // 控制玩家水平旋转（绕Y轴）
        playerTrans.rotation = Quaternion.Euler(0, m_deltX, 0);

        // 控制摄像机的垂直旋转（绕X轴）
        camTrans.localRotation = Quaternion.Euler(m_deltY, 0, 0);
    }

    // 规划角度
    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}
