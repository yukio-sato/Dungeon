﻿#pragma warning disable CA1416 // beep warning disabler
Console.OutputEncoding = System.Text.Encoding.UTF8; // emoji/ special character fix

string action, // type of action choosed 
hpbar = "", // visual hp bar string
playerhpbar = "", // visual hp bar string
luck_mode = ""; // luck status (now)

bool firstEncounter = true, // first interact for monster encounter
secretEcounter = false;

var you=(name: "",hp: 0,at: 0,lu: 0,lv: 1,exp: 0, gold: 0); // [1] Name, [2] HP, [3] ATK, [4] LUCK, [5] LV, [6] EXP
var enemy=(name: "",hp: 0,at: 0,lv: 0); // [1] Name, [2] HP, [3] ATK, [4] LV
var narrador=(act1: "",act2: "",act3: "",act4: ""); // [1] Interactions, [2] Interactions, [3] Interactions, [4] Interactions
var item=(name: "",cost: 0, max: 0,heal: 0); // [1] Item Name, [2] Cost, [3] Max amount, [4] Heal
var inventory=(slot1: "─", slot2: "─", slot3: "─", slot4: "─"); // Player Inventory Slots
var shopMenu=(option1: 0, option2: 0, option3: 0, option4: 0); // Shop placeholder slots

int stage = 0, finalStage = 11, // stage settings
rolled = 1, result, // dice placeholder
encounter, secret=0, // monster random encounter
hp, // health actual from monster
xp, // xp actual from monster
player_hp, // health actul from player
plaHL, monHL, // monster/player half length math for design
luck_Test, atk_Test, def_Test, // player/enemy tests
atk_dmg = 0, def_dmg = 0, // damage player/enemy for atk and def
heartLength = 5, // the number of character of Heart Bar
interactNumber = 0, // random number for interact
next = 25, // exp modify for next love
xpMath = 10 + (next*(you.lv - 1)), // math of next exp for love
selected = 1, // selected
gold = 125; // player gold

void secretEncounter()
{
encounter = dice(1,100,0);
stage++;
secretEcounter = false;
if (encounter <= 25) // chance for YOU?
{
    secretEcounter = true;
    secret = 0;
}
}


void FoodInventory(string food, int amount)
{
    if (inventory.slot1 == "─" || inventory.slot1.Substring(0,food.Length) == food)
    {
        if (inventory.slot1 != "─" && inventory.slot1.Substring(0,food.Length) == food)
        {
            inventory.slot1 = food+"_"+(Convert.ToInt16(inventory.slot1.Substring(food.Length+1))+amount);
        }
        else
        {
            inventory.slot1 = food+"_"+amount;
        }
        gold -= item.cost;
    }
    else if (inventory.slot2 == "─" || inventory.slot2.Substring(0,food.Length) == food)
    {
        if (inventory.slot2 != "─" && inventory.slot2.Substring(0,food.Length) == food)
        {
            inventory.slot2 = food+"_"+(Convert.ToInt16(inventory.slot2.Substring(food.Length+1))+amount);
        }
        else
        {
            inventory.slot2 = food+"_"+amount;
        }
        gold -= item.cost;
    }
    else if (inventory.slot3 == "─" || inventory.slot3.Substring(0,food.Length) == food)
    {
        if (inventory.slot3 != "─" && inventory.slot3.Substring(0,food.Length) == food)
        {
            inventory.slot3 = food+"_"+(Convert.ToInt16(inventory.slot3.Substring(food.Length+1))+amount);
        }
        else
        {
            inventory.slot3 = food+"_"+amount;
        }
        gold -= item.cost;
    }
    else if (inventory.slot4 == "─" || inventory.slot4.Substring(0,food.Length) == food)
    {
        if (inventory.slot4 != "─" && inventory.slot4.Substring(0,food.Length) == food)
        {
            inventory.slot4 = food+"_"+(Convert.ToInt16(inventory.slot4.Substring(food.Length+1))+amount);
        }
        else
        {
            inventory.slot4 = food+"_"+amount;
        }
        gold -= item.cost;
    }
    else
    {
        textDialog("Não há espaço no seu Inventário",12);
        Console.ReadKey();
        shopDesign();
    }
    Console.WriteLine(inventory);
    Console.ReadKey();
    shopDesign();
}
string inventoryVerify(string food)
{
    string finded = "─";
    if (inventory.slot1.Length >= food.Length && inventory.slot1.Substring(0,food.Length) == food)
    {
        finded = inventory.slot1;
    }
    else if (inventory.slot2.Length >= food.Length && inventory.slot2.Substring(0,food.Length) == food)
    {
        finded = inventory.slot2;
    }
    else if (inventory.slot3.Length >= food.Length && inventory.slot3.Substring(0,food.Length) == food)
    {
        finded = inventory.slot3;
    }
    else if (inventory.slot4.Length >= food.Length && inventory.slot4.Substring(0,food.Length) == food)
    {
        finded = inventory.slot4;
    }
    return finded;
}

