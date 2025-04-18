using System;
using System.Security.Cryptography.X509Certificates;
using static System.Collections.Specialized.BitVector32;
using System.Text.Json;
using System.Text.Json.Serialization;
using static SpartaRPG.Program;

namespace SpartaRPG
{

    public enum ItemType
    {
        Weapon,
        Helmet,
        Chest,
        Gloves,
        Shoes,
        Ring,
        Neckles
    }

    [Serializable]
    public class Item
    {
        [JsonInclude] public string Name;
        [JsonInclude] public string Description;
        [JsonInclude] public ItemType Type;
        [JsonInclude] public int Atk;
        [JsonInclude] public int Def;
        [JsonInclude] public int Price;
        [JsonInclude] public bool IsEquipped;
        [JsonInclude] public bool IsDrop;


        public Item(string name, string description, ItemType type, int atk, int def, int price, bool isDrop = false)
        {
            Name = name;
            Description = description;
            Type = type;
            Atk = atk;
            Def = def;
            Price = price;
            IsEquipped = false;
            IsDrop = isDrop;
        }
    }

    [Serializable]
    public class Player
    {
        [JsonInclude]public int Level;
        [JsonInclude] public string Name;
        [JsonInclude] public string Job;
        [JsonInclude] public int Atk;
        [JsonInclude] public int Def;
        [JsonInclude] public int Hp;
        [JsonInclude] public int Gold;
        [JsonInclude] public int AddAtk;
        [JsonInclude] public int AddDef;
        [JsonInclude] public int Exp;
        [JsonInclude] public int MaxExp;

