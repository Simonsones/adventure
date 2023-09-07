using System.Reflection;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Channels;

namespace adventure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Hero hero = new Hero();
            while (hero.Location != "quit")
            {
                switch (hero.Location)
                {
                    case "Main":
                        WriteLineButCool("Hello and welcome to Text Adventure");
                        Console.ReadLine();
                        hero.Location = "NewGame";
                        break;
                    case "NewGame":
                        NewGame(hero);
                        break;
                    case "TableRoom":
                        TableRoom(hero);
                        break;
                    case "Corridor":
                        Corridor(hero);
                        break;
                    case "TreasureRoom":
                        TreasureRoom(hero);
                        break;
                    case "EntranceHall":
                        EntranceHall(hero);
                        break;
                    case "Outside":
                        Outside(hero);
                        break;
                    case "GameOver":
                        GameOver(hero);
                        break;
                    case "GameWin":
                        GameWin(hero);
                        break;
                    default:
                        Console.Error.WriteLine($"You forgot to implement '{hero.Location}'!");
                        break;
                }
            }
        }

        static void NewGame(Hero hero)
        {
            Console.Clear();
            string name = "";
            do
            {
                name = Program.Ask("What is your name, adventurer? ");
            } while (!Program.AskYesOrNo($"So, {name} it is? "));

            hero.Items.Clear();
            Item woodenSword = new Item("wooden sword", 8, 12);
            hero.Items.Add(woodenSword);

            hero.Health = 100;
            hero.Name = name;

            hero.Location = "TableRoom";
        }

        static void TableRoom(Hero hero)
        {
            Console.Clear();
            WriteLineButCool("You are in a unknown building, equipped with one wooden sword, and your task " +
                             "is to escape the building.\n");
            Thread.Sleep(1500);
            WriteLineButCool("Your stats: ");
            showStats(hero);
            Thread.Sleep(1500);
            WriteLineButCool("\nIn front of you is a stone table with two items on it, " +
                             "a knife and a key.");
            Thread.Sleep(1500);
            WriteLineButCool("You can only pick up one of these items.");

            bool ifPick = AskYesOrNo($"Would you like to pick up one of the items, {hero.Name}? ");
            if (ifPick)
            {
                string choice = AskSpellingCheck("Do you want to pick up the key or the knife? ", "key", "knife");
                if (choice == "knife")
                {
                    WriteButCool($"You dropped your {hero.Items[0].Name} ");
                    Item knife = new Item("knife", 12, 18);
                    RemoveFromItemList(hero, "wooden sword");
                    hero.Items.Add(knife);
                    WriteLineButCool(
                        $"and picked up the {hero.Items[0].Name} (Damage: {hero.Items[0].MinDamage}-{hero.Items[0].MaxDamage}.)");
                }
                else
                {
                    WriteLineButCool("You picked up the key! ");
                    Item key = new Item("key");
                    hero.Items.Add(key);
                }
            }
            else
            {
                WriteLineButCool("You don't pick anything up!");
            }

            WriteLineButCool("You continue forward!");
            Console.ReadLine();
            hero.Location = "Corridor";
        }

        static void Corridor(Hero hero)
        {
            Console.Clear();
            WriteLineButCool("You exit the room and find yourself standing in a hallway.");
            Thread.Sleep(1500);
            WriteLineButCool(
                "You can either enter another room on your right side, or continue down the hallway on your left.");
            Thread.Sleep(1500);
            bool leftDoorChecked = false;
            while (true)
            {
                string choice = AskSpellingCheck("Do you want to go right or left? ", "right", "left");
                if (choice == "right")
                {
                    Console.Clear();
                    WriteLineButCool(
                        "When trying to open the door you notice it's locked and need a key to be opened!");
                    if (InItemList(hero, "key"))
                    {
                        if (AskYesOrNo("Would you use your key to open the door? "))
                        {
                            WriteLineButCool("You use the key to open the locked door and you walk in! ");
                            RemoveFromItemList(hero, "key");
                            hero.Location = "TreasureRoom";
                            break;
                        }
                    }


                    if (!InItemList(hero, "key"))
                    {
                        WriteLineButCool("You dont have a key to open the door!");
                    }

                    WriteLineButCool("You turn around and walk to the other door! ");
                    if (!leftDoorChecked)
                    {
                        WriteLineButCool("When arriving to the door you try and open it and you succeed.");
                        leftDoorChecked = true;
                    }

                    if (AskYesOrNo("Would you like to enter through the door? "))
                    {
                        WriteLineButCool("You enter to the next room.");
                        hero.Location = "EntranceHall";
                        break;
                    }

                    WriteLineButCool("You close the door and step back into the room.");
                }
                else
                {
                    Console.Clear();
                    if (!leftDoorChecked)
                    {
                        WriteLineButCool("When arriving to the door you try and open it and you succeed.");
                        leftDoorChecked = true;
                    }

                    if (AskYesOrNo("Would you like to enter through the door? "))
                    {
                        WriteLineButCool("You enter to the next room.");
                        hero.Location = "EntranceHall";
                        break;
                    }

                    WriteLineButCool("You close the door and step back into the room.");
                }
            }
        }

        static void TreasureRoom(Hero hero)
        {
            Item shinySword = new Item("shiny sword", 15, 25);
            Monster monster1 = new Monster("Unknown", shinySword, 35);

            Console.Clear();
            WriteLineButCool("When entering you see only darkness. You decide to explore " +
                             "and take a few steps in. \nUnable to see where you place your feet, " +
                             "you trip over something and fall to the ground.");
            Thread.Sleep(4000);
            WriteLineButCool("\"WHO DARE TO DISTURB MY SLEEP\" someone yelled in the dark." +
                             "\"DON'T YOU KNOW WHO I AM, PEASANT?? I WILL SLICE YOU OPEN!\"");
            Console.ReadLine();
            Fight(hero, monster1);
            if (hero.Location != "GameOver")
            {
                Console.ReadLine();
                WriteLineButCool("With a finishing blow you slayed the enemy. " +
                                 "Out of breath you notice something shiny in its hand...\n" +
                                 "It's a shiny sword...");
                if (AskYesOrNo("Would you like to pick up the shiny sword? "))
                {
                    WriteButCool($"You dropped your {hero.Items[0].Name} ");
                    RemoveFromItemList(hero, "wooden sword");
                    hero.Items.Add(shinySword);
                    WriteLineButCool(
                        $"and picked up the {hero.Items[0].Name} (Damage: {hero.Items[0].MinDamage}-{hero.Items[0].MaxDamage})");
                    Console.ReadLine();
                }

                WriteLineButCool("There is no other door than the one you came from so you " +
                                 "decide to go back to the corridor and enter through the door on the left.");
                Console.ReadLine();
                hero.Location = "EntranceHall";
            }
        }

        static void EntranceHall(Hero hero)
        {
            Console.Clear();
            WriteLineButCool("You enter through and see yourself in a big room that resembles a mansion entrance.\n" +
                             "Across the room you spot a pair of large doors.\n" +
                             "However in the middle of the room you see something shiny. ");
            string choice =
                AskSpellingCheck(
                    "Do you want to go directly towards the doors or do you want to check out what's shining?\n(doors/shining) ",
                    "doors",
                    "shining");
            if (choice == "doors")
            {
                WriteLineButCool("You open the entrance doors and walk outside.");
            }
            else
            {
                WriteLineButCool("In front of you lays two amulets with number on each of them, " +
                                 "representing the answers to a question printed on the floor next to them.");
                string amulets = AskSpellingCheck("Answer this and take the amulet:\n" +
                                                  "What is 9+10?\n19 OR 21? ", "19", "21");
                bool amulet;
                int chances = new Random().Next(5);
                Item cursedAmulet = new Item("cursed amulet");
                Item blessedAmulet = new Item("blessed amulet");
                if (amulets == "19")
                {
                    if (chances == 0)
                    {
                        amulet = true;
                        WriteLineButCool($"You are lucky, take this {blessedAmulet.Name} and enjoy its power " +
                                         $"(+25% dodge chance)!\n");
                    }
                    else
                    {
                        amulet = false;
                        WriteLineButCool(
                            $"Unlucky, you are stuck with the effects of the {cursedAmulet.Name} " +
                            "(-25% dodge chance)!\n");
                    }
                }
                else
                {
                    if (chances != 0)
                    {
                        amulet = true;
                        WriteLineButCool($"You are lucky, take this {blessedAmulet.Name} and enjoy its power " +
                                         $"(+25% dodge chance)!\n");
                    }
                    else
                    {
                        amulet = false;
                        WriteLineButCool(
                            $"Unlucky, you are stuck with the effects of the {cursedAmulet.Name} " +
                            "(-25% dodge chance)!\n");
                    }
                }

                if (!amulet) // amulet == false, (amulet) == true.
                {
                    hero.Items.Add(cursedAmulet);
                    WriteLineButCool("You feel sluggish and walk towards the entrance doors and leaves the building.");
                }
                else
                {
                    hero.Items.Add(blessedAmulet);
                    WriteLineButCool(
                        "You feel light on your feet and skips towards the entrance doors and leaves the building.");
                }
            }

            Console.ReadLine();
            hero.Location = ("Outside");
        }

        static void Outside(Hero hero)
        {
            Console.Clear();
            Item horns = new Item("Horns", 15, 25);
            Monster minotaur = new Monster("Minotaur", horns, 100);
            WriteLineButCool("When you come outside you feel a chill in the air.\n" +
                             "Blinded by the sun you try to get an understanding of the environment.\n" +
                             "You react to an utterly strange smell getting stronger and stronger.\n" +
                             "At the same time you feel vibrations in the ground and start to look around, " +
                             "you look to the right and to the left where you se a big minotaur\n" +
                             "just a couple of meters away rushing towards you.\n");

            if (Dodge(hero))
            {
                WriteLineButCool("In the last second you were able to jump aside and the minotaur missed you.");
            }
            else
            {
                int monsterDamage = new Random().Next(minotaur.Item.MinDamage, minotaur.Item.MaxDamage + 1);
                WriteLineButCool($"the minotaur ran you over and you lose {monsterDamage} health!");
                hero.Health -= monsterDamage;
            }

            Fight(hero, minotaur);
            if (hero.Location != "GameOver")
            {
                Console.Clear();
                WriteLineButCool("You have slain the beast who guarded the house." +
                                 "You are free and walks into the world for new adventures.");
                Console.ReadLine();
                hero.Location = "GameWin";
            }
        }

        static void GameWin(Hero hero)
        {
            WriteLineButCool("CONGRATULATIONS!!!");
            Thread.Sleep(1000);
            WriteLineButCool("You have finished the game. Hope you enjoyed it.");
            Thread.Sleep(1500);
            if (AskYesOrNo("Do you want to play again? "))
            {
                hero.Location = "NewGame";
            }
            else hero.Location = "quit";
        }

        static void GameOver(Hero hero)
        {
            WriteLineButCool("You died and lost the game!");

            if (AskYesOrNo("Do you want to try again? "))
            {
                hero.Location = "NewGame";
            }
            else hero.Location = "quit";
        }

        public static string Ask(string question)
        {
            string response;
            do
            {
                WriteButCool(question);
                response = Console.ReadLine().Trim();
            } while (response == "");

            return response;
        }

        public static bool AskYesOrNo(string question)
        {
            while (true)
            {
                string response = Ask(question).ToLower();
                switch (response)
                {
                    case "yes":
                    case "ok":
                        return true;
                    case "no":
                        return false;
                }
            }
        }

        public static string AskSpellingCheck(string question, string answer1, string answer2, string? answer3 = null)
        {
            while (true)
            {
                string response = Ask(question).ToLower();
                if ((response == answer1) || (response == answer2) || (response == answer3))
                {
                    return response;
                }

                WriteLineButCool("That was not a answer! ");
            }
        }

        public static bool InItemList(Hero hero, string searchItem)
        {
            foreach (var item in hero.Items)
            {
                if (searchItem == item.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool RemoveFromItemList(Hero hero, string removeItem)
        {
            if (!InItemList(hero, removeItem))
            {
                return false;
            }

            foreach (var item in hero.Items)
            {
                if (removeItem == item.Name)
                {
                    hero.Items.Remove(item);
                    return true;
                }
            }

            return false;
        }

        static void Fight(Hero hero, Monster monster)
        {
            Random random = new Random();
            while (hero.Health > 0 && monster.Health > 0)
            {
                WriteLineButCool(
                    $"Your Health:{hero.Health}, Damage: {hero.Items[0].MinDamage}-{hero.Items[0].MaxDamage}\n" +
                    $"Enemy Health:{monster.Health}, Damage: {monster.Item.MinDamage}-{monster.Item.MaxDamage}");
                bool ifDodge = false;
                string choice = AskSpellingCheck("Do you want to attack, pray or dodge? ", "attack", "pray", "dodge");
                if (choice == "attack")
                {
                    int damage = random.Next(hero.Items[0].MinDamage, hero.Items[0].MaxDamage + 1);
                    WriteLineButCool($"You deal {damage} damage to that bitch!");
                    monster.Health -= damage;
                    if (monster.Health > 0)
                    {
                        WriteButCool("The monster attacks back");
                    }
                }
                else if (choice == "pray")
                {
                    int healHealth = random.Next(10, 26);
                    hero.Health += healHealth;
                    WriteLineButCool($"You healed your health with {healHealth}");
                    WriteButCool("The monster attacks,");
                }
                else
                {
                    ifDodge = Dodge(hero);
                }

                if (ifDodge == true)
                {
                    WriteLineButCool("You successfully dodged the attack and counterattacked!");
                    int damage = random.Next(hero.Items[0].MinDamage, hero.Items[0].MaxDamage + 1);
                    monster.Health -= damage;
                }
                else if (monster.Health > 0)
                {
                    int monsterDamage = random.Next(monster.Item.MinDamage, monster.Item.MaxDamage + 1);
                    WriteLineButCool($" you lose {monsterDamage} health!");
                    hero.Health -= monsterDamage;
                }
            }

            if (hero.Health <= 0)
            {
                hero.Location = "GameOver";
            }
            else WriteLineButCool("Good job, you have slain the enemy!");
        }

        static bool Dodge(Hero hero)
        {
            Random random = new Random();
            int dodgeChance = random.Next(4);
            if (InItemList(hero, "blessed amulet"))
            {
                if (dodgeChance >= 1)
                {
                    return true;
                }
            }
            else if (InItemList(hero, "cursed amulet"))
            {
                if (dodgeChance == 0)
                {
                    return true;
                }
            }
            else
            {
                if (dodgeChance >= 2)
                {
                    return true;
                }
            }

            WriteButCool("You failed to dodge and were struck, ");
            return false;
        }

        static void showStats(Hero hero)
        {
            WriteLineButCool($"Health: {hero.Health}");
            WriteLineButCool(
                $"Weapon: {hero.Items[0].Name} Damage: {hero.Items[0].MinDamage}-{hero.Items[0].MaxDamage}");
            if (InItemList(hero, "blessed amulet"))
            {
                WriteLineButCool("Dodge chance: 75%");
            }
            else if (InItemList(hero, "cursed amulet"))
            {
                WriteLineButCool("Dodge chance: 25%");
            }
            else
            {
                WriteLineButCool("Dodge chance: 50%");
            }
        }

        static void WriteLineButCool(string input)
        {
            WriteButCool(input);

            Console.WriteLine();
        }

        static void WriteButCool(string input)
        {
            char[] inputArray = input.ToArray();
            foreach (char character in inputArray)
            {
                Console.Write(character);
                Thread.Sleep(10);
            }
        }
    }

    class Hero
    {
        public string Name = "";
        public int Health = 100;
        public List<Item> Items = new List<Item>();
        public string Location = "Main";
    }

    class Monster
    {
        public string Name;
        public Item Item;
        public int Health;

        public Monster(string name, Item item, int health)
        {
            Name = name;
            Item = item;
            Health = health;
        }
    }

    class Item
    {
        public string Name;
        public int MinDamage;
        public int MaxDamage;

        public Item(string name, int minDamage, int maxDamage)
        {
            Name = name;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public Item(string name)
        {
            Name = name;
        }
    }
}
