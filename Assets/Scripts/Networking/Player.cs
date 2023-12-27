using System;
using UnityEngine;
using Coherence;
using Coherence.Toolkit;

namespace Networking
{
    public class Player : MonoBehaviour
    {
        //private CoherenceSync m_SyncComponent;
        public void Start()
        {
            //m_SyncComponent = GetComponent<CoherenceSync>();
        }

        public void Update()
        {
            //Nameof (function name), Who we are sending it too, "Parameter arguments"
            //Animator.SetTrigger("Wave"); Dont forget to set the local animator as well so there is no desync.
            //m_SyncComponent.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Wave");
        }
    }
}