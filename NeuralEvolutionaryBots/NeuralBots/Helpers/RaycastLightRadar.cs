using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLightRadar : MonoBehaviour {
    public LayerMask layerMaskForRaycast;
    public int numberOfRaycast = 12;
    public int offsetAngle = 0;
    public int arcAngle = 180;
    public float maxDistance = 99f;
    public bool showRaycast;

    public double[] GetInputs () {
        var inputs = new double[numberOfRaycast];
        var currentRotation = transform.eulerAngles.z;

        for (var i = 0; i < numberOfRaycast; i++) {
            var angle = (float) offsetAngle + (arcAngle * i) / numberOfRaycast;
            angle += currentRotation;
            var direction = Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.right;
            direction = direction.normalized;

            var ray = Physics2D.Raycast (transform.position, direction, maxDistance, layerMaskForRaycast);
            var length = ray.collider != null ? ray.distance / maxDistance : 1f;

            inputs[i] = length;
            
            if(showRaycast)
                Debug.DrawRay (transform.position, direction * length * maxDistance, Color.green);
        }
        return inputs;
    }
}