// Decompiled with JetBrains decompiler
// Type: Base_Mobilyz
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

using Object = UnityEngine.Object;

public class Base_Mobilyz : MonoBehaviour {
	public GameObject Cyclop;
	public GameObject Wand;
	public GameObject Intersected_Object;
	public float Debug_Hauteur_CameraMain;
	public Vector3 Mobilyz_Size;
	[HideInInspector]
	public float Interpupillaire;
	[HideInInspector]
	public bool Stop_Navigation;
	[HideInInspector]
	public float Offset_Taille;
	private float nearClip;
	private float farClip;
	private float vitesse_Deplacement;
	private float vitesse_Rotation;
	private bool Wand_Interaction;
	private bool deactive_All_Camera;
	[HideInInspector]
	public bool Floor_Detection;
	public bool is5Face_Mobilyz;
	private string Name_Camera_Collaborative;
	private Base_Mobilyz.centreLancerDeRayonElevation centreLancerRayon;
	private float _hauteurDenjambeeMaximale;
	private float distance_intersection_Wand;
	private int compteur;
	private bool License;
	private bool time_Elapsed;
	private bool Demande_Decollage_first;
	private float time_Ask_Decollage;
	private float Horizontal;
	private float Vertical;
	[HideInInspector]
	public List<Base_Mobilyz.Screen_Realyz> Mobilyz_Screen_List;
	private float old_Interpupillaire;
	private bool CameraIsHide;
	private GameObject g_centreLancerRayon;
	private float distance;
	private Vector3 intersected_Point_Floor;
	private GameObject RayWand;
	private GameObject BouleWand;
	private Vector3 Direction;
	private string _ConfigLocation = "Assets\\Data";
	private string _ConfigName = "Mobilyz.xml";
	private string _ConfigAppliLocation;
	private string _ConfigAppli = "Application.xml";
	private string _ConfigTracking;
	private XmlDocument xml;
	private float Length_Ray_Intersection;
	private float mHdg;
	private float mPitch;
	private float Wand_Interaction_Radius;
	public GameObject CollaborativeView;
	public GameObject Menu_Realyz;

	public float hauteurDenjambeeMaximale {
		get {
			return this._hauteurDenjambeeMaximale;
		}
	}

	public void Awake() {
        _ConfigAppliLocation = Application.dataPath + "/..";

		bool flag = false;
		this.Mobilyz_Screen_List = new List<Base_Mobilyz.Screen_Realyz>();
		this.License = true;
		if (this.License)
			this.Read_Config_File();
		this.Read_Application_Config_File();
		if (this.License) {
			this.Init();
		} else {
			if (!flag)
				Debug.Log((object)"License Error : Standard visualisation active");
			this.Init();
		}
	}

