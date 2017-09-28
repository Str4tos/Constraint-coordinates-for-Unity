using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(Coord))]
public class CoodrsInspectorPropery : PropertyDrawer
{

    SerializedProperty min, max;
    string name;
    bool cache = false;
    bool isEdditMode = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!cache)
        {
            //get the name before it's gone
            name = property.displayName;

            //get the X and Y values
            property.Next(true);
            min = property.Copy();
            property.Next(true);
            max = property.Copy();

            cache = true;
        }

        float newMinimum;
        float newMaximum;

        Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));
        float onePart = contentPosition.width / 3;

        //if (isEdditMode)
        {
            //Vector3 minPoint = ((Component)property.serializedObject.targetObject).transform.position;
            //minPoint.x = min.floatValue;
            //Handles.Label(minPoint, "MINIMUM");
            //Debug.Log(minPoint);
            //Vector3 test = Handles.PositionHandle(minPoint, Quaternion.identity);
            //newMinimum = Handles.PositionHandle(minPoint, Quaternion.identity).x;

            //Vector3 maxPoint = ((Component)property.serializedObject.targetObject).transform.position;
            //maxPoint.x = max.floatValue;
            //Handles.Label(maxPoint, "MAXIMUM");

            //newMaximum = Handles.PositionHandle(maxPoint, Quaternion.identity).x;
        }
        //else
        {

            //Check if there is enough space to put the name on the same line (to save space)
            if (position.height > 16f)
            {
                position.height = 16f;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += 18f;
            }

            GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

            //show the X and Y from the point
            EditorGUIUtility.labelWidth = 27f;
            contentPosition.width = onePart;
            EditorGUI.indentLevel = 0;

            // Begin/end property & change check make each field
            // behave correctly when multi-object editing.
            if (isEdditMode)
            {
                EditorGUI.LabelField(contentPosition, "Min: " + min.floatValue);
            }
            else
            {
                EditorGUI.BeginProperty(contentPosition, label, min);
                {
                    EditorGUI.BeginChangeCheck();
                    newMinimum = EditorGUI.FloatField(contentPosition, "Min", min.floatValue);
                    if (EditorGUI.EndChangeCheck())
                        min.floatValue = newMinimum;
                }
                EditorGUI.EndProperty();
            }
            contentPosition.x += onePart;
            if (isEdditMode)
            {
                EditorGUI.LabelField(contentPosition, "Max: " + max.floatValue);
            }
            else
            {
                EditorGUI.BeginProperty(contentPosition, label, max);
                {
                    EditorGUI.BeginChangeCheck();
                    newMaximum = EditorGUI.FloatField(contentPosition, "Max", max.floatValue);
                    if (EditorGUI.EndChangeCheck())
                        max.floatValue = newMaximum;
                }
                EditorGUI.EndProperty();
            }
        }
        contentPosition.x += onePart;
        if (isEdditMode)
            GUI.color = Color.green;
        if (GUI.Button(contentPosition, "Eddit"))
        {
            isEdditMode = !isEdditMode;
            if (isEdditMode)
            {
                DraggablePointDrawer.Init(min, max);
            }
            else
            {
                DraggablePointDrawer.Stop();
            }
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }
}