void buy(string food)
{
    string answer = "";
    Console.Clear();
    textDialog($"Quer comprar {food}?\n",12);
    answer = Console.ReadLine()!;
    if (answer.Trim() == "")
    {
        answer = ".";
    }
    if (answer.Trim().ToLower().Substring(0,1) == "s")
    {
        if (gold >= item.cost && (inventoryVerify(food) == "─" || Convert.ToInt32(inventoryVerify(food).Substring(food.Length+1)) < item.max))
        {
            FoodInventory(food,1);
        }
        else if (item.max <= 0 || inventoryVerify(food) != "─" && Convert.ToInt32(inventoryVerify(food).Substring(food.Length+1)) >= item.max)
        {
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog("Está carregando muito ",12);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        textDialog(food,12);
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog("!\n",12);
        Console.ReadKey();   
        }
        else if (gold < item.cost) 
        {
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog("Não possue ",12);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        textDialog("Gold ",12);
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog("suficiente!\n",12);
        Console.ReadKey();
        }
        shopDesign();
    }
    else if (answer.Trim().ToLower().Substring(0,1) == "n")
    {

    }
    else
    {
        buy(food);
    }
}
void shopSelectedDesign(int slot)
{
if (slot == selected)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    if (slot < 5)
    {
    Console.WriteLine($"[{item.name}]{"".PadRight(8-(item.name.Length+item.cost.ToString().Length),'.')}{item.cost} Gold");
    }
    else
    {
    Console.WriteLine($"[Exit]{"".PadRight(9,'.')}");   
    }
}
else
{
    Console.ForegroundColor = ConsoleColor.Gray;
    if (slot < 5)
    {
    Console.WriteLine($"{item.name.PadRight(10-item.cost.ToString().Length,'.')}{item.cost} Gold");  
    }
    else
    {
    Console.WriteLine($"Exit{"".PadRight(11,'.')}");   
    }
}
}
void shopDesign()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(".=──{Shop}───=.");
    shop(shopMenu.option1);
    shopSelectedDesign(1);
    shop(shopMenu.option2);
    shopSelectedDesign(2);
    shop(shopMenu.option3);
    shopSelectedDesign(3);
    shop(shopMenu.option4);
    shopSelectedDesign(4);
    shopSelectedDesign(5);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("'=───────────='");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"{gold} Gold");


   ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
    if (pressed.Key == ConsoleKey.UpArrow)
    {
        if (selected > 1)
        {
        selected -= 1;
        }
        else
        {
        selected = 5;
        }
        shopDesign();
    }
    else if (pressed.Key == ConsoleKey.DownArrow)
    {
        if (selected < 5)
        {
        selected += 1;
        }
        else
        {
        selected = 1;
        }
        shopDesign();
    }
    else if (pressed.Key == ConsoleKey.Enter)
    {
        if (selected < 5)
        {
            shop(selected);
            buy(item.name);
        }
    }
    else
    {
        shopDesign();
    }
}

