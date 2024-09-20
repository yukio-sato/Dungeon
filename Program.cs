using System;
using System.IO;
using System.Diagnostics;
Console.OutputEncoding = System.Text.Encoding.UTF8; // emoji/ special character fix

#pragma warning disable CA1416 // beep warning disabler
string hpbar = "", // Visual hp bar string
playerhpbar = "", // Visual hp bar string
luck_mode = "",  // If player is lucky or unlucky
playerNM = "", // Player name
monsterNM = "", // Monster name
itemNM = "", // Item name
itemDesc = "", // Item description
itemSelectDesc = ""; // Item select description

char heartShape = '━', // Heart Shape
heartNull = '─', // Damaged Heart
startNm = '「', // Start for nickname
endNm = '」', // End for nickname
sliceBar = '▁', // Design for Bar for Monster and Player
shopSummary = '.'; // Design for Bar in item shop like: item........

ConsoleColor normalHeart = ConsoleColor.Green, // default color for fullied Heart
voidHeart = ConsoleColor.DarkRed, // default color for damaged Heart
karmaHeart = ConsoleColor.DarkMagenta, // default color for karma Heart
bloodHeart = ConsoleColor.DarkRed; // default color for karma Heart

bool firstEncounter = true, // first interact for monster encounter
onShop = false, // shop detector
onInventory = false, // inventory detector
realStatus = false, // hide real Status
secretMode = true, // Enables Secret Encounters Aka Hardmode
secretEcounter = false, // Enabled when you are encoutering a Secret
soundEnable = true, // Effect Sound enabled
musicEnable = true, // Enable music battle
anti_spam = false; // When you press any key just jumping dialogs


double secret = 0, // monster random encounter
atkBns = 0, // attack bonus when realstatus actual from monster
hp, // health actual from monster
st, // stamina actual from monster
xp, // xp actual from monster
g, // gold actual from monster
player_hp, // health actual from player
player_kr = 0, // Karma actual from player
player_bl = 0, // BL actual from player
luck_Test = 0, atk_Test = 0, def_Test = 0, // player/enemy tests
atk_dmg = 0, def_dmg = 0, // damage player/enemy for atk and def
next = 25, // exp modify for next love
xpMath = 10, // math of next exp for love
gold = 0; // player gold

int stage = 0, finalStage = 15, // stage settings
rolled = 1, result, // dice placeholder
encounter, // monster random encounter
heartLength = 5, // the number of character of Heart Bar
shopLength = 8, // the number of character of Shop List
inveLength = 8, // the number of character in Item Inventory Menu
plaHL, monHL, // monster/player half length math for design
selected = 1, // selected on inventory/shop
actionSelect = 1, // select for action
cursedAction = 0, // disabled a action
interactNumber = 0, // random number for interact
foodLength = 7, // item max Length
choiceSetup = 0, // for progress when answered!
shopSlot1 = 0, // Selected food on shop
shopSlot2 = 0, // Selected food on shop
shopSlot3 = 0, // Selected food on shop
shopSlot4 = 0, // Selected food on shop
dialogSpeed = 2; // Dialog Basic Speed

double[] you=[
    0.0, // [0] HP
    0.0, // [1] ATK
    0.0, // [2] LUCK
    1,   // [3] LV
    0.0  // [4] EXP
];

double[] enemy=[
    0.0, // [0] HP
    0.0, // [1] ATK
    0.0, // [2] LV
    0.0, // [3] Stamina
    0.0, // [4] KARMA
    0.0, // [5] BLOOD
    0    // [6] Linked Battle
];

string [] narrador = [
    "", // [0] Interaction
    "", // [1]
    "", // [2]
    ""  // [3]
];

double[] item = [
    0.0, // [0] Cost
    0.0, // [1] Max
    0.0, // [2] Heal
    0.0, // [3] ATK Bonus
    0.0, // [4] LUCK Bonus
    0.0, // [5] Appear Chance
];

string[] inventory = [
    "─", // [0] Slot (format: name_x)
    "─", // [1]
    "─", // [2]
    "─"  // [3]
];

int[] foodId = [
    0, // [0] Food ID
    0, // [1]
    0, // [2]
    0  // [3]
];

int[] shopMenu = [
    0, // [0] Option 1
    0, // [1] Option 2
    0, // [2] Option 3
    0  // [3] Option 4
];

void heartExamples(char normal, char voided){
    for (int i = 0; i < 4; i++){
        ConsoleColor heartRGB = voidHeart;
        if (i < 3){
            if (i == 0){
                heartRGB = normalHeart;
            }
            else if(i == 1){
                heartRGB = karmaHeart;
            }
            else{
                heartRGB = bloodHeart;
            }
            textDialog(normal.ToString(),0,heartRGB);
        }
        else{
            textDialog(voided.ToString(),0,heartRGB);
        }
    }
    Console.WriteLine();
    Console.ResetColor();
}

void changingHeart(char origin, char nullOrigins){
    heartShape = origin;
    heartNull = nullOrigins;
}

void coloringHeart(ConsoleColor painting){
    switch (choiceSetup){
        case 1:
            normalHeart = painting;
        break;
        case 2:
            voidHeart = painting;
        break;
        default:
        break;
    }
}

void colorList(){
    textDialog("[1] Amarelo\n",0,ConsoleColor.Yellow);
    textDialog("[2] Verde\n",0,ConsoleColor.Green);
    textDialog("[3] Vermelho\n",0,ConsoleColor.Red);
    textDialog("[4] Azul\n",0,ConsoleColor.Blue);
    textDialog("[5] Ciano\n",0,ConsoleColor.Cyan);
    textDialog("[6] Preto\n",0,ConsoleColor.Black);
    textDialog("[7] Cinza Escuro\n",0,ConsoleColor.DarkGray);
    textDialog("[8] Cinza\n",0,ConsoleColor.Gray);
    textDialog("[9] Branco\n",0,ConsoleColor.White);
}

void choicingColor(int numbered){
    switch (numbered){
        case 1:
            coloringHeart(ConsoleColor.Yellow);
        break;
        case 2:
            coloringHeart(ConsoleColor.Green);
        break;
        case 3:
            coloringHeart(ConsoleColor.Red);
        break;
        case 4:
            coloringHeart(ConsoleColor.Blue);
        break;
        case 5:
            coloringHeart(ConsoleColor.Cyan);
        break;
        case 6:
            coloringHeart(ConsoleColor.Black);
        break;
        case 7:
            coloringHeart(ConsoleColor.DarkGray);
        break;
        case 8:
            coloringHeart(ConsoleColor.Gray);
        break;
        default:
            coloringHeart(ConsoleColor.White);
        break;
    }
}

void designBar(string txtChar){
    switch (choiceSetup){
        case 4:
            sliceBar = char.Parse(txtChar.Substring(0,1));
        break;
        case 5:
            startNm = char.Parse(txtChar.Substring(1,1));
            endNm = char.Parse(txtChar.Substring(2,1));
        break;
    }
}

void choicingBar(int numbered){
    switch (numbered){
        case 1:
            designBar("⑊「」");
        break;
        case 2:
            designBar("…⌯⌯");
        break;
        case 3:
            designBar("—˹˼");
        break;
        case 4:
            designBar("═╣╠");
        break;
        case 5:
            designBar("▁〈〉");
        break;
        case 6:
            designBar("■〚〛");
        break;
        case 7:
            designBar("   ");
        break;
        case 8:
            designBar(":╾╼");
        break;
        default:
            designBar("=←→");
        break;
    }
}

void answerConfig(){
    Console.Clear();
    textDialog("Sua Atual Configuração:\n",0,ConsoleColor.White);
    textDialog("".PadLeft(7,sliceBar)+startNm+"Name"+endNm+"".PadRight(7,sliceBar)+"\n",0,ConsoleColor.White);
    textDialog("HP ".PadRight(3+(heartLength/3),heartShape),0,normalHeart);
    textDialog("".PadRight(heartLength/3,heartShape),0,karmaHeart);
    textDialog("".PadRight(heartLength/3,heartNull),0,voidHeart);
    textDialog($" {heartLength/2}/{heartLength}\n",0,normalHeart);
    textDialog("Configuração Desejada? [S/N]\n",0,ConsoleColor.White);
    string answer = Console.ReadLine()!;
    if (answer.Length > 0){
        if (answer.Substring(0,1).ToLower() == "s"){}
        else if (answer.Substring(0,1).ToLower() == "n"){
            choiceSetup = 0;
            choice();
        }else{
            answerConfig();
        }
    }else{
        answerConfig();
    }
}

void soundBlock(){
    Console.Clear();
    textDialog("Habilitar Som? [S/N]\n",-9,ConsoleColor.Cyan);
    Console.ForegroundColor = ConsoleColor.White;
    string answer = Console.ReadKey().KeyChar.ToString().ToLower();
    if (answer == "n"){
        soundEnable = false;
    }
    else if (answer == "s"){
        soundEnable = true;
    } else{
        soundBlock();
    }
}

void musicBlock(){
    Console.Clear();
    textDialog("Habilitar Música? [S/N]\n",-9,ConsoleColor.Yellow);
    Console.ForegroundColor = ConsoleColor.White;
    string answer = Console.ReadKey().KeyChar.ToString().ToLower();
    if (answer == "n"){
        musicEnable = false;
    }
    else if (answer == "s"){
        musicEnable = true;
    } else{
        musicBlock();
    }
}

void hardMode(){
    Console.Clear();
    textDialog("Habilitar HardMode [S/N]\n",-9,ConsoleColor.DarkRed);
    Console.ForegroundColor = ConsoleColor.White;
    string answer = Console.ReadKey().KeyChar.ToString().ToLower();
    if (answer == "n"){
        secretMode = false;
    }
    else if (answer == "s"){
        secretMode = true;
    } else{
        hardMode();
    }
}

