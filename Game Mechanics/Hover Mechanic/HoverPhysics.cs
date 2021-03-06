using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows for any object with ot have
/// hover like physcis. The force applied on the 
/// object with vary depending on the force applied
/// on the object and the distance between the object
/// and ground.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class HoverPhysics : MonoBehaviour {
    #region Hover Properites
    [Header("Rigidbody Properties")]
    [ReadOnly] [SerializeField] protected float totalMass = 0;
    [Tooltip("Speed of slowing down movement.")]
    [MinValue(0)] [SerializeField] protected float drag = 1;
    [Tooltip("Speed of slowing down rotational movement.")]
    [MinValue(0)] [SerializeField] protected float angularDrag = 1;
    [Space(10)]
    [Header("Hover Physcis Properties")]
    [Tooltip("Locations of where the hover force is applied.")]
    [SerializeField] protected Transform[] hoverPoints;
    [Tooltip("Strength of the downward force.")]
    [AbsoluteValue] [SerializeField] protected float hoverForce = 200;
    [Tooltip("Distance between the ground and the object.")]
    [AbsoluteValue] [SerializeField] protected float hoverDistance = 0;

    [ExecuteInEditMode]
    private void OnValidate() {
        Rigidbody HoverBody = GetComponent<Rigidbody>();

        HoverBody.drag = drag;
        HoverBody.angularDrag = angularDrag;

        totalMass = HoverBody.mass;
    }
    #endregion

    private void FixedUpdate() {
        RaycastHit hit;
        foreach (Transform HoverPoint in hoverPoints) {
            Vector3 DownwardForce;
            float ForcePercentage;

            if (Physics.Raycast (HoverPoint.position, HoverPoint.up * -1, out hit, hoverDistance)) {
                // Determine the current distance between the object and ground
                ForcePercentage = 1 - (hit.distance / hoverDistance);

                // Calculate the amount of force to apply
                DownwardForce = transform.up * hoverForce * ForcePercentage;

                // Include the force applied on the object via mass
                DownwardForce *= Time.deltaTime * totalMass;

                // Apply force
                Rigidbody HoverBody = GetComponent<Rigidbody>();
                HoverBody.AddForceAtPosition(DownwardForce, HoverPoint.position);
            }
        }
    }

    void OnCollisionEnter(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            totalMass += Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }

    private void OnCollisionExit(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            totalMass -= Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }
}