using System;
using System.Collections.Generic;

namespace Epine.Infrastructure.Utilities {
	public static class SmsUtility {

		public static int CalculateParts(string value, out bool is7Bit ) {
			var response = 0;
			 is7Bit = true;

			foreach (var character in value) {
				if ((character >= 'a' && character <= 'z') || (character >= ' ' && character <= 'Z')) {
					response++;
				}
				else if (character == '^' || character == '{' || character == '}' || character == '|' || character == '\\' || character == '[' || character == ']' || character == '~' || character == 0x80 /* € */ ) {
					response += 2;
				}
				else if (character == '\n' || character == '\r' || character == '_' || character == 0xa1 /* ¡ */ ||
						character == 0xa3 /* £ */ || character == 0xa4 /* ¤ */ || character == 0xa5 /* ¥ */ ||
						character == 0xa7 /* § */ || character == 0xbf /* ¿ */ || character == 0xc4 /* Ä */ ||
						character == 0xc5 /* Å */ || character == 0xc6 /* Æ */ || character == 0xc7 /* Ç */ ||
						character == 0xc9 /* É */ || character == 0xd1 /* Ñ */ || character == 0xd6 /* Ö */ ||
						character == 0xd8 /* Ø */ || character == 0xdc /* Ü */ || character == 0xdf /* ß */ ||
                        character == 0xe0 /* à */ || character == 0xe4 /* ä */ || character == 0xe5 /* å */ || 
                        character == 0xe6 /* æ */ || character == 0xe7 /* ç */ ||
                        character == 0xe8 /* è */ || character == 0xe9 /* é */ || character == 0xec /* ì */ ||
						character == 0xf1 /* ñ */ || character == 0xf2 /* ò */ || character == 0xf6 /* ö */ ||
						character == 0xf8 /* ø */ || character == 0xf9 /* ù */ || character == 0xfc /* ü */ ) {

					response++;
				}
				else {
					is7Bit = false;
                  
				}
			}

			if (is7Bit) {

				if (response <= 160) {
					return 1;
				}
				else {
					response = (int)Math.Ceiling((double)response / 153);
				}
			}
			else {
				response = value.Length;

				if (response <= 70) {
					return 1;
				}
				else {
					response = (int)Math.Ceiling((double)response / 67);
				}
			}


			return response;
		}


        public static int CalculateParts(string value, out bool is7Bit, out List<char> charlist )
        {
            charlist  = new List<char>();
            var response = 0;
            is7Bit = true;

            foreach (var character in value)
            {
                if ((character >= 'a' && character <= 'z') || (character >= ' ' && character <= 'Z'))
                {
                    response++;
                }
                else if (character == '^' || character == '{' || character == '}' || character == '|' || character == '\\' || character == '[' || character == ']' || character == '~' || character == 0x80 /* € */ )
                {
                    response += 2;
                }
                else if (character == '\n' || character == '\r' || character == '_' || character == 0xa1 /* ¡ */ ||
                        character == 0xa3 /* £ */ || character == 0xa4 /* ¤ */ || character == 0xa5 /* ¥ */ ||
                        character == 0xa7 /* § */ || character == 0xbf /* ¿ */ || character == 0xc4 /* Ä */ ||
                        character == 0xc5 /* Å */ || character == 0xc6 /* Æ */ || character == 0xc7 /* Ç */ ||
                        character == 0xc9 /* É */ || character == 0xd1 /* Ñ */ || character == 0xd6 /* Ö */ ||
                        character == 0xd8 /* Ø */ || character == 0xdc /* Ü */ || character == 0xdf /* ß */ ||
                        character == 0xe0 /* à */ || character == 0xe4 /* ä */ || character == 0xe5 /* å */ ||
                        character == 0xe6 /* æ */ || character == 0xe7 /* ç */ ||
                        character == 0xe8 /* è */ || character == 0xe9 /* é */ || character == 0xec /* ì */ ||
                        character == 0xf1 /* ñ */ || character == 0xf2 /* ò */ || character == 0xf6 /* ö */ ||
                        character == 0xf8 /* ø */ || character == 0xf9 /* ù */ || character == 0xfc /* ü */ )
                {

                    response++;
                }
                else
                {
                    charlist.Add(character);
                    is7Bit = false;

                }
            }

            if (is7Bit)
            {

                if (response <= 160)
                {
                    return 1;
                }
                else
                {
                    response = (int)Math.Ceiling((double)response / 153);
                }
            }
            else
            {
                response = value.Length;

                if (response <= 70)
                {
                    return 1;
                }
                else
                {
                    response = (int)Math.Ceiling((double)response / 67);
                }
            }


            return response;
        }
    }
}
