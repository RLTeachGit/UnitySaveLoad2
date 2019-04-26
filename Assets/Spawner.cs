using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    GameObject PickupPrefab; //Set up in IDE

    float mBounds = 20.0f;

    Vector3 mTarget;

	// Use this for initialization
	void Start () {
        Debug.Assert(PickupPrefab != null, "No Prefab assigned");
        mTarget = NewTarget;
	}
	
	// Update is called once per frame
	void Update () {
        if((transform.position-mTarget).magnitude<0.1) {
            var tPickup = Instantiate(PickupPrefab, transform.position, Quaternion.identity).GetComponent<Pickup>();
            tPickup.mTimeToLive = Random.Range(15, 60);
            mTarget = NewTarget;
        } else {
            Vector3 tDirection = (mTarget - transform.position); //Direction to target
            float tDistance = tDirection.magnitude;
            if (tDistance>1.0f) {
                transform.position += tDirection/tDistance * Time.deltaTime * 20.0f; //Move to new location
            } else {
                transform.position += tDirection / tDistance * Time.deltaTime; //Move to new location
            }
        }
    }

    Vector3 NewTarget { //Make random target above plane
        get {
            Vector3 tPosition = transform.position;
            tPosition.x = Random.Range(-mBounds, mBounds);
            tPosition.z = transform.position.z;
            tPosition.z = Random.Range(-mBounds, mBounds); //New position
            return tPosition;
        }
    }
}
