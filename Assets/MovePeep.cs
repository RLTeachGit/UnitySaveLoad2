using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePeep : MonoBehaviour , SaveLoad { //Uses interface to include SaveLoad templates

    [SerializeField]
    float RotationSpeed = 360.0f;

    [SerializeField]
    float MoveSpeed = 10.0f;

    [SerializeField]
    float JumpSpeed = 1;


    [SerializeField]
    float DownLenght = 2.0f;

    bool mIsJumping = false;

    Rigidbody mRB;  //Used for physics

	// Use this for initialization
	void Start () {
        mRB = GetComponent<Rigidbody>();
        Debug.Assert(mRB != null, "Rigidbody Missing"); //Debug

    }
	
	// Simple non-physics move
	void FixedUpdate () {
        float tForward = Input.GetAxis("Vertical");     //Forward movement
        float tRotate = Input.GetAxis("Horizontal");    //Rotation input
        mRB.rotation *=Quaternion.Euler(0, tRotate * RotationSpeed * Time.deltaTime, 0); //Rotate to input
        Vector3 tMovement = Vector3.zero;   //No velocity to start with
        if (isGrounded && !mIsJumping)  {  //If on ground move
            tMovement += transform.forward* MoveSpeed *Time.deltaTime * MoveSpeed * tForward;     //Unity vector pointing forward relative to this transform
            if(Input.GetButton("Jump")) {
                mRB.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
                mIsJumping = true;
            }
        } else {
            mIsJumping = false;   //Single jump
        }
        mRB.position+=tMovement;
    }

    public bool Save() {
        Debug.Log("Save()");
        return true;
    }

    public bool Load() {
        Debug.Log("Load()");
        return true;
    }

    bool    isGrounded {
        get {
            Ray tDownRay = new Ray(mRB.position, Vector3.down);         //Sense ground below using raycast
            if(Physics.Raycast(tDownRay, DownLenght)) {
                Debug.DrawRay(tDownRay.origin, tDownRay.direction * DownLenght, Color.cyan);
                return true;
            } else {
                Debug.DrawRay(tDownRay.origin, tDownRay.direction * DownLenght, Color.red);
                return false;
            }

        }
    }

}
