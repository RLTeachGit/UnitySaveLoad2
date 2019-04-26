using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveRootParent : MonoBehaviour {


    //Keep this object a fixed distance over parent
    private void LateUpdate() {
        Vector3 tParentPosition = transform.root.position;
        transform.position = tParentPosition + Vector3.up * 3;
        transform.rotation = Quaternion.identity;
    }
}