	private void Init() {
		if (Cyclop == null) this.Cyclop = new GameObject("Cyclop");
		if ((Object)((Component)this).transform.FindChild("Wand") != (Object)null)
			this.Wand = ((Component)((Component)this).transform.FindChild("Wand")).gameObject;
		if ((Object)this.Wand == (Object)null) {
			this.Wand = (GameObject)Object.Instantiate<GameObject>((Resources.Load("Realyz_Data/Data/Wand") as GameObject));
			((Object)this.Wand).name = ("Wand");
		}
		if ((Object)this.Wand == (Object)null)
			Debug.Log((object)"Wand was not find !");
		//this.RayWand = ((Component)this.Wand.transform.FindChild("Ray")).gameObject;
		//Bounds bounds = ((Renderer)((Component)this.RayWand.transform).GetComponent<Renderer>()).bounds;
		//this.Wand_Interaction_Radius = (float)((Bounds)bounds).size.y;
		//this.BouleWand = ((Component)this.RayWand.transform.FindChild("Boule")).gameObject;
		try {
			if (this.Wand.tag == "Floor")
				this.Wand.tag = ("Untagged");
		} catch (Exception ex) {
			Debug.Log((object)ex);
			Debug.Log((object)"Tag Floor is not defined, Floor Manager won't works !");
		}
		if (this.deactive_All_Camera) {
			foreach (Camera allCamera in Camera.allCameras) {
				((Behaviour)allCamera).enabled = (false);
				if ((Object)((Component)allCamera).gameObject.GetComponent<AudioListener>() != (Object)null)
					((Behaviour)((Component)allCamera).gameObject.GetComponent<AudioListener>()).enabled = (false);
			}
		} else {
			foreach (Camera allCamera in Camera.allCameras) {
				if ((double)allCamera.depth > 0.0)
					allCamera.depth = (-1f);
				if ((Object)((Component)allCamera).gameObject.GetComponent<AudioListener>() != (Object)null)
					((Behaviour)((Component)allCamera).gameObject.GetComponent<AudioListener>()).enabled = (false);
			}
		}
		if (!this.License) {
			this.Cyclop.transform.parent = (((Component)this).transform);
			this.Cyclop.transform.localPosition = (new Vector3(0.0f, this.Debug_Hauteur_CameraMain, 0.0f));
			this.Cyclop.transform.localRotation = (Quaternion.identity);
			this.CollaborativeView = GameObject.Find(this.Name_Camera_Collaborative);
			if (this.Name_Camera_Collaborative != "CollaborativeView" && (Object)this.CollaborativeView == (Object)null) {
				Debug.LogError((object)"Error : L'Objet renseigné comme camera collaborative n'existe pas dans la scene.");
				Debug.Log((object)"Creation Camera Standard.");
			}
			if ((Object)this.CollaborativeView != (Object)null && (Object)this.CollaborativeView.GetComponent<Camera>() == (Object)null) {
				Debug.LogError((object)"Error : L'Objet renseigné comme camera collaborative n'est pas une Camera.");
				Debug.Log((object)"Creation Camera Standard.");
				this.CollaborativeView = (GameObject)null;
			}
			if ((Object)this.CollaborativeView == (Object)null) {
				this.CollaborativeView = new GameObject("CollaborativeView");
				//this.CollaborativeView.AddComponent<CAO_Camera>();
				Camera camera = (Camera)this.CollaborativeView.AddComponent<Camera>();
				this.CollaborativeView.transform.position = (((Component)this).gameObject.transform.position + new Vector3(0.0f, 1f, 0.0f));
				this.CollaborativeView.transform.rotation = (((Component)this).gameObject.transform.rotation);
				camera.nearClipPlane = (0.01f);
				camera.farClipPlane = (500f);
				camera.fieldOfView = (40f);
			}
		  ((Camera)this.CollaborativeView.GetComponent<Camera>()).cullingMask = (~(1 << LayerMask.NameToLayer("Mobilyz_View")));
            /*
			this.CollaborativeView.AddComponent<Fonction_Collaborative>();
			if ((Object)this.CollaborativeView.GetComponent<Camera_Dispo_Interface>() == (Object)null)
				this.CollaborativeView.AddComponent<Camera_Dispo_Interface>();
                */
			this.CollaborativeView.tag = ("Realyz_Element");
			this.Menu_Realyz = (GameObject)Object.Instantiate<GameObject>((Resources.Load("Realyz_Data/Data/Menu/Interface_5Ecran") as GameObject));
			this.Menu_Realyz.SetActive(true);
		} else {
			this.Wand.transform.position = (((Component)this).transform.position);
			this.Wand.transform.rotation = (((Component)this).transform.rotation);
			this.Cyclop.transform.position = (((Component)this).transform.position + ((Component)this).transform.TransformDirection(Vector3.up));
			this.Cyclop.transform.rotation = (((Component)this).transform.rotation);
			this.Cyclop.transform.parent = (((Component)this).transform);
			this.Cyclop.AddComponent<AudioListener>();
			this.Create_Projection_System_Config();
			Cursor.visible = (false);
			

			this.Stop_Navigation = false;
			if (this.is5Face_Mobilyz) {
				Camera camera = (Camera)null;
				this.CollaborativeView = GameObject.Find(this.Name_Camera_Collaborative);
				if (this.Name_Camera_Collaborative != "CollaborativeView" && (Object)this.CollaborativeView == (Object)null) {
					Debug.LogError((object)"Error : L'Objet renseigné comme camera collaborative n'existe pas dans la scene.");
					Debug.Log((object)"Creation Camera Standard.");
				}
				if ((Object)this.CollaborativeView != (Object)null) {
					camera = (Camera)this.CollaborativeView.GetComponent<Camera>();
					if ((Object)camera == (Object)null) {
						Debug.LogError((object)"Error : L'Objet renseigné comme camera collaborative n'est pas une Camera.");
						Debug.Log((object)"Creation Camera Standard.");
						this.CollaborativeView = (GameObject)null;
					} else
						camera.depth = (10f);
				}
				if ((Object)this.CollaborativeView == (Object)null) {
					this.CollaborativeView = new GameObject("CollaborativeView");
					this.CollaborativeView.transform.position = (((Component)this).gameObject.transform.position + new Vector3(0.0f, 1f, 0.0f));
					this.CollaborativeView.transform.rotation = (((Component)this).gameObject.transform.rotation);
					//this.CollaborativeView.AddComponent<CAO_Camera>();
					camera = (Camera)this.CollaborativeView.AddComponent<Camera>();
					camera.nearClipPlane = (0.01f);
					camera.farClipPlane = (50f);
					camera.fieldOfView = (40f);
				} else
					((Behaviour)camera).enabled = (true);
				camera.depth = (10f);
				camera.cullingMask = (~(1 << LayerMask.NameToLayer("Mobilyz_View")));
                /*
				this.CollaborativeView.AddComponent<Fonction_Collaborative>();
				if ((Object)this.CollaborativeView.GetComponent<Camera_Dispo_Interface>() == (Object)null)
					this.CollaborativeView.AddComponent<Camera_Dispo_Interface>();
                    */
				this.CollaborativeView.tag = ("Realyz_Element");
                /*
				((Component)this).gameObject.AddComponent<Size_Object>();
				this.Mobilyz_Size = ((Size_Object)((Component)this).gameObject.GetComponent<Size_Object>()).Affiche_Size();
                */
				camera.rect = (new Rect((float)this.Mobilyz_Screen_List.Count / ((float)this.Mobilyz_Screen_List.Count + 1f), 0.0f, (float)(1.0 / ((double)this.Mobilyz_Screen_List.Count + 1.0)), 1f));
				this.Menu_Realyz = (GameObject)Object.Instantiate<GameObject>((Resources.Load("Realyz_Data/Data/Menu/Interface_5Ecran") as GameObject));
				this.Menu_Realyz.SetActive(true);
			}
		}
		this.g_centreLancerRayon = this.centreLancerRayon != Base_Mobilyz.centreLancerDeRayonElevation.Lunettes ? (this.centreLancerRayon != Base_Mobilyz.centreLancerDeRayonElevation.centreSystemeImmersif ? (GameObject)null : ((Component)this).gameObject) : this.Cyclop;
		/*if ((Object)this.g_centreLancerRayon != (Object)null)
			this.Add_Floor_Collider();*/
		this.time_Elapsed = false;
		this.Demande_Decollage_first = true;
		this.old_Interpupillaire = this.Interpupillaire;
	}

