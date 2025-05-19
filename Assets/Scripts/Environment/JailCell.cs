using System;
using System.Collections;
using Monsters;
using Spine.Unity;
using UnityEngine;

namespace Environment
{
    public class JailCell : MonoBehaviour
    {
        [SerializeField] private GameObject cellDoorCloseButton;
        [SerializeField] private GameObject cellDoorCloseButtonLight;
        [SerializeField] private GameObject cellDoorOpenButton;
        [SerializeField] private GameObject cellDoorOpenButtonLight;
        [SerializeField] private SkeletonAnimation cellDoorSkeleton;
        [SerializeField] private float automaticUnsealTime = 5f;
        [SerializeField] private float empRecoveryTime = 10f;
        private Coroutine empRecoveryCoroutine;

        [Header("Animation Settings")] [SerializeField]
        private string breakAnimationName = "break";

        [SerializeField] private string breakBaiterAnimationName = "breakBaiter";
        [SerializeField] private string electricAnimationName = "electric";
        [SerializeField] private string idleAnimationName = "idle";
        [SerializeField] private string shakeAnimationName = "shake";
        [SerializeField] private string shakeBaiterAnimationName = "shakeBaiter";

        [Header("Seal Animations")] [SerializeField]
        Animator cellSealAnimator;

        [SerializeField] private string sealCloseAnimationName = "sealClose";
        [SerializeField] private string sealOpenAnimationName = "sealOpen";

        public bool IsSealed;
        private bool isEmpd;
        private Coroutine doorAutomaticUnsealCoroutine;
        private bool facilityNoPower;

        private void OnValidate()
        {
            if (cellDoorSkeleton == null)
                cellDoorSkeleton = GetComponentInChildren<SkeletonAnimation>();

            if (cellSealAnimator == null)
                cellSealAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            MonsterEvents.OnFaciliyNoPower += OnFacilityNoPower;
        }

        private void OnDisable()
        {
            MonsterEvents.OnFaciliyNoPower -= OnFacilityNoPower;
        }

        private void OnFacilityNoPower()
        {
            facilityNoPower = true;
            cellDoorCloseButtonLight.SetActive(false);
            cellDoorOpenButtonLight.SetActive(false);
            OpenSeal();
        }

        public void OpenSeal()
        {
            if (!IsSealed || facilityNoPower || isEmpd)
                return;

            IsSealed = false;
            cellSealAnimator.Play(sealOpenAnimationName);
            MonsterEvents.JailCellUnsealed(this);
            if (doorAutomaticUnsealCoroutine != null)
                StopCoroutine(doorAutomaticUnsealCoroutine);
        }

        public void CloseSeal()
        {
            if (IsSealed || facilityNoPower || isEmpd)
                return;

            IsSealed = true;
            cellSealAnimator.Play(sealCloseAnimationName);
            MonsterEvents.JailCellSealed(this);
            doorAutomaticUnsealCoroutine = StartCoroutine(AutomaticUnseal());
        }

        private IEnumerator AutomaticUnseal()
        {
            yield return new WaitForSeconds(automaticUnsealTime);
            OpenSeal();
        }

        public void EmpDoor()
        {
            if(IsSealed)
                OpenSeal();
            
            isEmpd = true;
            Electric();
            
            if(empRecoveryCoroutine != null)
                StopCoroutine(empRecoveryCoroutine);
            
            empRecoveryCoroutine = StartCoroutine(EmpRecovery());
            
            if (doorAutomaticUnsealCoroutine != null)
                StopCoroutine(doorAutomaticUnsealCoroutine);
        }

        private IEnumerator EmpRecovery()
        {
            yield return new WaitForSeconds(automaticUnsealTime);
            ClearElectric();
            isEmpd = false;
            Idle();
        }

        #region Animations

        public void BreakDoor()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(0, breakBaiterAnimationName, false);
        }

        public void BreakDoorBaiter()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(0, breakAnimationName, false);
        }

        private void Electric()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(1, electricAnimationName, true);
        }
        
        private void ClearElectric()
        {
            cellDoorSkeleton.AnimationState.SetEmptyAnimation(1, 0);
        }

        public void Idle()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(0, idleAnimationName, true);
        }

        public void Shake()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(0, shakeBaiterAnimationName, false);
        }

        public void ShakeBaiter()
        {
            cellDoorSkeleton.AnimationState.SetAnimation(0, shakeAnimationName, false);
        }

        #endregion
    }
}
