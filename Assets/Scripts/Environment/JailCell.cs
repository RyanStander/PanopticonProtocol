using System;
using Spine.Unity;
using UnityEngine;

namespace Environment
{
    public class JailCell : MonoBehaviour
    {
        [SerializeField] private GameObject cellDoorCloseButton;
        [SerializeField] private GameObject cellDoorOpenButton;
        [SerializeField] private SkeletonAnimation cellDoorSkeleton;
        [SerializeField] private GameObject cellSeal;

        public bool IsSealed => cellSeal != null && cellSeal.activeSelf;

        private void OnValidate()
        {
            if (cellDoorSkeleton == null)
                cellDoorSkeleton = GetComponentInChildren<SkeletonAnimation>();
        }

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

        public void PlayAnimation(string animationName, bool loop = false)
        {
            if (cellDoorSkeleton != null)
            {
                cellDoorSkeleton.AnimationState.SetAnimation(0, animationName, loop);
            }
        }
    }
}
