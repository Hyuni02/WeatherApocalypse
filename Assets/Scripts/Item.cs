using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Item {
    public string name;
    public string description;
    public Dictionary<string, float> properties;
    public HashSet<string> actions;

    public Item() {
        properties = new Dictionary<string, float>();
        actions = new HashSet<string> {
            "discard"
        };
    }
}

[Serializable]
public class Material : Item {
    public Material() : base() { }
}

[Serializable]
public abstract class Eatable : Item {
    //��Ŭ�� �޴��� �Ա� �߰�
    public Eatable() : base() {
        actions.Add("eat");
    }
}

[Serializable]
public class Chocolate : Eatable {
    public Chocolate() : base() {
        name = "Chocolate";
        description = "���ݸ�(�⺻����)";
        properties.Add("hunger", 20);
    }
}

[Serializable]
public abstract class Equipable : Item {
    //��Ŭ�� �޴��� ���� �߰�
    public Equipable() : base() {
        actions.Add("equip");
    }
}

[Serializable]
public class SurvivalKnife : Equipable {
    public SurvivalKnife() : base() {
        name = "Survival Knife";
        description = "��ɿ� Į(�⺻����)";
        properties.Add("acc", 10);
        properties.Add("dmg", 5);
    }
}

[Serializable]
public abstract class Useable : Item {
    public Useable() {
        actions.Add("use");
    }
}



