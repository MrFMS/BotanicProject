using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoundPixel;

public class CameraManager : MonoSingleton<CameraManager>
{

    public float Speed = 20f;
    public Camera mCamera;
    public Vector2 limit = new Vector2(40, 50);
    Vector3 pos;


    // Start is called before the first frame update
    void Start()
    {
        pos = mCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * Speed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, 20, 500);
        MoveCamera();
    }

    public void MoveForward()
    {
        pos.z += Speed * Time.deltaTime;
        MoveCamera();
        //mCamera.transform.Translate(mCamera.transform.forward * Speed * Time.deltaTime);
    }

    public void MoveBack()
    {
        pos.z -= Speed * Time.deltaTime;
        MoveCamera();
        //mCamera.transform.position += Vector3.back * Speed * Time.deltaTime;
    }

    public void MoveRight()
    {
        pos.x += Speed * Time.deltaTime;
        MoveCamera();
        // mCamera.transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        pos.x -= Speed * Time.deltaTime;
        MoveCamera();
        //mCamera.transform.Translate(Vector3.left * Speed * Time.deltaTime);
    }


    void MoveCamera()
    {
 
        mCamera.transform.position = pos;

    }
}
