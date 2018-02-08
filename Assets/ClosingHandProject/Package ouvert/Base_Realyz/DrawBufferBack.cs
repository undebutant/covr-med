// Decompiled with JetBrains decompiler
// Type: DrawBufferBack
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll

using System.Runtime.InteropServices;
using UnityEngine;

public class DrawBufferBack : MonoBehaviour
{
  public const uint GL_NONE = 0;
  public const uint GL_FRONT_LEFT = 1024;
  public const uint GL_FRONT_RIGHT = 1025;
  public const uint GL_BACK_LEFT = 1026;
  public const uint GL_BACK_RIGHT = 1027;
  public const uint GL_FRONT = 1028;
  public const uint GL_BACK = 1029;
  public const uint GL_LEFT = 1030;
  public const uint GL_RIGHT = 1031;
  public const uint GL_FRONT_AND_BACK = 1032;
  public const uint GL_AUX0 = 1033;
  public const uint GL_AUX1 = 1034;
  public const uint GL_AUX2 = 1035;
  public const uint GL_AUX3 = 1036;
  private bool reportErrorOnce;

  public DrawBufferBack()
  {
    //base.\u002Ector();
  }

  [DllImport("opengl32.dll")]
  public static extern uint glGetError();

  [DllImport("opengl32.dll", SetLastError = true)]
  private static extern void glDrawBuffer(uint mode);

  private void OnPreRender()
  {
    DrawBufferBack.glDrawBuffer(1029U);
    uint error = DrawBufferBack.glGetError();
    if ((int) error == 0 || !this.reportErrorOnce)
      return;
    Debug.Log((object) ("[GL Error] glDrawBuffer(GL_BACK);: " + (object) error));
    this.reportErrorOnce = false;
  }
}
