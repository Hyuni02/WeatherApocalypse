using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum InjureType {
    bruise, //Ÿ�ڻ�
    fracture, //����
    infection, //����
    bleeding, //����
    dehydration //Ż��
}

[Serializable]
public enum BodyType {
    head,
    thorax,
    stomach,
    arm_left,
    arm_right,
    leg_left,
    leg_right
}

[Serializable]
public class Injure {
    public InjureType type;
    public BodyType bodyType;
    public bool handled;
    public int duration;
}

[Serializable]
public class Player {
    public int maxHP;
    public int curHP;
    public int maxHunger;
    public int curHunger;
    public int energyConsum_move;
    public int energyConsum_attack;
    public int acc;
    public int dmg;
    public List<Injure> lst_injure; //�λ� ���
    public List<Item> lst_belonging; //����ǰ ���
    public List<Item> lst_inventory; //�κ��丮 ���(���̵� �� ��� �Ұ�)
    public Dictionary<string, List<Item>> lst_equiped; //���� ���

    public Player() {
        maxHP = 100;
        curHP = 100;
        maxHunger = 100;
        curHunger = 100;
        energyConsum_move = 2;
        energyConsum_attack = 5;
        acc = 70;
        dmg = 8;
        lst_injure = new List<Injure>();
        lst_belonging = new List<Item>();
        lst_inventory = new List<Item>();
        lst_equiped = new Dictionary<string, List<Item>>();
        lst_equiped.Add("weapon", new List<Item>());
        lst_equiped.Add("body", new List<Item>());
        lst_equiped.Add("bag", new List<Item>());
    }

    //������ ����
    public void Equip() {

    }

    //������ ����
    public void Unequip() {

    }

    public void Act(int move, int hunger, int dehydra) {
        curHP -= move;
        curHunger -= hunger;

        if(curHP <= 0) {
            StaticFunctions.Log("�÷��̾� ���");
        }
        if(curHunger <= 0) {
            StaticFunctions.Log("�谡 ������");
            curHunger = 0;
        }
    }

    //�λ� �ð� ����
    public void Update_Injure() {
        for (int i = lst_injure.Count - 1; i <= 0; i--) {
            //óġ �� �λ� �ð� ����
            if (lst_injure[i].handled) {
                lst_injure[i].duration--;
            }

            //todo óġ ���� ���� �λ� ���ۿ� ����


            //ġ��� �λ� ����
            if (lst_injure[i].duration <= 0) {
                lst_injure.RemoveAt(i);
            }
        }
    }

    //�λ� óġ
    public void Handle_Injure(ref Injure injure) {
        switch (injure.type) {
            case InjureType.bruise:
                Debug.Log("�ش�, �ҵ��� ���");
                break;
            case InjureType.fracture:
                Debug.Log("�ش�, �θ� ���");
                break;
            case InjureType.infection:
                Debug.Log("�ҵ��� ���");
                break;
            case InjureType.bleeding:
                Debug.Log("�ش� ���");
                break;
        }
    }

    public void GetDmg(Animal animal, int dmg) {
        StaticFunctions.Log($"{animal.name}�� {dmg.ToString()}�� ��");
        curHP -= dmg;

        if(curHP <= 0) {
            StaticFunctions.Log("�÷��̾� ���");
        }
    }
}
