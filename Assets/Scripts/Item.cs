using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Item {
    public string name;
    public string description;
    public Dictionary<string, int> properties;
    public HashSet<string> actions;

    public Item() {
        properties = new Dictionary<string, int>();
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

public class Material : Item {
    public Material() : base() { }
}

public class Fabric : Material {
    public Fabric() : base() {
        name = "Fabric";
        description = "옷감";
    }
}

public abstract class Eatable : Item {
    //우클릭 메뉴에 먹기 추가
    public Eatable() : base() {
        actions.Add("eat");
        properties.Add("hunger", 0);
        properties.Add("thirst", 0);
    }
    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "eat") {
            StaticFunctions.Log($"{name} : eat");
            if(HideoutManager.instance) {
                HideoutManager.instance.player.curHunger += properties["hunger"];
                if(HideoutManager.instance.player.curHunger > HideoutManager.instance.player.maxHunger ) {
                    HideoutManager.instance.player.curHunger = HideoutManager.instance.player.maxHunger;
                }
                HideoutManager.instance.player.curHP += (int)(properties["hunger"] * 0.5f);
                if (HideoutManager.instance.player.curHP > HideoutManager.instance.player.maxHP) {
                    HideoutManager.instance.player.curHP = HideoutManager.instance.player.maxHP;
                }
                //HideoutManager.instance.player.curHP += properties["thirst"];
                HideoutManager.instance.Update_All();
            }
            if (RaidManager.instance) {
                RaidManager.instance.player.curHunger += properties["hunger"];
                if (RaidManager.instance.player.curHunger > RaidManager.instance.player.maxHunger) {
                    RaidManager.instance.player.curHunger = RaidManager.instance.player.maxHunger;
                }
                RaidManager.instance.player.curHP += (int)(properties["hunger"] * 0.5f);
                if (RaidManager.instance.player.curHP > RaidManager.instance.player.maxHP) {
                    RaidManager.instance.player.curHP = RaidManager.instance.player.maxHP;
                }
                //HideoutManager.instance.player.curHP += properties["thirst"];
                RaidManager.instance.Update_All();
            }
        }
    }
}

public class Meat : Eatable {
    public Meat() : base() {
        name = "Meat";
        description = "생고기";
        properties["hunger"] = 20;
        //todo 식중독 확률
    }
}

public class Chocolate : Eatable {
    public Chocolate() : base() {
        name = "Chocolate";
        description = "초콜릿(기본지급)";
        properties["hunger"] = 20;
    }
}

public class Tuna : Eatable {
    public Tuna() : base() {
        name = "Tuna";
        description = "참치캔";
        properties["hunger"] = 25;
    }
}

public class WaterBottle : Eatable {
    public WaterBottle() : base() {
        name = "WaterBottle";
        description = "물병";
        properties["thirst"] = 20;
    }
}

public class Monster : Eatable {
    public Monster() : base() {
        name = "Monster";
        description = "몬스터";
        properties["thirst"] = 10;
    }
}

public abstract class Equipable : Item {
    //우클릭 메뉴에 장착 추가
    public Equipable() : base() {
        actions.Add("equip");
    }
}

public abstract class Weapon : Item {
    //우클릭 메뉴에 장착 추가
    public Weapon() : base() { }
    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "equip") {
            if (HideoutManager.instance) {
                if (HideoutManager.instance.player.lst_equiped["weapon"] == null
                    || HideoutManager.instance.player.lst_equiped["weapon"].Count == 0) {
                    HideoutManager.instance.player.lst_equiped["weapon"] = new List<Item>() { this };
                    from.Remove(this);
                    HideoutManager.instance.Update_All();
                }
            }
            if(RaidManager.instance) {
                if (RaidManager.instance.player.lst_equiped["weapon"] == null
                    || RaidManager.instance.player.lst_equiped["weapon"].Count == 0) {
                    RaidManager.instance.player.lst_equiped["weapon"] = new List<Item>() { this };
                    from.Remove(this);
                    RaidManager.instance.Update_All();
                }
            }
        }
    }
}

public class SurvivalKnife : Weapon {
    public SurvivalKnife() : base() {
        name = "Survival Knife";
        description = "사냥용 칼(기본지급)";
        properties.Add("acc", 10);
        properties.Add("dmg", 5);
    }
}

public abstract class Wearable : Equipable {
    //우클릭 메뉴에 장착 추가
    public Wearable() : base() { }
    public override void ContextAction(string action, List<Item> from) {
        base.ContextAction(action, from);
        if (action == "equip") {
            StaticFunctions.Log($"{name} : equip");
            if (HideoutManager.instance) {
                if (HideoutManager.instance.player.lst_equiped["body"] == null
                    || HideoutManager.instance.player.lst_equiped["body"].Count == 0) {
                    HideoutManager.instance.player.lst_equiped["body"] = new List<Item>() { this };
                    from.Remove(this);
                    HideoutManager.instance.Update_All();
                }
            }
            if (RaidManager.instance) {
                if (RaidManager.instance.player.lst_equiped["body"] == null
                    || RaidManager.instance.player.lst_equiped["body"].Count == 0) {
                    RaidManager.instance.player.lst_equiped["body"] = new List<Item>() { this };
                    from.Remove(this);
                    RaidManager.instance.Update_All();
                }
            }
        }
    }
}

public class RainCoat : Wearable {
    public RainCoat() : base() {
        name = "RainCoat";
        description = "우비";
        //actions.Add("repair");
        properties.Add("waterproof", 90);
    }
}

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

public class Bandage : Equipable {
    public Bandage() : base() {
        name = "Bandage";
        description = "붕대";
    }
}

public class Disinfectant : Equipable {
    public Disinfectant() : base() {
        name = "Disinfectant";
        description = "소독약";
    }
}

public class Gasoline : Equipable {
    public Gasoline() : base() {
        name = "Gasoline";
        description = "휘발유";
    }
}

