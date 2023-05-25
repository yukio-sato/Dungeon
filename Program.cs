
string monster, acao;
double hp, atk;
bool derrotado;
int stage = 1;
    var enemy=(
    monster="",
    hp=0,
    atk=0,
    derrotado = false
    );
void updt()
{
switch (stage)
{
    case 1:
    enemy=(
    monster="Lobo Mau",
    hp=5,
    atk=0,
    derrotado = false
    );
    break;
    case 2:
    enemy=(
    monster="Lobo Bom",
    hp=1,
    atk=0,
    derrotado = false
    );
    break;
    default:
    break;
}
}
void loop()
{
Console.WriteLine("Atacar ou Defender?");
acao = Console.ReadLine()!;
if (acao.Trim().ToLower().Substring(0,1) == "a")
{
    Console.WriteLine("Atacou!");
    if (hp > 0)
    {
    hp -= 1;
    }
    if (hp <= 0 && derrotado == false)
    {
        hp = 0;
        derrotado = true;
        Console.WriteLine($"{monster} foi derrotado!");
        Console.WriteLine("Pressione qualquer tecla para continuar. . .");
        Console.ReadKey();
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
Console.WriteLine($"{monster} HP {hp}/{enemy.Item2} ATK {atk}");
loop();
if (hp <= 0 && derrotado == true && stage < 2)
{
    stage++;
    updt();
    turns();
}
else if (hp > 0 && derrotado == false && stage < 2)
{
    turns();
}
else if (stage >= 2)
{
    Console.Clear();
    Console.WriteLine("Fim da Demo!");
}
}
updt();
turns();