void OpenFolder(string folderPath, string appName){
    if (musicEnable == true){
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        string _path = folderPath+appName;
        startInfo.Arguments = string.Format("/C start {0}", _path);
        process.StartInfo = startInfo;
        process.Start();
    }
}

void choice(){
    Console.ResetColor();
    Console.Clear();
    switch (choiceSetup){
        case 0:
            Console.WriteLine("Exemplo de Barra de Vida");
            Console.Write("[1] ");
            heartExamples('■','□');
            Console.Write("[2] ");
            heartExamples('◆','◇');
            Console.Write("[3] ");
            heartExamples('●','○');
            Console.Write("[4] ");
            heartExamples('━','─');
            Console.Write("[5] ");
            heartExamples('♥','♡');
            Console.Write("[6] ");
            heartExamples('█','▒');
            Console.Write("[7] ");
            heartExamples('⑉','—');
            Console.Write("[8] ");
            heartExamples('⑊','\\');
            Console.Write("[9] ");
            heartExamples('❚','⋮');
        break;
        case 1:
            textDialog($"Cor de {heartShape.ToString()}  Vida Intacta.\n",0,normalHeart);
            colorList();
        break;
        case 2:
            textDialog($"Cor de {heartNull.ToString()}  Vida Perdida.\n",0,voidHeart);
            colorList();
        break;
        case 3:
            textDialog($"Quantidade de barras de {heartShape}  Vida de Atual (1 a 9)\n",0,ConsoleColor.White);
        break;
        case 4:
            textDialog("Design de Barra de Divisor\n",0,ConsoleColor.White);
            textDialog("1° ⑊⑊⑊⑊⑊⑊⑊⑊⑊⑊⑊⑊⑊⑊\n",0,ConsoleColor.White);
            textDialog("2° ……………………………………\n",0,ConsoleColor.White);
            textDialog("3° ——————————————\n",0,ConsoleColor.White);
            textDialog("4° ══════════════\n",0,ConsoleColor.White);
            textDialog("5° ▁▁▁▁▁▁▁▁▁▁▁▁▁▁\n",0,ConsoleColor.White);
            textDialog("6° ■■■■■■■■■■■■■■\n",0,ConsoleColor.White);
            textDialog("7°               \n",0,ConsoleColor.White);
            textDialog("8° ::::::::::::::\n",0,ConsoleColor.White);
            textDialog("9° ==============\n",0,ConsoleColor.White);
        break;
        case 5:
            textDialog("Design de Fechamento de Nome\n",0,ConsoleColor.White);
            textDialog("1° 「Name」\n",0,ConsoleColor.White);
            textDialog("2° ⌯Name⌯\n",0,ConsoleColor.White);
            textDialog("3° ˹Name˼\n",0,ConsoleColor.White);
            textDialog("4° ╣Name╠\n",0,ConsoleColor.White);
            textDialog("5° 〈Name〉\n",0,ConsoleColor.White);
            textDialog("6° 〚Name〛\n",0,ConsoleColor.White);
            textDialog("7°  Name \n",0,ConsoleColor.White);
            textDialog("8° ╾Name╼\n",0,ConsoleColor.White);
            textDialog("9° ←Name→\n",0,ConsoleColor.White);
        break;
        case 6:
            textDialog("Velocidade de Texto (1 a 9)\nNota: quanto mais baixo for o número, mais rápido será.\n",0,ConsoleColor.White);
        break;
        default:
        break;
    }
    ConsoleKeyInfo pressing = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
    bool reallyInt = int.TryParse(pressing.KeyChar.ToString(),out int ignore);
    int numberPressed = 0;
    if (reallyInt == true){
        numberPressed = Convert.ToInt32(pressing.KeyChar.ToString());
    }
    if (numberPressed > 0 && numberPressed <= 9){
        switch (choiceSetup){
            case 0:
                switch (numberPressed){
                case 1:
                    changingHeart('■','□');
                break;
                case 2:
                    changingHeart('◆','◇');
                break;
                case 3:
                    changingHeart('●','○');
                break;
                case 4:
                    changingHeart('━','─');
                break;
                case 5:
                    changingHeart('♥','♡');
                break;
                case 6:
                    changingHeart('█','▒');
                break;
                case 7:
                    changingHeart('⑉','—');
                break;
                case 8:
                    changingHeart('⑊','\\');
                break;
                default:
                    changingHeart('❚','⋮');
                break;
                }
            break;
            case 1:
                choicingColor(numberPressed);
            break;
            case 2:
                choicingColor(numberPressed);
            break;
            case 3:
                heartLength = numberPressed;
            break;
            case 4:
                choicingBar(numberPressed);
            break;
            case 5:
                choicingBar(numberPressed);
            break;
            case 6:
                dialogSpeed = numberPressed+1;
            break;
            default:
            break;
        }
        
        choiceSetup++;
        if (choiceSetup <= 6){
            choice();
        }
        else{
            answerConfig();
        }
    }
    else{
        choice();
    }
}

void secretEncounter(){
    if (secretMode){ // enable secret encounter in hardmode
        encounter = dice(1,100,0);
    } else{ // normal mode without secrets!
        encounter = 100;
    }

    if (enemy[6] > 0){
        secretEcounter = true;
        secret = enemy[6];
        if (secret == 2){
            Console.Clear();
            textDialog(startNm.ToString(),12,ConsoleColor.White);
            textDialog("Sans",12,ConsoleColor.Yellow);
            textDialog(endNm.ToString()+": ...Nós dois estamos cansados, abaixa essa arma e talvez podemos repensar sobre isto?\n",12,ConsoleColor.White);
            textDialog("✶ ",12,ConsoleColor.White);
            textDialog("Sans",12,ConsoleColor.Yellow);
            textDialog(" está de ",12,ConsoleColor.White);
            textDialog("POUPANDO",12,ConsoleColor.Yellow);
            textDialog(".\n\n",12,ConsoleColor.White);
            textDialog("Poupar? (S/N)\n",12,ConsoleColor.Yellow);
            textDialog("Nota: qualquer tecla pressionada a não ser\n[S] vai ser considerado não.\n",12,ConsoleColor.Gray);
            ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
            Console.Clear();
            if (pressed.Key == ConsoleKey.S){
                textDialog(startNm.ToString(),12,ConsoleColor.White);
                textDialog("Sans",12,ConsoleColor.Yellow);
                textDialog(endNm.ToString()+": caso você realmente esteja me poupando, nunca mais volte aqui.\n",12,ConsoleColor.White);
                textDialog("✶ ",12,ConsoleColor.White);
                textDialog("Sans",12,ConsoleColor.Yellow);
                textDialog(" aproveita a abertura recebida pela ação e o ataca pela costas...\n",12,ConsoleColor.White);
                player_kr = you[0];
            } else{
                textDialog(startNm.ToString(),12,ConsoleColor.White);
                textDialog("Sans",12,ConsoleColor.Yellow);
                textDialog(endNm.ToString()+": pelo jeito você prefere do jeito difícil.\n",12,ConsoleColor.White);
            }
            loaded();
        }
    }
    else{
        stage++;
        secretEcounter = false;

        if (encounter <= 5){ // chance for Sans
            secretEcounter = true;
            secret = 1;
        }
        else if (encounter <= 10){ // chance for YOU?
            secretEcounter = true;
            secret = 0;
        }
    }
}

void shopCall(int opt, int opt2, int opt3, int opt4){
    shopMenu[0] = opt;
    shopMenu[1] = opt2;
    shopMenu[2] = opt3;
    shopMenu[3] = opt4;
    onShop = true;
    shopDesign();
}

void randomBar(){
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(" | ");
}

bool lastSlot = false;
bool purchased = false;
void otimizedSlot(string foodWanted, int idFood){
    lastSlot = false;
    purchased = false;
    for (int i = 0; i < 4; i++){
        if (i == 3){
            lastSlot = true;
        }
        string slot = inventory[i];
        int foodPlace = foodId[i];
        string hasItem = inventoryVerify(foodWanted);
        int foundPlace = 666;
        for (int i2 = 0; i2 < 4; i2++){
            if (foundPlace == 666){
                if (inventory[i2] == hasItem){
                    foundPlace = i2;
                }
            }
        }
        if (slot == "─" && purchased == false || slot != "─"  && slot.Substring(0,slot.Length-2) == foodWanted && purchased == false){
            if (slot != "─" && slot.Substring(0,slot.Length-2) == foodWanted && (foundPlace == i || foundPlace == 666)){
                inventory[i] = foodWanted+"_"+Convert.ToInt32(Convert.ToInt32(slot.Substring(foodWanted.Length+1))+1);
                purchased = true;
            }else if(foundPlace == i || foundPlace == 666){
                inventory[i] = foodWanted+"_1";
                foodId[i] = idFood;
                purchased = true;
            }
            gold -= item[0];
        }
        else if(lastSlot == true  && slot != "─"){
            textDialog("Não há espaço no seu Inventário",12,ConsoleColor.Red);
            Console.ReadKey();
            shopDesign();
        }
    }
}

void FoodInventory(string food, int Id){
    otimizedSlot(food, Id);
    //Console.WriteLine(inventory); // Enable it if you wanna see inventory after buying
    shopDesign();
}

string inventoryVerify(string food){
    string finded = "─";
    for (int i = 0; i < inventory.Length; i++){
        if (inventory[i].Length >= food.Length && inventory[i].Substring(0,food.Length) == food){
            finded = inventory[i];
        }
    }
    return finded;
}

