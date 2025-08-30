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
public class Fabric : Eatable {
    public Fabric() : base() {
        name = "Fabric";
        description = "�ʰ�";
    }
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
public class Tuna : Eatable {
    public Tuna() : base() {
        name = "Tuna";
        description = "��ġĵ";
        properties.Add("hunger", 25);
    }
}

[Serializable]
public class WaterBottle : Eatable {
    public WaterBottle() : base() {
        name = "WaterBottle";
        description = "����";
        properties.Add("thirst", 20);
    }
}

[Serializable]
public class Monster : Eatable {
    public Monster() : base() {
        name = "Monster";
        description = "����";
        properties.Add("thirst", 10);
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
public class RainCoat : Equipable {
    public RainCoat() : base() {
        name = "RainCoat";
        description = "���";
        actions.Add("repair");
        properties.Add("waterproof", 90);
    }
}

[Serializable]
public abstract class Useable : Item {
    public Useable() {
        actions.Add("use");
    }
}

[Serializable]
public class Bandage : Equipable {
    public Bandage() : base() {
        name = "Bandage";
        description = "�ش�";
    }
}

[Serializable]
public class Disinfectant : Equipable {
    public Disinfectant() : base() {
        name = "Disinfectant";
        description = "�ҵ���";
    }
}

[Serializable]
public class Gasoline : Equipable {
    public Gasoline() : base() {
        name = "Gasoline";
        description = "�ֹ���";
    }
}

