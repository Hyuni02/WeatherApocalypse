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
            "discard",
            "tobelonging",
            "toinventory"
        };
    }

    public virtual void ContextAction(string action, List<Item> from) {
        if(action == "discard") {
            from.Remove(this);
            if (HideoutManager.instance) {
                HideoutManager.instance.Update_All();
            }
            if(RaidManager.instance) {
                RaidManager.instance.Update_All();
            }
        }
        if(action == "tobelonging") {
            if (HideoutManager.instance) {
                Item item = this;
                HideoutManager.instance.player.lst_belonging.Add(item);
                from.Remove(this);
                HideoutManager.instance.Update_All();
            }
            if (RaidManager.instance) {
                Item item = this;
                RaidManager.instance.player.lst_belonging.Add(item);
                from.Remove(this);
                RaidManager.instance.Update_All();
            }
        }
        if(action == "toinventory") {
            if(HideoutManager.instance) {
                Item item = this;
                HideoutManager.instance.player.lst_inventory.Add(item);
                from.Remove(this);
                HideoutManager.instance.Update_All();
            }
            if (RaidManager.instance) {
                Item item = this;
                RaidManager.instance.player.lst_belonging.Add(item);
                from.Remove(this);
                RaidManager.instance.Update_All();
            }
        }
    }
}

[Serializable]
public class Material : Item {
    public Material() : base() { }
}

[Serializable]
public class Fabric : Material {
    public Fabric() : base() {
        name = "Fabric";
        description = "옷감";
    }
}

[Serializable]
public abstract class Eatable : Item {
    //우클릭 메뉴에 먹기 추가
    public Eatable() : base() {
        actions.Add("eat");
    }
    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "eat") {
            StaticFunctions.Log($"{name} : eat");
        }
    }
}

[Serializable]
public class Meat : Eatable {
    public Meat() : base() {
        name = "Meat";
        description = "생고기";
        properties.Add("hunger", 20);
        //todo 식중독 확률
    }
}

[Serializable]
public class Chocolate : Eatable {
    public Chocolate() : base() {
        name = "Chocolate";
        description = "초콜릿(기본지급)";
        properties.Add("hunger", 20);
    }
}

[Serializable]
public class Tuna : Eatable {
    public Tuna() : base() {
        name = "Tuna";
        description = "참치캔";
        properties.Add("hunger", 25);
    }
}

[Serializable]
public class WaterBottle : Eatable {
    public WaterBottle() : base() {
        name = "WaterBottle";
        description = "물병";
        properties.Add("thirst", 20);
    }
}

[Serializable]
public class Monster : Eatable {
    public Monster() : base() {
        name = "Monster";
        description = "몬스터";
        properties.Add("thirst", 10);
    }
}

[Serializable]
public abstract class Equipable : Item {
    //우클릭 메뉴에 장착 추가
    public Equipable() : base() {
        actions.Add("equip");
    }
    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "equip") {
            StaticFunctions.Log($"{name} : equip");
        }
    }
}

[Serializable]
public class SurvivalKnife : Equipable {
    public SurvivalKnife() : base() {
        name = "Survival Knife";
        description = "사냥용 칼(기본지급)";
        properties.Add("acc", 10);
        properties.Add("dmg", 5);
    }
}

[Serializable]
public class RainCoat : Equipable {
    public RainCoat() : base() {
        name = "RainCoat";
        description = "우비";
        actions.Add("repair");
        properties.Add("waterproof", 90);
    }
}

[Serializable]
public abstract class Useable : Item {
    public Useable() : base() {
        actions.Add("use");
    }

    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "use") {
            StaticFunctions.Log($"{name} : use");
        }
    }
}

[Serializable]
public class Bandage : Equipable {
    public Bandage() : base() {
        name = "Bandage";
        description = "붕대";
    }
}

[Serializable]
public class Disinfectant : Equipable {
    public Disinfectant() : base() {
        name = "Disinfectant";
        description = "소독약";
    }
}

[Serializable]
public class Gasoline : Equipable {
    public Gasoline() : base() {
        name = "Gasoline";
        description = "휘발유";
    }
}

