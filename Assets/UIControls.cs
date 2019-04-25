using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour {

    public  void DoSave() {
        foreach(MovePeep tPeep in FindObjectsOfType<MovePeep>()) {
            tPeep.Save();
        }
    }

}