        public Player(int level, string name, string job, int atk, int def, int hp, int gold, int exp, int maxExp)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            AddAtk = 0;
            AddDef = 0;
            Exp = 0;
            MaxExp = 100;
        }
    }

    public enum MonsterType
    {
        Goblin,
        Orc,
        Skeleton,
        Troll,
        Dragon
    }

    [Serializable]
    public class Monster
    {
        public string Name;
        public int Level;
        public int Hp;
        public int Atk;
        public int Exp;
        public int MonsterGold;
        public bool IsBoss;

        public Monster(string name, int level, int hp, int atk, int exp, int monsterGold,  bool isBoss)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            Exp = exp;
            MonsterGold = monsterGold;
            IsBoss = isBoss;
        }
    }

    internal class Program
    {
        static Player player;
        static List<Item> inventory;
        static List<Item> shopItems;

        static void Main(string[] args)
        {
            GameStart();
            PlayGameIntro();
        }

        static void GameStart()
        {
            Console.WriteLine(" 스파르타 마을에 오신 여러분 환영합니다. ");
            Console.WriteLine(" 1. 새 게임 ");
            Console.WriteLine(" 2. 게임 불러오기 ");
            Console.WriteLine(" >> ");

            int choice = CheckInput(1, 2);
            if (choice == 2)
            {
                LoadGame();
                if (player != null)
                {
                    PlayGameIntro();
                    return;
                }
            }

            Console.WriteLine(" 원하는 이름을 설정해 주세요. ");
            string playerName = Console.ReadLine();

            Console.WriteLine(" 직업을 선택해주세요. ");
            Console.WriteLine(" 1. 전사 ");
            Console.WriteLine(" 2. 궁수 ");
            Console.WriteLine(" 3. 마법사 ");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.WriteLine(" >> ");


            int jobChoice = CheckInput(1, 3);

            string job = "";
            switch (jobChoice)
            {
                case 1:
                    job = "전사";
                    break;
                case 2:
                    job = "궁수";
                    break;
                case 3:
                    job = "마법사";
                    break;
            }

            player = new Player(1, playerName, job, 10, 5, 100, 1500, 0, 100);
            InitializeGameData();
        }

        // GameData 내용 초기화를 위해서 - 하지 않으면 에러뜸
        static void InitializeGameData()
        {
            inventory = new List<Item>();
            shopItems = new List<Item>();
            
            // ##상점 아이템##
            // 무기
            shopItems.Add(new Item("청동 검", "청동으로 만든 검", ItemType.Weapon, 5, 0, 800));
            shopItems.Add(new Item("스파르타 지팡이", "스파르타 정통의 지팡이", ItemType.Weapon, 6, 0, 1000));
            shopItems.Add(new Item("스파르타 창", "스파르타 정통의 창", ItemType.Weapon, 7, 0, 1200));
            //갑옷
            shopItems.Add(new Item("청동 갑옷", "청동으로 만든 갑옷", ItemType.Chest, 0, 3, 500));
            shopItems.Add(new Item("강철 갑옷", "강철로 만든 갑옷", ItemType.Chest, 0, 4, 700));
            shopItems.Add(new Item("스파르타 로브", "스파르타 정통의 로브", ItemType.Chest, 0, 5, 900));
            //투구
            shopItems.Add(new Item("청동 투구", "청동으로 만든 투구", ItemType.Helmet, 0, 3, 500));
            shopItems.Add(new Item("강철 투구", "강철로 만든 투구", ItemType.Helmet, 0, 4, 700));
            shopItems.Add(new Item("마법 투구", "마법이 깃든 투구", ItemType.Helmet, 0, 5, 900));
            //장갑
            shopItems.Add(new Item("헤진 가죽 장갑", "가죽으로 만든 헤진 장갑", ItemType.Gloves, 2, 1, 700));
            shopItems.Add(new Item("고급 가죽 장갑", "좋은 가죽으로 만든 장갑", ItemType.Gloves, 3, 1, 900));
            shopItems.Add(new Item("마법 장갑", "마법이 깃든 장갑", ItemType.Gloves, 3, 2, 1200));
            //신발
            shopItems.Add(new Item("헤진 가죽 신발", "가죽으로 만든 헤진 신발", ItemType.Shoes, 1, 1, 700));
            shopItems.Add(new Item("고급 가죽 신발", "좋은 가죽으로 만든 신발", ItemType.Shoes, 1, 2, 900));
            shopItems.Add(new Item("마법 신발", "마법이 깃든 신발", ItemType.Shoes, 1, 3, 1200));
            //반지
            shopItems.Add(new Item("루비 반지", "루비로 만든 반지", ItemType.Ring, 1, 1, 1500));
            shopItems.Add(new Item("사파이어 반지", "사파이어로 만든 반지", ItemType.Ring, 2, 1, 3000));
            //목걸이
            shopItems.Add(new Item("다이아몬드 목걸이", "다이아몬드로 만든 목걸이", ItemType.Neckles, 2, 1, 5000));
        }

        static void PlayGameIntro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" 스파르타 마을에 오신 여러분 환영합니다. ");
                Console.WriteLine(" 이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다. ");
                Console.WriteLine();
                Console.WriteLine(" 1. 상태보기 ");
                Console.WriteLine(" 2. 인벤토리 ");
                Console.WriteLine(" 3. 상점 ");
                Console.WriteLine(" 4. 은신처 ");
                Console.WriteLine(" 5. 던전 입장 ");
                Console.WriteLine(" 6. 게임 저장 ");
                Console.WriteLine(" 0. 게임 종료 ");
                Console.WriteLine();
                Console.WriteLine(" 원하는 행동을 입력해주세요. ");
                Console.WriteLine(" >> ");

                int input = CheckInput(0, 6);
                switch (input)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        PlayInfo();
                        break;
                    case 2:
                        PlayInventory();
                        break;
                    case 3:
                        PlayShop();
                        break;
                    case 4:
                        House();
                        break;
                    case 5:
                        EnterDungeon();
                        break;
                    case 6:
                        SaveGame();
                        break;
                }
            }
        }

        // 입력 검증 메서드 ######
        static int CheckInput(int min,  int max)
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
                Console.Write(" >> ");
            }
        }

        static void PlayInfo()
        {
            Console.Clear();
            Console.WriteLine(" [[상태 보기]] ");
            Console.WriteLine(" 캐릭터의 정보가 표시됩니다. ");
            Console.WriteLine();
            Console.WriteLine($" Lv. {player.Level:D2} (경험치 : {player.Exp} / {player.MaxExp})");
            Console.WriteLine($" {player.Name} ({player.Job})");
            Console.WriteLine($" 공격력 : {player.Atk} (+{player.AddAtk})");
            Console.WriteLine($" 방어력 : {player.Def} (+{player.AddDef})");
            Console.WriteLine($" 체  력 : {player.Hp}");
            Console.WriteLine($" G o l d : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.Write(" >> ");

            int input = CheckInput(0, 0);
            if (input == 0)
                PlayGameIntro();
        }

        static void PlayInventory()
        {
            Console.Clear();
            Console.WriteLine(" [[인벤토리]] ");
            Console.WriteLine(" 보유 중인 아이템을 관리할 수 있습니다. ");
            Console.WriteLine();
            Console.WriteLine(" [아이템 목록] ");

            // 아이템을 보여주는 기능
            for (int i = 0; i < inventory.Count; i++)
            {
                string equippedMark = inventory[i].IsEquipped ? "[E]" : "";
                Console.WriteLine($" {equippedMark} {inventory[i].Name} | {GetEffectDescription(inventory[i])}");
            }



            Console.WriteLine();
            Console.WriteLine(" 1. 아이템 장착/ 해제 ");
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.Write(" >> ");

            // 1. 아이템 장착/ 해제를 눌렀을 시 기능
            int input = CheckInput(0, 1);
            switch (input)
            {
                case 0:
                    PlayGameIntro();
                    break;
                case 1:
                    EquipItem();
                    break;
            }
            
        }

        static string GetEffectDescription (Item item)
        {
            string effect = "";
            if (item.Atk != 0) effect += $"공격력 +{item.Atk} ";
            if (item.Def != 0) effect += $"방어력 +{item.Def} ";
            return effect;
        }

        static void EquipItem()
        {
            Console.Clear();
            Console.WriteLine(" 인벤토리 - 아이템 장착/해제 ");
            Console.WriteLine(" 장착 또는 해제할 아이템을 선택해주세요. ");
            Console.WriteLine();

            for (int i = 0;i < inventory.Count;i++)
            {
                string equippedMark = inventory[i].IsEquipped ? "[E]" : "";
                Console.WriteLine($"{i + 1} {equippedMark}{inventory[i].Name} | {GetEffectDescription(inventory[i])}");
            }

            Console.WriteLine();
            Console.WriteLine(" 0. 나가기 ");
            Console.Write(" >> ");

            int input = CheckInput (0, inventory.Count);
            if (input == 0)
            {
                PlayInventory();
            }
            else
            {
                Item selectedItem = inventory[input - 1];

                if (selectedItem.IsEquipped)
                {
                    selectedItem.IsEquipped = false;
                    player.AddAtk -= selectedItem.Atk;
                    player.AddDef -= selectedItem.Def;
                    Console.WriteLine($" {selectedItem.Name}을 해제했습니다. ");
                }
                else
                {
                    foreach (var item in inventory)
                    {
                        if (item.IsEquipped && item.Type == selectedItem.Type)
                        {
                            item.IsEquipped = false ;
                            player.AddAtk -= item.Atk;
                            player.AddDef -= item.Def;
                        }
                    }

                    selectedItem.IsEquipped = true;
                    player.AddAtk += selectedItem.Atk;
                    player.AddDef += selectedItem.Def;
                    Console.WriteLine($" {selectedItem.Name}을(를) 장착했습니다. ");
                }

                PlayInventory();
            }
        }
        

        static void PlayShop()
        {
            Console.Clear();
            Console.WriteLine(" [[상점]] ");
            Console.WriteLine(" 필요한 아이템을 구매할 수 있는 상점입니다. ");
            Console.WriteLine();
            Console.WriteLine(" [보유 골드] ");
            Console.WriteLine($" | {player.Gold} G | ");
            Console.WriteLine();
            Console.WriteLine(" [아이템 목록] ");

            // 아이템 목록 진열 ##
            for (int i = 0; i < shopItems.Count; i++)
            {
                bool alreadyPurchased = IsItemPurchased( shopItems[i].Name );
                string purchasedStatus = alreadyPurchased ? "구매완료" : $"{shopItems[i].Price} G";
                Console.WriteLine($" {shopItems[i].Name} | {GetEffectDescription(shopItems[i])} | {shopItems[i].Description} | {purchasedStatus}");
            }


            Console.WriteLine();
            Console.WriteLine(" 1. 아이템 구매 2. 아이템 판매");
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.Write(" >> ");

            int input = CheckInput(0, 2);
            switch (input)
            // 1. 아이템 구매를 눌렀을 시 기능
            {
                case 0:
                    PlayGameIntro(); break;
                case 1:
                    BuyItem();
                    break;
                case 2:
                    SellItem(); break;
            }
        }

        static bool IsItemPurchased( string itemName )
        {
            var dropItem = inventory.FirstOrDefault(item => item.Name == itemName && item.IsDrop);
            if (dropItem != null) return false;

            return inventory.Any(item => item.Name == itemName);
        }

        static void BuyItem()
        {
            Console.Clear();
            Console.WriteLine(" [[상점]] ");
            Console.WriteLine(" 필요한 아이템을 구매할 수 있는 상점입니다. ");
            Console.WriteLine();
            Console.WriteLine(" [보유 골드] ");
            Console.WriteLine($" | {player.Gold} G | ");
            Console.WriteLine();
            Console.WriteLine(" [아이템 목록] ");

            // 아이템 목록 진열 ##
            for (int i = 0; i < shopItems.Count; i++)
            {
                bool alreadyPurchased = IsItemPurchased(shopItems[i].Name);
                string purchasedStatus = alreadyPurchased ? "구매완료" : $"{shopItems[i].Price} G";
                Console.WriteLine($" {i + 1}. {shopItems[i].Name} | {GetEffectDescription(shopItems[i])} | {shopItems[i].Description} | {purchasedStatus}");
            }


            Console.WriteLine();
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.Write(" >> ");

            int input = CheckInput(0, shopItems.Count);
            if (input == 0)
            {
                PlayShop();
            }
            else
            {
                Item selectedItem = shopItems[input - 1];
                bool alreadyPurchased = IsItemPurchased(selectedItem.Name);

                if (alreadyPurchased)
                {
                    Console.WriteLine(" 이미 구매한 아이템입니다. ");
                }
                else if (player.Gold >= selectedItem.Price)
                {
                    player.Gold -= selectedItem.Price;
                    inventory.Add(new Item(selectedItem.Name, selectedItem.Description, selectedItem.Type, selectedItem.Atk, selectedItem.Def, selectedItem.Price));
                    Console.WriteLine($" {selectedItem.Name}을(를) 구매했습니다. ");
                }
                else
                {
                    Console.WriteLine(" Gold가 부족합니다. ");
                }

                BuyItem();
            }
        }

        static void SellItem()
        {
            Console.Clear();
            Console.WriteLine(" [[상점]] ");
            Console.WriteLine(" 필요한 아이템을 구매할 수 있는 상점입니다. ");
            Console.WriteLine();
            Console.WriteLine(" [보유 골드] ");
            Console.WriteLine($" | {player.Gold} G | ");
            Console.WriteLine();
            Console.WriteLine(" [아이템 목록] ");

            if (inventory.Count == 0)
            {
                Console.WriteLine(" 판매할 수 있는 아이템이 없습니다. ");
            }
            else
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    string dropInfo = inventory[i].IsDrop ? "[Drop]" : "";
                    Console.WriteLine($"{i + 1}. {dropInfo}{inventory[i].Name} | {GetEffectDescription(inventory[i])} | 판매가 : {(int)(inventory[i].Price * 0.8f)} G");
                }
            }


            Console.WriteLine(" \n0. 나가기 ");
            Console.WriteLine(" >> ");

            int input = CheckInput(0, inventory.Count);
            if (input == 0) return;

            Item selectedItem = inventory[input - 1];
            int sellPrice = (int)(selectedItem.Price * 0.8f);

            player.Gold += sellPrice;
            inventory.Remove(selectedItem);

            var shopItem = shopItems.FirstOrDefault(item => item.Name == selectedItem.Name);
            //if (shopItem != null && !selectedItem.IsDrop)
            //{
            //    Console.WriteLine($"");
            //}

            Console.WriteLine($" \n{selectedItem.Name}을(를) {sellPrice} G에 판매했습니다. ");
            Console.WriteLine($" 현재 골드 : {player.Gold} G");
            Thread.Sleep(1500);
            SellItem();

        }
        
        static void House()
        {
            Console.Clear();
            Console.WriteLine(" [은신처] ");
            Console.WriteLine(" 모험 중 지친 몸을 이끌고 휴식을 취하는 공간 ");
            Console.WriteLine();
            Console.WriteLine(" 500 Gold를 내고 체력을 회복할 수 있습니다. ");
            Console.WriteLine();
            Console.WriteLine($" 현재 보유 골드 : {player.Gold}G");
            Console.WriteLine($" 현재 체력 : {player.Hp}");
            Console.WriteLine();
            Console.WriteLine(" 1. 휴식하기 ");
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine();
            Console.WriteLine(" 원하는 행동을 입력해주세요. ");
            Console.Write(" >> ");

            int input = CheckInput(0, 1);
            if (input == 0)
            {
                PlayGameIntro();
            }
            else
            {
                if (player.Hp == 100)
                {
                    Console.WriteLine(" 이미 체력이 충분합니다. ");
                }
                else
                {
                    if (player.Gold >= 500)
                    {
                        player.Gold -= 500;
                        player.Hp = 100;
                        Console.WriteLine(" 휴식을 완료했습니다. ");
                        Console.WriteLine($" 체력이 {player.Hp}으로 회복되었습니다. ");
                        Console.WriteLine($" 남은 골드 : {player.Gold}");
                    }
                    else
                    {
                        Console.WriteLine(" Gold가 부족합니다. ");
                        Console.WriteLine($" 필요 골드 : 500 G, 보유 골드 : {player.Gold}");
                    }
                }
                

                Console.WriteLine();
                Console.WriteLine(" 0. 나가기");
                Console.WriteLine(" >> ");

                int exitinput = CheckInput(0, 0);
                if (exitinput == 0)
                {
                    PlayGameIntro();
                }
            }
        }

        static void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine(" [던전 입장] ");
            Console.WriteLine(" 스테이지를 선택하세요. ( 1 ~ 5 )");
            Console.WriteLine($" 현재 레벨 : {player.Level}");
            Console.WriteLine($" 체력 : {player.Hp} / 100");
            Console.WriteLine($" 경험치 : {player.Exp} / {player.MaxExp}");
            Console.WriteLine($" Gold : {player.Gold} G");
            Console.WriteLine(" >> ");

            int stage = CheckInput(1, 5);
            StartStage(stage);
        }

        static void StartStage(int stage)
        {
            Monster[] monsters = MakeMonsters(stage);
            Monster boss = MakeBoss(stage);

            Console.Clear();
            Console.WriteLine($" === 스테이지 {stage} ===");

            foreach (var monster in monsters)
            {
                if (!FightMonster(monster))
                {
                    Console.WriteLine(" \n전투에서 패배했습니다. ");
                    Thread.Sleep(1500); // 딜레이 없이 넘어가 어떻게 진행되는지 알기 힘들어 찾아봄
                    return;
                }

                if (!AskContinue(" 마을로 돌아가시겠습니까? "))
                {
                    return;
                }
            }

            Console.WriteLine("\n 다음은 보스입니다. ");
            Console.WriteLine($" {boss.Name} (Lv.{boss.Level})");
            Console.WriteLine($" Hp : {boss.Hp} Atk : {boss.Atk})");
            Console.WriteLine(" ========보스전에서는 도망칠 수 없습니다.======== ");

            if (AskContinue(" 보스에게 도전하시겠습니까? "))
            {
                if (FightMonster(boss))
                {
                    Console.WriteLine($" {boss.Name}을(를) 처치했습니다. ");
                    Console.WriteLine($" 스테이지 {stage} 클리어. ");
                    CheckDrop(boss);
                }
                else
                {
                    Console.WriteLine(" 보스에게 패배했습니다. ");
                }
                Thread.Sleep(1500);
            }
            
        }

        static bool FightMonster(Monster monster)
        {
            bool isBossBattle = monster.IsBoss;

            Console.WriteLine($" {monster.Name} (Lv.{monster.Level}) Hp : {monster.Hp} Atk : {monster.Atk} ");
            Console.WriteLine($"{player.Name} Hp : {player.Hp} / 100 Atk : {player.Atk}(+{player.AddAtk}) Def : {player.Def}(+{player.AddDef})");
            Console.WriteLine($" Gold : {player.Gold} G");
            Console.WriteLine("============================================");

            if (isBossBattle)
            {
                Console.WriteLine(" 1. 공격 ========보스전에서는 도망칠 수 없습니다.========");
            }
            else
            {
                Console.WriteLine(" 1. 공격 2. 마을로 귀환");
            }
            Console.WriteLine(" >> ");

            int minChoice = isBossBattle ? 1 : 1;
            int maxChoice = isBossBattle ? 1 : 2;
            
            int Attack = CheckInput(minChoice, maxChoice);

            if (Attack == 2) // 2번 선택시 마을로 귀환
            {
                Console.WriteLine("\n전투에서 도망쳤습니다!");
                Thread.Sleep(1000);
                return false;
            }


            while (true)
            {
                // 플레이어 공격
                monster.Hp -= player.Atk + player.AddAtk;
                Console.WriteLine($" {player.Name}이(가) {player.Atk + player.AddAtk}의 공격. {monster.Name}에게 {player.Atk + player.AddAtk} 만큼 데미지. ");

                if (monster.Hp <= 0)
                {
                    Console.WriteLine($" \n{monster.Name}을(를) 처치했습니다. ");
                    player.Exp += monster.Exp;
                    player.Gold += monster.MonsterGold;

                    Console.WriteLine($" 경험치 {monster.Exp} 획득. ");
                    Console.WriteLine($" Gold {monster.MonsterGold} 획득. ");
                    CheckDrop(monster);
                    CheckLevelUp();
                    return true;
                }


                // 몬스터 공격
                int damage = Math.Max(1, monster.Atk - (player.Def + player.AddDef));
                player.Hp -= damage;
                Console.WriteLine($" {monster.Name}의 공격. {player.Name}에게 {damage}만큼 데미지. ");

                if (player.Hp <= 0)
                {
                    DeathPenalty();
                    return false;
                }

                Console.WriteLine($" {monster.Name} Hp : {monster.Hp}");
                Console.WriteLine($" {player.Name} Hp : {player.Hp} / 100 ");   
                Console.WriteLine("====================================");

                if (isBossBattle)
                {
                    Console.WriteLine(" 1. 공격 ========보스전에서는 도망칠 수 없습니다.========");
                }
                else
                {
                    Console.WriteLine(" 1. 공격 2. 마을로 귀환");
                }
                Console.WriteLine(" >> ");

                Attack = CheckInput(minChoice, maxChoice);
                if (Attack == 2) // 2번 선택시 마을로 귀환
                {
                    Console.WriteLine("\n전투에서 도망쳤습니다!");
                    Thread.Sleep(1000);
                    return false;
                }

            }
        }

        static void DeathPenalty()
        {
            Console.WriteLine("\n전투에서 패배했습니다. ");

            int lostExp = (int)(player.Exp * 0.2f);
            player.Exp = Math.Max(1, player.Exp - lostExp);

            player.Hp = 100;

            Console.WriteLine($" 경험치 {lostExp}를 잃었습니다. (현재 경험치 : {player.Exp}/{player.MaxExp})");
            Console.WriteLine(" 체력이 100으로 회복되었습니다. ");
            Console.WriteLine(" 마을로 돌아갑니다. ");
            Thread.Sleep(1500);
        }

        static Monster[] MakeMonsters(int stage)
        {
            int baseLevel = stage;
            int count = 5 + stage;

            Monster[] monsters = new Monster[count];
            string[] names = { "고블린", "오크", "스켈레톤", "트롤" };

            for (int i = 0; i < count; i++)
            {
                string name = names[new Random().Next(names.Length)];
                int level = baseLevel + new Random().Next(0, 3);
                int hp = 30 + (stage * 10) + (level * 5);
                int atk = 5 + stage + (level * 2);
                int exp = 10 + (stage * 2) + (level * 2);
                int monsterGold = 20 + (stage * 5);

                monsters[i] = new Monster(name, level, hp, atk, exp, monsterGold, false);
            }

            return monsters; // 여기 만들어진 몬스터 가져가서 사용하세요 라는 의미
        }

        static Monster MakeBoss(int stage)
        {
            string[] BossNames = { "드래곤", "데몬", "리치", "타이탄", "나이트메어" };
            string name = BossNames[stage];
            int hp = 100 + (stage * 20);
            int atk = 25 + (stage * 5);
            int exp = 30 + (stage * 10);
            int monsterGold = 50 + (stage * 5);

            return new Monster(name, stage * 2, hp, atk, exp, monsterGold, true);
        }

        static void CheckLevelUp()
        {
            if (player.Exp >= player.MaxExp)
            {
                player.Level++;
                player.Exp -= player.MaxExp;
                player.MaxExp = (int)(player.MaxExp * 1.5f);

                player.Atk += 2;
                player.Def += 1;
                player.Hp = 100; // 체력 회복

                Console.WriteLine($" Lv UP. Lv.{player.Level}이 되었습니다. ");
                Console.WriteLine(" 공격력 +2, 방어력 +1, 체력이 모두 회복되었습니다. ");
                Thread.Sleep(1500);
            }
        }

        static void CheckDrop(Monster monster)
        {
            Random rand = new Random();

            if (monster.IsBoss && rand.Next(100) < 3)
            {
                Item rareItem = MakeRareItem();
                Console.WriteLine($" 보스가 레어 아이템 {rareItem.Name}을(를) 드롭했다. ");
                inventory.Add( rareItem );
                Thread.Sleep(1500);
                return;
            }

            float dropChance = monster.IsBoss ? 0.2f : 0.1f;
            if (rand.NextDouble() < dropChance)
            {
                var availableItem = shopItems.Where(item => !IsItemPurchased(item.Name)).ToList();
                if (availableItem.Count > 0)
                {
                    Item droppedItem = availableItem[rand.Next(availableItem.Count)];
                    Item dropCopy = new Item(
                        droppedItem.Name,
                        droppedItem.Description,
                        droppedItem.Type,
                        droppedItem.Atk,
                        droppedItem.Def,
                        droppedItem.Price,
                        true
                        );
                    inventory.Add(dropCopy);
                    Console.WriteLine($"\n{monster.Name}이(가) [{droppedItem.Name}]을(를) 떨어트렸다. ");
                    Thread.Sleep(1500);
                }
            }

        }

        static Item MakeRareItem()
        {
            Item[] rareItems =
            {
                new Item("엑스칼리버", "전설로 내려오는 검", ItemType.Weapon, 15, 2, 8000, true),
                new Item("용의 갑옷", "용의 뱃속에 있던 갑옷", ItemType.Chest, 3, 10, 8000, true),
                new Item("용의 투구", "용의 뱃속에 있던 투구", ItemType.Helmet, 0, 12, 10000, true),
                new Item("용의 장갑", "용의 뱃속에 있던 장갑", ItemType.Gloves, 5, 2, 10000, true),
                new Item("용의 신발", "용의 뱃속에 있던 신발", ItemType.Shoes, 2, 5, 10000, true),
                new Item("용의 반지", "용의 뱃속에 있던 반지", ItemType.Ring, 7, 2, 15000, true),
                new Item("용의 목걸이", "용의 뱃속에 있던 목걸이", ItemType.Neckles, 5, 1, 20000, true),
                new Item("이그드라실 활", "세계수로 만들어진 활", ItemType.Weapon, 17, 3, 15000, true),
                new Item("요정의 반지", "요정의 힘이 깃든 반지", ItemType.Ring, 8, 3, 20000, true),
                new Item("어둠의 지팡이", "어둠의 힘이 모여 만들어진 지팡이", ItemType.Weapon, 18, 4, 20000, true),
                new Item("초월의 목걸이", "힘의 한계를 넘어선듯한 목걸이", ItemType.Neckles, 7, 3, 30000, true)
            };

            Random rand = new Random();
            return rareItems[rand.Next(rareItems.Length)];
        }

        static bool AskContinue(string message)
        {
            Console.WriteLine($"\n현재 상태 - Hp : {player.Hp} / 100");
            Console.WriteLine($"Exp: {player.Exp} / {player.MaxExp}");
            Console.WriteLine($" Gold : {player.Gold} G");
            Console.WriteLine($"{message} ( 1. 계속 나아가기, 0. 마을로 귀환 )");
            Console.WriteLine(" >> ");

            int choice = CheckInput(0, 1);
            return choice == 1;
        }


        [Serializable]
        public class SaveData
        {
            public Player PlayerData {  get; set; }
            public List<Item> Inventory { get; set; }
            public List<Item> ShopItems { get; set; }

            public SaveData()
            {
                Inventory = new List<Item>();
                ShopItems = new List<Item>();
            }
        }
        static void SaveGame()
        {
            try
            {
                SaveData saveData = new SaveData()
                {
                    PlayerData = player,
                    Inventory = inventory,
                    ShopItems = shopItems
                };

                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "savegame.json");
                Console.WriteLine($"저장 경로 : {savePath}");
                Thread.Sleep(2000);


                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                string json = JsonSerializer.Serialize(saveData, options);
                File.WriteAllText(savePath, json);

                Console.WriteLine(" Game Data가 저장되었습니다. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Game Data 저장 실패 : {ex.Message}\n{ex.StackTrace}");
            }
            Thread.Sleep(1000);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "savegame.json");
            Console.WriteLine($"File exists: {File.Exists(path)}");
            if (File.Exists(path))
            {
                Console.WriteLine(" 저장 파일 내용 ");
                Console.WriteLine(File.ReadAllText(path));
            }
            
        }

        static void LoadGame()
        {
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "savegame.json");

            if (!File.Exists(savePath))
            {
                Console.WriteLine(" 저장된 Game Data가 없습니다. ");
                Thread.Sleep(1000);
                return;
            }

            try
            {
                string json = File.ReadAllText(savePath);
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                SaveData? saveData = JsonSerializer.Deserialize<SaveData>(json, options);

                if (saveData == null)
                {
                    throw new Exception(" 저장 데이터 파싱 실패 ");
                }

                player = saveData.PlayerData;
                inventory = saveData.Inventory;
                shopItems = saveData.ShopItems;

                Console.WriteLine($" Game Data를 불러왔습니다. 경로 : {savePath} ");
                
            }
            catch ( Exception ex )
            {
                Console.WriteLine($" 불러오기 실패 : {ex.Message}\n{ex.StackTrace}");
            }
            Thread.Sleep(1000);
        }
    }
}
