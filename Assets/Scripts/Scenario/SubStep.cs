using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStep {

    int number;
    string instruction;
    int userId;
    Condition accomplishmentCondition;
    int otherObjectId;

    public int Number {
        get {
            return this.number;
        }
        set {
            this.number = value;
        }
    }

    public Condition AccomplishmentCondition {
        get {
            return this.accomplishmentCondition;
        }
    }

    public int UserId {
        get {
            return this.userId;
        }
    }

    public int OtherObjectId {
        get {
            return this.otherObjectId;
        }
    }

    public string Instruction {
        get {
            return this.instruction;
        }
    }

    public void AddOtherObjectId(int id) {
        this.otherObjectId = id;
    }

    public SubStep(int numberParam, string instructionParam, int userIdParam, Condition accomplishmentConditionParam) {
        number = numberParam;
        instruction = instructionParam;
        userId = userIdParam;
        accomplishmentCondition = accomplishmentConditionParam;
        otherObjectId = 0;
    }
}
