var stage = 1, finalStage = 2;

var secretEncouter = false;

var ImageLink = "";

var player_HP = 0, player_KR = 0, exp = 0, gold = 0;
var monster_HP = 0, monster_KR = 0;

var you = [
    "", // name
    1, // lv
    10, // next
    0, // at
    0, // hp
    0, // lu
    "", // food1
    "", // food2
    "", // food3
    "" // food4
]

var monster = [
    "", // name
    1, // lv
    0, // exp
    0, // gold
    0, // hp
    0, // at
    0, // lu
    0, // karma
    0 // linkedBattle
]

function guiding(newLocation)
{
document.location.href = newLocation;
}

function expCalculator() {
    you.next = 10 + (15*(you.lv-1))
    if (you.next >= exp)
    {
        exp -= you.next;
        you.lv += 1;
        if (you.lv % 4 != 0)
        {
            you.at += 1;
            alert("AT");
        }
        else if (you.lv % 4 == 0)
        {
            you.hp += 4;
            player_HP += 4;
            alert("HP");
        }
    }
}

function updt()
{
    if (secretEncouter == false)
    {
        switch (stage) { // [0] Name [1] LV, [2] EXP [3] Gold [4] HP [5] AT [6] LU [7] KR [8] LinkedBattle
            case 1:
                monster = ["teste",1,15,3,20,5,0,0,0];
                ImageLink = "data:image/png;base64,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";
            break;
            case 2:
                monster = ["irineu",1,15,3,20,5,0,0,0];
                ImageLink = "images/teste.png";
            break;
            default:
                monster = ["",0,0,0,0,0,0,0,0];
                ImageLink = "";
            break;
        }
    }
    monster_HP = monster[4];
    monster_KR = 0;
    document.getElementById("Nm").textContent = monster[0];
    document.getElementById("Img").src = ImageLink;
    document.getElementById("bar").value = monster_HP;
    document.getElementById("bar").max = monster[4];
    stage += 1;
}
function dmg(){
    if (monster_HP > 1)
    {
        monster_HP -= 1;
    }
    document.getElementById("bar").value = monster_HP;
    document.getElementById("bar").max = monster[4];
}