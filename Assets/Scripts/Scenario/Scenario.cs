using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public enum Condition {
    UserNextToSelectableObject,
    ObjectSelected,
    UserNextToStationaryObject,
    ObjectReleased,
}

public class Scenario : MonoBehaviour {

    [Tooltip("The inserted object's id must correspond to the one written in the XML file")]
    public Selectable[] selectableObjects;

    private Selectable[] selectedObject;

    [Tooltip("The inserted object's id must correspond to the one written in the XML file")]
    public Stationary[] stationaryObjects;

    public TextAsset scenarioFile;

    //  Steps and substeps
    List<Step> steps;
    int currentStepId;

    //  Distance needed between a user and an object for the step to be considered as done
    public float userObjectAccomplishingDistance = 3.0f;

    //  Displayer
    public GameObject displayer;

    XmlDocument instructionsDoc;
    XmlNodeList nodesList;
    XmlNode root;

    /// <summary>
    ///     Returns true if the distance between obj1 and obj2 is lower than the indicated radius
    /// </summary>
    /// <param name="obj1">GameObject 1</param>
    /// <param name="obj2">GameObject 2</param>
    /// <param name="radius">Distance between both game objects</param>
    /// <returns></returns>
    bool CheckDistance(GameObject obj1, GameObject obj2, float radius) {
        return Vector3.Distance(obj1.transform.position, obj2.transform.position) < radius ? true : false;
    }

    public void SetSelectedObject(GameObject selectedObject) {
        foreach(Selectable selectableObject in selectableObjects) {
            if (selectableObject.associatedObject == selectedObject)
                selectableObject.IsSelected = true;
        }
    }

    public void UnsetSelectedObject(GameObject releasedObject) {
        foreach (Selectable selectableObject in selectableObjects) {
            if (selectableObject.associatedObject == releasedObject)
                selectableObject.IsSelected = false;
        }
    }

    GameObject GetSelectedObject() {
        foreach(Selectable selectableObject in selectableObjects) {
            if (selectableObject.IsSelected)
                return selectableObject.associatedObject;
        }
		return null;
    }
		
	public void MoveToNextStep() {
        if (currentStepId + 1 < steps.Count)
            currentStepId++;
        else
            Debug.Log("Reached the end of the scenario, there is no more step after the current one");
	}



    // Use this for initialization
    void Start () {

        //  Load the XML document
        instructionsDoc = new XmlDocument();
        instructionsDoc.LoadXml(scenarioFile.text);

        nodesList = instructionsDoc.SelectNodes("ScenarioManager");
        root = nodesList[0];

        //  steps initialisation
        steps = new List<Step>();

        foreach (XmlNode xnode in root.ChildNodes)
        {
            //  Selectable Objects
            if (xnode.Name == "SelectableObjects") {

                int i = 0;
                foreach (XmlNode selectableObjectXml in xnode.ChildNodes) {

                    // Id
                    selectableObjects[i].Id = Int32.Parse(selectableObjectXml.Attributes["id"].Value);
                    //  Name
                    selectableObjects[i].ObjectName = selectableObjectXml["name"].InnerText;
                    //  Description
                    selectableObjects[i].Description = selectableObjectXml["description"].InnerText;

                    i++;
                }
            }
            else if (xnode.Name == "StationaryObjects") {
                int i = 0;
                foreach (XmlNode stationaryObjectXml in xnode.ChildNodes) {
                    // Id
                    stationaryObjects[i].Id = Int32.Parse(stationaryObjectXml.Attributes["id"].Value);
                    //  Name
                    stationaryObjects[i].ObjectName = stationaryObjectXml["name"].InnerText;
                    //  Description
                    stationaryObjects[i].Description = stationaryObjectXml["description"].InnerText;
                }
            }
            else if (xnode.Name == "Scenario") {
                foreach (XmlNode stepXml in xnode.ChildNodes) {
                    Step step = new Step(Int32.Parse(stepXml.Attributes["number"].Value), stepXml.Attributes["name"].Value);
                    foreach (XmlNode subStepXml in stepXml.ChildNodes) {

                        SubStep subStep = new SubStep(
                            Int32.Parse(subStepXml.Attributes["number"].Value), 
                            subStepXml["Instruction"].InnerText, 
                            Int32.Parse(subStepXml["EndsWhen"].Attributes["userId"].Value), 
                            (Condition) Enum.Parse(typeof(Condition), subStepXml["EndsWhen"].Attributes["condition"].Value)
                            );

                        if(subStep.AccomplishmentCondition == Condition.UserNextToSelectableObject) {
                            subStep.AddOtherObjectId(Int32.Parse(subStepXml["EndsWhen"].Attributes["objectId"].Value));
                        }

                        step.AddSubStep(subStep);
                    }
                    steps.Add(step);
                }
            }
        }

        //  Set current step
        currentStepId = 0;

        //  Debug Zone
        Debug.Log(selectableObjects[0].associatedObject.name);
    }
	
	// Update is called once per frame
	void Update () {
        Step currentStep = steps[currentStepId];
        Debug.Log(currentStep.CurrentSubStep.AccomplishmentCondition);
        displayer.GetComponent<CanvasController>().SetText(currentStep.CurrentSubStep.Instruction);
        if (currentStep.CurrentSubStep.AccomplishmentCondition == Condition.UserNextToSelectableObject) {
            GameObject user = currentStep.CurrentSubStep.UserId == 0 ? GameObject.Find("Camera") : GameObject.Find("Camera"); //    Awful, TODO : manage the logic behind the users
            Debug.Log("Check distance : " + CheckDistance(user, selectableObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject, userObjectAccomplishingDistance));
            if(CheckDistance(user, selectableObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject, userObjectAccomplishingDistance)) {
                currentStep.MoveToNextSubStep();
            }
        }
		if (currentStep.CurrentSubStep.AccomplishmentCondition == Condition.UserNextToStationaryObject) {
			GameObject user = currentStep.CurrentSubStep.UserId == 0 ? GameObject.Find("Camera") : GameObject.Find("Camera"); //    Awful, TODO : manage the logic behind the users
			Debug.Log("Check distance : " + CheckDistance(user, stationaryObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject, userObjectAccomplishingDistance));
			if(CheckDistance(user, stationaryObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject, userObjectAccomplishingDistance)) {
				currentStep.MoveToNextSubStep();
			}
		}
        else if (currentStep.CurrentSubStep.AccomplishmentCondition == Condition.ObjectSelected) {
            if (GetSelectedObject() == selectableObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject) {
                currentStep.MoveToNextSubStep();
            }
        }
		else if (currentStep.CurrentSubStep.AccomplishmentCondition == Condition.ObjectReleased) {
			if (GetSelectedObject() == null) {
				currentStep.MoveToNextSubStep();
			}
		}
    }
}
