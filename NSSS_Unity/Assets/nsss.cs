using UnityEngine;

namespace Assets
{
    public class nsss : MonoBehaviour {
        
        // Use this for initialization
        public void Start () {
            Debug.Log("Displays connected: " + Display.displays.Length);
            foreach (var display in Display.displays) {
                display.Activate(display.systemWidth, display.systemHeight, 60);
            }
        }

        // Update is called once per frame
        public void Update () {
            if (Input.anyKey || !Input.GetAxis("Mouse X").Equals(0.0f) || !Input.GetAxis("Mouse Y").Equals(0.0f))
            {
                Application.Quit();
            }
            this.transform.Rotate(0, 90 * Time.deltaTime, 0);
        }
    }
}
