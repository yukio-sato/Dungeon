# Dungeon
Simulator of "Fighting Fantasy" on C#.
___
## 🎮 How to play! 🎮
- First you need to have a installed [.NET SDK](https://dotnet.microsoft.com/download) to run C# in Visual Studio or in terminal itself.
- When installed, go to the folder and use the command "dotnet run" in terminal, and then done! Have fun!
___
## ⚙️ Game Explanation ⚙️
| Status |               Description                   |
|--------|------------------------------------------|
|   HP   | Amount of damage you can get before die.<br>If you HP reach to 0 you loose! |
|   AT   | Value that helps to land a hit on enemy.<br>But its doesnt define 100% if you hit. |
|   LU   | Value that make you get "lucky" or "unlucky" when used.<br>Lucky: increase your damage by 2 and reduce damage taken by 1.<br>Unlucky: decrease your damage by 1 and increase damage taken by 1.<br>Monster Mechanic: when attacked its dodges. |
|   LV   | Level of Violence, its mean how dangerous you are.<br>When more LV, more EXP needed and grants more AT and HP. |
|   EXP  | Execution Points, its mean how much monster you have hurt too far. |
|  Gold  | Value dropped from monster when killed.<br>Have a certain amount of it, can buy some items. |
|  Item  | Earned by buying in shop along the adventure.<br>Grants Heal and some buffs, but isnt every food that doesnt have debuffs. |
|   KR   | Karma: damages you until you HP reach 1 or the KR value reach 0.
|   BL   | Blood: reduce the amount of heal by BL value, when healed its decrease it. |

The game ends when you reach the last floor from the "Dungeon".
___
## ⚠️ Hardmode ⚠️
In this mode, its possible to find some Secret Bosses, and even the enemies and items changes. Good luck trying to beat it!
___
