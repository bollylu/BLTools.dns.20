﻿using System;
using BLTools.Frames;
using System.Diagnostics;
using BLTools;
using BLTools.ConsoleExtension;
using BLTools.Json;
using BLTools.Text;
using ConsoleEx = BLTools.ConsoleExtension.ConsoleExtension;


namespace BLTools.Json.ConsoleTest {
  class Program {
    static void Main(string[] args) {


      //string Source = "[{\"Id\":\"\",\"Name\":\"Disney002\",\"Description\":\"\",\"Header\":{\"Id\":\"\",\"Name\":\"Disney002\",\"Description\":\"Les dessins animés de Disney, volume 2\",\"CreatedBy\":\"Luc\",\"CreationTime\":\"2016-07-25T16:07:00\",\"Language\":\"FR\",\"Category\":\"fun\\/disney\"},\"Questions\":[{\"Id\":\"\",\"Name\":\"Princes\",\"Description\":\"Questions sur les princes et les héros\",\"Questions\":[{\"Id\":\"\",\"Name\":\"Qui vit en haut d'une tour\",\"Description\":\"\",\"QuestionType\":\"QCM1\",\"Choices\":[{\"Id\":\"\",\"Name\":\"Réponse\",\"Description\":\"Bizarre comme nom\",\"IsCorrect\":false},{\"Id\":\"\",\"Name\":\"Quasimodo\",\"Description\":\"Dans Notre-Dame de Paris\",\"IsCorrect\":true},{\"Id\":\"\",\"Name\":\"Shrek\",\"Description\":\"Ogre vert\",\"IsCorrect\":false}]},{\"Id\":\"\",\"Name\":\"Qui commence comme voleur, puis devient prince\",\"Description\":\"\",\"QuestionType\":\"QCM1\",\"Choices\":[{\"Id\":\"\",\"Name\":\"Bob l'éponge\",\"Description\":\"Au fond de la mer\",\"IsCorrect\":false},{\"Id\":\"\",\"Name\":\"Aladdin\",\"Description\":\"Dans Aladdin\",\"IsCorrect\":true},{\"Id\":\"\",\"Name\":\"Hercule\",\"Description\":\"Fils de Zeus\",\"IsCorrect\":false}]}],\"Counter\":0}]},{\"Id\":\"\",\"Name\":\"Disney001\",\"Description\":\"\",\"Header\":{\"Id\":\"\",\"Name\":\"Disney001\",\"Description\":\"Les dessins animés de Disney, volume 1\",\"CreatedBy\":\"Luc\",\"CreationTime\":\"2016-07-25T14:17:00\",\"Language\":\"FR\",\"Category\":\"fun\"},\"Questions\":[{\"Id\":\"\",\"Name\":\"Princesses\",\"Description\":\"Questions sur les princesses\",\"Questions\":[{\"Id\":\"\",\"Name\":\"Qui est la plus belle ?\",\"Description\":\"\",\"QuestionType\":\"QCM1\",\"Choices\":[{\"Id\":\"\",\"Name\":\"Blanche-Neige\",\"Description\":\"La princesse Blanche-neige\",\"IsCorrect\":true},{\"Id\":\"\",\"Name\":\"Cendrillon\",\"Description\":\"La future princesse Cendrillon\",\"IsCorrect\":false},{\"Id\":\"\",\"Name\":\"Le Belle-au-bois-dormant\",\"Description\":\"La princesse qui s'endort\",\"IsCorrect\":false}]},{\"Id\":\"\",\"Name\":\"Qui nage et respire sous l'eau ?\",\"Description\":\"\",\"QuestionType\":\"QCM1\",\"Choices\":[{\"Id\":\"\",\"Name\":\"Arielle\",\"Description\":\"Dans la petite sirène\",\"IsCorrect\":true},{\"Id\":\"\",\"Name\":\"La grenouille\",\"Description\":\"Dans La princesse et la grenouille\",\"IsCorrect\":false},{\"Id\":\"\",\"Name\":\"O'Malley\",\"Description\":\"Dans les aristochats\",\"IsCorrect\":false}]}],\"Counter\":0}]}]";

      //string SourceWithNumbers = @"{""Achat"":12.39,""Vente"":14.39,""Benefice"":2.1}";


      string Source = @"
{""Name"":""\\[Blondes (Les)\\]"",
""Items"":[
{""Name"":""Tome 1"",""Number"":""01""},
{""Test"": [""Tome 2"",{""Number"":2.23}]},
{""Name"":true,""Number"":3}
]
}";

      JsonObject Test = JsonValue.Parse<JsonObject>(Source);

      Frame JsonFrame = new Frame(2, 2, Console.WindowWidth - 3, ( Console.WindowHeight / 2 ) - 1);
      JsonFrame.Title = $"JsonTest - Width={JsonFrame.Width} - Height={JsonFrame.Height}";
      string JsonTest = Test.RenderAsString(true);
      JsonFrame.Write(JsonTest);

      Frame EmptyFrame = new Frame(2, Console.WindowHeight - ( Console.WindowHeight / 2 ) + 1, Console.WindowWidth - 3, Console.WindowHeight - 3);
      EmptyFrame.Title = $"Process management - Width={EmptyFrame.Width} - Height={EmptyFrame.Height}";
      EmptyFrame.ManualRefresh = true;
      EmptyFrame.WriteLine("First attempt to write text\nwith a second line");
      EmptyFrame.WriteLine("And another line");
      EmptyFrame.Refresh();

      //JsonFrame.Refresh();

      //JsonFrame.Write("\n");
      //JsonFrame.Write(TextBox.BuildHorizontalRowWithText(" Process completed ", TextBox.HorizontalRowType.Backslash));

      //Console.WriteLine($"Benefice is {Test.SafeGetValueFirst<double>("Benefice", -1)}");

      ConsoleEx.Pause();
    }
  }
}
