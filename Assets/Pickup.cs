using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    public float mTimeToLive = 0;

    Text mText;

    public Color StartColour {
        get {
            return GetComponent<MeshRenderer>().material.color;
        }
        set {
            GetComponent<MeshRenderer>().material.color = value;
        }

    }

    public int Score;

    private void Start() {
        mText = GetComponentInChildren<Text>();
    }
    // Update is called once per frame
    void Update () {
        mTimeToLive -= Time.deltaTime;
        mText.text = string.Format("{0:f1} s {1}", mTimeToLive,Score); //Show Timer
        if(mTimeToLive<0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision vCollision) { //On collsion give a small shove and set death time
        MovePeep tPeep = vCollision.gameObject.GetComponent<MovePeep>();
        if (tPeep != null) {
            Vector3 tDirection = (transform.position - tPeep.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(tDirection*10.0f, ForceMode.Impulse);
            tPeep.Score += Score;
            FindObjectOfType<UpdateScore>().ScoreText = string.Format("{0}", tPeep.Score);
            Destroy(gameObject,Random.Range(1.0f,5.0f));
        }
    }
}
