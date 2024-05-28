﻿using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E3Core.Data
{
    public class MelodyIfs
    {
        public String MelodyName;
        public String MelodyIf;
		public String MelodyIfName;
        public MelodyIfs()
        {

        }
        public MelodyIfs(string melodyName, IniData parsedData)
        {
            string[] melodyArray = melodyName.Split('/');

            MelodyName = melodyArray[0];

            if(melodyArray.Length>1)
            {
				MelodyIfName = Spell.GetArgument<string>(melodyArray[1]);
                var section = parsedData.Sections["Ifs"];
                if (section != null)
                {
                    var keyData = section[MelodyIfName];
                    if (!String.IsNullOrWhiteSpace(keyData))
                    {
                        MelodyIf = keyData;
                    }
                }
            }
        }
		public string ToConfigEntry()
		{
			//This is C#'s ternary conditional operator
			//its condition if true do 1st, else 2nd. 
			//in this case, if ifskeys is null or empty, set to string empty
			//else use /Ifs|{IfsKeys}
			string t_Ifs = (String.IsNullOrWhiteSpace(this.MelodyIfName)) ? String.Empty : $"/Ifs|{MelodyIfName}";
			
			return $"{MelodyName}{t_Ifs}";
		}
	}
}