	private void Create_Projection_System_Config() {
		//Base_Mobilyz.Screen_Realyz screenRealyz = new Base_Mobilyz.Screen_Realyz();
		Material mat = new Material(Shader.Find("Diffuse"));
		//mat.mainTexture = (Resources.Load("Realyz_Data/Textures/logo-realyz") as Texture);
		mat.color = (new Color(1f, 1f, 1f, 0.33f));
		if (this.is5Face_Mobilyz) {
			for (int index = 0; index < this.Mobilyz_Screen_List.Count; ++index) {
				Base_Mobilyz.Screen_Realyz mobilyzScreen = this.Mobilyz_Screen_List[index];
				mobilyzScreen.Viewport_Size.x = (float)(mobilyzScreen.Viewport_Size.x * (double)this.Mobilyz_Screen_List.Count / (double)(this.Mobilyz_Screen_List.Count + 1));
				mobilyzScreen.Viewport_Position.x = (float)(mobilyzScreen.Viewport_Position.x * (double)this.Mobilyz_Screen_List.Count / (double)(this.Mobilyz_Screen_List.Count + 1));
			}
		}
		for (int index = 0; index < this.Mobilyz_Screen_List.Count; ++index) {
			Base_Mobilyz.Screen_Realyz mobilyzScreen = this.Mobilyz_Screen_List[index];
			Vector3 Scale = new Vector3((float)(mobilyzScreen.Size.x / 10.0), 1f, (float)(mobilyzScreen.Size.y / 10.0));
			Vector3 Pos = (((((Component)this).transform.position + ((Component)this).transform.TransformDirection(((float)mobilyzScreen.Position.x * Vector3.right))) + ((Component)this).transform.TransformDirection(((float)mobilyzScreen.Position.y * Vector3.up))) + ((Component)this).transform.TransformDirection(((float)mobilyzScreen.Position.z * Vector3.forward)));
			mobilyzScreen.Ecran = this.Create_Dalle(mobilyzScreen.Name, Scale, Pos, mobilyzScreen.Orientation, ((Component)this).transform, mat);
			mobilyzScreen.Ecran.transform.Rotate(Vector3.right * -90f, (Space)1);
			mobilyzScreen.Ecran.transform.Rotate(Vector3.up * 180f, (Space)1);
			this.Create_Camera(mobilyzScreen);
		}
	}

	private void Rotate_Ecran(GameObject ecran, Quaternion Rotation) {
		GameObject gameObject = new GameObject();
		gameObject.transform.position = (((Component)this).transform.position);
		gameObject.transform.rotation = (((Component)this).transform.rotation);
		ecran.transform.parent = (gameObject.transform);
		gameObject.transform.Rotate(((Vector3.right * (float)Rotation.x) * (float)Rotation.w), (Space)1);
		ecran.transform.parent = ((Transform)null);
		gameObject.transform.position = (((Component)this).transform.position);
		gameObject.transform.rotation = (((Component)this).transform.rotation);
		ecran.transform.parent = (gameObject.transform);
		gameObject.transform.Rotate(((Vector3.up * (float)Rotation.y) * (float)Rotation.w), (Space)1);
		ecran.transform.parent = ((Transform)null);
		gameObject.transform.position = (((Component)this).transform.position);
		gameObject.transform.rotation = (((Component)this).transform.rotation);
		ecran.transform.parent = (gameObject.transform);
		gameObject.transform.Rotate(((Vector3.forward * (float)Rotation.z) * (float)Rotation.w), (Space)1);
		ecran.transform.parent = ((Transform)null);
		Object.DestroyImmediate((Object)gameObject);
	}

	private GameObject Create_Dalle(string name, Vector3 Scale, Vector3 Pos, Quaternion Rot, Transform pere, Material mat) {
		GameObject primitive = GameObject.CreatePrimitive((PrimitiveType)4);
		if (this.CameraIsHide)
			((Object)primitive).hideFlags = ((HideFlags)61);
		((Object)primitive).name = (name);
		primitive.transform.rotation = (((Component)this).transform.rotation);
		primitive.transform.position = (((Component)this).transform.position);
		this.Rotate_Ecran(primitive, Rot);
		primitive.transform.position = (Pos);
		primitive.transform.localScale = (Scale);
		primitive.transform.parent = (pere);
		((Renderer)primitive.GetComponent<Renderer>()).material = (mat);
		((Renderer)primitive.GetComponent<Renderer>()).enabled = (false);
		Object.DestroyImmediate((Object)primitive.GetComponent<Collider>());
		return primitive;
	}

	private void Create_Camera(Base_Mobilyz.Screen_Realyz Screen_realyz) {
		Transform transform_aux = new GameObject().transform;
		if (this.CameraIsHide)
            ((Object)transform_aux).hideFlags = ((HideFlags)61);
        ((Object)transform_aux).name = ("Pere_Cam " + Screen_realyz.Name);
        transform_aux.position = (this.Cyclop.transform.position);
        transform_aux.parent = (this.Cyclop.transform);
        transform_aux.rotation = (Quaternion.LookRotation(Screen_realyz.Ecran.transform.position - transform_aux.position, Vector3.up));
		for (int index = 0; index < 2; ++index) {
			int num1 = -1;
			int num2 = 1;
			int num3 = 0;
			string str = ") left";
			if (index == 1) {
				num1 = 1;
				num3 = 1;
				str = ") right";
			}
			GameObject gameObject = new GameObject("Camera (" + Screen_realyz.Name + str);
			if (this.CameraIsHide)
				((Object)gameObject).hideFlags = ((HideFlags)61);
			Camera camera = (Camera)gameObject.AddComponent<Camera>();
			if (Screen_realyz.Invert_Stereo)
				num2 = -1;
			camera.cullingMask = (~(1 << LayerMask.NameToLayer("Collaborative_View")));
			gameObject.transform.position = ((this.Cyclop.transform.position + ((this.Cyclop.transform.TransformDirection(((this.Interpupillaire / 2f) * Vector3.right)) * (float)num1) * (float)num2)));
            gameObject.transform.rotation = (transform_aux.rotation);
            gameObject.transform.parent = (transform_aux);
			camera.nearClipPlane = (this.nearClip);
			camera.farClipPlane = (this.farClip);
			((Realyz_Camera)gameObject.AddComponent<Realyz_Camera>()).myScreen = Screen_realyz.Ecran;
			if (index == 1)
				gameObject.AddComponent<DrawBufferRight>();
			else
				gameObject.AddComponent<DrawBufferLeft>();
			((Camera)gameObject.GetComponent<Camera>()).nearClipPlane = (this.nearClip);
			((Camera)gameObject.GetComponent<Camera>()).farClipPlane = (this.farClip);
			((Camera)gameObject.GetComponent<Camera>()).depth = ((float)num3);
			((Camera)gameObject.GetComponent<Camera>()).rect = (new Rect((float)Screen_realyz.Viewport_Position.x, (float)Screen_realyz.Viewport_Position.y, (float)Screen_realyz.Viewport_Size.x, (float)Screen_realyz.Viewport_Size.y));
			if (num1 == -1)
				Screen_realyz.Left_Camera = gameObject;
			else
				Screen_realyz.Right_Camera = gameObject;
		}
		++this.compteur;
	}

