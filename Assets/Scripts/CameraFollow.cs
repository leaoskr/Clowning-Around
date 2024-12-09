using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{      
    public Transform VirtualCamera;
    public Transform p1;
    public Transform p2;
    public float offset = 10;
    private float startZ;
    private float NowZ;
    private float cameraZ;
    void Start()
    {   
        transform.position = VirtualCamera.position;
        transform.rotation = VirtualCamera.rotation;
        startZ = (p1.position.z + p2.position.z) / 2;
        cameraZ = VirtualCamera.position.z + offset;
    }
    // Update is called once per frame
    void Update()
    {  
        if (p1 != null && p2 != null)
        {
            NowZ = Mathf.Min(p1.position.z, p2.position.z);
            transform.position = new Vector3(VirtualCamera.position.x, VirtualCamera.position.y, cameraZ + (NowZ - startZ));
        }
    }
}
