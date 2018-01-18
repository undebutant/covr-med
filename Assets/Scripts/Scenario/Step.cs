using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step {
    int number;
    string stepName;
    List<SubStep> subSteps;
    SubStep currentSubStep;

    public int Number {
        get {
            return this.number;
        }
        set {
            this.number = value;
        }
    }

    public string StepName {
        get {
            return this.stepName;
        }
        set {
            this.stepName = value;
        }
    }

    public SubStep CurrentSubStep {
        get {
            return this.currentSubStep;
        }
    }

    public Step(int numberParam, string stepNameParam) {
        Number = numberParam;
        StepName = stepNameParam;
        subSteps = new List<SubStep>();
        currentSubStep = null;
    }

    public void AddSubStep(SubStep sub) {
        subSteps.Add(sub);
        currentSubStep = currentSubStep == null ? subSteps[0] : currentSubStep;
    }

    public void MoveToNextSubStep() {
		if (currentSubStep.Number + 1 < subSteps.Count)
			currentSubStep = subSteps [currentSubStep.Number + 1];
		else
			GameObject.Find("ScenarioManager").GetComponent<Scenario>().MoveToNextStep();
    }
}
