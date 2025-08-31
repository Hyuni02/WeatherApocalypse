using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum InjureType {
    bruise, //타박상
    fracture, //골절
    infection, //감염
    bleeding, //출혈
    dehydration //탈수
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
    public List<Injure> lst_injure; //부상 목록
    public List<Item> lst_belonging; //소지품 목록
    public List<Item> lst_inventory; //인벤토리 목록(레이드 내 사용 불가)
    public Dictionary<string, List<Item>> lst_equiped; //장착 목록

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

    //아이템 장착
    public void Equip() {

    }

    //아이템 해제
    public void Unequip() {

    }

    public void Act(int move, int hunger, int dehydra) {
        curHP -= move;
        curHunger -= hunger;

        if(curHP <= 0) {
            StaticFunctions.Log("플레이어 사망");
        }
        if(curHunger <= 0) {
            StaticFunctions.Log("배가 고프다");
            curHunger = 0;
        }
    }

    //부상 시간 관리
    public void Update_Injure() {
        for (int i = lst_injure.Count - 1; i <= 0; i--) {
            //처치 된 부상 시간 차감
            if (lst_injure[i].handled) {
                lst_injure[i].duration--;
            }

            //todo 처치 되지 않은 부상 부작용 발현


            //치료된 부상 제거
            if (lst_injure[i].duration <= 0) {
                lst_injure.RemoveAt(i);
            }
        }
    }

    //부상 처치
    public void Handle_Injure(ref Injure injure) {
        switch (injure.type) {
            case InjureType.bruise:
                Debug.Log("붕대, 소독약 사용");
                break;
            case InjureType.fracture:
                Debug.Log("붕대, 부목 사용");
                break;
            case InjureType.infection:
                Debug.Log("소독약 사용");
                break;
            case InjureType.bleeding:
                Debug.Log("붕대 사용");
                break;
        }
    }

    public void GetDmg(Animal animal, int dmg) {
        StaticFunctions.Log($"{animal.name}이 {dmg.ToString()}를 줌");
        curHP -= dmg;

        if(curHP <= 0) {
            StaticFunctions.Log("플레이어 사망");
        }
    }
}
