using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 400f;
    public float scrollSpeed = 50f;
    public float speedModifierFactor = 70f;

    float movespeedModifier = 1f;

    [SerializeField] bool mouse0Down = false;

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
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

            transform.parent.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY);
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
