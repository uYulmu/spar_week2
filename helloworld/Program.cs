using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

internal class Program
{
    private static Character player;
    private static Item wand;
    private static Item robe;
    private static Inventory myinventory;
    private static Equipment equipmentitem;


    static void Main(string[] args)
    {
        GameDataSetting();
        DisplayGameIntro();
    }

    static void GameDataSetting()
    {
        // 캐릭터 정보 세팅
        player = new Character("장성민", "마법사", 1, 10, 5, 100, 1500);

        // 인벤토리 정보 세팅
        myinventory = new Inventory();
        equipmentitem = new Equipment(0, 0, 0);

        // 아이템 정보 세팅
        wand = new Item("완드", "기초적인 마법 구사를 보조하는 무기입니다.", 5, 0, 0);
        robe = new Item("로브", "초보 마법사들을 위한 값싼 방어구입니다.", 0, 5, 0);
    }

    static void DisplayGameIntro()
    {
        Console.Clear();

        Console.WriteLine("스파르타 마을에 오신 것을 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int input = CheckValidInput(1, 2);
        switch (input)
        {
            case 1:
                DisplayMyInfo();
                break;

            case 2:
                myinventory.AddItemInventory(wand);
                myinventory.AddItemInventory(robe);
                DisplayInventory();
                break;
        }
    }

    static void DisplayMyInfo()
    {
        Console.Clear();

        Console.WriteLine("상태보기");
        Console.WriteLine("캐릭터의 정보를 표시합니다.");
        Console.WriteLine();
        Console.WriteLine($"Lv.{player.Level}");
        Console.WriteLine($"{player.Name}({player.Job})");
        Console.WriteLine($"공격력 : {player.Atk} (+{equipmentitem.AddAtk})");
        Console.WriteLine($"방어력 : {player.Def} (+{equipmentitem.AddDef})");
        Console.WriteLine($"체력 : {player.Hp} (+{equipmentitem.AddHp})");
        Console.WriteLine($"Gold : {player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 0);
        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;
        }
    }

    static void DisplayInventory()
    {
        Console.Clear();

        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine($"- 1 {wand.ItemName}      | 공격력 +{wand.ItemAtk} | {wand.ItemInfo}");
        Console.WriteLine($"- 2 {robe.ItemName}      | 방어력 +{robe.ItemDef} | {robe.ItemInfo}");
        Console.WriteLine();
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 1);
        switch (input)
        {
            case 0:
                DisplayGameIntro();
                break;

            case 1:
                DisplayMyEquipItem();
                break;
        }
    }

    static void DisplayMyEquipItem()
    {
        Console.Clear();

        Console.WriteLine("인벤토리 - 장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine($"-1 {(wand.IsEquip ? "[E]" : "")}{wand.ItemName}      | 공격력 +{wand.ItemAtk} | {wand.ItemInfo}");
        Console.WriteLine($"-2 {(robe.IsEquip ? "[E]" : "")}{robe.ItemName}      | 방어력 +{robe.ItemDef} | {robe.ItemInfo}");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");

        int input = CheckValidInput(0, 2);
        switch (input)
        {
            case 0:
                DisplayInventory();
                break;

            case 1:
                if (wand.IsEquip == false)
                {
                    equipmentitem.EquipingItem(wand);
                }
                else
                {
                    equipmentitem.UnEquipingItem(wand);
                }
                DisplayMyEquipItem();
                break;

            case 2:
                if (robe.IsEquip == false)
                {
                    equipmentitem.EquipingItem(robe);
                }
                else
                {
                    equipmentitem.UnEquipingItem(robe);
                }
                DisplayMyEquipItem();
                break;
        }
    }

    static int CheckValidInput(int min, int max)
    {
        while (true)
        {
            string input = Console.ReadLine();

            bool parseSuccess = int.TryParse(input, out var ret);
            if (parseSuccess)
            {
                if (ret >= min && ret <= max)
                    return ret;
            }

            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    public class Inventory
    {
        public List<Item> InventoryItem = new List<Item>();
        public void AddItemInventory(Item item)
        {
            InventoryItem.Add(item);
        }
        public void RemoveItemInventory(Item item)
        {
            InventoryItem.Remove(item);
        }
    }

    public class Equipment
    {
        public List<Item> EquipItems = new List<Item>();
        public void EquipingItem(Item item)
        {
            EquipItems.Add(item);
            equipmentitem.AddEquipItem();
            item.IsEquip = !item.IsEquip;
        }
        public void UnEquipingItem(Item item)
        {
            EquipItems.Remove(item);
            equipmentitem.AddEquipItem();
            item.IsEquip = !item.IsEquip;
        }
        public void AddEquipItem()
        {
            AddAtk = AddDef = AddHp = 0;
            for (int i = 0; EquipItems.Count != i; i++)
            {
                AddAtk += EquipItems[i].ItemAtk;
                AddDef += EquipItems[i].ItemDef;
                AddHp += EquipItems[i].ItemHp;
            }
        }
        public int AddAtk { get; set; }
        public int AddDef { get; set; }
        public int AddHp { get; set; }
        public Equipment(int addatk, int adddef, int addhp)
        {
                AddAtk = addatk;
                AddDef = adddef;
                AddHp = addhp;
        }
    }

    public class Item
    {
        public string ItemName { get; }
        public string ItemInfo { get; }
        public int ItemAtk { get; }
        public int ItemDef { get; }
        public int ItemHp { get; }
        public bool IsEquip { get; set; }

        public Item(string itemname, string iteminfo, int itematk, int itemdef, int itemhp)
        {
            ItemName = itemname;
            ItemInfo = iteminfo;
            ItemAtk = itematk;
            ItemDef = itemdef;
            ItemHp = itemhp;
            IsEquip = false;
        }
    }


    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }
}