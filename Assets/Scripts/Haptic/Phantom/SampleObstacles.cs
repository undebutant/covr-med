/// ----------------------------///
///     RICCA Aylen 2016        ///
///     aricca8@gmail.com       ///
///     Internship IBISC        ///
/// ----------------------------///

//---------------------------------------------------------------------------
// INCLUDES
//---------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

//---------------------------------------------------------------------------
// SAMPLE OBSTACLES
//---------------------------------------------------------------------------

/// <summary>
/// Beahviours for sample tissue objects such as a cyst (target)
/// </summary>
public class SampleObstacles : Obstacles
{
    //---------------------------------------------------------------------------
    // CONSTANTS
    //---------------------------------------------------------------------------

    /// <summary>
	/// Scale for Unity
	/// </summary>
	private const float Scale = 100f;

    /// <summary>
    /// Position of the center of the object [mm]
    /// </summary>
    public Vector3 Position = Vector3.zero;

    /// <summary>
    /// The radius of the sphere [mm]
    /// </summary>
    public float Radius = 0f;

    /// <summary>
    /// The spring constant of the force applied when pushed from the sample object surface [N/mm]
    /// </summary>
    private const float Stiffness = 1.0f;

    /// <summary>
    /// Damping factor of when you are in the object [Ns/mm]
    /// </summary>
    private const float Dumping = 0.0f;

    /// <summary>
    /// The maximum value of the generated force [N]
    /// </summary>
    private const float ForceLimit = 0.9f;

    //---------------------------------------------------------------------------
    // OBJECT ATTRIBUTES
    //---------------------------------------------------------------------------

    /// <summary>
    /// Stores the predefined object's material
    /// </summary>
    private Material OriginalMaterial = null;

    /// <summary>
    /// Stores the material to apply to the object each cycle
    /// </summary>
    private Material CurrentMaterial = null;

    /// <summary>
    /// User defined material force visual feedback of collision
    /// </summary>
    public Material OnCollisionMaterial = null;

    //---------------------------------------------------------------------------
    // FUNCTIONS
    //---------------------------------------------------------------------------

    /// <summary>
    /// Process at the start of the simulation
    /// </summary>
    private void Start()
    {
        // initialize position and radius
        InitializeShape();

        // Store the initial material configuration
        OriginalMaterial = transform.GetComponent<Renderer>().material;

        // Initialise the material to apply in each cycle
        CurrentMaterial = transform.GetComponent<Renderer>().material;

        // Set a red coloured material in case none was defined
        if (OnCollisionMaterial == null)
        {
            var newMaterial = new Material(OriginalMaterial);
            newMaterial.color = Color.red;
            OnCollisionMaterial = newMaterial;
        }
    }

    /// <summary>
    /// Process each frame
    /// </summary>
    private void Update()
    {
        if (this.transform.hasChanged)
        {
            InitializeShape();
        }

        // Update material
        transform.GetComponent<Renderer>().material = CurrentMaterial;
    }

    /// <summary>
    /// Initializes position and radius of the sphere
    /// </summary>
    private void InitializeShape()
    {
        Position = transform.position * Scale;
        Radius = transform.localScale.x * Scale;
        transform.hasChanged = false;
    }

    /// <summary>
    /// Seek the forces generated when the operating point is in contact with the sample object
    /// </summary>
    /// <param name="tipPosition">The position of the tip [mm]</param>
    /// <param name="tipVelocity">The speed of the tip [mm/s]</param>
    /// <returns>The force generated due to contact with the sample object</returns>
    public new Vector3 CalculateForce(Vector3 tipPosition, Vector3 tipVelocity)
    {
        Vector3 vec = tipPosition - this.Position;
        float distance = vec.magnitude;

        // No force in the outside and the middle of the sphere
        if (distance >= this.Radius || distance == 0)
        {
            return Vector3.zero;
        }

        vec /= distance;    // Normalization
        float f = Stiffness / (this.Radius - distance);
        if (f > ForceLimit) f = ForceLimit;

        return Vector3.zero;
        //return (f * vec) - Dumping * tipVelocity;
    }
}
