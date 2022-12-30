using System;
using System.Collections.Generic;

namespace KeyboardMenu
{
    class TextManagment
    {
        public static List<string> Normalize(string text, int width, int heigth)
        {
            List<string> listText= new List<string>();
            if(heigth<1) return listText;
            if(text.Length < width) 
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
        public static void NormalizeRec(string text, int width, int height, int index, List<string> list)
        {
            if(index == height) 
            {
                list.Add(text.Substring(0, Math.Min(width, text.Length-1)));
                return;
            }
            if(text.Length<width) 
            {
                list.Add(text);
                return;
            }
            string newText = text.Substring(0, width);
            list.Add(newText);
            NormalizeRec(text.Substring(width), width, height, index+1, list);
        }
    }
}
    
