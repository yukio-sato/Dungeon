Console.OutputEncoding = System.Text.Encoding.UTF8;
string 
monster, // monster name 
acao, // type of action choosed 
you, // player name 
hpbar = "", // visual hp bar string
luck_mode = ""; // luck status (now)

bool derrotado, // monster defeat
player_derrotado = false; // player defeat

int stage = 1, finalStage = 11, // stage settings
rolagem = 1, result, // dice placeholder
hp, atk, // monster stats (now)
player_hp, player_Maxhp, player_at, player_lu, // player stats
luck_Test, atk_Test, def_Test, // player/enemy tests
atk_dmg = 0, def_dmg = 0; // damage player/enemy for atk and def;
var enemy=(monster="",hp=0,atk=0, derrotado = false); // where set the above var

void frase(string txt, int cooldown)
{
    for (int i = 0; i < txt.Length; i++)
    {
        Console.Write(txt[i]);
        if (txt[i].ToString() != " " && txt[i].ToString() != "")
        {
        Thread.Sleep(cooldown);
        }
    }
}
int dice(int quantity,int maximo, int bns)
{
    result = 0;
    for (int i = 0; i < quantity; i++)
    {
        rolagem = new Random().Next(0,maximo+1);
        if (result <= 0)
        {result = rolagem;}
        else
        {result += rolagem;}
    }
    return (result+bns);
}
void bar(int now, int max)
{
    if (now >= max)
    {
        hpbar = "♥♥♥♥♥";
    }
    else if (now <= max-(max*0.2) && now > max-(max*0.4))
    {
        hpbar = "♥♥♥♥♡";
    }
    else if (now <= max-(max*0.4) && now > max-(max*0.6))
    {
        hpbar = "♥♥♥♡♡";
    }
    else if (now <= max-(max*0.6) && now > max-(max*0.8))
    {
        hpbar = "♥♥♡♡♡";
    }
    else if (now <= max-(max*0.8) && now > 0)
    {
        hpbar = "♥♡♡♡♡";
    }
    else if (now <= 0)
    {
        hpbar = "♡♡♡♡♡";
    }
}
void loaded()
{
    Console.ForegroundColor = ConsoleColor.White;
    frase("Pressione qualquer ⌨  tecla para continuar",10);
    frase(" . . . ➟\n",37);
    Console.ReadKey();
}
Console.Clear();
Console.ForegroundColor = ConsoleColor.Gray;
frase("✎ Digite o nome: ",25);
you = Console.ReadLine()!.Trim();
player_at = dice(1,6,6);
Console.ForegroundColor = ConsoleColor.Red;
frase($"⟨➶  AT: {player_at}",25);
player_Maxhp = dice(1,6,6);
player_hp = player_Maxhp;
Console.ForegroundColor = ConsoleColor.Green;
frase($" ❤  HP: {player_hp}",25);
player_lu = dice(2,6,12);
Console.ForegroundColor = ConsoleColor.DarkGreen;
frase($" ✤  LU: {player_lu}⟩\n",25);
loaded();
void updt()
{
switch (stage)
{
    case 1:
    enemy=(monster="Lobo Cinzento",hp=3,atk=3,derrotado = false);
    break;
    case 2:
    enemy=(monster="Lobo Branco",hp=3,atk=3,derrotado = false);
    break;
    case 3:
    enemy=(monster="Goblin",hp=5,atk=4,derrotado = false);
    break;
    case 4:
    enemy=(monster="Orc Vesgo",hp=5,atk=5,derrotado = true);
    break;
    case 5:
    enemy=(monster="Orc Barbudo",hp=5,atk=5,derrotado = false);
    break;
    case 6:
    enemy=(monster="Zumbi Manco",hp=6,atk=7,derrotado = false);
    break;
    case 7:
    enemy=(monster="Zumbi Balofo",hp=6,atk=7,derrotado = false);
    break;
    case 8:
    enemy=(monster="Troll",hp=8,atk=7,derrotado = false);
    break;
    case 9:
    enemy=(monster="Ogro",hp=8,atk=9,derrotado = false);
    break;
    case 10:
    enemy=(monster="Ogro Furioso",hp=10,atk=9,derrotado = false);
    break;
    case 11:
    enemy=(monster="Necromante Maligno",hp=12,atk=12,derrotado = false);
    break;
    default:
    break;
}
}
void morte()
{
    if (player_hp <= 0 && player_derrotado == false)
    {
        player_hp = 0;
        player_derrotado = true;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        frase($"✝ {you} caiu! ✝",25);
        frase(" . . . ➟\n",50);
    }
    else if (hp <= 0 && derrotado == false)
    {
        hp = 0;
        derrotado = true;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        bar(hp,enemy.Item2);
        Console.WriteLine($"{monster} HP {hpbar} {hp}/{enemy.Item2} AT {atk}➶");
        Console.ForegroundColor = ConsoleColor.Red;
        frase($"✝ {monster} foi derrotado! ✝\n", 25);
        if (stage >= finalStage)
        {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Clear();
        frase("❚ ❚ Fim da Demo! ❚ ❚\n",25);
        Console.ResetColor();
        }
    }
}
void status_math()
{
    if (luck_mode == "")
    {
        atk_dmg = 2;
        def_dmg = 2;
    }
    else if (luck_mode == "lucky")
    {
        atk_dmg = 4;
        def_dmg = 1;
    }
    else if (luck_mode == "unlucky")
    {
        atk_dmg = 1;
        def_dmg = 3;
    }
}
void loop()
{
Console.ForegroundColor = ConsoleColor.White;
frase("╔─── Ações ──────────────╗\n",0);
Console.ForegroundColor = ConsoleColor.Red;
frase("│ Ⓐ tacar                │\n",25);
Console.ForegroundColor = ConsoleColor.DarkGray;
frase("│ Ⓓ efender              │\n",25);
Console.ForegroundColor = ConsoleColor.DarkGreen;
frase("│ Ⓢ orte                 │\n",25);
Console.ForegroundColor = ConsoleColor.White;
frase("╚────────────────────────╝\n",0);
acao = Console.ReadLine()!;
if (acao.Trim() == "")
{
    acao = ".";
}
if (acao.Trim().ToLower().Substring(0,1) == "s")
{
    if (hp > 0 && luck_mode == "")
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        frase($"✤ {you} testou a Sorte!\n",25);
        luck_Test = dice(2,6,0);
        if (luck_Test <= player_lu)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            frase("Sortudo\n",45);
            luck_mode = "lucky";
            loaded();
        }
        else if (luck_Test > player_lu)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            frase("Azarado\n",45);
            luck_mode = "unlucky";
            loaded();
        }
        player_lu -= 1;
    }
    else if (hp > 0 && luck_mode != "")
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        frase("Você já testou sua Sorte!\n",37);
        loaded();
    }
}

