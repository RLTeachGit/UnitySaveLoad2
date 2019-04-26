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

    public override void Load(FileStream vFS, BinaryFormatter vFormatter) {
        transform.position = (Vector3)vFormatter.Deserialize(vFS); //Load Basic information
        transform.rotation = (Quaternion)vFormatter.Deserialize(vFS);
        GetComponent<Pickup>().mTimeToLive= (float)vFormatter.Deserialize(vFS); //Also has a time to live
    }

    public override void Save(FileStream vFS, BinaryFormatter vFormatter) {
        vFormatter.Serialize(vFS, transform.position); //Save Basic information
        vFormatter.Serialize(vFS, transform.rotation);
        vFormatter.Serialize(vFS, GetComponent<Pickup>().mTimeToLive); //Also time to live

    }
}