void slotDesign(string food,int slotSpace)
{
    string foodName = food.Substring(0,food.Length-2);
    int autoSize = 10-(foodName.Length/2);
    if ((slotSpace % 2) == 0)
    {
        if (slotSpace == selected)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"".PadLeft(autoSize-1,' ')}[{foodName}]{"".PadRight(autoSize-1,' ')}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"".PadLeft(autoSize,' ')}{foodName}{"".PadRight(autoSize,' ')}");  
        }
    }
    else if (slotSpace < 5)
    {
        if (slotSpace == selected)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{"".PadLeft(autoSize-1,' ')}[{foodName}]{"".PadRight(autoSize-1,' ')}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"".PadLeft(autoSize,' ')}{foodName}{"".PadRight(autoSize,' ')}");  
        }
    }
    else
    {
        if (slotSpace == selected)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"".PadLeft(7,' ')}[Exit]{"".PadRight(7,' ')}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"".PadLeft(8,' ')}Exit{"".PadRight(8,' ')}");  
        } 
    }
}

void inventoryMenu()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(".=──{Item}───=.");
    slotDesign(inventory.slot1,1);
    slotDesign(inventory.slot2,2);
    slotDesign(inventory.slot3,3);
    slotDesign(inventory.slot4,4);
    slotDesign("Exit",5);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("'=───────────='");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"{gold} Gold");

       ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
    if (pressed.Key == ConsoleKey.UpArrow)
    {
        if (selected > 1)
        {
        selected -= 1;
        }
        else
        {
        selected = 5;
        }
        inventoryMenu();
    }
    else if (pressed.Key == ConsoleKey.DownArrow)
    {
        if (selected < 5)
        {
        selected += 1;
        }
        else
        {
        selected = 1;
        }
        inventoryMenu();
    }
    else if (pressed.Key == ConsoleKey.Enter)
    {
        if (selected < 5)
        {
            if (selected == 1)
            {
                if (Convert.ToInt32(inventory.slot1.Substring(inventory.slot1.Length-1)) > 0)
                {
                inventory.slot1 = inventory.slot1.Substring(0,inventory.slot1.Length-1)+(Convert.ToInt32(inventory.slot1.Substring(inventory.slot1.Length-1))-1);
                }
            }
            else if (selected == 2)
            {
                if (Convert.ToInt32(inventory.slot2.Substring(inventory.slot2.Length-1)) > 0)
                {
                inventory.slot2 = inventory.slot2.Substring(0,inventory.slot2.Length-1)+(Convert.ToInt32(inventory.slot2.Substring(inventory.slot2.Length-1))-1);
                }
            }
            else if (selected == 3)
            {
                if (Convert.ToInt32(inventory.slot3.Substring(inventory.slot3.Length-1)) > 0)
                {
                inventory.slot3 = inventory.slot3.Substring(0,inventory.slot3.Length-1)+(Convert.ToInt32(inventory.slot3.Substring(inventory.slot3.Length-1))-1);
                }   
            }
            else if (selected == 4)
            {
                if (Convert.ToInt32(inventory.slot4.Substring(inventory.slot4.Length-1)) > 0)
                {
                inventory.slot4 = inventory.slot4.Substring(0,inventory.slot4.Length-1)+(Convert.ToInt32(inventory.slot4.Substring(inventory.slot4.Length-1))-1);
                }
            }
            inventoryMenu();
        }
    }
    else
    {
        inventoryMenu();
    }
}

void shop(int x)
{
switch (x)
{
case 1:
item=("Doce",15,3,5);
break;
case 2:
item=("A",1,1,1);
break;
case 3:
item=("B",0,0,0);
break;
case 4:
item=("E",666,6,6);
break;
default:
item=("",0,0,0);
break;
}
}

void Levelviolence() // love for every kill you do
{
    xpMath = 10 + (next*(you.lv - 1));
    if (you.exp >= xpMath)
    {
        you.exp -= xpMath;
        you.lv++;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        textDialog($"{you.name} subiu de LV {you.lv-1} > {you.lv}!\n",25);
        if (you.lv % 4 == 0)
        {
        player_hp += 1;
        you.hp += 1;
        Console.ForegroundColor = ConsoleColor.Green;
        textDialog($"Aumentou 1 de HP!\n",25);
        }
        else
        {
        you.at += 1;
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog($"Aumentou 1 de AT!\n",25);
        }
    }
}

