#pragma warning disable CA1416 // beep warning disabler
Console.OutputEncoding = System.Text.Encoding.UTF8; // emoji/ special character fix

string 
monster, // monster name 
action, // type of action choosed 
you = "", // player name 
hpbar = "", // visual hp bar string
playerhpbar = "", // visual hp bar string
luck_mode = "", // luck status (now)
interaction1 = "", // first interact
interaction2 = "", // second interact
interaction3 = "", // third interact
interaction4 = ""; // four interact

bool defeat = false, // monster defeat
player_defeat = false,
firstEncounter = true; // player defeat

int stage = 1, finalStage = 11, // stage settings
rolled = 1, result, // dice placeholder
hp, atk, // monster stats (now)
encounter, // monster encounter when begin
player_hp, player_Maxhp, player_at, player_lu, // player stats
luck_Test, atk_Test, def_Test, // player/enemy tests
atk_dmg = 0, def_dmg = 0, // damage player/enemy for atk and def
heartLength = 5, // the number of character of Heart Bar
interactNumber = 1; // random number for interact


var enemy=(monster="",hp=0,atk=0); // where set the above var
var narrador=(interaction1="",interaction2="",interaction3="",interaction4=""); // text writer about monster/interactions!

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
Console.Write($"✎ Digite o Nome: {you.PadRight(12,'_')}"); // fill with underline (custom)

ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var

