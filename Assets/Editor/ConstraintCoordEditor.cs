/* Created by Str4tos. 2017 */

// If not using Z-Axis then turn off the #define line
//#define CoordsIn3D

//#define CopyVectorProperty

using UnityEngine;
using UnityEditor;

#if CopyVectorProperty
[CustomPropertyDrawer(typeof(CoordsFromVector))]
#else
[CustomPropertyDrawer(typeof(ConstraintCoord))]
#endif
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
    private SerializedProperty minInstP, maxInstP;
    private string displayName;
    private bool isCache = false;

    private static bool isLocalCoords;
    private static bool isShowOptions;
    private static Vector2 copyValues;

    #region Edit mode
    private static bool isEdditMode;
    private static SerializedProperty minEdditedP;
    private static SerializedProperty maxEdditedP;
    private static Transform targetTran;
    private static int currAxis;
    #endregion /Edit mode
    #endregion /Variables

    #region Edit coords Mode

    public static void UpdateEditMode(SceneView sceneView)
    {
        try
        {
            Vector3 minimumPoint = isLocalCoords ? targetTran.localPosition : targetTran.position;
            Vector3 maximumPoint = minimumPoint;
            ConstraintCoord result = new ConstraintCoord(minEdditedP.floatValue, maxEdditedP.floatValue);

            if (isLocalCoords)
                result.TransformToWorld(targetTran);

            minimumPoint[currAxis] = result.min;
            maximumPoint[currAxis] = result.max;
            Vector2 direction = Vector2.zero;
            direction[currAxis] = 1.0f;
            result.min = RoundValue(Handles.Slider(minimumPoint, -direction)[currAxis]);
            result.max = RoundValue(Handles.Slider(maximumPoint, direction)[currAxis]);

            if (currAxis == 1) // if YAxis
            {
                Handles.DrawLine(minimumPoint + Vector3.right, minimumPoint + Vector3.left);
                Handles.DrawLine(maximumPoint + Vector3.right, maximumPoint + Vector3.left);
            }
            else // Equal for XAxis and ZAxis
            {
                Handles.DrawLine(minimumPoint + Vector3.up, minimumPoint + Vector3.down);
                Handles.DrawLine(maximumPoint + Vector3.up, maximumPoint + Vector3.down);
            }

            Handles.color = Color.magenta;
            Handles.DrawLine(minimumPoint, maximumPoint);


            if (isLocalCoords)
                result.TransformToLocal(targetTran);

            minEdditedP.floatValue = result.min;
            maxEdditedP.floatValue = result.max;
            maxEdditedP.serializedObject.ApplyModifiedProperties();
        }
        catch
        {
            StopEdditMode();
        }
    }
    public static void StopEdditMode()
    {
        SceneView.onSceneGUIDelegate -= UpdateEditMode;
        isEdditMode = false;
        minEdditedP = null;
        maxEdditedP = null;
        targetTran = null;
        SceneView.RepaintAll();
    }

    private static float RoundValue(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }
    #endregion

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!isCache)
            CasheProperties(property);

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

        contentPosition.width = onePart;
        ShowCoordValue(contentPosition, minInstP, label);

        contentPosition.x += onePart;
        ShowCoordValue(contentPosition, maxInstP, label);
        if (EditorApplication.isPlaying)
        {
            return;
        }

        contentPosition.width -= 10.0f;
        contentPosition.x += onePart + 10.0f;

        if (isShowOptions != GUI.Toggle(contentPosition, isShowOptions, "Options", "Button"))
        {
            if (isEdditMode)
            {
                StopEdditMode();
            }
            isShowOptions = !isShowOptions;
        }

        if (isShowOptions)
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
        //get the name before it's gone
        displayName = property.displayName;

        property.Next(true);
        minInstP = property.Copy();
        property.Next(true);
        maxInstP = property.Copy();
        isCache = true;

        if (minInstP.floatValue == 0.0f && maxInstP.floatValue == 0.0f)
        {
#if CopyVectorProperty
            CoordsFromVector coordsFromVector = attribute as CoordsFromVector;
            if (coordsFromVector != null)
            {
                var searchProperty = property.serializedObject.FindProperty(coordsFromVector.originPropertyName);
                if (searchProperty.propertyType == SerializedPropertyType.Vector2)
                {
                    Vector2 searchVector = searchProperty.vector2Value;
                    min.floatValue = searchVector.x;
                    max.floatValue = searchVector.y;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
#else
            Vector3 worldPosition = ((Component)property.serializedObject.targetObject).transform.position;
            minInstP.floatValue = worldPosition.x;
            maxInstP.floatValue = worldPosition.y;
            property.serializedObject.ApplyModifiedProperties();
#endif
        }
    }
    private void ShowCoordValue(Rect contentPosition, SerializedProperty value, GUIContent label)
    {
        if (isEdditMode || EditorApplication.isPlaying)
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
        Rect optionButtons = EditorGUI.IndentedRect(position);
        optionButtons.y = contentPositionY + 18.0f;
        float onePart;
#if CoordsIn3D
        onePart = optionButtons.width / 6.0f;
#else
        onePart = optionButtons.width / 5.0f;
#endif
        optionButtons.width = onePart;

        if (!isEdditMode)
            if (GUI.Button(optionButtons, "Copy"))
            {
                copyValues = new Vector2(minInstP.floatValue, maxInstP.floatValue);
            }

        optionButtons.x += onePart;
        if (!isEdditMode && copyValues != Vector2.zero)
            if (GUI.Button(optionButtons, "Paste"))
            {
                minInstP.floatValue = copyValues.x;
                maxInstP.floatValue = copyValues.y;
            }

        optionButtons.x += onePart;
        if (isLocalCoords != GUI.Toggle(optionButtons, isLocalCoords, isLocalCoords ? "Local" : "World", "Button"))
        {
            isLocalCoords = !isLocalCoords;
        }

        int axisCount;
#if CoordsIn3D
            axisCount = 3;
#else
        axisCount = 2;
#endif
        for (int i = 0; i < axisCount; i++)
        {
            optionButtons.x += onePart;
            if (isEdditMode != GUI.Toggle(optionButtons, isEdditMode, ((Axis)i).ToString(), "Button"))
            {
                if (isEdditMode)
                {
                    StopEdditMode();
                }
                else
                {
                    isEdditMode = true;
                    minEdditedP = minInstP;
                    maxEdditedP = maxInstP;
                    targetTran = ((Component)property.serializedObject.targetObject).transform;
                    currAxis = i;

                    SceneView.onSceneGUIDelegate += UpdateEditMode;
                    SceneView.RepaintAll();
                }
            }
        }
    }
}