int xpMonsterMath() // xp math with monster lv when kill
{
    int monsterXp = 10 + (5*(enemy.lv - 1));
    return monsterXp-(you.lv-1);
}

void X_Beep(int x, int x2) // last message/letter after die + beep
{
    Console.Beep(x,x2);
    Console.Write("ˣ");
}

void dmgSound(string from) // attack sound/effect
{
    if (from == "" || from == null) // nobody hit sound
    {
    for (int i = 0; i < 4; i++)
    {
        Console.Beep(800,125);
    }
        Console.Beep(625,575);
    }

    else if (from == "Monster") // monster hit sound
    {
    for (int i = 0; i < 4; i++)
    {
        Console.Beep(550,100);
    }
    Console.Beep(550,450);
    }

    else if (from == "Player") // player hit sound
    {
    for (int i = 0; i < 4; i++)
    {
    Console.Beep(600,100);
    }
    Console.Beep(600,450);
    }
}
void textDialog(string txt, int cooldown) // write texts like RPGs NPCs
{
    for (int i = 0; i < txt.Length; i++) // how much length the txt have
    {
        if (txt[i].ToString() != " " && txt[i].ToString() != "") // skip cooldown if the letter is blank or space
        {
        Console.Beep(1125,cooldown-1);
        }
        Console.Write(txt[i]); // write on 'i' position on txt
    }
}
int dice(int quantity,int maximo, int bns) // random number generator syle RPG Custom Dice
{
    result = 0; // set to 0 or will stack
    for (int i = 0; i < quantity; i++) // number of dices
    {
        rolled = new Random().Next(0,maximo+1); // the dice
        if (result <= 0) // checking if dice wasn't rolled yet
        {
            result = rolled;
        }
        else // if dice was rolled yet, then stack
        {
            result += rolled;
        }
    }
    return (result+bns); // return all dices values
}

void soul(int actual, string who) // shorter heart fill
{
    if (who == "Monster") // if is monster hearts
    {
        hpbar = "";
        for (int i = 0; i < actual; i++) // fill with fully hearts
        {
            hpbar += "♥";
        }
        hpbar = hpbar.PadRight(heartLength,'♡'); // fill with void hearts
    }
    else if (who == "Player") // if is player hearts
    {
        playerhpbar = ""; // set to nothing or will stack
        for (int i = 0; i < actual; i++) // fill with fully hearts
        {
            playerhpbar += "♥";
        }
        playerhpbar = playerhpbar.PadRight(heartLength,'♡'); // fill with void hearts
    }
}

void bar(int now, int max, string who) // Heart Bar
{
    if (now >= max) // Health 100% or above.
    {
        soul(heartLength,who);
    }
    else if (now <= max-(max*0.2) && now > max-(max*0.4)) // Health 80% or less and Health above from 60%.
    {
        soul(4,who);
    }
    else if (now <= max-(max*0.4) && now > max-(max*0.6)) // Health 60% or less and Health above from 40%.
    {
        soul(3,who);
    }
    else if (now <= max-(max*0.6) && now > max-(max*0.8)) // Health 40% or less and Health above from 20%.
    {
        soul(2,who);
    }
    else if (now <= max-(max*0.8) && now > 0) // Health 20% or less and Health above from 0%.
    {
        soul(1,who);
    }
    else if (now <= 0) // Health 0% or less.
    {
        soul(0,who);
    }
}

