// Decompiled with JetBrains decompiler
// Type: VRPNLightAPI_testCS.WrapperVrpnLightAPI
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll

using System.Runtime.InteropServices;

namespace VRPNLightAPI_testCS
{
  internal class WrapperVrpnLightAPI
  {
    [DllImport("vrpnClientLightAPI.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int connectToTracker(string name);

    [DllImport("vrpnClientLightAPI.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getTrackerData(int trackerID, ref trackerData data);

    [DllImport("vrpnClientLightAPI.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int closeTracker(int trackerID);
  }
}
