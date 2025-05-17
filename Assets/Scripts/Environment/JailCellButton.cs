using System;
using UnityEngine;

namespace Environment
{
    public class JailCellButton : MonoBehaviour
    {
        [SerializeField] private JailCell jailCell;
        [SerializeField] private bool isOpenButton;
        
        private void OnValidate()
        {
            if (jailCell == null)
                jailCell = GetComponentInParent<JailCell>();
        }

        private void OnMouseDown()
        {
            if (jailCell == null)
            {
                Debug.LogError("JailCell is not assigned or found in the parent.");
                return;
            }

            if (isOpenButton)
            {
                jailCell.OpenSeal();
            }
            else
            {
                jailCell.CloseSeal();
            }
        }
    }
}
