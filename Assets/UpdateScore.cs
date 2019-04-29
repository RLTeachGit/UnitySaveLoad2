using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour {

    public string ScoreText {
        get {
            return gameObject.GetComponent<Text>().text;
        }
        set {
            gameObject.GetComponent<Text>().text = value;
        }
    }

}
