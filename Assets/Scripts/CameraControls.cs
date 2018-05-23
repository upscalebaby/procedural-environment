using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    private bool leftDown;
	
	// Update is called once per frame
	void Update () {

        Vector3 input = new Vector3 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw ("Mouse Y"), Input.GetAxisRaw ("Mouse ScrollWheel"));

        if(Input.mousePosition.x > Screen.width / 6) {
            if (Input.GetMouseButtonDown (0))
                leftDown = true;
            if (Input.GetMouseButtonUp (0))
                leftDown = false;

            if(leftDown) {
                transform.RotateAround (new Vector3(0,0,0), Vector3.up, input.normalized.x * Time.deltaTime * 80);
                transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, input.normalized.y * Time.deltaTime * 80);

                transform.LookAt (new Vector3(0,0,0));

            }
        }
            
        transform.Translate (0, 0, input.z * Time.deltaTime * 10000);
            
	}
}