void buy(string food, int id){
    int cd = 12;
    if (anti_spam == false){
        anti_spam = true;
        cd = -9;
    }
    textDialog($"Quer comprar {food}? [S/N]\n",cd,ConsoleColor.White);
    ConsoleKeyInfo keyAnswer = Console.ReadKey();
    string answered = keyAnswer.KeyChar.ToString().Trim();
    if (answered == ""){
        answered = ".";
    }
    if (answered.ToLower() == "s"){
        if (gold >= item[0] && ((inventoryVerify(food) == "─" && item[1] > 0) || Convert.ToInt32(inventoryVerify(food).Substring(food.Length+1)) < item[1])){
            FoodInventory(food,id);
        }
        else if (item[1] <= 0 || inventoryVerify(food) != "─" && Convert.ToInt32(inventoryVerify(food).Substring(food.Length+1)) >= item[1]){
            textDialog("Está carregando muito ",cd,ConsoleColor.Red);
            textDialog(food,cd,ConsoleColor.DarkGreen);
            textDialog("!\n",cd,ConsoleColor.Red);
            Console.ReadKey();   
        }
        else if (gold < item[0]){
            textDialog("Não possue ",cd,ConsoleColor.Red);
            textDialog("Gold ",cd,ConsoleColor.DarkYellow);
            textDialog("suficiente!\n",cd,ConsoleColor.Red);
            Console.ReadKey();
        }
        shopDesign();
    }
    else if (answered.ToLower() == "n"){
        shopDesign();
    }
    else{
        shopDesign();
    }
}

void shopSelectedDesign(int slot){
    if (slot == selected){
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        if (slot < 5){
            if (itemNM != ""){
                Console.WriteLine(startNm+itemNM+endNm+$"{"".PadRight(8+shopLength-(itemNM.Length+item[0].ToString().Length),shopSummary)}{item[0]} Gold");
            } else{
                Console.WriteLine($"{itemNM.PadRight(15+shopLength,shopSummary)}");        
            }
        } else{
            Console.WriteLine(startNm+"Exit"+endNm+$"{"".PadRight(9+shopLength,shopSummary)}");   
        }
    } else{
        Console.ForegroundColor = ConsoleColor.Gray;
        if (slot < 5){
            if (itemNM != ""){
                Console.WriteLine($"{itemNM.PadRight((10+shopLength)-item[0].ToString().Length,shopSummary)}{item[0]} Gold");  
            } else{
                Console.WriteLine($"{itemNM.PadRight((15+shopLength),shopSummary)}");        
            }
        } else{
            Console.WriteLine($"Exit{"".PadRight((11+shopLength),shopSummary)}");   
        }
    }
}

void shopDesign(){
    if (onShop == true){
        Console.Clear();
        textDialog(".=".PadRight(shopLength/2+5,'─')+"{Shop}"+"=.\n".PadLeft(shopLength/2+6,'─'),-9,ConsoleColor.DarkGray);
        for (int i = 0; i < 4; i++){
            shop(shopMenu[i]);
            shopSelectedDesign(i+1);
        }
        shopSelectedDesign(5);
        textDialog("'=".PadRight(shopLength+13,'─')+"='\n",-9,ConsoleColor.DarkGray);
        int foodID = 0;
        for (int i = 0; i < 4; i++){
            if (selected == i+1){
                foodID = shopMenu[i];
            }
        }
        shop(foodID);
        textDialog(itemSelectDesc+"\n",-9,ConsoleColor.DarkGray);
        textDialog($"{gold} Gold\n",-9,ConsoleColor.DarkYellow);
        ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
        if (pressed.Key == ConsoleKey.Enter){
            if (selected < 5){
                if (itemNM != ""){
                    buy(itemNM, foodID);
                }
            } else{
                onShop = false;
                anti_spam = false;
            }
        }
        else if (pressed.Key == ConsoleKey.UpArrow){
            if (selected > 1){
                selected -= 1;
            } else{
                selected = 5;
            }
        }
        else if (pressed.Key == ConsoleKey.DownArrow){
            if (selected < 5){
                selected += 1;
            } else{
                selected = 1;
            }
        }
        if (onShop == true){
            shopDesign();
        }   
    }
}

void slotDesign(string food,int slotSpace){
    string foodName = "─";
    for (int i = 0; i < (inveLength/6); i++){
        foodName += "─";
    }
    if (food.Length > 2){
        foodName = food.Substring(0,food.Length-2);
    }
    else{
        food = "..─0";
    }
    int pair = foodName.Length/2;
    int autoSize = inveLength-pair;
    if ((slotSpace % 2) == 0){
        if (slotSpace == selected){
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"".PadLeft(autoSize-2,' ')}[{foodName}] x{food.Substring(food.Length-1)}{"".PadRight(autoSize-2,' ')}");
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"".PadLeft(autoSize-1,' ')}{foodName} x{food.Substring(food.Length-1)}{"".PadRight(autoSize-1,' ')}");  
        }
    }
    else if (slotSpace < 5){
        if (slotSpace == selected){
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{"".PadLeft(autoSize-2,' ')}[{foodName}] x{food.Substring(food.Length-1)}{"".PadRight(autoSize-2,' ')}");
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{"".PadLeft(autoSize-1,' ')}{foodName} x{food.Substring(food.Length-1)}{"".PadRight(autoSize-1,' ')}");  
        }
    }
    else{
        if (slotSpace == selected){
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"".PadLeft(inveLength-2,' ')}[Exit]{"".PadRight(inveLength-2,' ')}");
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"".PadLeft(inveLength-1,' ')}Exit{"".PadRight(inveLength-1,' ')}");  
        } 
    }
}

void foodHeal(){
    textDialog($"Utilizou {itemNM}!\n",25,ConsoleColor.Yellow);
    textDialog(itemDesc+"\n",25,ConsoleColor.White);
    
    double healed = item[2]-player_bl;
    if (healed < 0){
        healed = 0;
    }

    player_bl = Math.Round(player_bl-item[2],1);
    if (player_bl < 0 && item[2] > 0){
        player_bl = 0;
    }

    if ((player_hp + item[2]) < player_hp){
        textDialog($"Perdeu {item[2]} HP!\n",25,ConsoleColor.Red);
        player_hp = Math.Round(player_hp+item[2],1);
        if (player_hp <= 0){
            Console.Clear();
            onInventory = false;
        }
    }
    else if ((player_hp + healed) > player_hp && (player_hp + healed) < you[0]){
        textDialog($"Recuperou {healed} HP!\n",25,ConsoleColor.Green);
        player_hp = Math.Round(player_hp+healed,1);
    }
    else if ((player_hp + healed) >= you[0] && you[0] != player_hp){
        textDialog($"Maximixou seu HP!\n",25,ConsoleColor.Green);
        player_hp = you[0];
    }

    if ((you[1] + item[3]) < you[1]){
        textDialog($"Perdeu {item[3]} AT!\n",25,ConsoleColor.Red);
    }else if ((you[1] + item[3]) > you[1]){
        textDialog($"Ganhou {item[3]} AT!\n",25,ConsoleColor.DarkRed);
    }
    you[1] = Math.Round(you[1]+item[3],1);

    if ((you[2] + item[4]) < you[2]){
        textDialog($"Perdeu {item[4]} LU!\n",25,ConsoleColor.Red);
    }else if ((you[2] + item[4]) > you[2]){
        textDialog($"Ganhou {item[4]} LU!\n",25,ConsoleColor.DarkGreen);
    }
    you[2] = Math.Round(you[2]+item[4],1);
    loaded();
}

string selectingDescription(){
    string actualDesc = "";
    switch (selected){
        case 1:
            shop(foodId[0]);
        break;
        case 2:
            shop(foodId[1]);
        break;
        case 3:
            shop(foodId[2]);
        break;
        case 4:
            shop(foodId[3]);
        break;
        default:
            shop(666);
        break;
    }
    actualDesc = itemSelectDesc;
    return actualDesc;
}

void inventoryMenu(){
    bool used = false;
    if (onInventory == true){
        Console.Clear();
        playerStatus();
        textDialog(".=".PadRight(inveLength/2+12,'─')+"{Item}"+"=.\n".PadLeft(inveLength/2+12,'─'),-9,ConsoleColor.DarkGray);
        slotDesign(inventory[0],1);
        slotDesign(inventory[1],2);
        slotDesign(inventory[2],3);
        slotDesign(inventory[3],4);
        slotDesign("Exit",5);
        textDialog("'=".PadRight(inveLength+27,'─')+"='\n",-9,ConsoleColor.DarkGray);
        textDialog($"{selectingDescription()}\n",12,ConsoleColor.DarkGray);
        ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var
        if (pressed.Key == ConsoleKey.Enter){
            if (selected < 5){
                for (int i = 0; i < inventory.Length; i++){
                    if (selected == i+1 && inventory[i] != "─" && used == false){
                        if (Convert.ToInt32(inventory[i].Substring(inventory[i].Length-1)) > 0){
                            used = true;
                            inventory[i] = inventory[i].Substring(0,inventory[i].Length-1)+Convert.ToInt32(Convert.ToInt32(inventory[i].Substring(inventory[i].Length-1))-1);
                            shop(foodId[i]);
                            foodHeal();
                            if (Convert.ToInt32(inventory[i].Substring(inventory[i].Length-1)) < 1){
                                inventory[i] = "─";
                                foodId[i] = i;
                            }
                        }
                    }
                }
            } else{
                onInventory = false;
            }
        }
        else if (pressed.Key == ConsoleKey.LeftArrow){
            if (selected > 1){
                selected -= 1;
            } else{
                selected = 5;
            }
        }
        else if (pressed.Key == ConsoleKey.RightArrow){
            if (selected < 5){
                selected += 1;
            } else{
                selected = 1;
            }
        }
        else if (pressed.Key == ConsoleKey.UpArrow){
            if (selected > 2){
                selected -= 2;
            } else{
                selected += 3;
            }
        }
        else if (pressed.Key == ConsoleKey.DownArrow){
            if (selected < 3){
                selected += 2;
            } else{
                selected = 5;
            }
        }
        if (onInventory == true){
            inventoryMenu();
        }       
    }
}

