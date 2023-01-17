using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 400f;
    public float scrollSpeed = 50f;
    public float speedModifierFactor = 70f;

    public float verticalClampMin = 10;
    public float verticalClampMax = 90;

    float movespeedModifier = 1f;

    bool mouse0Down = false;

    void Update() {
        //GET MOUSEDOWN
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            mouse0Down = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            mouse0Down = false;
        }

        //ROTATION
        if (mouse0Down) {
            float rotY = transform.parent.eulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            float rotX = transform.eulerAngles.x + Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

            rotX = Mathf.Clamp(rotX, verticalClampMin, verticalClampMax);

            transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, rotY, transform.parent.eulerAngles.z);
            transform.localEulerAngles = new Vector3(rotX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        //ZOOM
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            Vector3 localCamPos = transform.GetChild(0).transform.localPosition;
            localCamPos += new Vector3(0, 0, scroll*scrollSpeed*movespeedModifier);

            if (localCamPos.z < 0) {
                movespeedModifier = -localCamPos.z/speedModifierFactor;
                transform.GetChild(0).transform.localPosition = localCamPos;
            }
        }
    }

    void FixedUpdate()
    {
        //MOVEMENT
        if (Input.GetKey(KeyCode.W)) {
            transform.parent.position += transform.parent.forward*moveSpeed*movespeedModifier;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.parent.position -= transform.parent.forward*moveSpeed*movespeedModifier;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.parent.position += -transform.parent.right*moveSpeed*movespeedModifier;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.parent.position += transform.parent.right*moveSpeed*movespeedModifier;
        }
    }
}
