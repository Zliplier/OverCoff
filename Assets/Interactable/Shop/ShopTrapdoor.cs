using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

public class ShopTrapdoor : Singleton<ShopTrapdoor>
{
    public Transform spawnPoint;
    public HingeJoint leftDoor;
    public HingeJoint rightDoor;
    
    private Coroutine co_Closing = null;
    public bool isClosing => co_Closing != null;

    [Range(0, 180)] public float openAngle = 150f;
    public float closeDelay = 1f;

    private List<GameObject> passObjects = new List<GameObject>();

    [Button(nameof(Open))] public bool OpenButton;
    [Button(nameof(Close))] public bool CloseButton;
    
    private void Start()
    {
        
    }

    public void Open()
    {
        if (isClosing)
            StopCoroutine(co_Closing);
        
        leftDoor.gameObject.GetComponent<Collider>().enabled = false;
        JointSpring leftSpring = leftDoor.spring;
        leftSpring.targetPosition = -openAngle;
        leftDoor.spring = leftSpring;
        
        rightDoor.gameObject.GetComponent<Collider>().enabled = false;
        JointSpring rightSpring = rightDoor.spring;
        rightSpring.targetPosition = -openAngle;
        rightDoor.spring = rightSpring;
    }

    public void Close()
    {
        if (isClosing)
            StopCoroutine(co_Closing);

        StartCoroutine(Closing());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Item item))
            return;
        
        Open();
        
        if (!passObjects.Contains(other.gameObject))
            passObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Item item))
            return;
        
        if (passObjects.Contains(other.gameObject))
            passObjects.Remove(other.gameObject);
        
        if (passObjects.Count <= 0)
            Close();
    }

    private IEnumerator Closing()
    {
        yield return new WaitForSeconds(closeDelay);
        
        leftDoor.gameObject.GetComponent<Collider>().enabled = true;
        JointSpring leftSpring = leftDoor.spring;
        leftSpring.targetPosition = 0;
        leftDoor.spring = leftSpring;
        
        rightDoor.gameObject.GetComponent<Collider>().enabled = true;
        JointSpring rightSpring = rightDoor.spring;
        rightSpring.targetPosition = 0;
        rightDoor.spring = rightSpring;
    }
}