void shop(int x){
    if (secretMode == false){
        switch (x) { // [0] Cost, [1] Max amount, [2] Heal, [3] Attack, [4] Luck, [5] Shop Chance 
        case 1:
            itemNM = "Durex";
            itemDesc = "✶ Você remenda seus ferimentos.\n✶ Parabéns por não morrer até agora!";
            itemSelectDesc = "Útil para remendar alguns ferimentos!";
            item=[5,5,3,0,0,70];
        break;
        case 2:
            itemNM = "Veneno";
            itemDesc = "✶ Você decide tomar o Veneno. . .\n✶ Alguma coisa não caiu bem.";
            itemSelectDesc = "Você sabe do que isto é capaz. . .";
            item=[3,1,Math.Round(Convert.ToDouble(you[0]/5),1)*-1,0,10,66];
        break;
        case 3:
            itemNM = "Vitamina D";
            itemDesc = "✶ Você espera a luz do Sol!\n✶ Vitamina D deixou você refrescado!";
            itemSelectDesc = "Vitamina D, sempre importante!";
            item=[45,4,4,4,0,40];
        break;
        case 4:
            itemNM = "Trevo";
            itemDesc = "✶ Bem me quer, Mal me quer.\n✶ Bem me quer, Mal me quer. . .";
            itemSelectDesc = "Coincidênciamente, pode-se carregar no máximo 4.";
            item=[12,4,0,0,4,44];
        break;
        case 5:
            itemNM = "Café";
            itemDesc = "✶ Você bebe o café.\n✶ Você sente-se revigorado!";
            itemSelectDesc = "Para vós, apresenta-se nosso caFÉ abençoado.";
            item=[25,1,you[0],0,0,25];
        break;
        case 6:
            itemNM = "Cereal";
            itemDesc = "✶ Cada cereal tem seu sabor único.\n✶ Principalmente este!";
            itemSelectDesc = "\"Consome LU para recuperar aleatoriamente seu HP!\"";
            item=[4,3,Math.Round(Convert.ToDouble(dice(1,Convert.ToInt32(you[0]),0)),1),0,0,85];
        break;
        default:
            itemNM = "";
            itemDesc = "";
            itemSelectDesc = "";
            item=[0,0,0,0,0,100];
        break;
        }
    } else if(secretMode == true){
        switch (x) { // [0] Cost, [1] Max amount, [2] Heal, [3] Attack, [4] Luck, [5] Shop Chance 
        case 1:
            itemNM = "Bandad";
            itemDesc = "✶ Diferente do que pensaste...\n✶ Não baniu seu pai.";
            itemSelectDesc = "\"Bandad! Compre agora para poder banir seu [Censurado]!\"";
            item=[10,10,8,0.1,0.1,85];
        break;
        case 2:
            itemNM = "X-Dead";
            itemDesc = "✶ De alguma maneira é reconfortante...\n✶ Mas a sensação acaba durante a queda para o chão.";
            itemSelectDesc = "Mais potente que o \"Veneno\" Original.";
            item=[10,2,Math.Round(Convert.ToDouble(you[0]/4),1)*-1,Math.Round(Convert.ToDouble(you[0]/16),1),-0.5,66];
        break;
        case 3:
            itemNM = "HDeusO";
            itemDesc = "✶ Você bebe a água benta!\n✶ Deixando-o abençoado!";
            itemSelectDesc = "Realmente, para que uma água convecional, quando pode-se beber água benta!";
            item=[99,1,Math.Round(Convert.ToDouble(you[0]*1.5),1),5,5,20];
        break;
        case 4:
            itemNM = "Rosa";
            itemDesc = "✶ Você cheira a Rosa!\n✶ Mas logo em seguida, você aperta um de seus espinhos.";
            itemSelectDesc = "Apenas uma Rosa, entretando não é realmente da cor rosa.";
            item=[20,5,-0.1,0,5.5,55];
        break;
        case 5:
            itemNM = "Torta";
            itemDesc = "✶ Você engole a torta.\n✶ Você sente-se entortado . . !";
            itemSelectDesc = "Uma torta bem entortada. . .";
            item=[42,1,you[0],1,0,25];
        break;
        case 6:
            itemNM = "Aposta Móvel";
            itemDesc = "✶ Não existe um lugar para apostar!";
            itemSelectDesc = "Sim, isto é comprável. Contudo na aposta ganha-se tudo!";
            item=[
                Math.Round(Convert.ToDouble(dice(1,Convert.ToInt32(gold),0)),1),
                1,
                Math.Round(Convert.ToDouble(dice(1,Convert.ToInt32(you[0]/2),-Convert.ToInt32(Math.Round(you[0]/4)))),1),
                Math.Round(Convert.ToDouble(dice(1,Convert.ToInt32(you[1]),-Convert.ToInt32(Math.Round(you[1]/2)))),1),
                Math.Round(Convert.ToDouble(dice(1,Convert.ToInt32(you[2]),-Convert.ToInt32(Math.Round(you[2]/2)))),1),
                85
                ];
        break;
        default:
            itemNM = "";
            itemDesc = "";
            itemSelectDesc = "";
            item=[0,0,0,0,0,100];
        break;
        }
    }
}

void Levelviolence(){ // love for every kill you do
    while (you[4] >= xpMath){
        xpMath = 10 + (next*(you[3] - 1));
        if (you[4] >= xpMath){
            you[4] -= xpMath;
            you[3]++;
            textDialog($"\"{playerNM}\" subiu de LV {you[3]-1} > {you[3]}!\n",25,ConsoleColor.DarkYellow);
            if (you[3] % 4 == 0){
                player_hp += 4;
                you[0] += 4;
                textDialog($"Aumentou 4 de HP!\n",25,ConsoleColor.Green);
            } else{
                you[1] += 1;
                textDialog($"Aumentou 1 de AT!\n",25,ConsoleColor.DarkRed);
            }
        }
    }
}

double xpMonsterMath(){ // xp math with monster lv when kill
    double monsterXp = 10 + (2.5*(enemy[2] - 1));
    return monsterXp-(you[3]-1);
}

double gMonster(){ // g with monster lv when kill
    double golden = dice(1,10,Convert.ToInt32(enemy[2])) + (3*(enemy[2] - 1));
    return golden*0.75;
}

void x_Beep(int x, int x2){ // last message/letter after die + beep
    if (soundEnable == true){
        Console.Beep(x,x2);
    }
    Console.Write("ˣ");
}

void dmgSound(string from){ // attack sound/effect
    if (soundEnable == true){
        if (from == "" || from == null){ // nobody hit sound
            for (int i = 0; i < 4; i++){
                Console.Beep(800,125);
            }
            Console.Beep(625,575);
        }

        else if (from == "Monster"){ // monster hit sound
            for (int i = 0; i < 4; i++){
                Console.Beep(550,100);
            }
            Console.Beep(550,450);
        }

        else if (from == "Player"){ // player hit sound
            for (int i = 0; i < 4; i++){
                Console.Beep(600,100);
            }
            Console.Beep(600,450);
        }
    }
}

void textDialog(string txt, int cooldown, ConsoleColor rgb){ // write texts like RPGs NPCs
    Console.ResetColor();
    Console.ForegroundColor = rgb;
    if (dialogSpeed > 2 && (cooldown+dialogSpeed) > 2 && anti_spam == false){
        for (int i = 0; i < txt.Length; i++){ // how much length the txt have
            if (txt[i].ToString() != " " && txt[i].ToString() != ""){ // skip cooldown if the letter is blank or space
                if (soundEnable == true){
                    Console.Beep(1125,cooldown+dialogSpeed);
                } else{
                    Thread.Sleep(cooldown+dialogSpeed);
                }
            }
            Console.Write(txt[i]); // write on 'i' position on txt
        }
    }
    else{
        Console.Write(txt);
    }
}

int dice(int quantity,int maximo, int bns){ // random number generator syle RPG Custom Dice
    result = 0; // set to 0 or will stack
    for (int i = 0; i < quantity; i++){ // number of dices
        rolled = new Random().Next(0,maximo+1); // the dice
        if (result <= 0){ // checking if dice wasn't rolled yet
            result = rolled;
        }
        else{ // if dice was rolled yet, then stack
            result += rolled;
        }
    }
    return result+bns; // return all dices values
}

void soul(int actual, string who){ // shorter heart fill
    if (who == "Monster"){ // if is monster hearts
        hpbar = "";
        for (int i = 0; i < actual; i++){ // fill with fully hearts
            hpbar += heartShape;
        }
        hpbar = hpbar.PadRight(heartLength,heartNull); // fill with void hearts
    }
    else if (who == "Player"){ // if is player hearts
        playerhpbar = ""; // set to nothing or will stack
        for (int i = 0; i < actual; i++){ // fill with fully hearts
            playerhpbar += heartShape;
        }
        playerhpbar = playerhpbar.PadRight(heartLength,heartNull); // fill with void hearts
    }
}

void bar(double now, double max, string who){ // Heart Bar
    for (double i = 0; i <= 10; i++){ // new % method by for
        if (now <= max-(max*(i/10)) && now > max-(max*((i/10)+0.1))){
            soul(heartLength-Convert.ToInt32(heartLength*(i/10)),who);
        }
    }
}

void loaded(){ // load design, wait user click any keybind for progress
    textDialog("Pressione qualquer ⌨   tecla para continuar",2,ConsoleColor.White);
    textDialog(" . . . ➟\n",112,ConsoleColor.White);
    Console.ReadKey();
}

