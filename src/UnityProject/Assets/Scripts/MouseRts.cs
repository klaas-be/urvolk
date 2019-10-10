//TAKEN FROM http://www.andrejeworutzki.de/game-developement/unity-realtime-strategy-camera/ BUT HEAVILY MODIFIED!

using UnityEngine;

public class MouseRts : MonoBehaviour {
    public int LevelArea = 100;

    public int ScrollArea = 50;
    public int ScrollSpeed = 25;
    public int DragSpeed = 100;



    // Update is called once per frame
    void Update() {
        // Init camera translation for this frame.
        var translation = Vector3.zero;


        // Move camera with arrow keys
        translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


            // Move camera if mouse pointer reaches screen borders
            if (Input.mousePosition.x < ScrollArea) {
                translation += Vector3.right * -ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - ScrollArea) {
                translation += Vector3.right * ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < ScrollArea) {
                translation += Vector3.up * -ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - ScrollArea) {
                translation += Vector3.up * ScrollSpeed * Time.deltaTime;
            }
        

        // Keep camera within level and zoom area
        var desiredPosition = GetComponent<Camera>().transform.position + translation;
        if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x) {
            translation.x = 0;
        }
        if (desiredPosition.y < -LevelArea || LevelArea < desiredPosition.y) {
            translation.y = 0;
        }

        // Finally move camera parallel to world axis
        GetComponent<Camera>().transform.position += translation;
    }
}