	private void Read_Config_File() {
		if (!Directory.Exists(this._ConfigLocation)) {
			this._ConfigLocation = "Assets\\Data";
			Debug.Log((object)("Repertoire fichier Config non valide :  Creation repertoire par default : " + this._ConfigLocation));
			Directory.CreateDirectory(this._ConfigLocation);
		}
		if (!File.Exists(this._ConfigLocation + "/" + this._ConfigName)) {
			this._ConfigName = "Mobilyz.xml";
			Debug.Log((object)("fichier Config introuvable :  Creation config par default : " + this._ConfigLocation + "\\" + this._ConfigName));
			this.Create_Standard_Mobilyz_Base();
		}
		this.Load_Config_File();
	}

	private void Read_Application_Config_File() {
		if (!Directory.Exists(this._ConfigAppliLocation)) {
            this._ConfigAppliLocation = Application.dataPath + "/.."; // Application.dataPath;
			Debug.Log((object)("Repertoire fichier Config non valide :  Creation repertoire par default : " + this._ConfigAppliLocation));
			Directory.CreateDirectory(this._ConfigAppliLocation);
		}
		if (!File.Exists(this._ConfigAppliLocation + "/" + this._ConfigAppli)) {
			this._ConfigAppli = "Application.xml";
			Debug.Log((object)("fichier Config Application introuvable :  Creation config par default : " + this._ConfigAppliLocation + "\\" + this._ConfigAppli));
			this.Create_Standard_Application_Base();
		}
		this.Load_Application_Config_File();
	}

	private void changement_Interpupillaire() {
		foreach (Base_Mobilyz.Screen_Realyz mobilyzScreen in this.Mobilyz_Screen_List) {
			GameObject leftCamera = mobilyzScreen.Left_Camera;
			GameObject rightCamera = mobilyzScreen.Right_Camera;
			int num = !mobilyzScreen.Invert_Stereo ? 1 : -1;
			leftCamera.transform.position = ((this.Cyclop.transform.position - ((this.Cyclop.transform.TransformDirection(((this.Interpupillaire / 2f) * Vector3.right))) * (float)num)));
			rightCamera.transform.position = ((this.Cyclop.transform.position + (this.Cyclop.transform.TransformDirection(((this.Interpupillaire / 2f) * Vector3.right)) * (float)num)));
		}
	}

	private void Update() {
		if (this.License) {
			if ((double)this.old_Interpupillaire != (double)this.Interpupillaire) {
				this.changement_Interpupillaire();
				this.old_Interpupillaire = this.Interpupillaire;
			}
		}
	}

