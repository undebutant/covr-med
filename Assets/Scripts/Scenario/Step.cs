using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step {
    int number;
    string stepName;
    List<SubStep> subSteps = new List<SubStep>();
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
            this.StepName = value;
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
        currentSubStep = subSteps[0];
    }

    public void AddSubStep(SubStep sub) {
        subSteps.Add(sub);
    }

    public void MoveToNextSubStep() {
        currentSubStep = subSteps[currentSubStep.Number + 1];
    }
}
