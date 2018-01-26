/// ----------------------------///
///     RICCA Aylen 2016        ///
///     aricca8@gmail.com       ///
///     Internship IBISC        ///
/// ----------------------------///

//---------------------------------------------------------------------------
// INCLUDES
//---------------------------------------------------------------------------
using UnityEngine;
using ManagedPhantom;
using System;
using System.Collections.Generic;

//---------------------------------------------------------------------------
// HAPTIC MANAGER
//---------------------------------------------------------------------------

/// <summary>
/// Haptic Manager class
/// </summary>
public class HapticManager : MonoBehaviour {
    //---------------------------------------------------------------------------
    // CLASS INFORMATION
    //---------------------------------------------------------------------------

    /// <summary>
    /// Tag for logging information (Debug purpose only)
    /// </summary>
    private static string _tag = ".::. HapticManager .::. ";

    //---------------------------------------------------------------------------
    // HAPTIC INFORMATION
    //---------------------------------------------------------------------------

    /// <summary>
    /// PHANTOM instance
    /// </summary>
    private SimplePhantomUnity Phantom = null;

    /// <summary>
    /// The gimbal position [mm]
    /// </summary>
    public Vector3 HandPosition = Vector3.zero;

    /// <summary>
    /// The gimbal linear speed [mm/s]
    /// </summary>
    private Vector3 HandVelocity = Vector3.zero;

    /// <summary>
    /// The gimbal rotation
    /// </summary>
    public Quaternion HandRotation = Quaternion.identity;

    /// <summary>
    /// Rotation matrix of the needle
    /// </summary>
    private double[] RotationMatrix;

    /// <summary>
    /// Force feedback to apply to device
    /// </summary>
    public Vector3 Force = Vector3.zero;

    //---------------------------------------------------------------------------
    // SIMULATOR CONSTANTS
    //---------------------------------------------------------------------------

    /// <summary>
    /// Exert force upper limit [N]
    /// </summary>
    private const float MAX_FORCE = 3.0f;

    /// <summary>
    /// Exert force scale
    /// </summary>
    private const float DEVICE_FORCE_SCALE = 0.4f;

    /// <summary>
    /// Unit conversion from mm to Unity
    /// </summary>
    private const float UNIT_LENGTH = 0.01f;

    /// <summary>
    /// Low value for rotation angle elimination
    /// </summary>
    private const float ROTATION_LOW_LIMIT = 1.0f;

    /// <summary>
    /// For determining X, Z restriction movements inside skin layers
    /// </summary>
    private const float OUTSIDE_POSITION = -999;

    /// <summary>
    /// Dimensions (half) of the tissue [mm] to know if it is inside the lateral boundaries
    /// </summary>
    private Vector3 TISSUE_DIMENSIONS = new Vector3(80, 0, 60);

    /// <summary>
    /// Table position during experimentation
    /// </summary>
    /// <remarks>Modify RigidObstacles position to have correct force feedback</remarks>
    public const float GROUND_LEVEL = -0.25f;

    /// <summary>
    /// Top position of first layer (Unity units)
    /// </summary>
    private const float FIRST_LAYER_TOP = 0.25f;

    /// <summary>
    /// Top position of second layer (Unity units)
    /// </summary>
    private const float SECOND_LAYER_TOP = 0.0f;

    /// <summary>
    /// Simulator state: no haptic feedback at all
    /// </summary>
    private const int SIMULATION_OFF = 0;

    /// <summary>
    /// Simulator state: position and rotation animation
    /// </summary>
    private const int SIMULATION_ON = 1;

    /// <summary>
    /// Simulator state: force feedback activated
    /// </summary>
    private const int NEEDLE_FEEDBACK_ON = 2;

    /// <summary>
    /// Simulator state: end of trial, target reached
    /// </summary>
    private const int TARGET_REACHED = 3;

    /// <summary>
    /// Simulator state: technical problem (manually activated by user/experimenter)
    /// </summary>
    private const int TECHNICAL_PROBLEM = 4;

    /// <summary>
    /// 
    /// </summary>
    private const int WAIT_TO_START_BUTTON = 5;

    /// <summary>
    /// 
    /// </summary>
    private const int SEARCHING_TARGET = 6;

    /// <summary>
    /// 
    /// </summary>
    private const int FREE_MANIPULATION = 7;


    //---------------------------------------------------------------------------
    // TISSUE ATTRIBUTES
    //---------------------------------------------------------------------------

    /// <summary>
    /// List of present obstacles
    /// </summary>
    private Obstacles[] Obstacles = null;

    /// <summary>
    /// Stiffness coefficient for Skin Layer [N/m]
    /// </summary>
    private float kStiffness1stLayerHaptic = 20f;

    /// <summary>
    /// Damping coefficient for Skin Layer [N/m]
    /// </summary>
    private float kDamping1stLayerHaptic = 2.2f;

    /// <summary>
    /// Cutting coefficient for Skin Layer [N/m]
    /// </summary>
    private float kCutting1stLayerHaptic = 1.8f;

    /// <summary>
    /// Stiffness coefficient for Skin Layer [N/m]
    /// </summary>
    private float kStiffness2ndLayerHaptic = 35f;

    /// <summary>
    /// Damping coefficient for Skin Layer [N/m]
    /// </summary>
    private float kDamping2ndLayerHaptic = 2.2f;

    /// <summary>
    /// Cutting coefficient for Skin Layer [N/m]
    /// </summary>
    private float kCutting2ndLayerHaptic = 1.8f;

    //---------------------------------------------------------------------------
    // NEEDLE ATTRIBUTES
    //---------------------------------------------------------------------------

    /// <summary>
    /// Current position of needle
    /// </summary>
    private Vector3 MyPosition = Vector3.zero;

    /// <summary>
    /// Current rotation of needle
    /// </summary>
    private Quaternion MyRotation = Quaternion.identity;

    /// <summary>
    /// Last frame rotation of needle for rotation filter
    /// </summary>
    private Quaternion PreviousMyRotation = Quaternion.identity;

    //---------------------------------------------------------------------------
    // NEEDLE STATE VARIABLES
    //---------------------------------------------------------------------------

    /// <summary>
    /// X position of the tip when entering the skin layers
    /// </summary>
    private float contactPositionX = OUTSIDE_POSITION;

