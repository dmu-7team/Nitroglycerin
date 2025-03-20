using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MarsFPSKit
{
    public class Kit_MenuModalQuestion : MonoBehaviour
    {
        public GameObject modalRoot;

        public bool isOpen;

        public UnityEvent onConfirm = new UnityEvent();

        public UnityEvent onNotConfirm = new UnityEvent();

        public TextMeshProUGUI title;
        public TextMeshProUGUI content;

        public void Switch()
        {
            if (isOpen) Close();
            else Open();
        }

        public void Confirm()
        {
            Close();
            onConfirm.Invoke();
        }

        public void NotConfirm()
        {
            Close();
            onNotConfirm.Invoke();
        }

        public void Open()
        {
            modalRoot.SetActiveOptimized(true);
        }

        public void Close(bool immediate = false)
        {
            modalRoot.SetActiveOptimized(false);
        }
    }
}