	private void Create_Standard_Mobilyz_Base() {
		this.xml = new XmlDocument();
		this.xml.AppendChild((XmlNode)this.xml.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
		XmlElement element1 = this.xml.CreateElement("Configuration_Mobilyz");
		this.xml.AppendChild((XmlNode)element1);
		XmlElement element2 = this.xml.CreateElement("Screen_Settings");
		element1.AppendChild((XmlNode)element2);
		Base_Mobilyz.Screen_Realyz screenRealyz = new Base_Mobilyz.Screen_Realyz();
		for (int index = 0; index < 4; ++index) {
			switch (index) {
				case 0:
					screenRealyz.Name = "Left";
					screenRealyz.Size = new Vector3(2f, 2f, 0.0f);
					screenRealyz.Position = new Vector3(-1.1f, 1f, 0.0f);
					screenRealyz.Orientation = new Quaternion(0.0f, 1f, 0.0f, -90f);
					screenRealyz.Viewport_Position = new Vector2(0.0f, 0.0f);
					screenRealyz.Viewport_Size = new Vector2(0.25f, 1f);
					screenRealyz.Invert_Stereo = false;
					break;
				case 1:
					screenRealyz.Name = "Front";
					screenRealyz.Size = new Vector3(2.2f, 2f, 0.0f);
					screenRealyz.Position = new Vector3(0.0f, 1f, 1f);
					screenRealyz.Orientation = new Quaternion(0.0f, 0.0f, 0.0f, 90f);
					screenRealyz.Viewport_Position = new Vector2(0.25f, 0.0f);
					screenRealyz.Viewport_Size = new Vector2(0.25f, 1f);
					screenRealyz.Invert_Stereo = false;
					break;
				case 2:
					screenRealyz.Name = "Right";
					screenRealyz.Size = new Vector3(2f, 2f, 0.0f);
					screenRealyz.Position = new Vector3(1.1f, 1f, 0.0f);
					screenRealyz.Orientation = new Quaternion(0.0f, 1f, 0.0f, 90f);
					screenRealyz.Viewport_Position = new Vector2(0.5f, 0.0f);
					screenRealyz.Viewport_Size = new Vector2(0.25f, 1f);
					screenRealyz.Invert_Stereo = false;
					break;
				case 3:
					screenRealyz.Name = "Bottom";
					screenRealyz.Size = new Vector3(2.2f, 2f, 0.0f);
					screenRealyz.Position = new Vector3(0.0f, 0.0f, 0.0f);
					screenRealyz.Orientation = new Quaternion(1f, 0.0f, 0.0f, 90f);
					screenRealyz.Viewport_Position = new Vector2(0.75f, 0.0f);
					screenRealyz.Viewport_Size = new Vector2(0.25f, 1f);
					screenRealyz.Invert_Stereo = true;
					break;
			}
			screenRealyz.Left_Camera = (GameObject)null;
			screenRealyz.Right_Camera = (GameObject)null;
			screenRealyz.Ecran = (GameObject)null;
			XmlElement element3 = this.xml.CreateElement("Screen_" + (object)index);
			element2.AppendChild((XmlNode)element3);
			XmlElement element4 = this.xml.CreateElement("Screen_Name");
			element4.InnerText = screenRealyz.Name;
			element3.AppendChild((XmlNode)element4);
			XmlElement element5 = this.xml.CreateElement("Size_Screen");
			element3.AppendChild((XmlNode)element5);
			XmlElement element6 = this.xml.CreateElement("Width");
			// ISSUE: explicit non-virtual call
			element6.InnerText = (screenRealyz.Size.x.ToString());
			element5.AppendChild((XmlNode)element6);
			XmlElement element7 = this.xml.CreateElement("Height");
			// ISSUE: explicit non-virtual call
			element7.InnerText = (screenRealyz.Size.y.ToString());
			element5.AppendChild((XmlNode)element7);
			XmlElement element8 = this.xml.CreateElement("Position_Screen");
			element3.AppendChild((XmlNode)element8);
			XmlElement element9 = this.xml.CreateElement("X");
			// ISSUE: explicit non-virtual call
			element9.InnerText = (screenRealyz.Position.x.ToString());
			element8.AppendChild((XmlNode)element9);
			XmlElement element10 = this.xml.CreateElement("Y");
			// ISSUE: explicit non-virtual call
			element10.InnerText = (screenRealyz.Position.y.ToString());
			element8.AppendChild((XmlNode)element10);
			XmlElement element11 = this.xml.CreateElement("Z");
			// ISSUE: explicit non-virtual call
			element11.InnerText = (screenRealyz.Position.z.ToString());
			element8.AppendChild((XmlNode)element11);
			XmlElement element12 = this.xml.CreateElement("Orientation_Screen");
			element3.AppendChild((XmlNode)element12);
			XmlElement element13 = this.xml.CreateElement("X");
			// ISSUE: explicit non-virtual call
			element13.InnerText = (screenRealyz.Orientation.x.ToString());
			element12.AppendChild((XmlNode)element13);
			XmlElement element14 = this.xml.CreateElement("Y");
			// ISSUE: explicit non-virtual call
			element14.InnerText = (screenRealyz.Orientation.y.ToString());
			element12.AppendChild((XmlNode)element14);
			XmlElement element15 = this.xml.CreateElement("Z");
			// ISSUE: explicit non-virtual call
			element15.InnerText = (screenRealyz.Orientation.z.ToString());
			element12.AppendChild((XmlNode)element15);
			XmlElement element16 = this.xml.CreateElement("Angle");
			// ISSUE: explicit non-virtual call
			element16.InnerText = (screenRealyz.Orientation.w.ToString());
			element12.AppendChild((XmlNode)element16);
			XmlElement element17 = this.xml.CreateElement("Viewport_Position");
			element3.AppendChild((XmlNode)element17);
			XmlElement element18 = this.xml.CreateElement("X");
			// ISSUE: explicit non-virtual call
			element18.InnerText = (screenRealyz.Viewport_Position.x.ToString());
			element17.AppendChild((XmlNode)element18);
			XmlElement element19 = this.xml.CreateElement("Y");
			// ISSUE: explicit non-virtual call
			element19.InnerText = (screenRealyz.Viewport_Position.y.ToString());
			element17.AppendChild((XmlNode)element19);
			XmlElement element20 = this.xml.CreateElement("Viewport_Size");
			element3.AppendChild((XmlNode)element20);
			XmlElement element21 = this.xml.CreateElement("X");
			// ISSUE: explicit non-virtual call
			element21.InnerText = (screenRealyz.Viewport_Size.x.ToString());
			element20.AppendChild((XmlNode)element21);
			XmlElement element22 = this.xml.CreateElement("Y");
			// ISSUE: explicit non-virtual call
			element22.InnerText = (screenRealyz.Viewport_Size.y.ToString());
			element20.AppendChild((XmlNode)element22);
			XmlElement element23 = this.xml.CreateElement("Invert_Stereo");
			element3.AppendChild((XmlNode)element23);
			element23.InnerText = screenRealyz.Invert_Stereo.ToString();
		}
		XmlElement element24 = this.xml.CreateElement("Global_Settings");
		element1.AppendChild((XmlNode)element24);
		XmlElement element25 = this.xml.CreateElement("Collaborative_Screen");
		element24.AppendChild((XmlNode)element25);
		XmlElement element26 = this.xml.CreateElement("is5Ecran");
		element25.AppendChild((XmlNode)element26);
		element26.InnerText = this.is5Face_Mobilyz.ToString();
		this.xml.Save(this._ConfigLocation + "\\" + this._ConfigName);
	}

	private void Create_Standard_Application_Base() {
		this.xml = new XmlDocument();
		this.xml.AppendChild((XmlNode)this.xml.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
		XmlElement element1 = this.xml.CreateElement("Configuration_Application");
		this.xml.AppendChild((XmlNode)element1);
		XmlElement element2 = this.xml.CreateElement("Global_Settings");
		element1.AppendChild((XmlNode)element2);
		XmlElement element3 = this.xml.CreateElement("Collaborative_Screen");
		element2.AppendChild((XmlNode)element3);
		XmlElement element4 = this.xml.CreateElement("Name_Camera");
		element3.AppendChild((XmlNode)element4);
		element4.InnerText = this.Name_Camera_Collaborative.ToString();
		XmlElement element5 = this.xml.CreateElement("Desactive_All_Camera");
		element2.AppendChild((XmlNode)element5);
		element5.InnerText = this.deactive_All_Camera.ToString();
		XmlElement element6 = this.xml.CreateElement("Interpupillaire");
		element2.AppendChild((XmlNode)element6);
		element6.InnerText = this.Interpupillaire.ToString();
		XmlElement element7 = this.xml.CreateElement("Clipping_Plane");
		element2.AppendChild((XmlNode)element7);
		XmlElement element8 = this.xml.CreateElement("Near_Plane");
		element7.AppendChild((XmlNode)element8);
		element8.InnerText = this.nearClip.ToString();
		XmlElement element9 = this.xml.CreateElement("Far_Plane");
		element7.AppendChild((XmlNode)element9);
		element9.InnerText = this.farClip.ToString();
		XmlElement element10 = this.xml.CreateElement("Speed_Movement");
		element2.AppendChild((XmlNode)element10);
		XmlElement element11 = this.xml.CreateElement("Translation_Speed");
		element10.AppendChild((XmlNode)element11);
		element11.InnerText = this.vitesse_Deplacement.ToString();
		XmlElement element12 = this.xml.CreateElement("Rotation_Speed");
		element10.AppendChild((XmlNode)element12);
		element12.InnerText = this.vitesse_Rotation.ToString();
		XmlElement element13 = this.xml.CreateElement("Offset_Height");
		element2.AppendChild((XmlNode)element13);
		element13.InnerText = this.Offset_Taille.ToString();
		XmlElement element14 = this.xml.CreateElement("Floor_Detection_Height");
		element2.AppendChild((XmlNode)element14);
		element14.InnerText = this._hauteurDenjambeeMaximale.ToString();
		XmlElement element15 = this.xml.CreateElement("Center_Floor_Detection");
		element2.AppendChild((XmlNode)element15);
		element15.InnerText = 2.ToString();
		XmlElement element16 = this.xml.CreateElement("Wand_is_Interactif");
		element2.AppendChild((XmlNode)element16);
		element16.InnerText = this.Wand_Interaction.ToString();
		XmlElement element17 = this.xml.CreateElement("Length_Ray_Intersection");
		element2.AppendChild((XmlNode)element17);
		element17.InnerText = 1000.ToString();
		this.xml.Save(this._ConfigAppliLocation + "\\" + this._ConfigAppli);
	}

	private void Create_Standard_Tracking_Base() {
		this.xml = new XmlDocument();
		this.xml.AppendChild((XmlNode)this.xml.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
		XmlElement element1 = this.xml.CreateElement("Configuration_Tracking");
		this.xml.AppendChild((XmlNode)element1);
		XmlElement element2 = this.xml.CreateElement("Tracker");
		element1.AppendChild((XmlNode)element2);
		XmlElement element3 = this.xml.CreateElement("Nom_VRPN");
		element3.InnerText = "Lunetteslocalhost";
		element2.AppendChild((XmlNode)element3);
		XmlElement element4 = this.xml.CreateElement("Nom_Objet");
		element4.InnerText = "Cyclop";
		element2.AppendChild((XmlNode)element4);
		XmlElement element5 = this.xml.CreateElement("Tracker");
		element1.AppendChild((XmlNode)element5);
		XmlElement element6 = this.xml.CreateElement("Nom_VRPN");
		element6.InnerText = "Wandlocalhost";
		element5.AppendChild((XmlNode)element6);
		XmlElement element7 = this.xml.CreateElement("Nom_Objet");
		element7.InnerText = "Wand";
		element5.AppendChild((XmlNode)element7);
		this.xml.Save(this._ConfigAppliLocation + "\\" + this._ConfigTracking);
	}

	private void Load_Config_File() {
		string path = this._ConfigLocation + "\\" + this._ConfigName;
		Debug.Log((object)path);
		string xml = File.ReadAllText(path);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("Global_Settings")) {
			foreach (XmlNode childNode in xmlNode.ChildNodes) {
				switch (childNode.Name) {
					case "Collaborative_Screen":
						IEnumerator enumerator = childNode.ChildNodes.GetEnumerator();
						try {
							while (enumerator.MoveNext()) {
								XmlNode current = (XmlNode)enumerator.Current;
								switch (current.Name) {
									case "is5Ecran":
										try {
											this.is5Face_Mobilyz = bool.Parse(current.InnerText);
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.is5Face_Mobilyz = false;
											Debug.Log((object)("Probleme lecture données + Collaborative_Screen_is5Face=> raz value : " + (object)this.is5Face_Mobilyz));
											continue;
										}
									case "Name_Camera":
										try {
											this.Name_Camera_Collaborative = current.InnerText;
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.Name_Camera_Collaborative = "CollaborativeView";
											Debug.Log((object)("Probleme lecture données + Collaborative_Screen_Camera_Name=> raz value : " + this.Name_Camera_Collaborative));
											continue;
										}
									default:
										continue;
								}
							}
							continue;
						} finally {
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
								disposable.Dispose();
						}
					default:
						continue;
				}
			}
		}
		foreach (XmlNode xmlNode1 in xmlDocument.GetElementsByTagName("Screen_Settings")) {
			foreach (XmlNode childNode1 in xmlNode1.ChildNodes) {
				Base_Mobilyz.Screen_Realyz screenRealyz = new Base_Mobilyz.Screen_Realyz();
				foreach (XmlNode childNode2 in childNode1.ChildNodes) {
					switch (childNode2.Name) {
						case "Screen_Name":
							screenRealyz.Name = childNode2.InnerText;
							continue;
						case "Size_Screen":
							XmlNodeList childNodes1 = childNode2.ChildNodes;
							Vector3 zero1 = Vector3.zero;
							IEnumerator enumerator1 = childNodes1.GetEnumerator();
							try {
								while (enumerator1.MoveNext()) {
									XmlNode current = (XmlNode)enumerator1.Current;
									switch (current.Name) {
										case "Width":
											try {
												zero1.x = float.Parse(current.InnerText);
												break;
											} catch (Exception ex) {
												Debug.Log((object)ex);
												zero1.x = 2.0f;
												Debug.Log((object)("Erreur Lecture Size_Screen.x sur :" + childNode2.Name + " valeur default = " + (object)(float)zero1.x));
												break;
											}
										case "Height":
											try {
												zero1.y = float.Parse(current.InnerText);
												break;
											} catch (Exception ex) {
												Debug.Log((object)ex);
												zero1.y = 2.0f;
												Debug.Log((object)("Erreur Lecture Size_Screen.x sur :" + childNode2.Name + " valeur default = " + (object)(float)zero1.y));
												break;
											}
									}
									screenRealyz.Size = zero1;
								}
								continue;
							} finally {
								IDisposable disposable = enumerator1 as IDisposable;
								if (disposable != null)
									disposable.Dispose();
							}
						case "Position_Screen":
							XmlNodeList childNodes2 = childNode2.ChildNodes;
							Vector3 zero2 = Vector3.zero;
							foreach (XmlNode xmlNode2 in childNodes2) {
								switch (xmlNode2.Name) {
									case "X":
										try {
											zero2.x = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											zero2.x = 0.0f;
											Debug.Log((object)("Erreur Lecture Position_Screen.x sur :" + childNode2.Name + " valeur default = " + (object)(float)zero2.x));
											continue;
										}
									case "Y":
										try {
											zero2.y = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											zero2.y = 1.0f;
											Debug.Log((object)("Erreur Lecture Position_Screen.y sur :" + childNode2.Name + " valeur default = " + (object)(float)zero2.y));
											continue;
										}
									case "Z":
										try {
											zero2.z = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											zero2.z = 0.0f;
											Debug.Log((object)("Erreur Lecture Position_Screen.z sur :" + childNode2.Name + " valeur default = " + (object)(float)zero2.z));
											continue;
										}
									default:
										continue;
								}
							}
							screenRealyz.Position = zero2;
							continue;
						case "Orientation_Screen":
							XmlNodeList childNodes3 = childNode2.ChildNodes;
							Quaternion identity = Quaternion.identity;
							foreach (XmlNode xmlNode2 in childNodes3) {
								switch (xmlNode2.Name) {
									case "X":
										try {
											identity.x = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											identity.x = 0.0f;
											Debug.Log((object)("Erreur Lecture Rotation_Screen.x sur :" + childNode2.Name + " valeur default = " + (object)(float)identity.x));
											continue;
										}
									case "Y":
										try {
											identity.y = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											identity.y = 1.0f;
											Debug.Log((object)("Erreur Lecture Rotation_Screen.y sur :" + childNode2.Name + " valeur default = " + (object)(float)identity.y));
											continue;
										}
									case "Z":
										try {
											identity.z = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											identity.z = 0.0f;
											Debug.Log((object)("Erreur Lecture Rotation_Screen.z sur :" + childNode2.Name + " valeur default = " + (object)(float)identity.z));
											continue;
										}
									case "Angle":
										try {
											identity.w = float.Parse(xmlNode2.InnerText);
											continue;
										} catch {
											identity.w = 0.0f;
											Debug.Log((object)("Erreur Lecture Rotation_Screen.w sur :" + childNode2.Name + " valeur default = " + (object)(float)identity.w));
											continue;
										}
									default:
										continue;
								}
							}
							screenRealyz.Orientation = identity;
							continue;
						case "Viewport_Position":
							XmlNodeList childNodes4 = childNode2.ChildNodes;
							Vector2 zero3 = Vector2.zero;
							IEnumerator enumerator2 = childNodes4.GetEnumerator();
							try {
								while (enumerator2.MoveNext()) {
									XmlNode current = (XmlNode)enumerator2.Current;
									switch (current.Name) {
										case "X":
											try {
												zero3.x = float.Parse(current.InnerText);
												break;
											} catch {
												zero3.x = 0.0f;
												Debug.Log((object)("Erreur Lecture ViewPort_Position.x sur :" + childNode2.Name + " valeur default = " + (object)(float)zero3.x));
												break;
											}
										case "Y":
											try {
												zero3.y = float.Parse(current.InnerText);
												break;
											} catch {
												zero3.y = 0.0f;
												Debug.Log((object)("Erreur Lecture ViewPort_Position.y sur :" + childNode2.Name + " valeur default = " + (object)(float)zero3.y));
												break;
											}
									}
									screenRealyz.Viewport_Position = zero3;
								}
								continue;
							} finally {
								IDisposable disposable = enumerator2 as IDisposable;
								if (disposable != null)
									disposable.Dispose();
							}
						case "Viewport_Size":
							XmlNodeList childNodes5 = childNode2.ChildNodes;
							Vector2 zero4 = Vector2.zero;
							IEnumerator enumerator3 = childNodes5.GetEnumerator();
							try {
								while (enumerator3.MoveNext()) {
									XmlNode current = (XmlNode)enumerator3.Current;
									switch (current.Name) {
										case "X":
											try {
												zero4.x = float.Parse(current.InnerText);
												break;
											} catch {
												zero4.x = 0.25f;
												Debug.Log((object)("Erreur Lecture Lecture ViewPort_Size.x sur :" + childNode2.Name + " valeur default = " + (object)(float)zero4.x));
												break;
											}
										case "Y":
											try {
												zero4.y = float.Parse(current.InnerText);
												break;
											} catch {
												zero4.y = 1.0f;
												Debug.Log((object)("Erreur Lecture ViewPort_Size.y sur :" + childNode2.Name + " valeur default = " + (object)(float)zero4.y));
												break;
											}
									}
									screenRealyz.Viewport_Size = zero4;
								}
								continue;
							} finally {
								IDisposable disposable = enumerator3 as IDisposable;
								if (disposable != null)
									disposable.Dispose();
							}
						case "Invert_Stereo":
							try {
								screenRealyz.Invert_Stereo = bool.Parse(childNode2.InnerText);
								continue;
							} catch {
								screenRealyz.Invert_Stereo = false;
								Debug.Log((object)("Erreur Lecture Invert_Stereo sur :" + childNode2.Name + " valeur default = " + (object)screenRealyz.Invert_Stereo));
								continue;
							}
						default:
							continue;
					}
				}
				this.Mobilyz_Screen_List.Add(screenRealyz);
			}
		}
	}

	private void Load_Application_Config_File() {
		string path = this._ConfigAppliLocation + "\\" + this._ConfigAppli;
		Debug.Log((object)path);
		string xml = File.ReadAllText(path);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("Global_Settings")) {
			foreach (XmlNode childNode in xmlNode.ChildNodes) {
				switch (childNode.Name) {
					case "Collaborative_Screen":
						IEnumerator enumerator1 = childNode.ChildNodes.GetEnumerator();
						try {
							while (enumerator1.MoveNext()) {
								XmlNode current = (XmlNode)enumerator1.Current;
								switch (current.Name) {
									case "Name_Camera":
										try {
											this.Name_Camera_Collaborative = current.InnerText;
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.Name_Camera_Collaborative = "CollaborativeView";
											Debug.Log((object)("Probleme lecture données + Collaborative_Screen_Camera_Name=> raz value : " + this.Name_Camera_Collaborative));
											continue;
										}
									default:
										continue;
								}
							}
							continue;
						} finally {
							IDisposable disposable = enumerator1 as IDisposable;
							if (disposable != null)
								disposable.Dispose();
						}
					case "Desactive_All_Camera":
						try {
							this.deactive_All_Camera = bool.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this.deactive_All_Camera = true;
							Debug.Log((object)("Probleme lecture données + Desactive_All_Camera => raz value : " + (object)this.deactive_All_Camera));
							continue;
						}
					case "Interpupillaire":
						try {
							this.Interpupillaire = float.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this.Interpupillaire = 0.065f;
							Debug.Log((object)("Probleme lecture données + Interpupillaire => raz value : " + (object)this.Interpupillaire));
							continue;
						}
					case "Clipping_Plane":
						IEnumerator enumerator2 = childNode.ChildNodes.GetEnumerator();
						try {
							while (enumerator2.MoveNext()) {
								XmlNode current = (XmlNode)enumerator2.Current;
								switch (current.Name) {
									case "Near_Plane":
										try {
											this.nearClip = float.Parse(current.InnerText);
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.nearClip = 0.01f;
											Debug.Log((object)("Probleme lecture données + Near_Plane=> raz value : " + (object)this.nearClip));
											continue;
										}
									case "Far_Plane":
										try {
											this.farClip = float.Parse(current.InnerText);
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.farClip = 500f;
											Debug.Log((object)("Probleme lecture données + Far_Plane=> raz value : " + (object)this.farClip));
											continue;
										}
									default:
										continue;
								}
							}
							continue;
						} finally {
							IDisposable disposable = enumerator2 as IDisposable;
							if (disposable != null)
								disposable.Dispose();
						}
					case "Speed_Movement":
						IEnumerator enumerator3 = childNode.ChildNodes.GetEnumerator();
						try {
							while (enumerator3.MoveNext()) {
								XmlNode current = (XmlNode)enumerator3.Current;
								switch (current.Name) {
									case "Translation_Speed":
										try {
											this.vitesse_Deplacement = float.Parse(current.InnerText);
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.vitesse_Deplacement = 2f;
											Debug.Log((object)("Probleme lecture données + Translation_Speed raz value = " + (object)this.vitesse_Deplacement));
											continue;
										}
									case "Rotation_Speed":
										try {
											this.vitesse_Rotation = float.Parse(current.InnerText);
											continue;
										} catch (Exception ex) {
											Debug.Log((object)ex);
											this.vitesse_Rotation = 2f;
											Debug.Log((object)("Probleme lecture données + Rotation_Speed raz value = " + (object)this.vitesse_Rotation));
											continue;
										}
									default:
										continue;
								}
							}
							continue;
						} finally {
							IDisposable disposable = enumerator3 as IDisposable;
							if (disposable != null)
								disposable.Dispose();
						}
					case "Offset_Height":
						try {
							this.Offset_Taille = float.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this.Offset_Taille = 0.0f;
							Debug.Log((object)("Probleme lecture données + Offset_Height raz value = " + (object)this.Offset_Taille));
							continue;
						}
					case "Floor_Detection_Height":
						try {
							this._hauteurDenjambeeMaximale = float.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this._hauteurDenjambeeMaximale = 1f;
							Debug.Log((object)("Probleme lecture données + Floor_Detection_Height => raz value : " + (object)this._hauteurDenjambeeMaximale));
							continue;
						}
					case "Center_Floor_Detection":
						int num;
						try {
							num = int.Parse(childNode.InnerText);
						} catch (Exception ex) {
							Debug.Log((object)ex);
							num = 2;
							Debug.Log((object)("Probleme lecture données + Center_Floor_Detection => raz value : " + (object)num));
						}
						switch (num) {
							case 0:
								this.centreLancerRayon = Base_Mobilyz.centreLancerDeRayonElevation.none;
								continue;
							case 1:
								this.centreLancerRayon = Base_Mobilyz.centreLancerDeRayonElevation.centreSystemeImmersif;
								continue;
							default:
								this.centreLancerRayon = Base_Mobilyz.centreLancerDeRayonElevation.Lunettes;
								continue;
						}
					case "Wand_is_Interactif":
						try {
							this.Wand_Interaction = bool.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this.Wand_Interaction = true;
							Debug.Log((object)("Probleme lecture données + Wand_is_Interactifraz value = " + (object)this.Wand_Interaction));
							continue;
						}
					case "Length_Ray_Intersection":
						try {
							this.distance_intersection_Wand = float.Parse(childNode.InnerText);
							continue;
						} catch (Exception ex) {
							Debug.Log((object)ex);
							this.distance_intersection_Wand = 1000f;
							Debug.Log((object)("Probleme lecture données + " + (object)this.Length_Ray_Intersection + " =>raz value : " + (object)this.distance_intersection_Wand));
							continue;
						}
					default:
						continue;
				}
			}
		}
	}

	public enum centreLancerDeRayonElevation {
		none,
		centreSystemeImmersif,
		Lunettes,
	}

	public class Screen_Realyz {
		public string Name;
		public Vector3 Size;
		public Vector3 Position;
		public Quaternion Orientation;
		public Vector2 Viewport_Position;
		public Vector2 Viewport_Size;
		public bool Invert_Stereo;
		public GameObject Left_Camera;
		public GameObject Right_Camera;
		public GameObject Ecran;
	}
}
