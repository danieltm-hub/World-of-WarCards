using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace Compiler
{
    public class Token
    {
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public CodeLocation Location { get; private set; }
        public Token(TokenType type, string value, CodeLocation location)
        {
            Type = type;
            Value = value;
            Location = location;
        }

        public override string ToString()
        {
            return $"Type:[{Type}] Value:[{Value}]";
        }

    }

    public class TokenReader
    {
        string Filename;
        string Code;
        public int pos;
        int line;
        int lastLB;

        public TokenReader(string filename, string code)
        {
            Filename = filename;
            Code = code;
            pos = 0;
            line = 1;
            lastLB = -1;
        }

        public CodeLocation Location => new CodeLocation(Filename, line, pos - lastLB);

        public char Peek()
        {
            if (pos < 0 || EOF) throw new Exception("Index out in Peek in TokenReader");

            return Code[pos];
        }

        public bool Reset(int backpos)
        {
            if (backpos < 0 || Code.Length <= backpos) return false;
            pos = backpos;
            return true;
        }

        public bool IOF => pos == 0;
        public bool EOF => pos >= Code.Length;
        public bool EOL => EOF || Code[pos] == '\n';

        public bool ContinuesWith(string prefix)
        {
            if (prefix.Length + pos > Code.Length) return false;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (Code[pos + i] != prefix[i]) return false;
            }

            return true;
        }

        public bool Match(string prefix, bool nextpos = true)
        {
            if (ContinuesWith(prefix))
            {
                if (nextpos) pos += prefix.Length;
                return true;
            }
            return false;
        }

        public bool ValidIdCharacter(char c, bool begining)
        {
            return c == '_' || begining ? char.IsLetter(c) : char.IsLetterOrDigit(c);
        }

        public bool ReadID(out string id)
        {
            id = "";
            while (!EOL && ValidIdCharacter(Peek(), id.Length == 0))
                id += ReadAny();

            return id.Length > 0;
        }

        public char ReadAny()
        {
            if (EOF) throw new Exception("EOF limit Exceed in ReadAny, error in code");

            if (EOL)
            {
                line++;
                lastLB = pos;
            }

            return Code[pos++];
        }

        public char ReadBack()
        {
            if (IOF) throw new Exception("Reading out Code input, in ReadBack pos == 0");
            return Code[pos--];
        }
        public bool ReadNumber(out string number)
        {
            number = "";

            //negative, lo que pense tambier era hacer un valid number char como en valid id number

            while (!EOL && char.IsDigit(Peek())) number += ReadAny();

            if (number.Length == 0) return false; //case .231

            if (!EOL && Match("."))
            {
                number += ".";

                int partial = number.Length;

                while (!EOL && char.IsDigit(Peek())) number += ReadAny();

                if (number.Length == partial) return false; //case 1334. 
            }

            return true;
        }

        public bool ReadText(out string text)
        {
            text = "";

            if (Match("\""))
            {

                while (!Match("\""))
                {
                    if (EOL) return false;

                    text += ReadAny();
                }

                return true;
            }
            return false;
        }
        public bool ReadUntil(string end, out string text, bool include = false)
        {
            text = "";
            while (!Match(end))
            {
                if (EOL) return false;

                text += ReadAny();
            }

            text += (include) ? end : "";

            return true;
        }

        public bool ReadWhiteSpace()
        {
            if (char.IsWhiteSpace(Peek()))
            {
                ReadAny();
                return true;
            }
            return false;
        }
    }

}