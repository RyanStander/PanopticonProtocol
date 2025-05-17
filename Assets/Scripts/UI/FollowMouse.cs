using UnityEngine;

namespace UI
{
    public class FollowMouse : MonoBehaviour
    {
        //follow the mouse position, specifically for ui
        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Set this to the distance from the camera
            transform.position = mousePosition;
        }
    }
}
