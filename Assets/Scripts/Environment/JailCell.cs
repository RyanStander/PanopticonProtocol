using UnityEngine;

namespace Environment
{
    public class JailCell : MonoBehaviour
    {
        [SerializeField] private GameObject cellDoorCloseButton;
        [SerializeField] private GameObject cellDoorOpenButton;
        [SerializeField] private GameObject cellDoor;
        [SerializeField] private GameObject cellSeal;
        
        public bool IsSealed => cellSeal != null && cellSeal.activeSelf;
        
        public void OpenSeal()
        {
            if (cellSeal != null)
            {
                cellSeal.SetActive(false);
            }
        }
        
        public void CloseSeal()
        {
            if (cellSeal != null)
            {
                cellSeal.SetActive(true);
            }
        }
    }
}
