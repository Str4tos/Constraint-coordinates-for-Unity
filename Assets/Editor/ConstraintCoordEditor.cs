#define CoordsIn3D

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConstraintCoord))]
public class ConstraintCoordEditor : PropertyDrawer
{
    public enum Axis
    {
        XAxis,
        YAxis,
#if CoordsIn3D
        ZAxis
#endif
    }

    #region Variables
    private SerializedProperty min, max;
    private string displayName;
    private bool isCache = false;

    private static bool isShowOptions;
    private static Vector2 copyValues;

    #region Edit mode
    private static bool isEdditMode;
    private static SerializedProperty minProperty;
    private static SerializedProperty maxProperty;
    private static Transform targetTran;
    private static Axis currAxis;
    #endregion /Edit mode
    #endregion /Variables

    #region Edit coords Mode
    public static void InitEditMode(SerializedProperty min, SerializedProperty max, Transform target, Axis direction)
    {
        isEdditMode = true;
        minProperty = min;
        maxProperty = max;
        targetTran = target;
        currAxis = direction;

        SceneView.onSceneGUIDelegate += UpdateEditMode;
        SceneView.RepaintAll();
    }
    public static void UpdateEditMode(SceneView sceneView)
    {
        try
        {
            Vector3 minimumPoint = targetTran.position;
            Vector3 maximumPoint = minimumPoint;

            switch (currAxis)
            {
                default:
                    minimumPoint.x = minProperty.floatValue;
                    maximumPoint.x = maxProperty.floatValue;

                    minProperty.floatValue = RoundValue(Handles.Slider(minimumPoint, Vector3.left).x);
                    maxProperty.floatValue = RoundValue(Handles.Slider(maximumPoint, Vector3.right).x);
                    break;
                case Axis.YAxis:
                    minProperty.floatValue = minProperty.floatValue;
                    maximumPoint.y = maxProperty.floatValue;

                    minProperty.floatValue = RoundValue(Handles.Slider(minimumPoint, Vector3.down).y);
                    maxProperty.floatValue = RoundValue(Handles.Slider(maximumPoint, Vector3.up).y);
                    break;
#if CoordsIn3D
                case Axis.ZAxis:
                    minimumPoint.z = minProperty.floatValue;
                    maximumPoint.z = maxProperty.floatValue;

                    minProperty.floatValue = RoundValue(Handles.Slider(minimumPoint, Vector3.back).z);
                    maxProperty.floatValue = RoundValue(Handles.Slider(maximumPoint, Vector3.forward).z);
                    break;
#endif
            }
            Handles.color = Color.green;
            Handles.DrawLine(minimumPoint, maximumPoint);
            
            maxProperty.serializedObject.ApplyModifiedProperties();
        }
        catch
        {
            StopEdditMode();
        }
    }
    public static void StopEdditMode()
    {
        isEdditMode = false;
        minProperty = null;
        maxProperty = null;
        targetTran = null;

        SceneView.onSceneGUIDelegate -= UpdateEditMode;
        SceneView.RepaintAll();
    }

    private static float RoundValue(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }
    #endregion

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CasheProperties(property);

        GUI.color = Color.white;
        position.height = 16f;
        Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(displayName));

        EditorGUIUtility.labelWidth = 27.0f;

        if (Screen.width < 333)
        {
            EditorGUI.indentLevel++;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        EditorGUI.indentLevel = 0;
        float onePart = contentPosition.width / 3;
        GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

        contentPosition.width = onePart;
        ShowCoordValue(contentPosition, min, label);

        contentPosition.x += onePart;
        ShowCoordValue(contentPosition, max, label);

        contentPosition.width -= 10.0f;
        contentPosition.x += onePart + 10.0f;
        if (isShowOptions)
        {
            GUI.color = Color.green;
        }
        if (GUI.Button(contentPosition, "Options"))
        {
            isShowOptions = !isShowOptions;
            if (isEdditMode)
            {
                isEdditMode = false;
                StopEdditMode();
            }
        }

        ShowOptions(position, property, contentPosition.y);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float result = 16.0f;
        if (Screen.width < 333)
        {
            result += 18f;
        }
        if (isShowOptions)
            result += 18f;
        return result;
    }

    private void CasheProperties(SerializedProperty property)
    {
        if (!isCache)
        {
            //get the name before it's gone
            displayName = property.displayName;

            //get the X and Y values
            property.Next(true);
            min = property.Copy();
            property.Next(true);
            max = property.Copy();

            isCache = true;
        }
    }
    private void ShowCoordValue(Rect contentPosition, SerializedProperty value, GUIContent label)
    {
        if (isEdditMode)
        {
            EditorGUI.LabelField(contentPosition, value.name + ": " + value.floatValue);
        }
        else
        {
            EditorGUI.BeginProperty(contentPosition, label, value);
            {
                EditorGUI.BeginChangeCheck();
                float tempValue = EditorGUI.FloatField(contentPosition, value.name, value.floatValue);
                if (EditorGUI.EndChangeCheck())
                    value.floatValue = tempValue;
            }
            EditorGUI.EndProperty();
        }
    }
    private void ShowOptions(Rect position, SerializedProperty property, float contentPositionY)
    {
        if (isShowOptions)
        {
            Rect optionButtons = EditorGUI.IndentedRect(position);
            optionButtons.y = contentPositionY + 18.0f;
            GUI.color = Color.white;
            float onePart =
#if CoordsIn3D
                optionButtons.width / 5.0f;
#else
                optionButtons.width / 4.0f;
#endif
            optionButtons.width = onePart;

            if (!isEdditMode)
                if (GUI.Button(optionButtons, "Cop"))
                {
                    copyValues = new Vector2(min.floatValue, max.floatValue);
                }

            optionButtons.x += onePart;

            if (!isEdditMode && copyValues != Vector2.zero)
                if (GUI.Button(optionButtons, "Past"))
                {
                    min.floatValue = copyValues.x;
                    max.floatValue = copyValues.y;
                }

            if (isEdditMode)
                GUI.color = Color.green;

            optionButtons.x += onePart;
            if (GUI.Button(optionButtons, "XAxis"))
            {
                ToggleEdditMode(property, Axis.XAxis);
            }
            optionButtons.x += onePart;
            if (GUI.Button(optionButtons, "YAxis"))
            {
                ToggleEdditMode(property, Axis.YAxis);
            }
#if CoordsIn3D
            optionButtons.x += onePart;
            if (GUI.Button(optionButtons, "ZAxis"))
            {
                ToggleEdditMode(property, Axis.ZAxis);
            }
#endif
        }
    }
    private void ToggleEdditMode(SerializedProperty property, Axis currAxis)
    {
        if (isEdditMode)
            StopEdditMode();
        else
            InitEditMode(min, max, ((Component)property.serializedObject.targetObject).transform, currAxis);

    }

}

