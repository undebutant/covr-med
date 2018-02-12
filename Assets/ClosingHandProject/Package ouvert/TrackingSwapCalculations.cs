// Decompiled with JetBrains decompiler
// Type: TrackingSwapCalculations
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll

using System;
using System.IO;
using System.Xml;
using UnityEngine;

public static class TrackingSwapCalculations
{
	/*
  private static string _ConfigTrackingMatrix = "Tracking_Matrix.xml";
  private static string _ConfigLocation = "Assets/Resources/Realyz_Data/Config_Mobilyz";
  private static Maths4Unity3D.Matrix3x3 matrix_trackingSoftware2Unity3D;
  private static Maths4Unity3D.Matrix3x3 rotation_matrix_trackingSoftware2Unity3D;
  private static bool rotation_matrix_trackingSoftware2Unity3D_isNotDefined;
*/
  public static Vector3 GetPositionFromTrackingSpaceToBaseMobilyzSpace(float posX_trackingSoftware, float posY_trackingSoftware, float posZ_trackingSoftware)
  {
		/*
    if (TrackingSwapCalculations.matrix_trackingSoftware2Unity3D == null || TrackingSwapCalculations.matrix_trackingSoftware2Unity3D == Maths4Unity3D.Matrix3x3.Zero())
      TrackingSwapCalculations.init_matrix_trackingSoftware2Unity3D();
    if (TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined)
      return new Vector3(posX_trackingSoftware, posY_trackingSoftware, posZ_trackingSoftware);
    return TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.MultiplyVector(new Vector3(posX_trackingSoftware, posY_trackingSoftware, posZ_trackingSoftware));
    */
		return new Vector3 (posX_trackingSoftware, posY_trackingSoftware, posZ_trackingSoftware);
  }