void nameAsk(){ // arcade user name select/choose
    Console.Clear();
    Console.Write($"✎ Digite o Nome: {playerNM.PadRight(12,'_')}"); // fill with underline (custom)

    ConsoleKeyInfo pressed = Console.ReadKey()!; // detect what keybind has clicked (note: shift + any key dont break) + new var

    if (pressed.Key != ConsoleKey.Backspace && pressed.Key != ConsoleKey.Enter && playerNM.Length <= 12){
        if (playerNM.Length < 12){ // fill blank/underline characters
            playerNM += pressed.KeyChar;
        }
        else if (playerNM.Length >= 12){ // if name got max length, then subtitute last character to new
            playerNM = playerNM.Substring(0,playerNM.Length-1)+pressed.KeyChar;
        }
        nameAsk();
    }
    else if (pressed.Key == ConsoleKey.Backspace){ // remove last character
        if (playerNM.Length > 0){
            playerNM = playerNM.Substring(0,playerNM.Length-1);
        }
        nameAsk();
    } else if (pressed.Key == ConsoleKey.Enter){ // when pressed enter finish user name select
        Console.WriteLine();
    } else{ // case if use anything that isn't on keyboard
        nameAsk();
    }
}

// -------------------------------- Configuration! ------------------------

Console.Clear();
choice();
soundBlock();
musicBlock();
hardMode();

// ------------------------------ where all begins! ----------------------

Console.Clear();
textDialog("≔ ≕ {Como Jogar} ≔ ≕\n\n",0,ConsoleColor.White);
textDialog("Botões de Confirmar: ",2,ConsoleColor.Gray);
textDialog("[Enter]\n",2,ConsoleColor.White);
textDialog("Botões de Selecionar: ",2,ConsoleColor.Gray);
textDialog("[←], [↑], [→] e [↓]\n",2,ConsoleColor.White);
textDialog("Gold: Moeda de troca, obtida após derrotar adversários.\n",2,ConsoleColor.DarkYellow);
textDialog("LV: Nível de Violência.\n",2,ConsoleColor.White);
textDialog("EXP: Pontos de Execução.\n",2,ConsoleColor.Gray);
textDialog("AT: Ataque, valor que auxilia ao atacar.\n",2,ConsoleColor.DarkRed);
textDialog("HP: Pontos de Vida, quantidade de dano que pode receber.\n",2,ConsoleColor.Green);
textDialog("LU: Sorte, utilizado para aumentar e diminuir o dano recebido, ao ser usado diminui o valor em 1.\n",2,ConsoleColor.DarkGreen);
textDialog("KR: Karma, constantemente causará dano a cada ação, contudo reduzindo o valor em 1.\n",2,karmaHeart);
textDialog("BL: Blood, impede o valor de cura baseado no seu valor, o pontos são reduzidos ao curar ou quando atingir um valor máximo.\n\n",2,bloodHeart);
textDialog("Sistema de LV funciona da seguinte forma:\n",2,ConsoleColor.White);
textDialog("A cada 1 LV você ganhará 1 de AT!\n",2,ConsoleColor.DarkRed);
textDialog("Contudo, a cada 4 LVs ao invés de receber AT você recebe 4 HP!\n",2,ConsoleColor.Green);
textDialog("Quanto mais LV você tiver, maior é a necessidade de EXP!\n\n",2,ConsoleColor.Gray);
textDialog("Caso seu HP cai para 0 ou menor, você perde!\n\n",2,ConsoleColor.Red);
textDialog("Você é um aventureiro preso em uma masmorra, seu objetivo é sair dela, entretando, como foi para entrar, na volta haverá problemas no decorrer de sua jornada.\n",7,ConsoleColor.Yellow);
textDialog("Boa Sorte!\n",2,ConsoleColor.DarkGreen);
loaded();

Console.Clear();
Console.ForegroundColor = ConsoleColor.White;
nameAsk(); // insert you name
you[1] = Math.Round(Convert.ToDouble(dice(1,6,6)));
textDialog($"⟨➶  AT: {you[1]}",25,ConsoleColor.DarkRed);
you[0] = Math.Round(Convert.ToDouble(dice(1,6,6)));
player_hp = you[0];
textDialog($" ❤  HP: {player_hp}",25,ConsoleColor.Green);
you[2] = Math.Round(Convert.ToDouble(dice(2,6,12)));
textDialog($" ✤  LU: {you[2]}⟩\n",25,ConsoleColor.DarkGreen);
loaded();

