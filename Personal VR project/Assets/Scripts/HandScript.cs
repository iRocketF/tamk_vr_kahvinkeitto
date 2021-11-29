using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandScript : MonoBehaviour
{
    [Space]
    [SerializeField] private ActionBasedController controller;
    [SerializeField] float followSpeed = 30f;
    [SerializeField] float rotateSpeed = 100f;

    [Space]
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    
    [Space]
    private Transform followTarget;
    private Rigidbody rb;
    [SerializeField]private Transform palm;
    [SerializeField] float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] private LayerMask grabbableLayer;

    private bool isGrabbing;
    private GameObject heldObject;
    private Transform grabPoint;
    private FixedJoint joint1, joint2;
    // Start is called before the first frame update
    void Start()
    {
        followTarget = controller.gameObject.transform;
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.mass = 20f;
        rb.maxAngularVelocity = 20f;

        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Released;

        rb.position = followTarget.position;
        rb.rotation = followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMove();
    }

    void PhysicsMove()
    {
        var positionWithOffset = followTarget.TransformPoint(positionOffset);

        float distance = Vector3.Distance(positionWithOffset, transform.position);
        rb.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        var rotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(rb.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        rb.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    private void Grab(InputAction.CallbackContext obj)
    {
        if (isGrabbing || heldObject)
            return;

        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);

        if (grabbableColliders.Length < 1)
            return;

        GameObject objectToGrab = grabbableColliders[0].transform.gameObject;

        Rigidbody objectBody = objectToGrab.GetComponent<Rigidbody>();

        if(objectBody != null)
        {
            heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectBody.GetComponentInParent<Rigidbody>();
            if(objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }

        StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
    }

    private void Released(InputAction.CallbackContext obj)
    {
        if (joint1 != null)
            Destroy(joint1);
        if (joint2 != null)
            Destroy(joint2);
        if (grabPoint != null)
            Destroy(grabPoint.gameObject);

        if(heldObject != null)
        {
            Rigidbody targetBody = heldObject.GetComponent<Rigidbody>();
            targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetBody.interpolation = RigidbodyInterpolation.None;
            heldObject.GetComponent<XRGrabInteractable>().enabled = true;

            heldObject = null;
        }

        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }

    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        isGrabbing = true;

        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = heldObject.transform;

        followTarget = grabPoint;

        while(grabPoint != null && Vector3.Distance(grabPoint.position, palm.position) > joinDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        joint1 = gameObject.AddComponent<FixedJoint>();
        joint1.connectedBody = targetBody;
        joint1.breakForce = float.PositiveInfinity;
        joint1.breakTorque = float.PositiveInfinity;

        joint1.connectedMassScale = 1;
        joint1.massScale = 1;
        joint1.enableCollision = false;
        joint1.enablePreprocessing = false;

        joint2 = heldObject.AddComponent<FixedJoint>();
        joint2.connectedBody = rb;
        joint2.breakForce = float.PositiveInfinity;
        joint2.breakTorque = float.PositiveInfinity;

        joint2.connectedMassScale = 1;
        joint2.massScale = 1;
        joint2.enableCollision = false;
        joint2.enablePreprocessing = false;

        heldObject.GetComponent<XRGrabInteractable>().enabled = false;

        followTarget = controller.gameObject.transform;

    }

}
