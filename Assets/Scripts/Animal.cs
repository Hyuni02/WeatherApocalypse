using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal
{
    public string name;
    public int hp;
    public int acc;
    public int dmg;
    public Dictionary<InjureType, int> dic_injure;
    public Dictionary<Item, int> dic_drop;
    
    public Animal() {
        dic_injure = new Dictionary<InjureType, int>();
        dic_drop = new Dictionary<Item, int>();

        //todo 임시구현
        dic_drop.Add(new Meat(), 100);
    }

    public bool GetDmg(int dmg) {
        hp -= dmg;
        StaticFunctions.Log($"플레이어가 {dmg} 피해를 줌");
        return (hp <= 0);
    }

    public void Attack(ref Player player) {
        System.Random random = new System.Random();
        if(random.Next(0, 100) < acc ) {
            player.GetDmg(this, dmg);
        }
        else{
            StaticFunctions.Log($"{name}의 공격이 빗나감");
        }
    }
}

public class Elk : Animal {
    public Elk() : base() {
        name = "고라니";
        hp = 20;
        acc = 40;
        dmg = 20;
        dic_injure.Add(InjureType.bruise, 20);
    }
}

public class Snake : Animal {
    public Snake() : base() {
        name = "뱀";
        hp = 10;
        acc = 30;
        dmg = 10;
        dic_injure.Add(InjureType.bleeding, 20);
    }
}

public class Boar : Animal {
    public Boar() : base() {
        name = "멧돼지";
        hp = 40;
        acc = 20;
        dmg = 30;
        dic_injure.Add(InjureType.fracture, 60);
    }
}

public class Grizzly : Animal {
    public Grizzly() : base() {
        name = "그리즐리";
        hp = 60;
        acc = 40;
        dmg = 40;
        dic_injure.Add(InjureType.fracture, 60);
        dic_injure.Add(InjureType.bruise, 60);
        dic_injure.Add(InjureType.bleeding, 60);
    }
}