void loaded() // load design, wait user click any keybind for progress
{
    Console.ForegroundColor = ConsoleColor.White;
    textDialog("Pressione qualquer ⌨   tecla para continuar",2);
    textDialog(" . . . ➟\n",125);
    Console.ReadKey();
}
void nameAsk() // arcade user name select/choose
{
Console.Clear();
Console.Write($"✎ Digite o Nome: {you.name.PadRight(12,'_')}"); // fill with underline (custom)

ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var

if (pressed.Key != ConsoleKey.Backspace && pressed.Key != ConsoleKey.Enter && you.name.Length <= 12)
{
    if (you.name.Length < 12) // fill blank/underline characters
    {
    you.name += pressed.KeyChar;
    }
    else if (you.name.Length >= 12) // if name got max length, then subtitute last character to new
    {
    you.name = you.name.Substring(0,you.name.Length-1)+pressed.KeyChar;
    }
    nameAsk();
}
else if (pressed.Key == ConsoleKey.Backspace) // remove last character
{
    if (you.name.Length > 0)
    {
    you.name = you.name.Substring(0,you.name.Length-1);
    }
    nameAsk();
}
else if (pressed.Key == ConsoleKey.Enter) // when pressed enter finish user name select
{
    Console.WriteLine();
}
else // case if use anything that isn't on keyboard
{
    nameAsk();
}
}

Console.Clear();
Console.ForegroundColor = ConsoleColor.White;
nameAsk(); // insert you name
you.at = dice(1,6,6);
Console.ForegroundColor = ConsoleColor.DarkRed;
textDialog($"⟨➶  AT: {you.at}",25);
you.hp = dice(1,6,6);
player_hp = you.hp;
Console.ForegroundColor = ConsoleColor.Green;
textDialog($" ❤  HP: {player_hp}",25);
you.lu = dice(2,6,12);
Console.ForegroundColor = ConsoleColor.DarkGreen;
textDialog($" ✤  LU: {you.lu}⟩\n",25);
loaded();
void updt()
{
if (secretEcounter == false){
switch (stage)
{
    case 1:
    enemy=("Lobo Cinzento",3,3,1);
    narrador=(
        $"{enemy.name} ataca ferozamente!",
        $"{enemy.name} late ferozamente!",
        $"{enemy.name} pula ferozamente!",
        $"{enemy.name} deita ferozamente?"
    );
    break;
    case 2:
    enemy=("Lobo Branco",3,3,1);
    narrador=(
        $"{enemy.name} ataca calmamente.",
        $"{enemy.name} late calmamente.",
        $"{enemy.name} pula calmamente.",
        $"ZZZZZZZZZ. . ."
    );
    break;
    case 3:
    enemy=("Goblin",5,4,2);
    narrador=(
        $"{enemy.name} te encara!",
        $"{enemy.name} começa a saltitar!",
        $"{enemy.name} faz. . .\n✶ Algo impossível de descrever?",
        $"{enemy.name} aponta sua lança!\n✶ PERA!\n✶ Onde veio isto!?"
    );
    break;
    case 4:
    enemy=("Orc Vesgo",5,5,4);
    narrador=(
        $"{enemy.name} acabou de intimidar até sua alma.",
        $"{enemy.name} tentou enxergar você.\n✶ Mas falha miseravelmente.",
        $"{enemy.name} cansou-se e começa a ler um livro.\n✶ Parece um de literatura.",
        $"{enemy.name} começa a falar algo sem sentido?"
    );
    break;
    case 5:
    enemy=("Orc Barbudo",5,5,4);
    narrador=(
        $"{enemy.name} demonstra sua barba!",
        $"{enemy.name} reflete a luz!",
        $"{enemy.name} começa a escovar sua barba!",
        $"{enemy.name} fez nada?"
    );
    break;
    case 6:
    enemy=("Zumbi Manco",6,7,5);
    narrador=(
        $"{enemy.name} corre rapidamente em sua direção!",
        $"{enemy.name} ele questiona o sentido da vida.",
        $"{enemy.name} ele começa a pular corda!",
        $"{enemy.name} reponhe seus ossos?"
    );
    break;
    case 7:
    enemy=("Zumbi Balofo",6,7,5);
    narrador=(
        $"urg.\n (Não consigo respirar!)",
        $"{enemy.name} está fedendo. . .",
        $"{enemy.name} abre sua boca em minha direçã-",
        $"Narrador morre após fedor de {enemy.name}. . ."
    );
    break;
    case 8:
    enemy=("Troll",8,7,7);
    narrador=(
        $"{enemy.name} fez uma TROLLagem em você!",
        $"{enemy.name} alegremente.\n✶ Começa a sorrir.",
        $"{enemy.name} demonstra seu sorriso.\n✶ Te lembra de algo?",
        $"{enemy.name}Face deu umas risadas."
    );
    break;
    case 9:
    enemy=("Ogro",8,9,7);
    narrador=(
        $"{enemy.name} senta-se.",
        $"{enemy.name} balança sua arma.",
        $"{enemy.name} começa contemplar a natureza.",
        $"{enemy.name} esqueceu do motivo para lutar."
    );
    break;
    case 10:
    enemy=("Ogro Furioso",10,9,10);
    narrador=(
        $"{enemy.name} lembrou-se do motivo para lutar!",
        $"{enemy.name} começa contemplar sua dor!",
        $"{enemy.name} prepara sua posição de combate!",
        $"{enemy.name} arremessa lama!\n✶ Parou na tua cara!"
    );
    break;
    case 11:
    enemy=("Necromante Maligno",12,12,12);
    narrador=(
        $"O ambiente começa a ficar mais tenso. . .",
        $"O cemitério.\n✶ Os ventos.\n* Te incomodam. . .",
        $"A aventura de {you.name} chegou ao fim. . .",
        $"Você só queria terminar essa Masmorra. . ."
    );
    break;
    default:
    break;
}
}
else if (secretEcounter == true)
{

    switch (secret)
    {
    case 0:
    enemy=($"{you.name}?",you.hp,you.at,you.lv);
    narrador=(
        $"O cenário está vazio. . .\n✶ Te deixa bastante confuso e desconfortavel. . .",
        $". . . ?",
        $"Sua mente esvazia-se. . .\n✶ Conforme a batalha procede. . .",
        $"Sua Determinação. . .\n✶ Sua Criação. . .\n✶ Vai colocar um Fim. . ."
    );
    break;
    default:
    break;
    }
}

// monster stats update
hp = enemy.hp;
xp = xpMonsterMath();
}

