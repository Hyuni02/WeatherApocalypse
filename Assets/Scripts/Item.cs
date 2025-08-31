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
        description = "�ʰ�";
    }
}

public abstract class Eatable : Item {
    //��Ŭ�� �޴��� �Ա� �߰�
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
        description = "�����";
        properties["hunger"] = 20;
        //todo ���ߵ� Ȯ��
    }
}

public class Chocolate : Eatable {
    public Chocolate() : base() {
        name = "Chocolate";
        description = "���ݸ�(�⺻����)";
        properties["hunger"] = 20;
    }
}

public class Tuna : Eatable {
    public Tuna() : base() {
        name = "Tuna";
        description = "��ġĵ";
        properties["hunger"] = 25;
    }
}

public class WaterBottle : Eatable {
    public WaterBottle() : base() {
        name = "WaterBottle";
        description = "����";
        properties["thirst"] = 20;
    }
}

public class Monster : Eatable {
    public Monster() : base() {
        name = "Monster";
        description = "����";
        properties["thirst"] = 10;
    }
}

public abstract class Equipable : Item {
    //��Ŭ�� �޴��� ���� �߰�
    public Equipable() : base() {
        actions.Add("equip");
    }
}

public abstract class Weapon : Item {
    //��Ŭ�� �޴��� ���� �߰�
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
        description = "��ɿ� Į(�⺻����)";
        properties.Add("acc", 10);
        properties.Add("dmg", 5);
    }
}

public abstract class Wearable : Equipable {
    //��Ŭ�� �޴��� ���� �߰�
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
        description = "���";
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
        description = "�ش�";
    }
}

public class Disinfectant : Equipable {
    public Disinfectant() : base() {
        name = "Disinfectant";
        description = "�ҵ���";
    }
}

public class Gasoline : Equipable {
    public Gasoline() : base() {
        name = "Gasoline";
        description = "�ֹ���";
    }
}

