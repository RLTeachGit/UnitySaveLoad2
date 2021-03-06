﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PickupSerialise : Serialise { //Uses base class as template
    static readonly string mPrefabName = "Prefabs/Pickup"; //Name which can be used to Instantiate this prefab
    public override string PrefabName {
        get {
            return mPrefabName;    //Need to do it this way to allow us to use a static variable for the Prefab name
        }
    }
    public override void Load(FileStream vFS) {
        transform.position = (Vector3)SaveGame.BF.Deserialize(vFS); //Load Basic information
        transform.rotation = (Quaternion)SaveGame.BF.Deserialize(vFS);
        GetComponent<Pickup>().mTimeToLive = (float)SaveGame.BF.Deserialize(vFS); //Also has a time to live
        GetComponent<Pickup>().StartColour = (Color)SaveGame.BF.Deserialize(vFS);
        GetComponent<Rigidbody>().velocity = (Vector3)SaveGame.BF.Deserialize(vFS);
        GetComponent<Rigidbody>().angularVelocity = (Vector3)SaveGame.BF.Deserialize(vFS);
        GetComponent<Pickup>().Score = (int)SaveGame.BF.Deserialize(vFS);
    }
    public override void Save(FileStream vFS) {
        SaveGame.BF.Serialize(vFS, transform.position); //Save Basic information
        SaveGame.BF.Serialize(vFS, transform.rotation);
        SaveGame.BF.Serialize(vFS, GetComponent<Pickup>().mTimeToLive); //Also time to live
        SaveGame.BF.Serialize(vFS, GetComponent<Pickup>().StartColour); //Load Colour
        SaveGame.BF.Serialize(vFS, GetComponent<Rigidbody>().velocity); 
        SaveGame.BF.Serialize(vFS, GetComponent<Rigidbody>().angularVelocity);
        SaveGame.BF.Serialize(vFS, GetComponent<Pickup>().Score);
    }
}