void counterCheck() // when you defend check if you counter the attack or not
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    if (atk_Test >= you.at)
    {
    hp = hp - (atk_dmg-1);
    textDialog($"➹ {you.name} Contra-ataca!\n",25);
    dmgSound("Player");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    textDialog($"♡ {enemy.name} perdeu {atk_dmg-1} HP\n",12);
    dmgSound("");
    }
    else if (atk_Test < you.at)
    {
    player_hp = player_hp - (def_dmg-1);
    textDialog($"➹ {you.name} Bloqueiou!\n",25);
    dmgSound("Monster");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    textDialog($"♡ {you.name} perdeu {def_dmg-1} HP\n",12);
    dmgSound("");
    }
}

void morte()
{
    if (player_hp <= 0) // player dead sound/animation
    {
        player_hp = 0;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        textDialog($"✝ {you.name} caiu! ✝",25);
        textDialog(" . . . ➟\n",125);
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(950-(i*50),900-(i*100));
        }
        Console.Beep(750,1000);
    }
    else if (hp <= 0) // if monster is defeat
    {
        hp = 0;
        Console.Clear();
        if (secretEcounter == false)
        {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        else
        {
        Console.ForegroundColor = ConsoleColor.Black;
        }
        bar(hp,enemy.hp, "Monster");
        monHL = 14-enemy.name.Length/2;
        Console.WriteLine("「".PadLeft((monHL),'▁')+enemy.name+"」".PadRight((monHL),'▁'));
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" LV "+enemy.lv.ToString().PadLeft(2,' ')+"  EXP ?/?");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" HP {hpbar} {hp.ToString().PadLeft(2,'0')}/{enemy.hp.ToString().PadLeft(2,'0')}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($" AT {enemy.at}➶ ");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($" LU ?✤\n");
        you.exp += xp;
        if (secretEcounter == false)
        {
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog($"✝ {enemy.name} foi derrotado! ✝\n", 25);
        Console.ForegroundColor = ConsoleColor.White;
        textDialog($"{you.name} ganhou {xpMonsterMath()} de EXP!\n",25);
        Levelviolence();
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(900,120);
        }
        Console.Beep(1100,270);
        }
        else if (secretEcounter == true)
        {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        textDialog($"{enemy.name} foi apagado. . .\n", 105);
        Console.ForegroundColor = ConsoleColor.Gray;
        textDialog($"{you.name} substituiu, ganhando {xpMonsterMath()} de EXP.\n",25);
        Levelviolence();
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(950-(i*50),900-(i*250));
        }
        Console.Beep(750,1000);
        }
        if (stage >= finalStage) // if you are on the last stage when enemy die (verify)
        {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Clear();
        textDialog($"❚ ❚ {you.name} matou todos os monstros! ❚ ❚\n",25);
        Console.Beep(1250,400);
        Console.Beep(750,400);
        Console.Beep(1250,400);
        Console.Beep(1550,600);
        Console.ResetColor();
        }
    }
}
void status_math() // taken or does damage math
{
    if (luck_mode == "") // when your luck_mode = none
    {
        atk_dmg = 2;
        def_dmg = 2;
    }
    else if (luck_mode == "lucky") // when your luck_mode = lucky
    {
        atk_dmg = 4;
        def_dmg = 1;
    }
    else if (luck_mode == "unlucky") // when your luck_mode = unlucky
    {
        atk_dmg = 1;
        def_dmg = 3;
    }
}
void loop() // actions: [A] attack, [D] defend, [S] luck or [I] item
{
if (firstEncounter == false)
{
interactNumber = dice(1,4,0);
}
else if (firstEncounter == true)
{
interactNumber = 1;
firstEncounter = false;
}
Console.ForegroundColor = ConsoleColor.White;
switch (interactNumber)
{
    case 1:
        textDialog("✶ "+narrador.act1+"\n",2);
    break;
    case 2:
        textDialog("✶ "+narrador.act2+"\n",2);
    break;
    case 3:
        textDialog("✶ "+narrador.act3+"\n",2);
    break;
    default:
        textDialog("✶ "+narrador.act4+"\n",2);
    break;
}

Console.WriteLine("╔─── Ações ──────────────╗");// begin of action box
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.WriteLine("│ Ⓐ tacar                │");
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("│ Ⓓ efender              │");
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("│ Ⓢ orte                 │");
Console.ForegroundColor = ConsoleColor.Green;
//Console.WriteLine("│ Ⓘ tem                  │");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("╚────────────────────────╝"); // end of action box
action = Console.ReadLine()!;
if (action.Trim() == "") // prevent of break code when used substring();
{
    action = ".";
}
if (action.Trim().ToLower().Substring(0,1) == "s") // luck test
{
    if (hp > 0 && luck_mode == "") // if you didn't tested you luck yet
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        textDialog($"✤ {you.name} testou a Sorte!\n",25);
        luck_Test = dice(2,6,0);
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(800,100);
        }
        Console.Beep(900,250);
        if (luck_Test <= you.lu)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            textDialog("Sortudo\n",45);
            luck_mode = "lucky";
        }
        else if (luck_Test > you.lu)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog("Azarado\n",45);
            luck_mode = "unlucky";
        }
        loaded();
        you.lu -= 1;
    }
    else if (hp > 0 && luck_mode != "") // if you arealdy have tested luck
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        textDialog("✤ Você já testou sua Sorte!\n",37);
        loaded();
    }
}

