using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Campfire))]
public class CampfireEditor : Editor {

    SerializedProperty spawnPointProp;
    Campfire campfire;

    void OnEnable()
    {
        campfire = (Campfire)target;
        spawnPointProp = serializedObject.FindProperty("spawnPoint"); //seraialized Object = terrainPiece (target)
    }

    void OnSceneGUI()
    {
        serializedObject.Update();

        //-------------draw Handle----------------------

        //transoform mountPoint from localspace to workdspace
        Vector3 worldMountPoint = campfire.transform.TransformPoint(spawnPointProp.vector3Value);


        //make the handle scale with zoom
        float sizeFactor = HandleUtility.GetHandleSize(worldMountPoint) * 0.25f;


        //Draw the Handle (rectabgle, 3D) 
        Handles.color = Color.magenta; //change folor for everything after this
        worldMountPoint = Handles.FreeMoveHandle(worldMountPoint, Quaternion.identity, sizeFactor * 0.5f, Vector3.zero, Handles.RectangleCap);

        //draw lines for Crisshair
        Handles.DrawLine(worldMountPoint - Vector3.up * sizeFactor, worldMountPoint + Vector3.up * sizeFactor);
        Handles.DrawLine(worldMountPoint - Vector3.right * sizeFactor, worldMountPoint + Vector3.right * sizeFactor);




        //convert back to localspace
        // mountPointProp.vector3Value = terrainPiece.transform.InverseTransformPoint(worldMountPoint);

        //for 2D, draw the gizmo on the same z koordinate
        Vector3 mountPointLocal = campfire.transform.InverseTransformPoint(worldMountPoint);
        mountPointLocal.z = 0;
        spawnPointProp.vector3Value = mountPointLocal;

        serializedObject.ApplyModifiedProperties(); //mark secene dirty after change

    }



}
