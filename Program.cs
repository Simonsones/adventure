using System.Reflection;
using System.Collections.Generic;

namespace adventure
{
    class Hero
    {
        public string Name = "";
        public int Health = 100;
        public List<string> Items = new List<string>();
        public string Location = "newgame";
    }
    class program
    {
        static string Ask(string question)
        {
            string response;
            do
            {
                Console.Write(question);
                response = Console.ReadLine().Trim();
            }
            while (response == "");
            return response;
        }
        static bool AskYesOrNo(string question)
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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello and welcome to Text Adventure");
            Hero hero = new Hero();
            while (hero.Location != "quit")
            {
                if (hero.Location == "newgame")
                {
                    NewGame(hero);
                } else if (hero.Location == "tableroom")
                {
                    TableRoom(hero);
                } else
                {
                    Console.Error.WriteLine($"You forgot to implement '{hero.Location}'!");
                }
            }
        }
        static void NewGame(Hero hero)
        {
            Console.Clear();
            string name = "";
            do
            {
                name = Ask("What is your name adventurer? ");
            } while (!AskYesOrNo($"So, {name} it is? "));
            hero.Name = name;
            hero.Location = "Tableroom";
        }
        static void TableRoom(Hero hero)
        {
            Console.Clear();
            string name = "";
            Console.WriteLine("You are equipped with one wooden sword, and your task " +
            "is to slay the monster at the end of the adventure. " +
            "" +
            "In front of you is a stone table with two items on it, " +
            "a knife and a key." +
            "" +
            "You can only pick up one of these items.");

            bool ifPick = AskYesOrNo($"Would you like to pick up one of the items, {name}?");    
             
        }
    }
}


