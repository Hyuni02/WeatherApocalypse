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
    
    public Animal() {
        dic_injure = new Dictionary<InjureType, int>();
    }

    public bool GetDmg(int dmg) {
        hp -= dmg;
        StaticFunctions.Log($"ÇÃ·¹ÀÌ¾î°¡ {dmg} ÇÇÇØ¸¦ ÁÜ");
        return (hp <= 0);
    }

    public void Attack(ref Player player) {
        System.Random random = new System.Random();
        if(random.Next(0, 100) < acc ) {
            player.GetDmg(this, dmg);
        }
        else{
            StaticFunctions.Log($"{name}ÀÇ °ø°ÝÀÌ ºø³ª°¨");
        }
    }
}

public class Elk : Animal {
    public Elk() : base() {
        name = "°í¶ó´Ï";
        hp = 20;
        acc = 40;
        dmg = 20;
        dic_injure.Add(InjureType.bruise, 20);
    }
}

public class Snake : Animal {
    public Snake() : base() {
        name = "¹ì";
        hp = 10;
        acc = 30;
        dmg = 10;
        dic_injure.Add(InjureType.bleeding, 20);
    }
}

public class Boar : Animal {
    public Boar() : base() {
        name = "¸äµÅÁö";
        hp = 40;
        acc = 20;
        dmg = 30;
        dic_injure.Add(InjureType.fracture, 60);
    }
}

public class Grizzly : Animal {
    public Grizzly() : base() {
        name = "±×¸®Áñ¸®";
        hp = 60;
        acc = 40;
        dmg = 40;
        dic_injure.Add(InjureType.fracture, 60);
        dic_injure.Add(InjureType.bruise, 60);
        dic_injure.Add(InjureType.bleeding, 60);
    }
}