if (action.Trim().ToLower().Substring(0,1) == "a") // attack action
{
    if (hp > 0)
    {
        status_math();
        luck_mode = "";
        atk_Test = you.at + dice(2,6,0);
        def_Test = enemy.at + dice(2,6,0);
        if (atk_Test > def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            hp = hp - atk_dmg;
            textDialog($"➹ {you.name} Ataca!\n",25);
            dmgSound("Player");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {enemy.name} perdeu {atk_dmg} HP\n",12);
            dmgSound("");
        }
        else if (atk_Test < def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            player_hp = player_hp - def_dmg;
            textDialog($"➹ {enemy.name} Contra-ataca!\n",25);
            dmgSound("Monster");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {you.name} perdeu {def_dmg} HP\n",12);
            dmgSound("");
        }
        else if (atk_Test == def_Test)
        {
            Console.ForegroundColor = ConsoleColor.White;
            textDialog("➹ Ambos erram o Ataque!\n",25);
            Console.Beep(850,1450);
        }
        morte();
        loaded();
    }
}
else if (action.Trim().ToLower().Substring(0,1) == "d") // defend action
{
    if (hp > 0)
    {
        status_math();
        luck_mode = "";
        def_Test = you.hp + dice(2,6,0);
        atk_Test = enemy.hp + dice(2,6,0);
        if (def_Test != atk_Test)
        {
            counterCheck();
        }
        else if (def_Test == atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.White;
            textDialog("➹ Ambos erram o Ataque!\n",25);
            Console.Beep(850,1450);
        }
        morte();
        loaded();
    }
}
/*else if (action.Trim().ToLower().Substring(0,1) == "i") // item action
{
    if (hp > 0)
    {
        inventoryMenu();
        morte();
        loaded();
    }
}*/
}

