using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
///     The enum of all available roles
/// </summary>
public enum PlayerRole {
    Surgeon,
    Nurse
}


/// <summary>
///     The enum of all available devices
/// </summary>
public enum DisplayDevice {
    Cave,
    Oculus,
    Monitor
}


/// <summary>
///     The enum of all available devices
/// </summary>
public enum InputDevice {
    Controller,
    Haptic,
    Remote
}



[System.Serializable]
public class StartingConfig {
    /// <summary>
    ///     The IP entered by the client, to connect to the server using this IP
    /// </summary>
    public string serverIP;

    /// <summary>
    ///     The port to use to connect to the server
    /// </summary>
    public int connectionPort;

    /// <summary>
    ///     The role of the local player
    /// </summary>
    public PlayerRole playerRole;

    /// <summary>
    ///     The device used to display the scene
    /// </summary>
    public DisplayDevice displayDevice;

    /// <summary>
    ///     The device used as player input
    /// </summary>
    public InputDevice inputDevice;
}
