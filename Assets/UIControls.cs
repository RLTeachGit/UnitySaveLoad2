using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour {


    public void DoClearButton() {
        SaveGame.DoClear();
    }

    public void DoSaveButton() {
        SaveGame.DoSave();
    }
    public void DoLoadButton() {
        SaveGame.DoLoad();
    }
}