# Constraint-coordinates-for-Unity
Structure constraint coordinates for unity on C#.<br>
Fields float min, max.<br>

## Functions
<b>bool IsZero()</b> - Return true if empty or min & max equals zero.<br>
<b>float Clamp(float)</b> - Return clamped value in range between min and max.<br>
<b>float GetRandom()</b> - Return random value in range between min and max.<br>
<b>bool IsOutOfLimits(float)</b> - Return true if value out of limit coordinates.<br>
<b>bool IsOutOfLimits(float, bool)</b> - Return true if value out of limit coordinates considering the direction.<br>
<b>public bool IsCloserMaxBound(float, float)</b> - Return true if value closer maximum coordinate. Second float - Offset of maximum.<br>
<b>public bool IsCloserMinBound(float, float)</b> - Return true if value closer minimum coordinate. Second float - Offset of minimum<br>
<b>void TransformToWorld(Transform)</b> - Transform minimum and maximum coordinates to world space from target.<br>
<b>void TransformToLocal(Transform)</b> - Transform minimum and maximum coordinates to local space from target.<br>

# Editor
Visual structure<br>
![coordsinspector](https://user-images.githubusercontent.com/22005013/31046816-9ee48e38-a608-11e7-9b68-b3a6a1e0bbb8.JPG)

## Editor functions
<b>Options</b> - Show/Hide options buttons.<br>
<b>Cop</b> - Copy the values to the buffer.<br>
<b>Past</b> - Paste values from the buffer.<br>
<b>World/Local</b> - Switch space coordinates (World/Local).<br>
<b>X,Y,Z -Axis</b> - Start/Stop edit coordinates mode by selected axis<br>

## Edit coordinates mode
In scene view witch enabled edit coordinates mode.<br>
![coordsvisualedit](https://user-images.githubusercontent.com/22005013/31046883-9797f6fa-a609-11e7-8212-71df669ac722.JPG)

## Copy Vector property
Optional method copy inspector value from Vector2.<br>
In scripts enable line "#define CopyVectorProperty"
<b>[CoordsFromVector(string vector2PropertyName)]</b><br>
On open inspector. If min & max equals zero then set values from vector.
 