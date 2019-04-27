using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


//Base class for saving & Loading
abstract public class Serialise : MonoBehaviour {

    public abstract string PrefabName { get; } //Used to get name of this prefab

    public abstract void Load(FileStream vFS); //Used to load the new values in
    public abstract void Save(FileStream vFS); //Used to save values out
}