void turns() // battle stats + loop
{
Console.Clear();
// --------------------------------------Monster-----------------------------------------
if (secretEcounter == false)
{
Console.ForegroundColor = ConsoleColor.DarkRed;
}
else if (secretEcounter == true)
{
Console.ForegroundColor = ConsoleColor.Black;
}
bar(hp,enemy.hp,"Monster");
monHL = 14-enemy.name.Length/2;
Console.WriteLine("「".PadLeft((monHL),'▁')+enemy.name+"」".PadRight((monHL),'▁'));
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(" LV "+enemy.lv.ToString().PadLeft(2,' ')+"  EXP ?/?");
Console.ForegroundColor = ConsoleColor.Green;
Console.Write($" HP {hpbar} {hp.ToString().PadLeft(2,'0')}/{enemy.hp.ToString().PadLeft(2,'0')}");
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.Write($" AT {enemy.at}➶ ");
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine($" LU ?✤\n");
// --------------------------------------Player-----------------------------------------
Console.ForegroundColor = ConsoleColor.Cyan;
bar(player_hp,you.hp,"Player");
plaHL = 14-you.name.Length/2;
Console.WriteLine("「".PadLeft((plaHL),'▁')+you.name+"」".PadRight((plaHL),'▁'));
Console.ForegroundColor = ConsoleColor.White;
Levelviolence();
Console.WriteLine(" LV "+you.lv.ToString().PadLeft(2,' ')+$" EXP {you.exp}/{xpMath}");
Console.ForegroundColor = ConsoleColor.Green;
Console.Write($" HP {playerhpbar} {player_hp.ToString().PadLeft(2,'0')}/{you.hp.ToString().PadLeft(2,'0')}");
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.Write($" AT {you.at}➶ ");
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine($" LU {you.lu}✤\n");
loop();
if (hp <= 0 && stage < finalStage) // stage/floor increase when monster die
{
    secretEncounter();
    firstEncounter = true;
    updt();
    turns();
}
else if (player_hp <= 0) // when player die
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
    textDialog("\n\nˣ GAME OVER ˣ\n",125);
    Thread.Sleep(500);
    Console.Clear();
    for (int i = 0; i < 6; i++)
    {
        X_Beep(550,800-(150*i));
    }
    for (int l = 0; l < 22; l++)
    {
        X_Beep(550,50-l);
    }
    Console.Clear();
    Console.ResetColor();
}
else if (hp > 0 && stage <= finalStage) // when you still alive and enemy too, then repeat
{
    turns();
}
}
shopMenu.option1 = 1;
shopMenu.option2 = 2;
shopMenu.option3 = 3;
shopMenu.option4 = 4;
//shopDesign();
secretEncounter();
updt(); // first monster update status
turns(); // begin of battle