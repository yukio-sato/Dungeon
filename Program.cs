string monster, acao, you, hpbar = "";
bool derrotado, player_derrotado = false;
int stage = 1, rolagem = 1, result, hp, atk, player_hp, player_Maxhp, player_at,player_lu;
var enemy=(monster="",hp=0,atk=0, derrotado = false);
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
string bar()
{
    if (hp >= enemy.Item2)
    {
        hpbar = "♥♥♥♥♥";
    }
    else if (hp <= enemy.Item2-(enemy.Item2*0.2) && hp > enemy.Item2-(enemy.Item2*0.4))
    {
        hpbar = "♥♥♥♥♡";
    }
    else if (hp <= enemy.Item2-(enemy.Item2*0.4) && hp > enemy.Item2-(enemy.Item2*0.6))
    {
        hpbar = "♥♥♥♡♡";
    }
    else if (hp <= enemy.Item2-(enemy.Item2*0.6) && hp > enemy.Item2-(enemy.Item2*0.8))
    {
        hpbar = "♥♥♡♡♡";
    }
    else if (hp <= enemy.Item2-(enemy.Item2*0.8) && hp > 1)
    {
        hpbar = "♥♡♡♡♡";
    }
    else if (hp <= 0)
    {
        hpbar = "♡♡♡♡♡";
    }
    return hpbar;
}
void loaded()
{
    Console.ForegroundColor = ConsoleColor.White;
    frase("Pressione qualquer ⌨  tecla para continuar",12);
    frase(" . . . ➟\n",37);
    Console.ReadKey();
}
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
    enemy=(monster="Lobo Mau",hp=4,atk=5,derrotado = false);
    break;

    case 2:
    enemy=(monster="Lobo Bom",hp=20,atk=10,derrotado = false);
    break;

    default:
    break;
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
if (acao.Trim().ToLower().Substring(0,1) == "a")
{
    if (hp > 0)
    {
        int atk_test = player_at + dice(2,6,0);
        int enemy_def = enemy.Item3 + dice(2,6,0);
        if (atk_test > enemy_def)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            hp = hp - 2;
            frase("➹ Acertou o Ataque!\n",25);
        }
        else if (atk_test < enemy_def)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            player_hp = player_hp - 2;
            frase("➹ O inimigo contra-ataca!\n",25);
        }
        else if (atk_test == enemy_def)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            frase("➹ Ambos erram o Ataque!\n",25);
        }
        Thread.Sleep(75);
    }
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
        Console.WriteLine($"{monster} HP {bar()} {hp}/{enemy.Item2} AT {atk}➶");
        Console.ForegroundColor = ConsoleColor.Red;
        frase($"✝ {monster} foi derrotado! ✝\n", 25);
        loaded();
        if (stage >= 2)
        {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Clear();
        frase("❚ ❚ Fim da Demo! ❚ ❚",25);
        Console.ResetColor();
        }
    }
}
else if (acao.Trim().ToLower().Substring(0,1) == "d")
{
    if (hp > 0)
    {
        player_hp = player_hp - atk;
        hp = hp - player_at;
        Console.ForegroundColor = ConsoleColor.Red;
        frase("➹ Atacou!\n",25);
        Thread.Sleep(55);
    }
}
else
{
    loop();
}
}
void turns()
{
Console.Clear();
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"{monster} HP {bar()} {hp}/{enemy.Item2} AT {atk}➶");
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"{you} HP {player_hp}/{player_Maxhp} AT {player_at}➶ LU {player_lu}✤\n");
loop();
if (hp <= 0 && derrotado == true && stage < 2)
{
    stage++;
    updt();
    turns();
}
else if (player_hp <= 0 && player_derrotado == true)
{
    Console.ForegroundColor = ConsoleColor.Red;
    frase("ˣˣˣVocê perdeuˣˣˣ",37);
    frase(" . . . ➟\n",50);
    loaded();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Clear();
    frase("❚ ❚ Fim da Demo! ❚ ❚\n",25);
    Console.ResetColor();
}
else if (hp > 0 && derrotado == false && stage <= 2)
{
    turns();
}
}
updt();
turns();