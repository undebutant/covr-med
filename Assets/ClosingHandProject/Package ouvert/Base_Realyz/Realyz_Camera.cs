// Decompiled with JetBrains decompiler
// Type: Realyz_Camera
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll

using UnityEngine;

public class Realyz_Camera : MonoBehaviour
{
  [HideInInspector]
  public GameObject myScreen;

/*
  public Realyz_Camera()
  {
    base.\u002Ector();
  }
*/
  private void LateUpdate()
  {
    Camera component = (Camera) ((Component) this).GetComponent<Camera>();
    ((Component) component).transform.rotation = ((this.myScreen.transform.rotation * Quaternion.Euler(90f, 180f, 0.0f)));
    Mesh sharedMesh = (this.myScreen.GetComponent(typeof (MeshFilter)) as MeshFilter).sharedMesh;
    //Bounds bounds1 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    // ISSUE: variable of the null type
    //var x1 = ((Bounds) bounds1).size.x;
    // ISSUE: variable of the null type
    //var x2 = this.myScreen.transform.localScale.x;
    //Bounds bounds2 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    // ISSUE: variable of the null type
    //var y1 = ((Bounds) bounds2).size.y;
    // ISSUE: variable of the null type
    //var y2 = this.myScreen.transform.localScale.y;
    //Bounds bounds3 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    // ISSUE: variable of the null type
    //var z1 = ((Bounds) bounds3).size.z;
    // ISSUE: variable of the null type
    //var z2 = this.myScreen.transform.localScale.z;
    Vector3 vector3 = ((Component) component).transform.InverseTransformPoint(this.myScreen.transform.position);
    // ISSUE: variable of the null type
    var y3 = vector3.y;
    Bounds bounds4 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    double num1 = ((Bounds) bounds4).size.z * this.myScreen.transform.localScale.z / 2.0;
    float top = (float) ((y3 + num1) / (100.0 * vector3.z));
    // ISSUE: variable of the null type
    var y4 = vector3.y;
    Bounds bounds5 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    double num2 = ((Bounds) bounds5).size.z * this.myScreen.transform.localScale.z / 2.0;
    float bottom = (float) ((y4 - num2) / (100.0 * vector3.z));
    // ISSUE: variable of the null type
    var x3 = vector3.x;
    Bounds bounds6 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    double num3 = ((Bounds) bounds6).size.x * this.myScreen.transform.localScale.x / 2.0;
    float left = (float) ((x3 - num3) / (100.0 * vector3.z));
    // ISSUE: variable of the null type
    var x4 = vector3.x;
    Bounds bounds7 = sharedMesh.bounds;
    // ISSUE: explicit reference operation
    double num4 = ((Bounds) bounds7).size.x * this.myScreen.transform.localScale.x / 2.0;
    float right = (float) ((x4 + num4) / (100.0 * vector3.z));
    Matrix4x4 matrix4x4 = Realyz_Camera.PerspectiveOffCenter(left, right, bottom, top, component.nearClipPlane, component.farClipPlane);
    component.projectionMatrix = (matrix4x4);
  }

  private static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
  {
    float num1 = (float) (2.0 * (double) near / ((double) right - (double) left));
    float num2 = (float) (2.0 * (double) near / ((double) top - (double) bottom));
    float num3 = (float) (((double) right + (double) left) / ((double) right - (double) left));
    float num4 = (float) (((double) top + (double) bottom) / ((double) top - (double) bottom));
    float num5 = (float) (-((double) far + (double) near) / ((double) far - (double) near));
    float num6 = (float) (-(2.0 * (double) far * (double) near) / ((double) far - (double) near));
    float num7 = -1f;
    Matrix4x4 zero = Matrix4x4.zero;
    zero.SetRow(0, new Vector4(num1, 0f, num3, 0f));
    zero.SetRow(1, new Vector4(0f, num2, num4, 0f));
    zero.SetRow(2, new Vector4(0f, 0f, num5, num6));
    zero.SetRow(3, new Vector4(0f, 0f, num7, 0f));

    /*
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(0, 0, num1);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(0, 1, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(0, 2, num3);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(0, 3, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(1, 0, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(1, 1, num2);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(1, 2, num4);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(1, 3, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(2, 0, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(2, 1, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(2, 2, num5);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(2, 3, num6);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(3, 0, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(3, 1, 0.0f);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(3, 2, num7);
    // ISSUE: explicit reference operation
    ((Matrix4x4) zero).set_Item(3, 3, 0.0f);
    */
    return zero;
  }
}
