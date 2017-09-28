using UnityEngine;
using System.Collections;
using UnityEditor;



[CustomEditor(typeof(MonoBehaviour), true)]
public class DraggablePointDrawer : Editor
{
    readonly GUIStyle style = new GUIStyle();

    private static bool isEnabled;
    private static SerializedProperty min;
    private static SerializedProperty max;

    public static void Init(SerializedProperty min, SerializedProperty max)
    {
        DraggablePointDrawer.min = min;
        DraggablePointDrawer.max = max;
        isEnabled = true;
    }
    public static void Stop()
    {
        min = null;
        max = null;
        isEnabled = false;
        
    }

    void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        
        isEnabled = false;
    }

    private void OnDisable()
    {
        Stop();
    }
    public void OnSceneGUI()
    {
        if (!isEnabled)
            return;

        Vector3 targetPosition = ((Component)target).transform.position;
        Vector3 minPoint = targetPosition;
        minPoint.x = min.floatValue;
        Handles.Label(minPoint, "MIN");
        min.floatValue = Handles.PositionHandle(minPoint, Quaternion.identity).x;
        
        Vector3 maxPoint = targetPosition;
        maxPoint.x = max.floatValue;
        Handles.Label(maxPoint, "MAX");

        max.floatValue = Handles.PositionHandle(maxPoint, Quaternion.identity).x;
        serializedObject.ApplyModifiedProperties();
    }
}