if (pressed.Key != ConsoleKey.Backspace && pressed.Key != ConsoleKey.Enter && you.Length <= 12)
{
    if (you.Length < 12) // fill blank/underline characters
    {
    you += pressed.KeyChar;
    }
    else if (you.Length >= 12) // if name got max length, then subtitute last character to new
    {
    you = you.Substring(0,you.Length-1)+pressed.KeyChar;
    }
    nameAsk();
}
else if (pressed.Key == ConsoleKey.Backspace) // remove last character
{
    if (you.Length > 0)
    {
    you = you.Substring(0,you.Length-1);
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
player_at = dice(1,6,6);
Console.ForegroundColor = ConsoleColor.DarkRed;
textDialog($"⟨➶  AT: {player_at}",25);
player_Maxhp = dice(1,6,6);
player_hp = player_Maxhp;
Console.ForegroundColor = ConsoleColor.Green;
textDialog($" ❤  HP: {player_hp}",25);
player_lu = dice(2,6,12);
Console.ForegroundColor = ConsoleColor.DarkGreen;
textDialog($" ✤  LU: {player_lu}⟩\n",25);
loaded();
void updt()
{
switch (stage)
{
    case 1:
    enemy=("Lobo Cinzento",3,3);
    narrador=(
        $"{enemy.Item1} ataca ferozamente!",
        $"{enemy.Item1} late ferozamente!",
        $"{enemy.Item1} pula ferozamente!",
        $"{enemy.Item1} deita ferozamente?"
    );
    break;
    case 2:
    enemy=("Lobo Branco",3,3);
    narrador=(
        $"{enemy.Item1} ataca calmamente.",
        $"{enemy.Item1} late calmamente.",
        $"{enemy.Item1} pula calmamente.",
        $"ZZZZZZZZZ. . ."
    );
    break;
    case 3:
    enemy=("Goblin",5,4);
    narrador=(
        $"{enemy.Item1} te encara!",
        $"{enemy.Item1} começa a saltitar!",
        $"{enemy.Item1} faz. . . algo impossível de descrever?",
        $"{enemy.Item1} aponta sua lança! PERA! onde veio isto!?"
    );
    break;
    case 4:
    enemy=("Orc Vesgo",5,5);
    narrador=(
        $"{enemy.Item1} acabou de intimidar até sua alma.",
        $"{enemy.Item1} tentou enxergar você, mas falha miseravelmente.",
        $"{enemy.Item1} cansou-se e começa a ler um livro, parece um de literatura.",
        $"{enemy.Item1} começa a falar algo sem sentido?"
    );
    break;
    case 5:
    enemy=("Orc Barbudo",5,5);
    narrador=(
        $"{enemy.Item1} demonstra sua barba!",
        $"{enemy.Item1} reflete a luz!",
        $"{enemy.Item1} começa a escovar sua barba!",
        $"{enemy.Item1} fez nada?"
    );
    break;
    case 6:
    enemy=("Zumbi Manco",6,7);
    narrador=(
        $"{enemy.Item1} corre rapidamente em sua direção!",
        $"{enemy.Item1} ele questiona o sentido da vida.",
        $"{enemy.Item1} ele começa a pular corda!",
        $"{enemy.Item1} reponhe seus ossos?"
    );
    break;
    case 7:
    enemy=("Zumbi Balofo",6,7);
    narrador=(
        $"{enemy.Item1} urg (Não consigo respirar!)",
        $"{enemy.Item1} está fedendo. . .",
        $"{enemy.Item1} abre sua boca em minha direçã-",
        $"Narrador morre após fedor de {enemy.Item1}. . ."
    );
    break;
    case 8:
    enemy=("Troll",8,7);
    narrador=(
        $"{enemy.Item1} fez uma TROLLagem em você!",
        $"{enemy.Item1} desconfortavel começa a sorrir.",
        $"{enemy.Item1} o sorriso te lembra de algo?",
        $"{enemy.Item1}Face deu umas risadas."
    );
    break;
    case 9:
    enemy=("Ogro",8,9);
    narrador=(
        $"{enemy.Item1} senta-se.",
        $"{enemy.Item1} balança sua arma.",
        $"{enemy.Item1} começa contemplar a natureza.",
        $"{enemy.Item1} esqueceu do motivo para lutar."
    );
    break;
    case 10:
    enemy=("Ogro Furioso",10,9);
    narrador=(
        $"{enemy.Item1} lembrou-se do motivo para lutar!",
        $"{enemy.Item1} começa contemplar sua dor!",
        $"{enemy.Item1} prepara sua posição de combate!",
        $"{enemy.Item1} arremessa lama na tua cara!"
    );
    break;
    case 11:
    enemy=("Necromante Maligno",12,12);
    narrador=(
        $"O ambiente começa a ficar mais tenso. . .",
        $"O cemitério, os ventos, te incomodam. . .",
        $"A aventura de {you} chegou ao fim. . .",
        $"Você só queria terminar essa Masmorra. . ."
    );
    break;
    default:
    enemy=($"{you}?",player_Maxhp,player_at);
    narrador=(
        $"O cenário vazio te deixa bastante confuso e desconfortavel. . .",
        $". . .",
        $"Sua mente se esvazia conforme a batalha progressede. . .",
        $"Sua Determinação. . . Sua Criação. . . vai colocar um Fim. . ."
    );
    break;
}
// monster stats update
monster = enemy.Item1;
hp = enemy.Item2;
atk = enemy.Item3;
}
void morte()
{
    if (player_hp <= 0 && player_defeat == false) // player dead sound/animation
    {
        player_hp = 0;
        player_defeat = true;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        textDialog($"✝ {you} caiu! ✝",25);
        textDialog(" . . . ➟\n",125);
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(950-(i*50),900-(i*100));
        }
        Console.Beep(750,1000);
    }
    else if (hp <= 0 && defeat == false) // if monster is defeat
    {
        hp = 0;
        defeat = true;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        bar(hp,enemy.Item2, "Monster");
        Console.WriteLine("「".PadLeft((14-monster.Length/2),'▁')+monster+"」".PadRight((14-monster.Length/2),'▁'));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" HP {hpbar} {hp.ToString().PadLeft(2,'0')}/{enemy.Item2.ToString().PadLeft(2,'0')}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($" AT {atk}➶\n");
        Console.ForegroundColor = ConsoleColor.Red;
        textDialog($"✝ {monster} foi derrotado! ✝\n", 25);
        Console.Beep(900,120);
        Console.Beep(900,120);
        Console.Beep(900,120);
        Console.Beep(900,120);
        Console.Beep(1100,270);
        if (stage >= finalStage) // if you are on the last stage when enemy die (verify)
        {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Clear();
        textDialog($"❚ ❚ {you} matou todos os monstros! ❚ ❚\n",25);
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
void loop() // actions: [A] attack, [D] defend or [S] luck
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
        textDialog(narrador.Item1+"\n",25);
    break;
    case 2:
        textDialog(narrador.Item2+"\n",25);
    break;
    case 3:
        textDialog(narrador.Item3+"\n",25);
    break;
    default:
        textDialog(narrador.Item4+"\n",25);
    break;
}

Console.WriteLine("╔─── Ações ──────────────╗");// begin of action box
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.WriteLine("│ Ⓐ tacar                │");
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("│ Ⓓ efender              │");
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("│ Ⓢ orte                 │");
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
        textDialog($"✤ {you} testou a Sorte!\n",25);
        luck_Test = dice(2,6,0);
        for (int i = 0; i < 4; i++)
        {
            Console.Beep(800,100);
        }
        Console.Beep(900,250);
        if (luck_Test <= player_lu)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            textDialog("Sortudo\n",45);
            luck_mode = "lucky";
            loaded();
        }
        else if (luck_Test > player_lu)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog("Azarado\n",45);
            luck_mode = "unlucky";
            loaded();
        }
        player_lu -= 1;
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
        atk_Test = player_at + dice(2,6,0);
        def_Test = enemy.Item3 + dice(2,6,0);
        if (atk_Test > def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            hp = hp - atk_dmg;
            textDialog($"➹ {you} Ataca!\n",25);
            dmgSound("Player");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {monster} perdeu {atk_dmg} HP\n",12);
            dmgSound("");
        }
        else if (atk_Test < def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            player_hp = player_hp - def_dmg;
            textDialog($"➹ {monster} Contra-ataca!\n",25);
            dmgSound("Monster");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {you} perdeu {def_dmg} HP\n",12);
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
        def_Test = player_Maxhp + dice(2,6,0);
        atk_Test = enemy.Item2 + dice(2,6,0);
        if (def_Test > atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (atk_Test < player_at)
            {
            hp = hp - (atk_dmg-1);
            textDialog($"➹ {you} Contra-ataca!\n",25);
            dmgSound("Player");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {monster} perdeu {atk_dmg-1} HP\n",12);
            dmgSound("");
            }
            else if (atk_Test >= player_at)
            {
            player_hp = player_hp - (def_dmg-1);
            textDialog($"➹ {you} Bloqueiou!\n",25);
            dmgSound("Monster");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {you} perdeu {def_dmg-1} HP\n",12);
            dmgSound("");
            }
        }
        else if (def_Test < atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (def_Test < atk)
            {
            player_hp = player_hp - (def_dmg-1);
            textDialog($"➹ {monster} Ataca!\n",25);
            dmgSound("Monster");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {you} perdeu {def_dmg-1} HP\n",12);
            dmgSound("");
            }
            else if (def_Test >= atk)
            {
            hp = hp - (atk_dmg-1);
            textDialog($"➹ {monster} Bloqueiou!\n",25);
            dmgSound("Player");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            textDialog($"♡ {monster} perdeu {atk_dmg-1} HP\n",12);
            dmgSound("");
            }
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
}

void turns() // battle stats + loop
{
Console.Clear();
// --------------------------------------Monster-----------------------------------------
Console.ForegroundColor = ConsoleColor.DarkRed;
bar(hp,enemy.Item2,"Monster");
Console.WriteLine("「".PadLeft((14-monster.Length/2),'▁')+monster+"」".PadRight((14-monster.Length/2),'▁'));
Console.ForegroundColor = ConsoleColor.Green;
Console.Write($" HP {hpbar} {hp.ToString().PadLeft(2,'0')}/{enemy.Item2.ToString().PadLeft(2,'0')}");
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.WriteLine($" AT {atk}➶\n");
// --------------------------------------Player-----------------------------------------
Console.ForegroundColor = ConsoleColor.Cyan;
bar(player_hp,player_Maxhp,"Player");
Console.WriteLine("「".PadLeft((14-you.Length/2),'▁')+you+"」".PadRight((14-you.Length/2),'▁'));
Console.ForegroundColor = ConsoleColor.Green;
Console.Write($" HP {playerhpbar} {player_hp.ToString().PadLeft(2,'0')}/{player_Maxhp.ToString().PadLeft(2,'0')}");
Console.ForegroundColor = ConsoleColor.DarkRed;
Console.Write($" AT {player_at}➶ ");
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine($" LU {player_lu}✤\n");
loop();
if (hp <= 0 && defeat == true && stage < finalStage) // stage/floor increase when monster die
{
    stage++;
    defeat = false;
    firstEncounter = true;
    updt();
    turns();
}
else if (player_hp <= 0 && player_defeat == true) // when player die
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    textDialog(" . . . ➟  ",200);
    Console.ReadKey();
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
else if (hp > 0 && defeat == false && stage <= finalStage) // when you still alive and enemy too, then repeat
{
    turns();
}
}

encounter = dice(1,100,0);
if (encounter <= 5) // chance for YOU?
{
    stage = 0;
}

updt(); // first monster update status
turns(); // begin of battle