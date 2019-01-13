using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneState : MonoBehaviour {
    public int state
    {
        get; private set;
    }

    public int planeId;

    private int[] rots = { -30, 0, 30 };
    private int dir = 1;
    private Quaternion targetRotation = Quaternion.identity;

    private void Awake()
    {
        targetRotation = transform.localRotation;
        state = 1;
    }

    void Update()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 270 * Time.deltaTime);
    }

    public bool OnInteract()
    {
        state += dir;
        if (state > 2 || state < 0)
        {
            dir *= -1;
            state = 1;
        }
        targetRotation = Quaternion.Euler(Vector3.up * rots[state]);
        Debug.Log("Changed plane " + planeId + " to " + state + " (" + (state == 0 ? "DEP" : state == 2 ? "ARR" : "CRZ") + ")");
        return false;
    }
}