if (acao.Trim().ToLower().Substring(0,1) == "a")
{
    if (hp > 0)
    {
        status_math();
        luck_mode = "";
        atk_Test = player_at + dice(2,6,0);
        def_Test = enemy.Item3 + dice(2,6,0);
        if (atk_Test > def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            hp = hp - atk_dmg;
            frase($"➹ {you} Acertou!\n",25);
        }
        else if (atk_Test < def_Test)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            player_hp = player_hp - def_dmg;
            frase($"➹ {monster} contra-ataca!\n",25);
        }
        else if (atk_Test == def_Test)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            frase("➹ Ambos erram o Ataque!\n",25);
        }
        morte();
        loaded();
    }
}
else if (acao.Trim().ToLower().Substring(0,1) == "d")
{
    if (hp > 0)
    {
        status_math();
        luck_mode = "";
        def_Test = player_Maxhp + dice(2,6,0);
        atk_Test = enemy.Item2 + dice(2,6,0);
        if (def_Test > atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            hp = hp - atk_dmg;
            frase($"➹ {you} contra-ataca!\n",25);
        }
        else if (def_Test < atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            player_hp = player_hp - def_dmg;
            frase($"➹ {monster} Acertou!\n",25);
        }
        else if (def_Test == atk_Test)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            frase("➹ Ambos erram o Ataque!\n",25);
        }
        morte();
        loaded();
    }
}
}
void turns()
{
Console.Clear();
Console.ForegroundColor = ConsoleColor.White;
bar(hp,enemy.Item2);
Console.WriteLine($"{monster} HP {hpbar} {hp}/{enemy.Item2} AT {atk}➶");
Console.ForegroundColor = ConsoleColor.Cyan;
bar(player_hp,player_Maxhp);
Console.WriteLine($"{you} HP {hpbar} {player_hp}/{player_Maxhp} AT {player_at}➶ LU {player_lu}✤\n");
loop();
if (hp <= 0 && derrotado == true && stage < finalStage)
{
    stage++;
    updt();
    turns();
}
else if (player_hp <= 0 && player_derrotado == true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    frase("ˣˣˣVocê perdeuˣˣˣ",37);
    frase(" . . . ➟\n",50);
    loaded();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Clear();
    frase("❚ ❚ Fim da Demo! ❚ ❚\n",25);
    Console.ResetColor();
}
else if (hp > 0 && derrotado == false && stage <= finalStage)
{
    turns();
}
}
updt();
turns();