  public static Quaternion GetRotationFromTrackingSpaceToBaseMobilyzSpace(float quatX_trackingSoftware, float quatY_trackingSoftware, float quatZ_trackingSoftware, float quatW_trackingSoftware)
  {
		/*
    if (TrackingSwapCalculations.matrix_trackingSoftware2Unity3D == null || TrackingSwapCalculations.matrix_trackingSoftware2Unity3D == Maths4Unity3D.Matrix3x3.Zero())
      TrackingSwapCalculations.init_matrix_trackingSoftware2Unity3D();
    if (TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D == null)
      TrackingSwapCalculations.init_rotation_matrix_trackingSoftware2Unity3D();
    if (TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined)
      return new Quaternion(quatX_trackingSoftware, quatY_trackingSoftware, quatZ_trackingSoftware, quatW_trackingSoftware);
    Maths4Unity3D.Matrix3x3 matrix3x3 = Maths4Unity3D.Matrix3x3FromQuaternion(new Quaternion(quatX_trackingSoftware, quatY_trackingSoftware, quatZ_trackingSoftware, quatW_trackingSoftware));
    return Maths4Unity3D.QuaternionFromMatrix(TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D * matrix3x3 * TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D.Transpose());
    */
		return new Quaternion (quatX_trackingSoftware, quatY_trackingSoftware, quatZ_trackingSoftware, quatW_trackingSoftware);
  }
	/*
  public static void init_matrix_trackingSoftware2Unity3D()
  {
    TrackingSwapCalculations.Read_Tracking_Matrix_Config_File();
  }

  private static void Create_Standard_Tracking_Matrix()
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode xmlDeclaration = (XmlNode) xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", (string) null);
    xmlDocument.AppendChild(xmlDeclaration);
    XmlElement element1 = xmlDocument.CreateElement("Tracking_Matrix");
    xmlDocument.AppendChild((XmlNode) element1);
    XmlElement element2 = xmlDocument.CreateElement("LigneA");
    element2.InnerText = new Vector3(1f, 0.0f, 0.0f).ToString();
    element1.AppendChild((XmlNode) element2);
    XmlElement element3 = xmlDocument.CreateElement("LigneB");
    element3.InnerText = new Vector3(0.0f, 1f, 0.0f).ToString();
    element1.AppendChild((XmlNode) element3);
    XmlElement element4 = xmlDocument.CreateElement("LigneC");
    element4.InnerText = new Vector3(0.0f, 0.0f, -1f).ToString();
    element1.AppendChild((XmlNode) element4);
    xmlDocument.Save(TrackingSwapCalculations._ConfigLocation + "\\" + TrackingSwapCalculations._ConfigTrackingMatrix);
  }

  private static void Read_Tracking_Matrix_Config_File()
  {
    if (!Directory.Exists(TrackingSwapCalculations._ConfigLocation))
    {
      TrackingSwapCalculations._ConfigLocation = "Assets\\Data";
      Debug.Log((object) ("Repertoire fichier Config non valide :  Creation repertoire par default : " + TrackingSwapCalculations._ConfigLocation));
      Directory.CreateDirectory(TrackingSwapCalculations._ConfigLocation);
    }
    if (!File.Exists(TrackingSwapCalculations._ConfigLocation + "/" + TrackingSwapCalculations._ConfigTrackingMatrix))
    {
      TrackingSwapCalculations._ConfigTrackingMatrix = "Tracking_Matrix.xml";
      Debug.Log((object) ("fichier Config introuvable :  Creation config par default : " + TrackingSwapCalculations._ConfigLocation + "\\" + TrackingSwapCalculations._ConfigTrackingMatrix));
      TrackingSwapCalculations.Create_Standard_Tracking_Matrix();
    }
    TrackingSwapCalculations.Load_Tracking_Matrix_Config_File();
  }

  private static void Load_Tracking_Matrix_Config_File()
  {
    string xml = File.ReadAllText(TrackingSwapCalculations._ConfigLocation + "/" + TrackingSwapCalculations._ConfigTrackingMatrix);
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(xml);
    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Tracking_Matrix");
    TrackingSwapCalculations.matrix_trackingSoftware2Unity3D = new Maths4Unity3D.Matrix3x3(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    foreach (XmlNode xmlNode1 in elementsByTagName)
    {
      XmlNodeList childNodes = xmlNode1.ChildNodes;
      int i = 0;
      Vector3.get_zero();
      foreach (XmlNode xmlNode2 in childNodes)
      {
        Vector3 v = TrackingSwapCalculations.Vector3FromStringA(xmlNode2.InnerText);
        TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.SetRow(i, v);
        ++i;
      }
    }
  }

  private static Vector3 Vector3FromStringA(string Vector3string)
  {
    int startIndex1 = 1;
    int num1 = Vector3string.IndexOf(",");
    int num2 = num1;
    float num3;
    try
    {
      num3 = float.Parse(Vector3string.Substring(startIndex1, num1 - 1));
    }
    catch (Exception ex)
    {
      num3 = 0.0f;
      Debug.Log((object) ("Erreur lecture MatriceTracking en X : " + (object) ex));
    }
    int startIndex2 = num2 + 1;
    int num4 = Vector3string.IndexOf(",", startIndex2);
    int num5 = num4;
    float num6;
    try
    {
      num6 = float.Parse(Vector3string.Substring(startIndex2, num4 - startIndex2));
    }
    catch (Exception ex)
    {
      num6 = 0.0f;
      Debug.Log((object) ("Erreur lecture MatriceTracking en y : " + (object) ex));
    }
    int startIndex3 = num5 + 1;
    float num7;
    try
    {
      num7 = float.Parse(Vector3string.Substring(startIndex3, Vector3string.Length - 1 - startIndex3));
    }
    catch (Exception ex)
    {
      num7 = 0.0f;
      Debug.Log((object) ("Erreur lecture MatriceTracking en z : " + (object) ex));
    }
    Vector3 vector3;
    // ISSUE: explicit reference operation
    ((Vector3) @vector3).\u002Ector(num3, num6, num7);
    return vector3;
  }

  private static void init_rotation_matrix_trackingSoftware2Unity3D()
  {
    TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D = new Maths4Unity3D.Matrix3x3(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    if (!TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.isOrthogonal())
    {
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined = true;
      throw new TrackingSwapCalculations.SwapMatrixIsNotOrthogonalException("La matrice permettant de passer du logiciel de tracking à Unity3D n'est pas orthogonale.");
    }
    float num1 = Maths4Unity3D.det3(TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m00, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m01, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m02, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m10, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m11, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m12, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m20, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m21, TrackingSwapCalculations.matrix_trackingSoftware2Unity3D.m22);
    float num2 = 1f / 1000f;
    if ((double) num1 < 1.0 + (double) num2 && (double) num1 > 1.0 - (double) num2)
    {
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D = TrackingSwapCalculations.matrix_trackingSoftware2Unity3D;
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined = false;
    }
    else if ((double) num1 < (double) num2 - 1.0 && (double) num1 > -1.0 - (double) num2)
    {
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D = -TrackingSwapCalculations.matrix_trackingSoftware2Unity3D;
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined = false;
    }
    else
    {
      Debug.Log((object) ("La matrice de passage du logiciel de tracking vers Unity3D a un déterminant qui vaut " + (object) num1 + ", donc différent de +1 ou -1 : elle n'est pas orthogonale et ne représente donc pas une rotation (ni meme une anti-rotation)."));
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D = Maths4Unity3D.Matrix3x3.Identity();
      TrackingSwapCalculations.rotation_matrix_trackingSoftware2Unity3D_isNotDefined = true;
    }
  }

  public class SwapMatrixIsNotOrthogonalException : UnityException
  {
    public SwapMatrixIsNotOrthogonalException(string message = "La matrice permettant de passer du logiciel de tracking à Unity3D n'est pas orthogonale.")
    {
      base.\u002Ector(message);
    }
  }
  */
}
