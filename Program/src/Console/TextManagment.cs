using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Visual
{
    class TextManagment
    {
        public static List<string> Normalize(string text, int width, int heigth)
        {
            List<string> listText = new List<string>();
            if (heigth < 1) return listText;
            string[] splited;
            if (text.Contains("\n"))
            {
                splited = text.Split("\n");
                int numberofLines = splited.Length;
                int numberofLinesLeft = numberofLines % heigth;
                if (numberofLines - numberofLinesLeft > 0)
                {
                    for (int i = 0; i < numberofLines - numberofLinesLeft; i++)
                    {
                        splited[i] = Regex.Replace(splited[i], "\n", " ");
                        NormalizeRec(splited[i], width, heigth - listText.Count - 1, 0, listText);
                        if (listText.Count >= heigth) return listText;
                    }
                }
                else
                {
                    for (int i = 0; i < numberofLines; i++)
                    {
                        splited[i] = Regex.Replace(splited[i], "\n", " ");
                        NormalizeRec(splited[i], width, (heigth - listText.Count - 1)<0? 0 : (heigth - listText.Count), 0, listText);
                        if (listText.Count >= heigth) return listText;
                    }
                    return listText;
                }

            }
            if (text.Length < width)
            {
                listText.Add(text);
                return listText;
            }
            else
            {
                NormalizeRec(text, width, heigth, 0, listText);
                return listText;
            }

        }
        private static void NormalizeRec(string text, int width, int height, int index, List<string> list)
        {
            if (index == height)
            {
                list.Add(text.Substring(0, Math.Min(width, Math.Min(text.Length - 1, 0))));
                return;
            }
            if (text.Length < width)
            {
                list.Add(text);
                return;
            }
            string newText = text.Substring(0, width);
            list.Add(newText);
            NormalizeRec(text.Substring(width), width, height, index + 1, list);
        }


    }
}