void updt(){
    realStatus = false;
    atkBns = 0;
    cursedAction = 0;
    if (secretEcounter == false && secretMode == false){
        switch (stage){  // [1] Name, [2] HP, [3] ATK, [4] LV, [5] Stamina,[6] Karma, [7] Blood, [8] Linked Battle
            case 1:
            monsterNM = "Lobo Cinzento";
            enemy=[3,3,1,0,0,0,0];
            narrador=[
                $"{monsterNM} ataca ferozamente!",
                $"{monsterNM} late ferozamente!",
                $"{monsterNM} pula ferozamente!",
                $"{monsterNM} deita ferozamente?"
            ];
            break;
            case 2:
            monsterNM = "Lobo Branco";
            enemy=[3,3,1,0,0,0,0];
            narrador=[
                $"{monsterNM} ataca calmamente.",
                $"{monsterNM} late calmamente.",
                $"{monsterNM} pula calmamente.",
                $"ZZZZZZZZZ. . ."
            ];
            break;
            case 3:
            monsterNM = "Goblin";
            enemy=[5,4,2,0,0,0,0];
            narrador=[
                $"{monsterNM} te encara!",
                $"{monsterNM} começa a saltitar!",
                $"{monsterNM} faz. . .\n✶ Algo impossível de descrever?",
                $"{monsterNM} aponta sua lança!\n✶ PERA!\n✶ Onde veio isto!?"
            ];
            break;
            case 4:
            monsterNM = "Orc Vesgo";
            enemy=[5,5,4,0,0,0,0];
            narrador=[
                $"{monsterNM} acabou de intimidar\naté sua alma.",
                $"{monsterNM} tentou enxergar você.\n✶ Mas falha miseravelmente.",
                $"{monsterNM} cansou-se e começa a \nler um livro.\n✶ Parece um de literatura.",
                $"{monsterNM} começa a falar algo \nsem sentido?"
            ];
            break;
            case 5:
            monsterNM = "Orc Barbudo";
            enemy=[5,5,4,0,0,0,0];
            narrador=[
                $"{monsterNM} demonstra sua barba!",
                $"{monsterNM} reflete a luz!",
                $"{monsterNM} começa a escovar sua\nbarba!",
                $"{monsterNM} fez nada?"
            ];
            break;
            case 6:
            monsterNM = "Zumbi Manco";
            enemy=[6,7,5,0,0,0,0];
            narrador=[
                $"{monsterNM} corre rapidamente em\nsua direção!",
                $"{monsterNM} ele questiona o sen-\ntido da vida.",
                $"{monsterNM} ele começa a pular\ncorda!",
                $"{monsterNM} reponhe seus ossos?"
            ];
            break;
            case 7:
            monsterNM = "Zumbi Balofo";
            enemy=[6,7,5,0,0,0,0];
            narrador=[
                $"urg.\n (Não consigo respirar!)",
                $"{monsterNM} está fedendo. . .",
                $"{monsterNM} abre sua boca em\nminha direçã-",
                $"Narrador morre após fedor de\n{monsterNM}. . ."
            ];
            break;
            case 8:
            monsterNM = "Troll";
            enemy=[8,7,7,0,0,0,0];
            narrador=[
                $"{monsterNM} fez uma TROLLagem em você!",
                $"{monsterNM} alegremente.\n✶ Começa a sorrir.",
                $"{monsterNM} demonstra seu sorriso.\n✶ Te lembra de algo?",
                $"{monsterNM}Face deu umas risadas."
            ];
            break;
            case 9:
            monsterNM = "Ogro";
            enemy=[8,9,7,0,0,0,0];
            narrador=[
                $"{monsterNM} senta-se.",
                $"{monsterNM} balança sua arma.",
                $"{monsterNM} começa contemplar a nature-\nza.",
                $"{monsterNM} esqueceu do motivo para lu-\ntar."
            ];
            break;
            case 10:
            monsterNM = "Ogro Furioso";
            enemy=[10,9,9,0,0,0,0];
            narrador=[
                $"{monsterNM} lembrou-se do moti-\nvo para lutar!",
                $"{monsterNM} começa contemplar\nsua dor!",
                $"{monsterNM} prepara sua posição\nde combate!",
                $"{monsterNM} arremessa lama!\n✶ Parou na tua cara!"
            ];
            break;
            case 11:
            monsterNM = "Necromante Maligno";
            enemy=[12,12,10,0,0,0,0];
            narrador=[
                $"O ambiente começa a ficar mais\ntenso. . .",
                $"O cemitério.\n✶ Os ventos.\n* Te incomodam. . .",
                $"A aventura de \"{playerNM}\"\nchegou ao fim. . .",
                $"Você só queria terminar essa\nMasmorra. . ."
            ];
            break;
            case 12:
            monsterNM = "Zumbi Mutante";
            enemy=[15,20,10,0,0,2,0];
            narrador=[
                $"{monsterNM} bloqueia seu cami-\nnho.",
                $"Ele se movimenta rapidamente. . .",
                $"Seus ataques não demonstram efe-\nito. . .",
                $"Você ve seu corpo deformando com\na mutação. . ."
            ];
            break;
            case 13:
            monsterNM = "Rei Javali";
            enemy=[20,15,10,0,0,3,0];
            narrador=[
                $"{monsterNM} rosna ferozamente.",
                $"{monsterNM} grita por reforços.\n* Mas ninguém veio.",
                $"{monsterNM} prepara um ataque\ndemolidor.",
                $"{monsterNM} levanta-se após a\ndestruição."
            ];
            break;
            case 14:
            monsterNM = "Enxame de Parasitas";
            enemy=[9,10,10,14,5,0,0];
            narrador=[
                $"Os parasitas te percebem!",
                $"Atento: parasitas tem veneno em\nsuas mandíbulas.",
                $"Parece que os parasitas são\ninfinitos. . .",
                $"Por que eles estão ai?"
            ];
            atkBns = 3;
            break;
            case 15:
            monsterNM = "Ceifador";
            enemy=[30,20,12,20,0,4,0];
            narrador=[
                $"\"{playerNM}\" presencia as\nespirais do tempo. . .",
                $"{monsterNM} escorre cada vez mais\nlodo.",
                $"A sensação é a mesma quando\nestava próximo da morte.",
                $"Tudo realmente necessita de um\nfim?"
            ];
            atkBns = 2;
            OpenFolder(@".\music\","ceifador_oprpg.mp3");
            break;
            default:
            break;
        }
    }
    else if (secretEcounter == false && secretMode == true){
        switch (stage){  // [0] HP, [1] ATK, [2] LV, [3] Stamina, [4] Karma, [5] Blood, [6] Linked Battle
            case 1:
            monsterNM = "Lobo Infernal";
            enemy=[6,4,1,3,0,0,0];
            narrador=[
                $"{monsterNM} ataca violentamente!",
                $"{monsterNM} rosna violentamente!",
                $"{monsterNM} salta violentamente!",
                $"{monsterNM} morde um osso no chão?"
            ];
            break;
            case 2:
            monsterNM = "Lobo Glacial";
            enemy=[5,4,1,4,0,0,0];
            narrador=[
                $"{monsterNM} ataca friamente.",
                $"{monsterNM} olha friamente.",
                $"{monsterNM} corre friamente.",
                $"{monsterNM} respira friamente."
            ];
            break;
            case 3:
            monsterNM = "Goblin Guerreiro";
            enemy=[13,5,2,0,0,0,0];
            narrador=[
                $"{monsterNM} te incrimina!",
                $"{monsterNM} começa a gritar!",
                $"{monsterNM} faz. . .\n✶ O possivel para manter você desatento.",
                $"{monsterNM} aponta sua arma!\n✶ PERA!\n✶ É uma 9mm?!?"
            ];
            break;
            case 4:
            monsterNM = "Ciclope";
            enemy=[25,8,3,0,0,0,0];
            narrador=[
                $"{monsterNM} acabou de encarar\naté sua alma.",
                $"{monsterNM} Pisa profundamente.\n✶ Mas acaba criando um buraco.",
                $"{monsterNM} cansou-se e começa a \nler um livro.\n✶ Parece um descrito como \"Death Zote\".",
                $"{monsterNM} começa a filosofar\n✶ Mesmo ele não entendendo a vida"
            ];
            break;
            case 5:
            monsterNM = "Orc Ancião";
            enemy=[20,6,4,10,0,0,0];
            narrador=[
                $"{monsterNM} demonstra su\nsabedoria!",
                $"{monsterNM} reflete a luz da\nsabedoria!",
                $"{monsterNM} começa a apreciar sua\nsabedoria!",
                $"{monsterNM} diz assuntos da\nsabedoria"
            ];
            break;
            case 6:
            monsterNM = "Zumbi Decapitado";
            enemy=[5,10,3,7,0,5,0];
            narrador=[
                $"{monsterNM} jorra mais sangue. . .",
                $"O cheiro é horrivel.",
                $"Quem fez isso?",
                $"{monsterNM} tenta gritar.\n✶ Mas falha por incapacitade."
            ];
            break;
            case 7:
            monsterNM = "Zumbi Insano";
            enemy=[10,8,5,3,2,2,0];
            narrador=[
                $"A loucura é contagiosa.",
                $"{monsterNM} grita por nenhum motivo.",
                $"{monsterNM} corre em sua direção\ncom toda sua força",
                $"HAH4HAH4HAHH4HHAH4HHAHHH4HA..."
            ];
            break;
            case 8:
            monsterNM = "Troll Colossal";
            enemy=[25,9,7,0,0,5,0];
            narrador=[
                $"{monsterNM} tem um ar de superioridade.",
                $"{monsterNM} grita e arremessa pedras.",
                $"{monsterNM} parece realmente um colosso.",
                $"{monsterNM}nanismo é real!"
            ];
            break;
            case 9:
            monsterNM = "Ogro Real";
            enemy=[22,10,6,0,0,0,0];
            narrador=[
                $"{monsterNM} realmente existe!\n✶ Poderia ter sido abstrato.",
                $"{monsterNM}iza um manobra...\n✶ Realeza Flip???",
                $"{monsterNM}ista é artista.\n✶ Só se for da morte...",
                $"{monsterNM}? Mais para Ogro.\n✶ Afinal, de real tem nada."
            ];
            break;
            case 10:
            monsterNM = "Ogro Berserk";
            enemy=[17,13,8,0,0,5,0];
            narrador=[
                $"{monsterNM} troca HP para AT!\n✶ Talvez burrice...",
                $"{monsterNM} sente a agônia\n incansável da dor.",
                $"{monsterNM} não parece feliz!\n✶ Talvez um abraço o acalme?",
                $"{monsterNM} Só faltou a armadura..."
            ];
            break;
            case 11:
            monsterNM = "Necromante Pecador";
            enemy=[20,18,10,10,10,0,0];
            narrador=[
                $"Os pecados talvez não sejam\nsomente dele...",
                $"Quando mais você progredia\nmais almas ele obteve.",
                $"Você sente algum remorso\n para aqueles que não estão\n mais entre nós?",
                $"Se ele é o pecador, você\né o juiz?"
            ];
            break;
            case 12:
            monsterNM = "Experimento H2";
            enemy=[15,20,10,15,4,3,0];
            narrador=[
                $"<h2>Experimento</h2>",
                $"Algumas partes são carne viva.",
                $"Quem poderia ter feito isto?",
                $"A deformação, a pertubação.\n✶ Esse foi apenas o começo."
            ];
            break;
            case 13:
            monsterNM = "Imperador Javali";
            enemy=[25,15,10,10,0,6,0];
            narrador=[
                $"{monsterNM} faz um referência\n antes de começar a luta.",
                $"{monsterNM} não precisa de ninguém.",
                $"{monsterNM} possui um espiríto\nde luta enorme.",
                $"{monsterNM} limpa suas patas."
            ];
            break;
            case 14:
            monsterNM = "Parasita de Culpa";
            enemy=[1,10,10,37,7,0.1,0];
            narrador=[
                $"A sua conciência pesa.",
                $"Sua frazqueza é a força\ndeles.",
                $"A culpa é algo\nindescritivel.",
                $"É SUA CULPA!\nÉ SUA CULPA!\nÉ SUA CULPA!\nÉ SUA CULPA!\nÉ SUA CULPA!"
            ];
            atkBns = 6;
            break;
            case 15:
            monsterNM = "Deus da MORTE";
            enemy=[33,30,13,33,3,3,0];
            narrador=[
                $"É possível matar a própria\nMORTE?",
                $"Tudo precisa de um FIM.",
                $"Esse encontro é apenas um\nANACRONISMO.",
                $"O Infinito sem FIM."
            ];
            atkBns = 3;
            OpenFolder(@".\music\","heilag_vagga.mp3");
            break;
            default:
            break;
        }
    }
    else if (secretEcounter == true){
        switch (secret){
            case 0:
            monsterNM = $"{playerNM}?";
            enemy=[you[0],you[1],you[3],you[2],0,0,0];
            narrador=[
                $"O cenário está vazio. . .\n✶ Te deixa bastante confuso e des-\nconfortavel. . .",
                $". . . ?",
                $"Sua mente esvazia-se. . .\n✶ Conforme a batalha procede. . .",
                $"Sua Determinação. . .\n✶ Sua Criação. . .\n✶ Vai colocar um Fim. . ."
            ];
            OpenFolder(@".\music\","one_step_ahead_yourself.mp3");
            break;
            case 1:
            monsterNM = "Sans";
            enemy=[1,1,1,15,Convert.ToDouble(you[3]/4)+1,0,2];
            narrador=[
                $"Você sente seus pecados raste-\njando em suas costas.",
                $"Você sente que vai ter um tempo\nRUIM.",
                $"Você escuta sons de ossos que-\nbrando no corredor.",
                $"Você se enche de KARMA."
            ];
            OpenFolder(@".\music\","megalovania_hallowen_hack.mp3");
            realStatus = true;
            atkBns = -2;
            break;
            case 2:
            monsterNM = "Sans";
            enemy=[1,1,1,15,Convert.ToDouble(you[3]/2)+2,0,0];
            narrador=[
                $"A verdadeira batalha começa.",
                $"Os ataques se intensificam.",
                $"Sans prepara algo. . .",
                $"O espaço-tempo indo \npara lá e para cá."
            ];
            OpenFolder(@".\music\","megalovania.mp3");
            realStatus = true;
            atkBns = 1;
            break;
            default:
            break;
        }
    }

    // monster stats update
    hp = enemy[0];
    st = enemy[3];
    xp = xpMonsterMath();
    g = gMonster();
    if (secretEcounter == true){
        xp = 10 + (next*(you[3] + 2));
        g = g*2+you[3];
    }
}

void timelineDilation(){ // Sans Passive
    if (secret == 1 || secret == 2){ // Sans value
        actionSelect = dice(1,4,1);
    }
}

void aplierCondition(double plusKR, double plusBL){
    if ((player_kr + plusKR) > player_kr){
        textDialog($"{monsterNM} Infligiu KR!\n",6,karmaHeart);
    }
    player_kr += plusKR;

    if ((player_bl + plusBL) > player_bl){
        textDialog($"{monsterNM} Infligiu BL!\n",6,bloodHeart);
    }
    player_bl = Math.Round(player_bl+plusBL,1);
}

void coditionEffect(){ 
    if (player_hp > 1 && player_kr > 0){ // Karma Effect
        player_hp = Math.Round(player_hp-0.1,1);
    }
    if (player_kr > 0){
        player_kr = Math.Round(player_kr-0.1,1);
    }

    if (player_hp > 0 && player_bl > (you[0]*1.5)){ // Blood Effect
        player_bl -= 1;
        you[0] -= 1;
        if (player_hp > you[0]){
            player_hp = you[0];
        }
    }
}

void playerDMG(string action){ // math damage on player
    if (action == "attack"){
        player_hp = Math.Round(player_hp-def_dmg,1);
        aplierCondition(enemy[4],enemy[5]);
    }
    else if (action == "defend"){
        aplierCondition(enemy[4],((def_dmg-1)+enemy[5]));
    }
}

void counterCheck(){ // when you defend check if you counter the attack or not
    if (you[1] < (enemy[1]+atkBns) || def_Test <  atk_Test){
        playerDMG("defend");
        textDialog($"➹ \"{playerNM}\" Bloqueiou!\n",25,ConsoleColor.Cyan);
        dmgSound("Monster");
        textDialog($"♡ \"{playerNM}\" protegeu {def_dmg-1} HP\n",12,ConsoleColor.DarkRed);
        dmgSound("");
    }
    else if (you[1] > (enemy[1]+atkBns) || def_Test > atk_Test){
        textDialog($"➹ \"{playerNM}\" Contra-ataca!\n",25,ConsoleColor.Cyan);
        dmgSound("Player");
        dmgMathStat(atk_dmg-1);
        dmgSound("");
    }
}

void morte(){
    if (player_hp <= 0 && anti_spam == false){ // player dead sound/animation
        anti_spam = true;
        player_hp = 0;
        textDialog($"✝ \"{playerNM}\" caiu! ✝ ",25,ConsoleColor.DarkGray);
        if (soundEnable == true){
            for (int i = 0; i < 4; i++){
                Console.Beep(950-(i*50),900-(i*100));
            }
            Console.Beep(750,1000);
        }
        textDialog(" . . . ➟\n",125,ConsoleColor.DarkGray);
        Console.ReadKey();
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        if (soundEnable == true){
            for (int i = 0; i < 25; i++){
                x_Beep(1900-(i*25),1800-(i*62));
            }
        }
    }
    if (hp <= 0 && enemy[6] <= 0){ // if monster is defeat
        hp = 0;
        Console.Clear();
        if (secretEcounter == false){
            Console.ForegroundColor = ConsoleColor.DarkRed;
        } else{
            Console.ForegroundColor = ConsoleColor.Black;
        }
        monsterStatus();
        you[4] += xp;
        gold += g;
        if (secretEcounter == false){
            textDialog($"✝ {monsterNM} foi derrotado! ✝\n", 25,ConsoleColor.Red);
            textDialog($"\"{playerNM}\" ganhou {g} de Gold!\n",25,ConsoleColor.DarkYellow);
            textDialog($"\"{playerNM}\" ganhou {xp} de EXP!\n",25,ConsoleColor.White);
            Levelviolence();
            if (soundEnable == true){
                for (int i = 0; i < 4; i++){
                    Console.Beep(900,120);
                }
                Console.Beep(1100,270);
            }
        }
        else if (secretEcounter == true){
            textDialog($"{monsterNM} foi apagado. . .\n", 105,ConsoleColor.DarkGray);
            textDialog($"\"{playerNM}\" substituiu, ganhando {g} de Gold.\n",25,ConsoleColor.Gray);
            textDialog($"Contudo, ganhou {xp} de EXP.\n",25,ConsoleColor.Gray);
            Levelviolence();
            if (soundEnable == true){
                for (int i = 0; i < 4; i++){
                    Console.Beep(950-(i*50),900-(i*250));
                }
                Console.Beep(750,1000);
            }
        }
        loaded();
        if (stage >= finalStage){ // if you are on the last stage when enemy die (verify)
            textDialog($"❚ ❚ \"{playerNM}\" matou todos os monstros! ❚ ❚\n",25,ConsoleColor.Yellow);
            if (soundEnable == true){
                Console.Beep(1250,400);
                Console.Beep(750,400);
                Console.Beep(1250,400);
                Console.Beep(1550,600);
            }
            Console.ResetColor();
        }
    }
}

void status_math(){ // taken or does damage math
    if (luck_mode == ""){ // when your luck_mode = none
        atk_dmg = 2;
        def_dmg = 2;
    }
    else if (luck_mode == "lucky"){ // when your luck_mode = lucky
        atk_dmg = 4;
        def_dmg = 1;
    }
    else if (luck_mode == "unlucky"){ // when your luck_mode = unlucky
        atk_dmg = 1;
        def_dmg = 3;
    }
}

void dmgMathStat(double dmgTaken){
    if (st > 0){
        st -= dmgTaken;
        if (st < 1){
            hp += st;
            st = 0;
        }
        textDialog($"➹ {monsterNM} esquivou!\n",12,ConsoleColor.Gray);
    } else{
        hp -= dmgTaken;
        textDialog($"♡ {monsterNM} perdeu {dmgTaken} HP\n",12,ConsoleColor.DarkRed);
    }
}

void attackTest(string from){
    if (from == "attack"){
        if (atk_Test > def_Test){
            textDialog($"➹ \"{playerNM}\" Ataca!\n",25,ConsoleColor.Cyan);
            dmgSound("Player");
            dmgMathStat(atk_dmg);
            dmgSound("");
        }
        else if (atk_Test < def_Test){
            playerDMG("attack");
            textDialog($"➹ {monsterNM} Contra-ataca!\n",25,ConsoleColor.Red);
            dmgSound("Monster");
            textDialog($"♡ \"{playerNM}\" perdeu {def_dmg} HP\n",12,ConsoleColor.DarkRed);
            dmgSound("");
        }
        else if (atk_Test == def_Test){
            textDialog("➹ Ambos erram o Ataque!\n",25,ConsoleColor.White);
            if (soundEnable == true){
                Console.Beep(850,1450);
            }
        }     
    }
    else if (from == "defend"){
        if (def_Test != atk_Test){
            counterCheck();
        }
        else if (def_Test == atk_Test){
            textDialog("➹ Ambos erram o Ataque!\n",25,ConsoleColor.White);
            if (soundEnable == true){
                Console.Beep(850,1450);
            }
        }
    }
    else if (from == "run"){
        if (atk_Test <= def_Test){
            playerDMG("attack");
            textDialog($"➹ {monsterNM} Ataca!\n",25,ConsoleColor.Red);
            dmgSound("Monster");
            textDialog($"♡ \"{playerNM}\" perdeu {def_dmg} HP\n",12,ConsoleColor.DarkRed);
            dmgSound("");
        }
    }
}

void actionSelectedOption(string dsgn, ConsoleColor colored, int order){
    string selectedDsgn = dsgn;
    Console.ResetColor();
    textDialog("│",-9,ConsoleColor.White);
    if (order == actionSelect){
        textDialog("✦ ",-9,colored);
        Console.BackgroundColor = ConsoleColor.DarkGray;
    } else{
        textDialog("- ",-9,ConsoleColor.White);
    }
    Console.ForegroundColor = colored;
    Console.Write(selectedDsgn.PadRight(29,' '));
    Console.ResetColor();
    textDialog(" │\n",-9,ConsoleColor.White);
}


void actionBox(){
    ConsoleColor atColor = ConsoleColor.DarkRed,
    luColor = ConsoleColor.DarkGreen,
    dfColor = ConsoleColor.DarkCyan,
    itColor = ConsoleColor.DarkYellow,
    runColor = ConsoleColor.DarkGray;

    if (secret == 2){
        switch (cursedAction){
            case 1:
                atColor = ConsoleColor.Magenta;
            break;
            case 2:
                luColor = ConsoleColor.Magenta;
            break;
            case 3:
                dfColor = ConsoleColor.Magenta;
            break;
            case 4:
                itColor = ConsoleColor.Magenta;
            break;
            case 5:
                runColor = ConsoleColor.Magenta;
            break;
        }
    }

    textDialog("╔─── Ações ──────────────────────╗\n",0,ConsoleColor.White);
    actionSelectedOption("Atacar",atColor,1);
    actionSelectedOption("Sorte",luColor,2);
    actionSelectedOption("Defender",dfColor,3);
    actionSelectedOption("Item",itColor,4);
    actionSelectedOption("Fugir",runColor,5);
    textDialog("╚────────────────────────────────╝\n",0,ConsoleColor.White);
}

void actingAction(){
    if (hp > 0){
        switch (actionSelect){
            case 1: // attack Action
                anti_spam = false;
                status_math();
                luck_mode = "";
                atk_Test = you[1] + dice(2,6,0);
                if (realStatus == false){
                    def_Test = enemy[1] + dice(2,6,0);
                }
                else if (realStatus == true){
                    def_Test = you[1] + dice(2,6,Convert.ToInt32(atkBns));
                }
                attackTest("attack");
            break;
            case 2: // luck Action
                if (luck_mode == ""){ // if you didn't tested you luck yet
                    textDialog($"✤ \"{playerNM}\" testou a Sorte!\n",25,ConsoleColor.DarkGreen);
                    luck_Test = dice(2,6,0);
                    if (soundEnable == true){
                        for (int i = 0; i < 4; i++){
                            Console.Beep(800,100);
                        }
                        Console.Beep(900,250);
                    }
                    if (luck_Test <= you[2]){
                        textDialog("Sortudo\n",45,ConsoleColor.Green);
                        luck_mode = "lucky";
                    }
                    else if (luck_Test > you[2]){
                        textDialog("Azarado\n",45,ConsoleColor.DarkRed);
                        luck_mode = "unlucky";
                    }
                    you[2] -= 1;
                }
                else if (luck_mode != ""){ // if you arealdy have tested luck
                    textDialog("✤ Você já testou sua Sorte!\n",37,ConsoleColor.DarkGreen);
                }
            break;
            case 3: // defend Action
                anti_spam = false;
                status_math();
                luck_mode = "";
                def_Test = you[0] + dice(2,6,0);
                if (realStatus == false){
                    atk_Test = enemy[0] + dice(2,6,0);
                }
                else if (realStatus == true){
                    atk_Test = you[0] + dice(2,6,Convert.ToInt32(atkBns));
                }
                attackTest("defend");
            break;
            case 4: // item Action
                onInventory = true;
                selected = 1;
                shop(666);
                inventoryMenu();
            break;
            default: // run Action
                anti_spam = false;
                textDialog($"Você começou a correr!\n",25,ConsoleColor.White);
                int player_run = dice(2,6,Convert.ToInt32(player_hp));
                int enemy_run = dice(2,6,Convert.ToInt32(hp));
                if (soundEnable == true){
                    for (int i = 0; i < 4; i++){
                        Console.Beep(850,450);
                    }
                }
                if (player_run >= enemy_run){
                    textDialog($"Você conseguiu fugir do {monsterNM}!\n",25,ConsoleColor.Gray);
                    if (stage > 1 && secretEcounter == false && stage < finalStage){
                        int choosing = dice(1,100,0);
                        if (luck_mode == "lucky"){
                            choosing -= 75;
                        }
                        else if (luck_mode == "unlucky"){
                            choosing += 25;
                        }
                        if (choosing < 50){
                            stage -= 2;
                        }
                        secretEncounter();
                        updt();
                    }
                    else{
                        textDialog($"Mas não tem onde para fugir. . .\n",25,ConsoleColor.Gray);
                    }
                }
                else if (player_run < enemy_run){
                    textDialog($"Você falha em fugir do {monsterNM}. . .\n",25,ConsoleColor.Gray);
                    status_math();
                    atk_Test = you[1] + dice(1,6,0);
                    if (realStatus == false){
                        def_Test = enemy[1] + dice(2,6,0);
                    }
                    else if (realStatus == true){
                        def_Test = you[1] + dice(2,6,Convert.ToInt32(atkBns));
                    }
                    attackTest("run");
                }
                luck_mode = "";
            break;
        }
        if (actionSelect != 4){
            loaded();
        }
        if (actionSelect != 2){
            anti_spam = false;
            morte();
        }
    }
    timelineDilation(); // Sans Passive
}

void loop(){ // actions: [A] attack, [D] defend, [S] luck or [I] item
    if (firstEncounter == false && anti_spam == false){
        interactNumber = dice(1,4,0);
        if (secret == 2){
            cursedAction = dice(1,4,1);
        }
    }
    else if (firstEncounter == true && anti_spam == false){
        interactNumber = 1;
        firstEncounter = false;
        if (secret == 2){
            cursedAction = dice(1,4,1);
        }
    }
    anti_spam = true;
    switch (interactNumber){
        case 1:
            textDialog("✶ "+narrador[0]+"\n\n",2,ConsoleColor.White);
        break;
        case 2:
            textDialog("✶ "+narrador[1]+"\n\n",2,ConsoleColor.White);
        break;
        case 3:
            textDialog("✶ "+narrador[2]+"\n\n",2,ConsoleColor.White);
        break;
        default:
            textDialog("✶ "+narrador[3]+"\n\n",2,ConsoleColor.White);
        break;
    }
    actionBox();
    anti_spam = false;
    morte();
    anti_spam = true;
    if (player_hp > 0){
        ConsoleKeyInfo action = Console.ReadKey()!;
        if (action.Key == ConsoleKey.Enter){
            actingAction();
            if (actionSelect == cursedAction){
                player_kr += 1;
            }
        }
        else if (action.Key == ConsoleKey.UpArrow){
            if (actionSelect > 1){
                actionSelect -= 1;
            } else{
                actionSelect = 5;
            }
            if (actionSelect == cursedAction){
                player_kr += 1;
            }
        }
        else if (action.Key == ConsoleKey.DownArrow){
            if (actionSelect < 5){
                actionSelect += 1;
            } else{
                actionSelect = 1;
            }
            if (actionSelect == cursedAction){
                player_kr += 1;
            }
        }
    }
}

void PlayerHPKR(){
coditionEffect();
bar(player_hp,you[0],"Player");
ConsoleColor actualColor = normalHeart;
if (player_kr > 0){
    actualColor = karmaHeart;
}
textDialog("HP ",-9,actualColor);
for (int i = 0; i < heartLength; i++){
    if ((player_hp-player_kr) < ((i+1)*(you[0]/heartLength)) && player_kr > 0 && playerhpbar[i] == heartShape){
        Console.ForegroundColor = karmaHeart;
    }
    else if (playerhpbar[i] == heartShape){
        Console.ForegroundColor = normalHeart;
        if ((player_hp-player_bl) < ((i+1)*(you[0]/heartLength)) && player_bl > 0){
            Console.ForegroundColor = bloodHeart;
        }
    }
    else{
        Console.ForegroundColor = voidHeart;
    }
    Console.Write(playerhpbar[i]);
}
if (player_kr > 0){
    actualColor = karmaHeart;
}
textDialog($" {player_hp.ToString().PadLeft(2,'0')}/{you[0].ToString().PadLeft(2,'0')}",-9,actualColor);
}

void playerStatus(){
    bar(player_hp,you[0],"Player");
    plaHL = 16-playerNM.Length/2;
    textDialog(startNm.ToString().PadLeft((plaHL),sliceBar)+playerNM+endNm.ToString().PadRight((plaHL),sliceBar)+"\n",-9,ConsoleColor.Cyan);
    Levelviolence();
    textDialog("LV "+you[3].ToString().PadRight(3,' '),-9,ConsoleColor.White);
    randomBar();
    textDialog($"EXP {you[4].ToString().PadLeft(3,' ')}/{xpMath.ToString().PadRight(3,' ')}",-9,ConsoleColor.White);
    randomBar();
    textDialog($"{gold.ToString().PadLeft(2,' ')} Gold\n",-9,ConsoleColor.Yellow);
    PlayerHPKR();
    textDialog($" AT {you[1]}➶ ",-9,ConsoleColor.DarkRed);
    textDialog($" LU {you[2]}✤\n",-9,ConsoleColor.DarkGreen);
    textDialog($"{player_kr.ToString().PadRight(3,' ')} KR",-9,karmaHeart);
    randomBar();
    textDialog($"{player_bl.ToString().PadRight(3,' ')} BL\n\n",-9,bloodHeart);
}

void MonsterHPBar(){
    bar(hp,enemy[0],"Monster");
    textDialog("HP ",-9,normalHeart);
    for (int i = 0; i < hpbar.Length; i++){
        if (hpbar[i] == heartShape){
            Console.ForegroundColor = normalHeart;
        }
        else{
            Console.ForegroundColor = voidHeart;
        }
        Console.Write(hpbar[i]);
    }
    textDialog($" {hp.ToString().PadLeft(2,'0')}/{enemy[0].ToString().PadLeft(2,'0')}",-9,normalHeart);
}

void monsterStatus(){
    Console.Clear();
    bar(hp,enemy[0],"Monster");
    monHL = 16-monsterNM.Length/2;
    ConsoleColor customColor = ConsoleColor.DarkRed;
    if (secretEcounter){
    customColor = ConsoleColor.Black;
    }
    textDialog(startNm.ToString().PadLeft((monHL),sliceBar)+monsterNM+endNm.ToString().PadRight((monHL),sliceBar)+"\n",-9,customColor);
    textDialog("LV "+enemy[2].ToString().PadRight(2,' ')+" ",-9,ConsoleColor.White);
    randomBar();
    textDialog("EXP ?/?\n",-9,ConsoleColor.White);
    MonsterHPBar();
    textDialog($" AT {enemy[1]}➶ ",-9,ConsoleColor.DarkRed);
    textDialog($" LU {st}✤\n\n",-9,ConsoleColor.DarkGreen);
}


void turns(){ // battle stats + loop
    morte();

    if (player_hp > 0){
        // --------------------------------------Monster-----------------------------------------
        monsterStatus();
        // --------------------------------------Player-----------------------------------------
        playerStatus();
        loop();
        if (hp <= 0 && stage < finalStage){ // stage/floor increase when monster die
            secretEncounter();
            firstEncounter = true;
            updt();
            shopSlot1 = 0;
            shopSlot2 = 0;
            shopSlot3 = 0;
            shopSlot4 = 0;
            for (int i = 0; i <= foodLength; i++){
                int foodDice = dice(1,foodLength,0);
                shop(foodDice);
                int foodChance = dice(1,100,0);  
                if (foodChance <= item[5]){
                    if (shopSlot1 != foodDice && shopSlot2 != foodDice && shopSlot3 != foodDice && shopSlot4 != foodDice){
                        if (shopSlot1 == 0){
                            shopSlot1 = foodDice;
                        } else if (shopSlot2 == 0){
                            shopSlot2 = foodDice;
                        } else if (shopSlot3 == 0){
                            shopSlot3 = foodDice;
                        } else if (shopSlot4 == 0){
                            shopSlot4 = foodDice;
                        }
                    }
                }
            }
            selected = 1;
            shopCall(shopSlot1,shopSlot2,shopSlot3,shopSlot4);
            turns();
        }
        else if (hp > 0 && stage <= finalStage){ // when you still alive and enemy too, then repeat
            Console.Clear();
            turns();
        }
    }
    else if(player_hp <= 0){ // when player dies
        Console.Clear();
        Console.ResetColor();
    }
}
secretEncounter();
updt(); // first monster update status
turns(); // begin of battle