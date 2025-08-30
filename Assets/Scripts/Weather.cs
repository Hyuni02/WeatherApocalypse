using System.Collections.Generic;

public enum Weather {
    sunny,
    heatwave,
    rain,
    heavyrain,
    fog,
    windy,
    snow
}

public class Sunny {
    public Dictionary<string, int> itemPool;
    public int totalweight;
    //Æò±Õ ³¯¾¾
    public Sunny() {
        itemPool = new Dictionary<string, int>();
        totalweight = 0;

        SetItemPool();

        foreach (var weight in itemPool.Values) {
            totalweight += weight;
        }
    }
    public virtual void SetItemPool() {
        itemPool.Add("null", 50);
        itemPool.Add("material", 20);
        itemPool.Add("animal", 5);
        itemPool.Add("food", 10);
        itemPool.Add("water", 10);
        itemPool.Add("medical", 5);
        itemPool.Add("equip", 3);
    }
    public string Pick() {
        System.Random random = new System.Random();
        int rand = random.Next(0, totalweight - 1);
        foreach(var item in itemPool) {
            rand -= item.Value;
            if(rand < 0) {
                return item.Key;
            }
        }
        return null;
    }
}

public class Heatwave : Sunny {
    //½Ä¼ö È®·ü0
    public override void SetItemPool() {
        base.SetItemPool();
        itemPool.Remove("water");
    }
}

public class Rain : Sunny {
    //À½½Ä È®·ü+
    //½Ä¼ö È®·ü+
    public override void SetItemPool() {
        base.SetItemPool();
        itemPool["food"] = 20;
        itemPool["water"] = 20;
    }
}

public class Heavyrain : Sunny {
    //µ¿¹° È®·ü--
    //½Ä¹° È®·ü--
    //Àåºñ È®·ü--
    public override void SetItemPool() {
        base.SetItemPool();
        itemPool["animal"] = 2;
        itemPool["water"] = 4;
    }
}

public class Fog : Sunny {
    //µ¿¹° È®·ü--
    //½Ä¹° È®·ü--
    //Àç·á È®·ü--
    //ÀÇ·á È®·ü--
    //Àåºñ È®·ü--
    public override void SetItemPool() {
        base.SetItemPool();
        itemPool["animal"] = 2;
        itemPool["food"] = 4;
        itemPool["materal"] = 8;
        itemPool["medical"] = 1;
        itemPool["equip"] = 1;
    }
}

public class Windy : Sunny {

}

public class Snow : Sunny {
    //½Ä¹°0
    //µ¿¹°--
    public override void SetItemPool() {
        base.SetItemPool();
        itemPool["animal"] = 1;
        itemPool.Remove("food");
    }
}
