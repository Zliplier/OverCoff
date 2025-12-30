using System;
using System.Collections;
using System.Diagnostics;
using Players.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemGrab : ItemScript
    {
        public GrabInteractor holder { get; private set; } = null;
        
        [Header("Config")]
        public bool grabPositionOverride;
        [DrawIf("grabPositionOverride")] public Vector3 overridePosition;
        public bool grabRotationOverride;
        [DrawIf("grabRotationOverride")] public Quaternion overrideRotation;
        public bool grabForceOverride;
        [DrawIf("grabForceOverride")] public float overrideGrabForce;
        
        public bool scrollDisable;
        public bool minScrollOverride;
        [DrawIf("minScrollOverride")] public float overrideMinScroll;
        public bool maxScrollOverride;
        [DrawIf("maxScrollOverride")] public float overrideMaxScroll;
        
        [Header("Event")]
        public UnityEvent onGrab, onDrop;
        
        private void OnDisable()
        {
            holder?.ResetGrab();
        }
        
        public void Grab(GrabInteractor holder)
        {
            if (holder != this.holder)
                this.holder?.ResetGrab();
            
            this.holder = holder;
            
            onGrab?.Invoke();
        }
        
        public void Drop()
        {
            Reset();
            
            onDrop?.Invoke();
        }
        
        public void Reset()
        {
            holder?.ResetGrab();
            holder = null;
        }
    }
}