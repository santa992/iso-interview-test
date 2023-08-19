using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsoTest
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject uiPanel;
        public void OnInteract()
        {
            uiPanel.SetActive(true);
            Debug.Log("Intereact");
        }
    }
}
