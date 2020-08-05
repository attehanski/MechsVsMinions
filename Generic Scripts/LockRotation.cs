using UnityEngine;
using System.Collections;

/// <summary>
/// Used to lock the rotation of the Game Object it is attached to. Can be used to lock different axes or combinations thereof. Can use the starting values or given ones.
/// </summary>
public class LockRotation : MonoBehaviour {

    #region Public variables
    /// <summary>
    /// Value to determine if the object's rotation is locked on the X axis. True means it is locked, false means it is free to rotate on this axis.
    /// </summary>
    public bool lockX = false;
    /// <summary>
    /// Value to determine if the object's rotation is locked on the Y axis. True means it is locked, false means it is free to rotate on this axis.
    /// </summary>
    public bool lockY = false;
    /// <summary>
    /// Value to determine if the object's rotation is locked on the Z axis. True means it is locked, false means it is free to rotate on this axis.
    /// </summary>
    public bool lockZ = false;

    /// <summary>
    /// Used to determine if using the start rotation values of the object or the given rotation. True means it uses start rotation and overrides the given values, false means the given values are used.
    /// </summary>
    public bool useStartRotation = false;

    /// <summary>
    /// The rotation on X axis where the object is locked to, if useStartRotation is false. Uses Euler angles.
    /// </summary>
    public float rotationX;
    /// <summary>
    /// The rotation on Y axis where the object is locked to, if useStartRotation is false. Uses Euler angles.
    /// </summary>
    public float rotationY;
    /// <summary>
    /// The rotation on Z axis where the object is locked to, if useStartRotation is false. Uses Euler angles.
    /// </summary>
    public float rotationZ;
    #endregion

    #region Private variables
    /// <summary>
    /// The transform of the GameObject this script is attached to.
    /// </summary>
    protected Transform tf;
    /// <summary>
    /// The X axis rotation used in calculating the updated rotation of the GameObject.
    /// </summary>
    protected float currentRotationX;
    /// <summary>
    /// The Y axis rotation used in calculating the updated rotation of the GameObject.
    /// </summary>
    protected float currentRotationY;
    /// <summary>
    /// The Z axis rotation used in calculating the updated rotation of the GameObject.
    /// </summary>
    protected float currentRotationZ;
    #endregion

    /// <summary>
    /// Initialises used values to the start rotation if useStartRotation is true.
    /// </summary>
    protected virtual void Start () {
        tf = gameObject.transform;
        if (useStartRotation)
        {
            rotationX = tf.eulerAngles.x;
            rotationY = tf.eulerAngles.y;
            rotationZ = tf.eulerAngles.z;
        }
    }
	
    /// <summary>
    /// Sets the object's rotation in each locked axis to the locked value.
    /// </summary>
	protected virtual void LateUpdate () {
        
        if (lockX) currentRotationX = rotationX;
        else currentRotationX = tf.eulerAngles.x;

        if (lockY) currentRotationY = rotationY;
        else currentRotationY = tf.eulerAngles.y;

        if (lockZ) currentRotationZ = rotationZ;
        else currentRotationZ = tf.eulerAngles.z;
        
        tf.eulerAngles = new Vector3(currentRotationX, currentRotationY, currentRotationZ);
    }
}
