using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_Cube : MonoBehaviour
{

    // 设置射线检测的Layer
    public LayerMask raycastLayer;
    private Material mat;
    private Collider m_Collider=null;
    public GameObject cubePrefab;

    void Update()
    {
        // 获取屏幕中心的屏幕坐标
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 将屏幕坐标转换为世界坐标
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // 射线碰撞检测
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
        {
            if(m_Collider==null)
            {
                m_Collider=hit.collider;
            }
            // 如果射线碰到物体，打印物体名称
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<Renderer>().material.color=Color.black;
            if(m_Collider!=hit.collider)
            {
                m_Collider.gameObject.GetComponent<Renderer>().material.color=Color.white;
                m_Collider=hit.collider;
                Debug.Log("发现新碰撞体");
            }


            //
            // 获取命中的物体（假设是一个立方体）
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.GetComponent<CubeParent>().isMove);

            if(Input.GetKeyDown(KeyCode.F))
            {
                // 获取该物体的碰撞体（假设是立方体）
                if (hitObject.CompareTag("Cube"))
                {
                    // 获取碰撞的面法线
                    Vector3 hitNormal = hit.normal;

                    // 判断是哪一面
                    Vector3 cubePosition = hitObject.transform.position;

                    // 创建新的立方体
                    GameObject newCube = Instantiate(cubePrefab);

                    // 根据法线确定放置新的立方体的位置
                    Vector3 placePosition = cubePosition + hitNormal ; // 0.5f是新立方体的大小一半
                    newCube.transform.position = placePosition;

                    // 将新立方体放置在合适的面上
                    newCube.transform.rotation = Quaternion.LookRotation(hitNormal);
                    Debug.Log("生成新立方体");
                }
            }
            if(Input.GetKeyDown(KeyCode.R)&&hitObject.GetComponent<CubeParent>().isMove)
            {
                Destroy(hitObject);
            }


        }
        else
        {
            // 如果射线没有碰到任何物体
            Debug.Log("No hit");
        }
    }


}
