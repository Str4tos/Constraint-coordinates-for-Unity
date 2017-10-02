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
![inspector](https://user-images.githubusercontent.com/22005013/31079391-24d03872-a78e-11e7-8dd1-e3a996bb004a.JPG)

## Editor functions
<b>Options</b> - Show/Hide options buttons.<br>
<b>Cop</b> - Copy the values to the buffer.<br>
<b>Past</b> - Paste values from the buffer.<br>
<b>World/Local</b> - Switch space coordinates (World/Local).<br>
<b>X,Y,Z -Axis</b> - Start/Stop edit coordinates mode by selected axis<br>

## Edit coordinates mode
In scene view witch enabled edit coordinates mode.<br>
![scene](https://user-images.githubusercontent.com/22005013/31079395-266a42ea-a78e-11e7-9485-cb1175f41459.JPG)

## Copy Vector property
Optional method copy inspector value from Vector2.<br>
In scripts enable line "#define CopyVectorProperty"<br>
Attribute: <b>[CoordsFromVector(string vector2PropertyName)]</b><br>
On open inspector. If min & max equals zero then set values from vector.
 
