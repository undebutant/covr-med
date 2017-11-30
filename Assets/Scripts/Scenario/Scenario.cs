﻿using System.Collections;
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

    [Tooltip("The inserted object's id must correspond to the one written in the XML file")]
    public Stationary[] stationaryObjects;

    public TextAsset scenarioFile;

    //  Steps and substeps
    List<Step> steps;
    Step currentStep;

    //  Distance needed between a user and an object for the step to be considered as done
    public float userObjectAccomplishingDistance = 10.0f;

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
    bool checkDistance(GameObject obj1, GameObject obj2, float radius) {
        return Vector3.Distance(obj1.transform.position, obj2.transform.position) < radius ? true : false;
    }



    // Use this for initialization
    void Start () {

        //  Load the XML document
        instructionsDoc = new XmlDocument();
        instructionsDoc.LoadXml(scenarioFile.text);

        nodesList = instructionsDoc.SelectNodes("ScenarioManager");
        root = nodesList[0];

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
                int i = 0;
                foreach (XmlNode stepXml in xnode.ChildNodes) {
                    Step step = new Step(Int32.Parse(stepXml.Attributes["number"].Value), stepXml.Attributes["name"].Value);
                    foreach (XmlNode subStepXml in stepXml.ChildNodes) {

                        SubStep subStep = new SubStep(
                            Int32.Parse(subStepXml.Attributes["number"].Value), 
                            subStepXml["Instruction"].InnerText, 
                            Int32.Parse(subStepXml["EndsWhen"].Attributes["userId"].Value), 
                            (Condition) Enum.Parse(typeof(Condition), subStepXml["EndsWhen"].Attributes["userId"].Value)
                            );

                        if(subStep.AccomplishmentCondition == Condition.UserNextToSelectableObject) {
                            subStep.AddOtherObjectId(Int32.Parse(subStepXml["EndWhen"].Attributes["objectId"].Value));
                        }

                        step.AddSubStep(subStep);
                    }
                }
            }
        }

        //  Set current step
        currentStep = steps[0];

        //  Debug Zone
        Debug.Log(selectableObjects[0].associatedObject.name);
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(currentStep.CurrentSubStep.AccomplishmentCondition);
        if (currentStep.CurrentSubStep.AccomplishmentCondition == Condition.UserNextToSelectableObject) {
            GameObject user = currentStep.CurrentSubStep.UserId == 0 ? GameObject.Find("Camera") : GameObject.Find("Camera"); //    Awful, TODO : manage the logic behind the users
            Debug.Log(checkDistance(user, selectableObjects[currentStep.CurrentSubStep.OtherObjectId].associatedObject, userObjectAccomplishingDistance));
        }
	}
}