    /// <summary>
    /// Z position of the tip when entering the skin layers
    /// </summary>
    private float contactPositionZ = OUTSIDE_POSITION;

    /// <summary>
    /// The gimbal position [mm] stored when contact with first membrane (reseted when transpasing it)
    /// </summary>
    public Vector3 contactPosition = Vector3.zero;

    /// <summary>
    /// Position of the tip when entering the skin layers
    /// </summary>
    private Vector3 lastPosDevice = Vector3.zero;

    /// <summary>
    /// Rotation of the tip when entering the skin layers
    /// </summary>
    private Quaternion lastRotDevice = Quaternion.identity;

    /// <summary>
    /// Position of the tip before reaching table
    /// </summary>
    private Vector3 previousPosition;

    //---------------------------------------------------------------------------
    // SIMULATOR VARIABLES
    //---------------------------------------------------------------------------

    /// <summary>
    /// Stiffness force corresponding to first layer
    /// </summary>
    public float forceStiffness1 = 0f;

    /// <summary>
    /// Friction force corresponding to first layer
    /// </summary>
    public float forceFriction1 = 0f;

    /// <summary>
    /// Cutting force corresponding to first layer
    /// </summary>
    public float forceCutting1 = 0f;

    /// <summary>
    /// Stiffness force corresponding to second layer
    /// </summary>
    public float forceStiffness2 = 0f;

    /// <summary>
    /// Friction force corresponding to second layer
    /// </summary>
    public float forceFriction2 = 0f;

    /// <summary>
    /// Cutting force corresponding to second layer
    /// </summary>
    public float forceCutting2 = 0f;

    /// <summary>
    /// Dumping force corresponding to first & second layers
    /// </summary>
    public float forceDumping12 = 0f;

    /// <summary>
    /// Addition of forces in the Y direction
    /// </summary>
    public float forceTotalY = 0f;

    /// <summary>
    /// Membrane forces before traspasing the membrane
    /// </summary>
    private Vector3 membraneForce;

    //---------------------------------------------------------------------------
    // SIMULATOR VARIABLES
    //---------------------------------------------------------------------------

    /// <summary>
    /// Mutex for thread safety for position & rotation variables
    /// </summary>
    private readonly object _lock = new object();

    /// <summary>
    /// Mutex for thread safety for simulator state
    /// </summary>
    private readonly object _lockState = new object();

    /// <summary>
    /// Simulator state: haptic state
    /// </summary>
    public int _hapticState = SIMULATION_OFF;

    /// <summary>
    /// State variable for haptic thread
    /// </summary>
    public int _state = SIMULATION_OFF;

    /// <summary>
    /// Initial position of the starting point
    /// </summary>
    private Vector3 StartPointPosition = new Vector3(0, 70, 0);

    /// <summary>
    /// 
    /// </summary>
    private bool _begin = true;

    /// ----------------------------------------------------------------------------------
    /// --FIXME ===>>>> FOR DEBUG
    /// ----------------------------------------------------------------------------------
    public Vector3 FUERZA = Vector3.zero;
    /// ----------------------------------------------------------------------------------

    //---------------------------------------------------------------------------
    // FUNCTIONS
    //---------------------------------------------------------------------------

    /// <summary>
    /// Initializes communication with Phantom device
    /// </summary>
    /// <returns><code>true</code> if everything was well instantiated, <code>false</code> if 
    /// it exist already a communication or if it was unsuccessful</returns>
    public bool InitHaptics(bool train) {
        // Initializes variables
        Init();

        if (Phantom != null)
            return false;

        // Instantiation of Phantom
        Phantom = new SimplePhantomUnity();

        // To start the iterative process
        if (Phantom != null) {
            // It specifies the method to be executed repeatedly
            if (!train)
                Phantom.AddSchedule(PhantomUpdate, Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);
            else
                Phantom.AddSchedule(PhantomUpdateTraining, Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);

            // test --FIXME
            while (!Phantom.IsAvailable) ;

            Debug.Log(_tag + "Initializing device...");
            Phantom.Start();

            // Get information about the device
            Debug.Log(_tag + "Information Phantom device :\n" +
                "Usable Workspace Max    = " + Phantom.UsableWorkspaceMaximum + "\n" +
                "Usable Workspace Min    = " + Phantom.UsableWorkspaceMinimum + "\n" +
                "Workspace Available Max = " + Phantom.WorkspaceMaximum + "\n" +
                "Workspace Available Min = " + Phantom.WorkspaceMinimum + "\n" +
                "Instant. Update Rate    = " + Phantom.GetInstantaneousUpdateRate() + "\n" +
                "Max nominal cont. Force = " + Phantom.GetContinuousForceLimit() + "\n" +
                "Max nominal Force       = " + Phantom.GetForceLimit() + "\n" +
                "Max force clamping enab = " + Phantom.IsEnabledMaxForceClamping() + "\n" +
                "SW force limit enab     = " + Phantom.IsEnabledSwForceLimit() + "\n");
        }

        return Phantom != null;
    }

    /// <summary>
    /// Process when disabling the application
    /// </summary>
    private void OnDisable() {
        Debug.Log(_tag + "Haptic go out on disable");
        StopHaptics();
    }

    /// <summary>
    /// Stops device communication
    /// </summary>
    /// <returns><code>true</code> in case of success, <code>false</code> otherwise</returns>
    public bool StopHaptics() {
        if (Phantom == null || !Phantom.IsRunning)
            return false;

        while (!Phantom.IsAvailable) Debug.Log(_tag + "...");

        // Exit the use of PHANTOM
        Phantom.Close();
        Phantom = null;

        return true;
    }

    /// <summary>
    /// Process at the start of the simulation
    /// </summary>
    private void Start() {
        // ...
        Init();
        InitHaptics(false);
    }

    /// <summary>
    /// Process when instantiating script
    /// </summary>
    void Awake() {
        // ...
    }

    /// <summary>
    /// Initialization of the manager
    /// </summary>
    private void Init() {
        // Initialization of hand position and orientation
        HandPosition = Vector3.zero;
        HandRotation = Quaternion.identity;

        // Initialization of speed and direction
        //HandDirection = Vector3.zero;
        HandVelocity = Vector3.zero;

        // Initialization of forces to apply
        Force = Vector3.zero;

        // Make a list of the rigid obstacles in the environment
        Obstacles = GameObject.FindObjectsOfType<Obstacles>();

        // Initialize object attributes;
        MyPosition = previousPosition = contactPosition = transform.position;
        MyRotation = transform.rotation;

        // The rest of the initializations
        contactPositionX = contactPositionZ = OUTSIDE_POSITION;
        lastRotDevice = Quaternion.identity;
        lastPosDevice = Vector3.zero;

        // state variables
        _hapticState = _state = SIMULATION_OFF;
        _begin = true;
    }

