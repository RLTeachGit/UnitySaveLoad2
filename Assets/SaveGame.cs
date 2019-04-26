using UnityEngine;
using System;       //Used for Time & Date
using System.Runtime.Serialization;

using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


// This class serializes a Vector3 object.
sealed class Vector3SerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Vector3
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Vector3 tVector3 = (Vector3) obj;
		info.AddValue("X", tVector3.x);
		info.AddValue("Y", tVector3.y);
		info.AddValue("Z", tVector3.z);
	}

	// Deserialize Vector3
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Vector3 tVector3 = (Vector3) obj;
		tVector3.x = (float)info.GetDouble("X");
		tVector3.y = (float)info.GetDouble("Y");
		tVector3.z = (float)info.GetDouble("Z");
		return tVector3;
	}
}

// This class serializes a Vector2 object.
sealed class Vector2SerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Vector2
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Vector2 tVector2 = (Vector2) obj;
		info.AddValue("X", tVector2.x);
		info.AddValue("Y", tVector2.y);
	}

	// Deserialize Vector2
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Vector2 tVector2 = (Vector2) obj;
		tVector2.x = (float)info.GetDouble("X");
		tVector2.y = (float)info.GetDouble("Y");
		return tVector2;
	}
}


// This class serializes a Color object.
sealed class ColourSerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Color
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Color tColor = (Color) obj;
		info.AddValue("R", tColor.r);
		info.AddValue("G", tColor.g);
		info.AddValue("B", tColor.b);
		info.AddValue("A", tColor.a);
	}

	// Deserialize Color
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Color tColor = (Color) obj;
		tColor.r = (float)info.GetDouble("R");
		tColor.g = (float)info.GetDouble("G");
		tColor.b = (float)info.GetDouble("B");
		tColor.a = (float)info.GetDouble("A");
		return tColor;
	}
}

sealed class QuaternionSerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Quaternion
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Quaternion tQuaternion = (Quaternion) obj;
		info.AddValue("X", tQuaternion.x);
		info.AddValue("Y", tQuaternion.y);
		info.AddValue("Z", tQuaternion.z);
		info.AddValue("W", tQuaternion.w);
	}

	// Deserialize Quaternion
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Quaternion tQuaternion = (Quaternion) obj;
		tQuaternion.x = (float)info.GetDouble("X");
		tQuaternion.y = (float)info.GetDouble("Y");
		tQuaternion.z = (float)info.GetDouble("Z");
		tQuaternion.w = (float)info.GetDouble("W");
		return tQuaternion;
	}
}

public class SaveGame : MonoBehaviour {

    //Serialisation is harder than is should be as Unity won't allow complex objects to be stored
    //To store game items the important information needs to be broken out and stored, the rest should be recreated
    //C# can store floats, strings etc. however not complex objects
    //A storage class is created and its this which is stored
    //C# store (serialise) generic lists List<>, so once the items are in a list the can all be saved
    //Loading (deserialisation) works the opposite way, the list of stored attributes is loaded into a list
    //From the copies of the prefab are made and using the loaded information, put in the right place on screen



    private static SurrogateSelector mSurrogateExtensions = null; //Variable with lazy initialisation
    public static SurrogateSelector SurrogateExtensions {
        get {
            if (mSurrogateExtensions == null) {
                mSurrogateExtensions = new SurrogateSelector(); //Initalise the first time only, then keep it in the static
                mSurrogateExtensions.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
                mSurrogateExtensions.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2SerializationSurrogate());
                mSurrogateExtensions.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());
                mSurrogateExtensions.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new ColourSerializationSurrogate());
            } //Will only create it first time
            return mSurrogateExtensions;
        }
    }


    static public void DoClear() {
        if (Camera.main != null) Camera.main.transform.SetParent(null);
        foreach (var tPeep in FindObjectsOfType<Serialise>()) { //Delete all seralisedable objects
            Destroy(tPeep.gameObject);  //Destroy old objects
        }
    }

    static  public void DoLoad() {
    string tFilename = "TestSave";

    DoClear();

    string tFullPath = Application.persistentDataPath + "/" + tFilename;
    FileStream tFS = null;
    if (File.Exists(tFullPath)) {   //Does file exist?
        try {       //This will try to run the code below, but if there is an error go straight to catch
            BinaryFormatter tBF = new BinaryFormatter();            //use C# Binary data, that way user cannot edit it easily
            tBF.SurrogateSelector = SaveGame.SurrogateExtensions;   //Add our additional SurrogateSelectors in so we can do Vectors etc.
            tFS = File.Open(tFullPath, FileMode.Open);       //Open File I/O
            int tItemCount = (int)tBF.Deserialize(tFS); //How many
            while (tItemCount-- > 0) {  //Make that number of items
                string tPrefabName = (string)tBF.Deserialize(tFS); //Which Prefab should we load
                var tPrefab = Resources.Load<GameObject>(tPrefabName); //Load prefab
                if (tPrefab == null) throw new Exception("No Prefab found");  //Throw error
                var tLoadObject = Instantiate(tPrefab).GetComponent<Serialise>(); //Make new prefab and get its Serialise Component
                tLoadObject.Load(tFS, tBF); //Set positions
            }

        } catch (Exception tE) {      //If an error happens above, comes here
            Debug.LogErrorFormat("Load Error:{0}", tE.Message);
        } finally {     //This will run at the end of the try, if it succeeded or failed
            if (tFS != null) {      //If we opened the file, close it again, this is in case we have an error above, we ensure file is closed
                tFS.Close();        //Close file
            }
        }
        } else {
            Debug.LogErrorFormat("File not found:", tFullPath);
        }

    }

    static public void DoSave() {
        string tFilename = "TestSave";

        string tFullPath = Application.persistentDataPath + "/" + tFilename;        //Get a safe place to store data from Unity
        FileStream tFS = null;          //If null file was not opened
        try {
            BinaryFormatter tBF = new BinaryFormatter();        //Store as binary
            tBF.SurrogateSelector = SaveGame.SurrogateExtensions;
            tFS = File.Create(tFullPath);       //Open File, if this fails it will throw
            Serialise[] tPeeps = FindObjectsOfType<Serialise>(); //Find all object marked as Serialiseable
            tBF.Serialize(tFS, tPeeps.Length);  //Item count
            foreach (var tSaveObject in tPeeps) {
                tBF.Serialize(tFS, tSaveObject.PrefabName);  //Name of this prefab
                tSaveObject.Save(tFS, tBF);
            }
        } catch (Exception tE) {        //Deal with error
            Debug.LogErrorFormat("Save Error:{0}", tE.Message);
        } finally {     //Make sure file is closed, if it was open
            if (tFS != null) {
                tFS.Close();        //Close file
            }
        }
    }
}

 