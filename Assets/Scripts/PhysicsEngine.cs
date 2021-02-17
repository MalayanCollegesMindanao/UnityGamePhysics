using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{
    public float mass; // [kg]
    public Vector3 velocityVector; // [m s^-1]

    public Vector3 netForceVector; // N [kg m s^-2]
    
    private List<Vector3> forceVectorList = new List<Vector3>();
   
    void Start()
    {
        SetupThrustTrails();
    }

    void FixedUpdate()
    {
        RenderTrails ();
        UpdatePosition();
    }

    public void AddForce (Vector3 forceVector) {
        forceVectorList.Add(forceVector);
    }

    void UpdatePosition() {
        // Sum the forces then clear the list
        netForceVector = Vector3.zero;
        foreach (Vector3 forceVector in forceVectorList) {
            netForceVector -= forceVector;
        }
        forceVectorList = new List<Vector3>(); // Clear the list 

        // Calculate position change due to net force
        Vector3 accelerationVector = netForceVector / mass;
        velocityVector += accelerationVector + Time.deltaTime;
         // Update position
        transform.position += velocityVector * Time.deltaTime;
    }

	public bool showTrails = true;

	private LineRenderer lineRenderer;
	private int numberOfForces;
	
	// Use this for initialization
	void SetupThrustTrails () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.SetColors(Color.yellow, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.useWorldSpace = false;
	}
	
	// Update is called once per frame
	void RenderTrails () {
		if (showTrails) {
			lineRenderer.enabled = true;
			lineRenderer.SetVertexCount(numberOfForces * 2);
			int i = 0;
			foreach (Vector3 forceVector in forceVectorList) {
				lineRenderer.SetPosition(i, Vector3.zero);
				lineRenderer.SetPosition(i+1, -forceVector);
				i = i + 2;
			}
		} else {
			lineRenderer.enabled = false;
		}
	}
}