    /// <summary>
    /// Process each frame - Updates graphic representation
    /// </summary>
    private void Update() {
        // save previous rotation
        PreviousMyRotation = transform.localRotation;

        lock (_lock) {
            // Set device position (unity length) and orientation
            transform.localPosition = MyPosition;
            if (Quaternion.Angle(PreviousMyRotation, MyRotation) > ROTATION_LOW_LIMIT)
                transform.localRotation = MyRotation;
        }

        // for manual testing without haptics buttons
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            lock (_lockState)
                _hapticState = TARGET_REACHED;
        }
    }

    /// <summary>
    /// Function to pause execution because there was a technical problem.
    /// Manually activated by user/experimenter
    /// </summary>
    public void PauseExecution() {
        // update haptic state to technical problem
        lock (_lockState)
            _hapticState = TECHNICAL_PROBLEM;
    }

    /// <summary>
    /// Function used to externally set an state for the haptic feedback control
    /// </summary>
    /// <param name="state">State to switch on to</param>
    public void SetState(int state) {
        if (state == SIMULATION_ON) {
            // clean all the previous measures (data is already with simualtor manager)
            Debug.Log(_tag + "Cleaning previous trial data...");

            if (_begin) {
                _begin = false;
                state = WAIT_TO_START_BUTTON;
            }
        }

        // update haptic state
        lock (_lockState)
            _hapticState = state;
    }

    /// <summary>
    /// Collects the trial time and needle final position when target reached
    /// </summary>
    /// <param name="data">Contains the total elapsed time for the trial and final position of the needle</param>
    /*
    public void GetTrialMeasures(out LogInfo data)
    {
        // Set device final position
        transform.localPosition = MyPosition;

        lock (_lock)
            _TrialFinalPosition = transform.position;

        data.TotalTime = -1;
        data.FinalPosition = _TrialFinalPosition;
    }*/

    /// <summary>
    /// Function to be executed asynchronously from the haptic device
    /// Responsable of all the haptic force feedback during simulation
    /// </summary>
    /// <returns><code>true</code> if everything was successfully applied</returns>
    bool PhantomUpdate() {
        // Get the position of the hand (gimbal part) [mm]
        HandPosition = Phantom.GetPosition();


        // Get the hand posture (orientation)
        HandRotation = Phantom.GetRotation();


        // Get the speed of the hand [mm/s]
        HandVelocity = Phantom.GetVelocity();


        // Re-init force feedback to 0
        Force = Vector3.zero;

        // get actual simulation state
        lock (_lockState)
            _state = _hapticState;

        if (_state == TECHNICAL_PROBLEM) {
            // reset contact position
            contactPositionX = contactPositionZ = OUTSIDE_POSITION;

            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = Vector3.zero;
                MyRotation = Quaternion.identity;
            }

            Debug.Log(_tag + "Technical problem... Abort trial");

            lock (_lockState)
                _hapticState = SIMULATION_OFF;

            return true;
        }

        // SIMULATION OFF - do nothing
        if (_state == SIMULATION_OFF) {
            // reset forces (= Vector3.zero)
            Phantom.SetForce(Force);
            return true;
        }

        if (_state == WAIT_TO_START_BUTTON) {
            // USER indicates that target is reached + force feedback 0
            if (Phantom.GetButton() == Buttons.Button2 || Phantom.GetButton() == Buttons.Button1) {
                lock (_lockState)
                    _hapticState = SIMULATION_ON;

                ////SimulatorManager.instance.PlayRollSound();
            }

            // reset forces
            Phantom.SetForce(Force);
            return true;
        }

        // SIMULATION ON - waiting for starting point
        if (_state == SIMULATION_ON) {
            // distance to the starting point
            float distanceToStartPoint = Mathf.Abs(StartPointPosition.y + 10 - HandPosition.y);

            // Needle at starting point
            if (distanceToStartPoint < 2f) {
                Debug.Log(_tag + "Starting point met");

                // notify simulator
                ////SimulatorManager.instance.MeasuresStarted();

                // update simulation state
                lock (_lockState)
                    _hapticState = NEEDLE_FEEDBACK_ON;

                // reset forces
                Phantom.SetForce(Force);
                return true;
            }

            // update force feedback around starting ball
            float ballStiffness = 1.5f;
            float clampValue = 1.2f;

            Vector3 ballForces = ballStiffness * (StartPointPosition - HandPosition);
            if (ballForces.magnitude > clampValue) {
                ballForces.Normalize();
                ballForces *= clampValue;
            }

            // update force
            Force += ballForces;

            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = HandPosition * UNIT_LENGTH;
                MyRotation = HandRotation;

                // If it is below table position -> set it back to ground level
                if (HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero)
                    MyPosition = previousPosition;
                else
                    previousPosition = MyPosition;
            }

            // set ball resistance forces
            Phantom.SetForce(Force);
            return true;
        }

        // TARGET REACHED - needle old position is the final position + force feedback 0
        if (_state == TARGET_REACHED) {
            // reset contact position
            contactPositionX = contactPositionZ = OUTSIDE_POSITION;

            Debug.Log(_tag + "Target reached notifying simulator manager");

            lock (_lockState)
                _hapticState = SIMULATION_OFF;

            // notify simulator manager
            ////SimulatorManager.instance.TargetReached();
            return true;
        }

        // USER indicates that target is reached + force feedback 0
        if (Phantom.GetButton() == Buttons.Button2 || Phantom.GetButton() == Buttons.Button1) {
            lock (_lockState)
                _hapticState = TARGET_REACHED;

            // reset forces
            Phantom.SetForce(Force);
            return true;
        }

        //---------------------------------------------------------------------------
        // OBSTACLES FORCE ADDITION
        //---------------------------------------------------------------------------

        // Calculate the force received from the obstacles
        if (Obstacles != null) {
            // Temporal variable to calculate the obstacles force
            Vector3 force = Vector3.zero;

            foreach (Obstacles obj in Obstacles) {
                try {
                    // calculate rigid obstacles forces (ex. Table)
                    force += ((RigidObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                }
                catch {
                    // calculate other obstacles forces (such as cysts)
                    force += ((SampleObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                }
            }

            // add obstacles force
            Force += force;
        }
        //---------------------------------------------------------------------------

        // Hand position & rotation in the Unity world
        Vector3 currentPosition = HandPosition * UNIT_LENGTH;
        Quaternion currentRotation = HandRotation;

        // init forces to apply to haptic in the Y direction
        forceStiffness1 = forceFriction1 = forceCutting1 = forceStiffness2 = forceFriction2 = forceCutting2 = forceDumping12 = forceTotalY = 0f;

        //---------------------------------------------------------------------------
        // FORCES FROM TISSUE - NEEDLE INTERACTION (1st and 2nd layer)
        //---------------------------------------------------------------------------

        // if within the square of tissue in X, Z coordinates (big cube with all tissue layers inside)
        if (Mathf.Abs(HandPosition.x) < TISSUE_DIMENSIONS.x && Mathf.Abs(HandPosition.z) < TISSUE_DIMENSIONS.z) {

            // get vertical position of the needle
            float verticalPosition = HandPosition.y * UNIT_LENGTH;

            // if it has traspased the membrane
            if (verticalPosition < FIRST_LAYER_TOP - 0.05) {
                contactPosition = Vector3.zero;

                // set contact position, store position and rotation of needle at the moment of penetration
                if (contactPositionX == OUTSIDE_POSITION && contactPositionZ == OUTSIDE_POSITION) {
                    contactPositionX = currentPosition.x;
                    contactPositionZ = currentPosition.z;
                    lastPosDevice = currentPosition;
                    lastRotDevice = HandRotation;

                    // get rotation matrix to get direction of the needle when penetrating
                    Phantom.GetRotationMatrix(out RotationMatrix);
                    //SimulatorManager.instance.InsideSkin(true);
                }
            } else {
                // reset contact position
                if (contactPositionX != OUTSIDE_POSITION)
                    //SimulatorManager.instance.InsideSkin(false);
                    contactPositionX = contactPositionZ = OUTSIDE_POSITION;
            }

            // init depth variables and velocity
            float probeDop = 0f;
            float probeDopStiffness = 0f;
            float velocity = 0f;

            // limit visual direction if needle inside tissue and calculate lateral forces
            if (contactPositionX != OUTSIDE_POSITION && contactPositionZ != OUTSIDE_POSITION) {
                //---------------------------------------------------------------------------
                // INSIDE SKIN LAYERS LATERAL FORCES ADDITION
                //---------------------------------------------------------------------------

                currentRotation = lastRotDevice;
                float t = (currentPosition.y - lastPosDevice.y) / -(float)RotationMatrix[9];

                // Temporal variable to calculate the lateral forces to limit position
                Vector3 lateralInsideForce = Vector3.zero;
                lateralInsideForce.x = CalculateLateralForce(currentPosition, HandVelocity, lastPosDevice, 0).x;
                lateralInsideForce.z = CalculateLateralForce(currentPosition, HandVelocity, lastPosDevice, 2).z;

                // add lateral forces
                Force += lateralInsideForce;

                //---------------------------------------------------------------------------

                // update current position with limits in the X and Z position
                currentPosition = new Vector3(t * -(float)RotationMatrix[8] + lastPosDevice.x, currentPosition.y, t * (float)RotationMatrix[10] + lastPosDevice.z);

                // depth in the skin from penetration point
                probeDop = (currentPosition - lastPosDevice).magnitude;
            }

            //---------------------------------------------------------------------------
            // FIRST LAYER FORCE ADDITION
            //---------------------------------------------------------------------------

            // limit depthness
            probeDop = Mathf.Clamp(probeDop, 0f, 0.35f);

            // get position from top 1st layer position
            probeDopStiffness = FIRST_LAYER_TOP + 0.125f - currentPosition.y;
            //probeDopStiffness = Mathf.Clamp(probeDopStiffness, 0f, 0.35f);

            // Damping force
            if (verticalPosition < SECOND_LAYER_TOP - 0.05) {
                // get velocity and limit it
                velocity = HandVelocity.y;
                velocity = Mathf.Clamp(velocity, -0.8f, 0.8f);

                // Force of first layer present when inside second layer --not documented the source of this force (present when is in second layer)
                forceDumping12 = (kDamping1stLayerHaptic) * (-velocity) * (SECOND_LAYER_TOP + GROUND_LEVEL);
                forceTotalY += forceDumping12 * DEVICE_FORCE_SCALE;
            } else if (probeDopStiffness > 0 && probeDop == 0) {
                //---------------------------------------------------------------------------
                // MEMBRANE STIFFNESS FORCE (before penetration)
                //---------------------------------------------------------------------------

                if (contactPosition == Vector3.zero)
                    contactPosition = HandPosition;

                // get velocity and limit it
                velocity = HandVelocity.y;
                velocity = Mathf.Clamp(velocity, -0.1f, 0.1f);

                // calculate stiffness force (Y direction)
                forceStiffness1 = (2.5f + kStiffness1stLayerHaptic) * probeDopStiffness + kDamping1stLayerHaptic * (-velocity) * probeDopStiffness;

                // apply scale factor for forces
                forceStiffness1 = forceStiffness1 * DEVICE_FORCE_SCALE;
                forceTotalY = forceStiffness1;

                float membraneDamping = 0.003f;
                float membraneStiffness = 0.04f;
                float distanceCoeficient = 0.08f;
                float ClampValue = 0.4f;

                // lateral forces within the membrane: damping force
                membraneForce = -membraneDamping * HandVelocity;
                if (membraneForce.magnitude > ClampValue) {
                    membraneForce.Normalize();
                    membraneForce *= ClampValue;
                }

                //Force += membraneForce;
                Force.x += membraneForce.x - probeDopStiffness * distanceCoeficient;
                Force.y += membraneForce.y;
                Force.z += membraneForce.z - probeDopStiffness * distanceCoeficient; ;

                // lateral forces within the membrane: dynamic stiffness force
                ClampValue = (float)Phantom.GetContinuousForceLimit();
                membraneForce = membraneStiffness * (contactPosition - HandPosition);
                if (membraneForce.magnitude > ClampValue) {
                    membraneForce.Normalize();
                    membraneForce *= ClampValue;
                }

                //Force += membraneForce;
                Force.x += membraneForce.x;
                Force.y += membraneForce.y;
                Force.z += membraneForce.z;

                if ((contactPosition - HandPosition).magnitude > 5)
                    contactPosition = HandPosition;

                //---------------------------------------------------------------------------
            } else if (probeDop > 0 && verticalPosition < FIRST_LAYER_TOP - 0.05) {
                //---------------------------------------------------------------------------
                // TISSUE FRICTION + CUTTING FORCE (after penetration)
                //---------------------------------------------------------------------------

                float f0 = 0.185f;
                float a0 = 0.12f;
                float b0 = -0.097f;
                //float f1 = -3.39f;
                //float a1 = -0.031f;
                //float b1 = 1.7f;

                // get velocity and limit it
                velocity = HandVelocity.y * UNIT_LENGTH;
                velocity = Mathf.Clamp(velocity, -1.5f, 1.5f);

                // calculate friction force
                forceFriction1 = (-velocity * 3 + 800 * ((f0 + b0) * Mathf.Exp(a0 * probeDopStiffness) + b0)) / kDamping1stLayerHaptic;
                //forceFriction1 = (-velocity * 30 + 400 * ((f1 + b1) * Mathf.Exp(a1 * probeDopStiffness) + b1)) / kDamping1stLayerHaptic;

                // apply scale factor for forces
                forceFriction1 = forceFriction1 * DEVICE_FORCE_SCALE;

                // add cutting force (= constant)
                forceCutting1 = kCutting1stLayerHaptic;
                forceTotalY = forceFriction1 + forceCutting1;

                //---------------------------------------------------------------------------
            } else {
                contactPosition = Vector3.zero;
            }

            //---------------------------------------------------------------------------

            //---------------------------------------------------------------------------
            // SECOND LAYER FORCE ADDITION
            //---------------------------------------------------------------------------

            if (currentPosition.y < SECOND_LAYER_TOP + 0.05f) {
                float t2 = (SECOND_LAYER_TOP - lastPosDevice.y) / -(float)RotationMatrix[9];
                Vector3 tempPosition = new Vector3(t2 * -(float)RotationMatrix[8] + lastPosDevice.x, SECOND_LAYER_TOP, t2 * (float)RotationMatrix[10] + lastPosDevice.z);

                // depth in the skin from penetration point
                probeDop = (currentPosition - tempPosition).magnitude;

                // limit depthness
                probeDop = Mathf.Clamp(probeDop, 0f, 0.35f);

                // get position from top 2nd layer position
                probeDopStiffness = SECOND_LAYER_TOP + 0.05f - currentPosition.y;
                //probeDopStiffness = Mathf.Clamp(probeDopStiffness, 0f, 0.35f);

                if (probeDopStiffness > 0 && verticalPosition > SECOND_LAYER_TOP - 0.05) {
                    //---------------------------------------------------------------------------
                    // MEMBRANE STIFFNESS FORCE (before penetration)
                    //---------------------------------------------------------------------------

                    forceStiffness2 = kStiffness2ndLayerHaptic * probeDopStiffness + kDamping2ndLayerHaptic * (-velocity) * probeDopStiffness;

                    // apply scale factor for forces
                    forceStiffness2 = forceStiffness2 * DEVICE_FORCE_SCALE;
                    forceTotalY += forceStiffness2;

                    //---------------------------------------------------------------------------
                } else if (probeDop > 0 && verticalPosition < SECOND_LAYER_TOP - 0.05) {
                    //---------------------------------------------------------------------------
                    // TISSUE FRICTION + CUTTING FORCE (after penetration)
                    //---------------------------------------------------------------------------

                    float f0 = 0.185f;
                    float a0 = 0.12f;
                    float b0 = -0.097f;
                    //float f1 = -3.39f;
                    //float a1 = -0.031f;
                    //float b1 = 1.7f;

                    // get velocity and limit it
                    velocity = HandVelocity.y * UNIT_LENGTH;
                    velocity = Mathf.Clamp(velocity, -1.5f, 1.5f);

                    forceFriction2 = (-velocity * 3 + 900 * ((f0 + b0) * Mathf.Exp(a0 * probeDopStiffness) + b0)) / kDamping2ndLayerHaptic;

                    // apply scale factor for forces
                    forceFriction2 = forceFriction2 * DEVICE_FORCE_SCALE;

                    forceCutting2 = kCutting2ndLayerHaptic;
                    forceTotalY += forceFriction2 + forceCutting2;

                    //---------------------------------------------------------------------------
                }
            }
            //---------------------------------------------------------------------------

            // update calculated forces
            Force[1] += forceTotalY;
            //---------------------------------------------------------------------------
        } else {
            contactPosition = Vector3.zero;

            if (contactPositionX != OUTSIDE_POSITION)
                //SimulatorManager.instance.InsideSkin(true);

                // reset contact position
                contactPositionX = contactPositionZ = OUTSIDE_POSITION;
        }
        //---------------------------------------------------------------------------

        //---------------------------------------------------------------------------
        // CLAMP FORCE AND SEND TO DEVICE
        //---------------------------------------------------------------------------

        // So as not to exceed the upper limit of the force
        //if (Force.sqrMagnitude > (MAX_FORCE * MAX_FORCE))
        //{
        //    Force.Normalize();
        //    Force *= MAX_FORCE;
        //}

        //---------------------------------------------------------------------------

        //---------------------------------------------------------------------------
        // for debug
        FUERZA = Force;
        //---------------------------------------------------------------------------

        // Force feedback to PHANTOM device [N]
        Phantom.SetForce(Force);

        bool outside = false;
        lock (_lock) {
            // set position and orientation for graphic needle
            MyPosition = currentPosition;
            MyRotation = currentRotation;

            // If it is below table position -> set it back to ground level
            if (outside = (HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero))
                MyPosition = previousPosition;
            else
                previousPosition = MyPosition;
        }

        currentPosition = outside ? previousPosition : currentPosition;

        // Log state
        //SimulatorManager.instance.LogEntry(new Vector3(Force.x, Force.y, Force.z), new Vector3(currentPosition.x, currentPosition.y, currentPosition.z));

        return true;
    }

    /// <summary>
    /// Function to be executed asynchronously from the haptic device
    /// Responsable of all the haptic force feedback during simulation
    /// </summary>
    /// <returns><code>true</code> if everything was successfully applied</returns>
    bool PhantomUpdateTraining() {
        // Get the position of the hand (gimbal part) [mm]
        HandPosition = Phantom.GetPosition();

        // Get the hand posture (orientation)
        HandRotation = Phantom.GetRotation();

        // Get the speed of the hand [mm/s]
        HandVelocity = Phantom.GetVelocity();

        // Re-init force feedback to 0
        Force = Vector3.zero;

        // get actual simulation state
        lock (_lockState)
            _state = _hapticState;

        if (_state == TECHNICAL_PROBLEM) {
            // reset contact position
            contactPositionX = contactPositionZ = OUTSIDE_POSITION;

            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = Vector3.zero;
                MyRotation = Quaternion.identity;
            }

            Debug.Log(_tag + "Technical problem... Abort trial");

            lock (_lockState)
                _hapticState = SIMULATION_OFF;

            return true;
        }

        // SIMULATION OFF - do nothing
        if (_state == SIMULATION_OFF) {
            // reset forces (= Vector3.zero)
            Phantom.SetForce(Force);
            return true;
        }

        if (_state == WAIT_TO_START_BUTTON) {
            // USER indicates that target is reached + force feedback 0
            if (Phantom.GetButton() == Buttons.Button2 || Phantom.GetButton() == Buttons.Button1) {
                lock (_lockState)
                    _hapticState = SIMULATION_ON;

                //SimulatorManager.instance.PlayRollSound();
            }

            // reset forces
            Phantom.SetForce(Force);
            return true;
        }

        if (_state == FREE_MANIPULATION) {
            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = HandPosition * UNIT_LENGTH;
                MyRotation = HandRotation;

                // If it is below table position -> set it back to ground level
                if (HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero)
                    MyPosition = previousPosition;
                else
                    previousPosition = MyPosition;
            }

            // set ball resistance forces
            Phantom.SetForce(Force);
            return true;
        }

        // SIMULATION ON - waiting for starting point
        if (_state == SIMULATION_ON) {
            // distance to the starting point
            float distanceToStartPoint = Mathf.Abs(StartPointPosition.y + 10 - HandPosition.y);

            // Needle at starting point
            if (distanceToStartPoint < 2f) {
                Debug.Log(_tag + "Starting point met");

                // notify simulator
                //SimulatorManager.instance.MeasuresStarted();

                // update simulation state
                lock (_lockState)
                    _hapticState = FREE_MANIPULATION;

                // reset forces
                Phantom.SetForce(Force);
                return true;
            }

            // update force feedback around starting ball
            float ballStiffness = 1.5f;
            float clampValue = 1.2f;

            Vector3 ballForces = ballStiffness * (StartPointPosition - HandPosition);
            if (ballForces.magnitude > clampValue) {
                ballForces.Normalize();
                ballForces *= clampValue;
            }

            // update force
            Force += ballForces;

            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = HandPosition * UNIT_LENGTH;
                MyRotation = HandRotation;

                // If it is below table position -> set it back to ground level
                if (HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero)
                    MyPosition = previousPosition;
                else
                    previousPosition = MyPosition;
            }

            // set ball resistance forces
            Phantom.SetForce(Force);
            return true;
        }

        if (_state == SEARCHING_TARGET) {
            // USER indicates that target is reached + force feedback 0
            if (Phantom.GetButton() == Buttons.Button2 || Phantom.GetButton() == Buttons.Button1) {
                lock (_lockState)
                    _hapticState = TARGET_REACHED;

                // reset forces
                Phantom.SetForce(Force);
                return true;
            }

            //---------------------------------------------------------------------------
            // OBSTACLES FORCE ADDITION
            //---------------------------------------------------------------------------

            // Calculate the force received from the obstacles
            if (Obstacles != null) {
                // Temporal variable to calculate the obstacles force
                Vector3 force = Vector3.zero;

                foreach (Obstacles obj in Obstacles) {
                    try {
                        // calculate rigid obstacles forces (ex. Table)
                        force += ((RigidObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                    }
                    catch {
                        // calculate other obstacles forces (such as cysts)
                        force += ((SampleObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                    }
                }

                // add obstacles force
                Force += force;
            }
            //---------------------------------------------------------------------------

            // update position & orientation
            lock (_lock) {
                // set position and orientation for graphic needle
                MyPosition = HandPosition * UNIT_LENGTH;
                MyRotation = HandRotation;

                // If it is below table position -> set it back to ground level
                if (HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero)
                    MyPosition = previousPosition;
                else
                    previousPosition = MyPosition;
            }

            // reset forces
            Phantom.SetForce(Force);
            return true;
        }

        // TARGET REACHED - needle old position is the final position + force feedback 0
        if (_state == TARGET_REACHED) {
            // reset contact position
            contactPositionX = contactPositionZ = OUTSIDE_POSITION;

            Debug.Log(_tag + "Target reached notifying simulator manager");

            lock (_lockState)
                _hapticState = SIMULATION_ON;

            // notify simulator manager
            //SimulatorManager.instance.TargetReached();
            return true;
        }

        //---------------------------------------------------------------------------
        // OBSTACLES FORCE ADDITION
        //---------------------------------------------------------------------------

        // Calculate the force received from the obstacles
        if (Obstacles != null) {
            // Temporal variable to calculate the obstacles force
            Vector3 force = Vector3.zero;

            foreach (Obstacles obj in Obstacles) {
                try {
                    // calculate rigid obstacles forces (ex. Table)
                    force += ((RigidObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                }
                catch {
                    // calculate other obstacles forces (such as cysts)
                    force += ((SampleObstacles)obj).CalculateForce(HandPosition, HandVelocity);
                }
            }

            // add obstacles force
            Force += force;
        }
        //---------------------------------------------------------------------------

        // Hand position & rotation in the Unity world
        Vector3 currentPosition = HandPosition * UNIT_LENGTH;
        Quaternion currentRotation = HandRotation;

        // init forces to apply to haptic in the Y direction
        forceStiffness1 = forceFriction1 = forceCutting1 = forceStiffness2 = forceFriction2 = forceCutting2 = forceDumping12 = forceTotalY = 0f;

        //---------------------------------------------------------------------------
        // FORCES FROM TISSUE - NEEDLE INTERACTION (1st and 2nd layer)
        //---------------------------------------------------------------------------

        // if within the square of tissue in X, Z coordinates (big cube with all tissue layers inside)
        if (Mathf.Abs(HandPosition.x) < TISSUE_DIMENSIONS.x && Mathf.Abs(HandPosition.z) < TISSUE_DIMENSIONS.z) {

            // get vertical position of the needle
            float verticalPosition = HandPosition.y * UNIT_LENGTH;

            // if it has traspased the membrane
            if (verticalPosition < FIRST_LAYER_TOP - 0.05) {
                contactPosition = Vector3.zero;

                // set contact position, store position and rotation of needle at the moment of penetration
                if (contactPositionX == OUTSIDE_POSITION && contactPositionZ == OUTSIDE_POSITION) {
                    contactPositionX = currentPosition.x;
                    contactPositionZ = currentPosition.z;
                    lastPosDevice = currentPosition;
                    lastRotDevice = HandRotation;

                    // get rotation matrix to get direction of the needle when penetrating
                    Phantom.GetRotationMatrix(out RotationMatrix);
                    //SimulatorManager.instance.InsideSkin(true);
                }
            } else {
                // reset contact position
                if (contactPositionX != OUTSIDE_POSITION)
                    //SimulatorManager.instance.InsideSkin(false);
                    contactPositionX = contactPositionZ = OUTSIDE_POSITION;
            }

            // init depth variables and velocity
            float probeDop = 0f;
            float probeDopStiffness = 0f;
            float velocity = 0f;

            // limit visual direction if needle inside tissue and calculate lateral forces
            if (contactPositionX != OUTSIDE_POSITION && contactPositionZ != OUTSIDE_POSITION) {
                //---------------------------------------------------------------------------
                // INSIDE SKIN LAYERS LATERAL FORCES ADDITION
                //---------------------------------------------------------------------------

                currentRotation = lastRotDevice;
                float t = (currentPosition.y - lastPosDevice.y) / -(float)RotationMatrix[9];

                // Temporal variable to calculate the lateral forces to limit position
                Vector3 lateralInsideForce = Vector3.zero;
                lateralInsideForce.x = CalculateLateralForce(currentPosition, HandVelocity, lastPosDevice, 0).x;
                lateralInsideForce.z = CalculateLateralForce(currentPosition, HandVelocity, lastPosDevice, 2).z;

                // add lateral forces
                Force += lateralInsideForce;

                //---------------------------------------------------------------------------

                // update current position with limits in the X and Z position
                currentPosition = new Vector3(t * -(float)RotationMatrix[8] + lastPosDevice.x, currentPosition.y, t * (float)RotationMatrix[10] + lastPosDevice.z);

                // depth in the skin from penetration point
                probeDop = (currentPosition - lastPosDevice).magnitude;
            }

            //---------------------------------------------------------------------------
            // FIRST LAYER FORCE ADDITION
            //---------------------------------------------------------------------------

            // limit depthness
            probeDop = Mathf.Clamp(probeDop, 0f, 0.35f);

            // get position from top 1st layer position
            probeDopStiffness = FIRST_LAYER_TOP + 0.125f - currentPosition.y;
            //probeDopStiffness = Mathf.Clamp(probeDopStiffness, 0f, 0.35f);

            // Damping force
            if (verticalPosition < SECOND_LAYER_TOP - 0.05) {
                // get velocity and limit it
                velocity = HandVelocity.y;
                velocity = Mathf.Clamp(velocity, -0.8f, 0.8f);

                // Force of first layer present when inside second layer --not documented the source of this force (present when is in second layer)
                forceDumping12 = (kDamping1stLayerHaptic) * (-velocity) * (SECOND_LAYER_TOP + GROUND_LEVEL);
                forceTotalY += forceDumping12 * DEVICE_FORCE_SCALE;
            } else if (probeDopStiffness > 0 && probeDop == 0) {
                //---------------------------------------------------------------------------
                // MEMBRANE STIFFNESS FORCE (before penetration)
                //---------------------------------------------------------------------------

                if (contactPosition == Vector3.zero)
                    contactPosition = HandPosition;

                // get velocity and limit it
                velocity = HandVelocity.y;
                velocity = Mathf.Clamp(velocity, -0.1f, 0.1f);

                // calculate stiffness force (Y direction)
                forceStiffness1 = (2.5f + kStiffness1stLayerHaptic) * probeDopStiffness + kDamping1stLayerHaptic * (-velocity) * probeDopStiffness;

                // apply scale factor for forces
                forceStiffness1 = forceStiffness1 * DEVICE_FORCE_SCALE;
                forceTotalY = forceStiffness1;

                float membraneDamping = 0.003f;
                float membraneStiffness = 0.04f;
                float distanceCoeficient = 0.08f;
                float ClampValue = 0.4f;

                // lateral forces within the membrane: damping force
                membraneForce = -membraneDamping * HandVelocity;
                if (membraneForce.magnitude > ClampValue) {
                    membraneForce.Normalize();
                    membraneForce *= ClampValue;
                }

                //Force += membraneForce;
                Force.x += membraneForce.x - probeDopStiffness * distanceCoeficient;
                Force.y += membraneForce.y;
                Force.z += membraneForce.z - probeDopStiffness * distanceCoeficient; ;

                // lateral forces within the membrane: dynamic stiffness force
                ClampValue = (float)Phantom.GetContinuousForceLimit();
                membraneForce = membraneStiffness * (contactPosition - HandPosition);
                if (membraneForce.magnitude > ClampValue) {
                    membraneForce.Normalize();
                    membraneForce *= ClampValue;
                }

                //Force += membraneForce;
                Force.x += membraneForce.x;
                Force.y += membraneForce.y;
                Force.z += membraneForce.z;

                if ((contactPosition - HandPosition).magnitude > 5)
                    contactPosition = HandPosition;

                //---------------------------------------------------------------------------
            } else if (probeDop > 0 && verticalPosition < FIRST_LAYER_TOP - 0.05) {
                //---------------------------------------------------------------------------
                // TISSUE FRICTION + CUTTING FORCE (after penetration)
                //---------------------------------------------------------------------------

                float f0 = 0.185f;
                float a0 = 0.12f;
                float b0 = -0.097f;
                //float f1 = -3.39f;
                //float a1 = -0.031f;
                //float b1 = 1.7f;

                // get velocity and limit it
                velocity = HandVelocity.y * UNIT_LENGTH;
                velocity = Mathf.Clamp(velocity, -1.5f, 1.5f);

                // calculate friction force
                forceFriction1 = (-velocity * 3 + 800 * ((f0 + b0) * Mathf.Exp(a0 * probeDopStiffness) + b0)) / kDamping1stLayerHaptic;
                //forceFriction1 = (-velocity * 30 + 400 * ((f1 + b1) * Mathf.Exp(a1 * probeDopStiffness) + b1)) / kDamping1stLayerHaptic;

                // apply scale factor for forces
                forceFriction1 = forceFriction1 * DEVICE_FORCE_SCALE;

                // add cutting force (= constant)
                forceCutting1 = kCutting1stLayerHaptic;
                forceTotalY = forceFriction1 + forceCutting1;

                //---------------------------------------------------------------------------
            } else {
                contactPosition = Vector3.zero;
            }

            //---------------------------------------------------------------------------

            //---------------------------------------------------------------------------
            // SECOND LAYER FORCE ADDITION
            //---------------------------------------------------------------------------

            if (currentPosition.y < SECOND_LAYER_TOP + 0.05f) {
                float t2 = (SECOND_LAYER_TOP - lastPosDevice.y) / -(float)RotationMatrix[9];
                Vector3 tempPosition = new Vector3(t2 * -(float)RotationMatrix[8] + lastPosDevice.x, SECOND_LAYER_TOP, t2 * (float)RotationMatrix[10] + lastPosDevice.z);

                // depth in the skin from penetration point
                probeDop = (currentPosition - tempPosition).magnitude;

                // limit depthness
                probeDop = Mathf.Clamp(probeDop, 0f, 0.35f);

                // get position from top 2nd layer position
                probeDopStiffness = SECOND_LAYER_TOP + 0.05f - currentPosition.y;
                //probeDopStiffness = Mathf.Clamp(probeDopStiffness, 0f, 0.35f);

                if (probeDopStiffness > 0 && verticalPosition > SECOND_LAYER_TOP - 0.05) {
                    //---------------------------------------------------------------------------
                    // MEMBRANE STIFFNESS FORCE (before penetration)
                    //---------------------------------------------------------------------------

                    forceStiffness2 = kStiffness2ndLayerHaptic * probeDopStiffness + kDamping2ndLayerHaptic * (-velocity) * probeDopStiffness;

                    // apply scale factor for forces
                    forceStiffness2 = forceStiffness2 * DEVICE_FORCE_SCALE;
                    forceTotalY += forceStiffness2;

                    //---------------------------------------------------------------------------
                } else if (probeDop > 0 && verticalPosition < SECOND_LAYER_TOP - 0.05) {
                    //---------------------------------------------------------------------------
                    // TISSUE FRICTION + CUTTING FORCE (after penetration)
                    //---------------------------------------------------------------------------

                    float f0 = 0.185f;
                    float a0 = 0.12f;
                    float b0 = -0.097f;
                    //float f1 = -3.39f;
                    //float a1 = -0.031f;
                    //float b1 = 1.7f;

                    // get velocity and limit it
                    velocity = HandVelocity.y * UNIT_LENGTH;
                    velocity = Mathf.Clamp(velocity, -1.5f, 1.5f);

                    forceFriction2 = (-velocity * 3 + 900 * ((f0 + b0) * Mathf.Exp(a0 * probeDopStiffness) + b0)) / kDamping2ndLayerHaptic;

                    // apply scale factor for forces
                    forceFriction2 = forceFriction2 * DEVICE_FORCE_SCALE;

                    forceCutting2 = kCutting2ndLayerHaptic;
                    forceTotalY += forceFriction2 + forceCutting2;

                    //---------------------------------------------------------------------------
                }
            }
            //---------------------------------------------------------------------------

            // update calculated forces
            Force[1] += forceTotalY;
            //---------------------------------------------------------------------------
        } else {
            contactPosition = Vector3.zero;

            if (contactPositionX != OUTSIDE_POSITION)
                //SimulatorManager.instance.InsideSkin(true);

                // reset contact position
                contactPositionX = contactPositionZ = OUTSIDE_POSITION;
        }
        //---------------------------------------------------------------------------

        // Force feedback to PHANTOM device [N]
        Phantom.SetForce(Force);

        lock (_lock) {
            // set position and orientation for graphic needle
            MyPosition = currentPosition;
            MyRotation = currentRotation;

            // If it is below table position -> set it back to ground level, then it is outside
            if (!(HandPosition.y * UNIT_LENGTH < GROUND_LEVEL && previousPosition != Vector3.zero))
                MyPosition = previousPosition;
            else
                previousPosition = MyPosition;
        }

        return true;
    }

    /// <summary>
    /// Seek the forces generated when the operating point is in contact with the lateral membrane
    /// </summary>
    /// <param name="tipPosition">The position of the tip [mm]</param>
    /// <param name="tipVelocity">The speed of the tip [mm/s]</param>
    /// <param name="lateralPosition">The stored lateral position</param>
    /// <param name="axe">To determine if it is X (<code>0</code>) or Z (<code>2</code>)</param>
    /// <returns>The force to apply</returns>
    public Vector3 CalculateLateralForce(Vector3 tipPosition, Vector3 tipVelocity, Vector3 lateralPosition, int axe) {
        // local constants
        const float BOUNDARY = 0.0f;
        const float STIFFNESS = 25f;
        const float DUMPING = 0.0f;
        const float FORCE_LIMIT = 3.0f;

        // Calculate the difference from the tip to the object center
        Vector3 differencePositions = tipPosition - lateralPosition;

        // The distance to planar object is assumed in the Y axis
        float distance = differencePositions[axe];

        // No force in the outside of the BOUNDARY with object and no visual feedback
        if (distance == 0) return Vector3.zero;

        // No forces with other planes different from axe
        for (int i = 0; i < 3; i++)
            if (i != axe) differencePositions[i] = 0;

        // Normalisation
        differencePositions /= distance;

        // STIFFNESS force calculation
        float force = STIFFNESS * (BOUNDARY - distance);

        // Restrict to max force
        if (force > FORCE_LIMIT) force = FORCE_LIMIT;

        return (force * differencePositions) - DUMPING * tipVelocity;
    }
}
