using System;
using System.IO;
using GameProgram;
using AST;

namespace Compiler
{
    public class Parser
    {
        public TokenStream Reader { get; private set; }

        public Parser(TokenStream reader)
        {
            Reader = reader;
        }

        public WarCardProgram ParseProgram(List<Error> CompilerErrors)
        {
            WarCardProgram program = new WarCardProgram(new CodeLocation());

            if (Reader.OUT()) return program;

            while (!Reader.OUT())
            {
                if (Reader.Match(TokenType.Card))
                {
                    Card card = ParseCard(CompilerErrors);
                    program.AddCard(card);
                }
            }

            return program;
        }

        public Card ParseCard(List<Error> CompilerErrors)
        {
            string name = "";

            CodeLocation location = Reader.Peek().Location;

            Effector effect = new Effector(new List<Objective>(), new List<Power>(), new CodeLocation());

            //current token is Card
            //Reader.MoveNext();

            //name
            if (Reader.Match(TokenType.ID))
            {
                name = Reader.Peek().Value;
                Reader.MoveNext();
            }
            else
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "Card Name"));
            }

            // {
            if (Reader.Match(TokenType.LSquareBracket))
            {
                Reader.MoveNext();
            }
            else
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "{"));
            }

            //Powers (here I have two options, reads to ; or reads to '\n', now I choose to read to ';')
            effect = ParseEffector(CompilerErrors);

            // }
            if (!Reader.Match(TokenType.RSquareBracket))
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "}"));
            }

            return new Card(name, effect, Reader.Peek().Location);
        }

        public Effector ParseEffector(List<Error> CompilerErrors)
        {
            //first token is a Power
            CodeLocation location = Reader.Peek().Location;

            Power power = new NullPower(new CodeLocation());

            Objective objective = new NullObjective(new CodeLocation());

            while (!Reader.END && !Reader.Match(TokenType.Power))
            {
                CompilerErrors.Add(new Error(ErrorCode.Invalid, Reader.Peek().Location, Reader.Peek().Value));
                Reader.MoveNext();
            }


            if (Reader.Match(TokenType.Power))
            {
                power = ParsePower(CompilerErrors);
            }


            //second token is Objective

            while (!Reader.END && !Reader.Match(TokenType.Objective))
            {
                CompilerErrors.Add(new Error(ErrorCode.Invalid, Reader.Peek().Location, Reader.Peek().Value));
                Reader.MoveNext();
            }
            if (Reader.Match(TokenType.Objective))
            {
                objective = ParseObjective(CompilerErrors);
            }

            return new Effector(new List<Objective> { objective }, new List<Power> { power }, location);
        }

        public Power ParsePower(List<Error> CompilerErrors)
        {
            //current token is Power
            CodeLocation location = Reader.Peek().Location;
            Reader.MoveNext();

            //Specific Power to Parsed

            throw new Exception("ParsePower was't implemented");
        }
        public Objective ParseObjective(List<Error> CompilerErrors)
        {
            //current token is Objective
            CodeLocation location = Reader.Peek().Location;
            Reader.MoveNext();

            //Specific Objective to Parsed
            throw new Exception("ParseObjective was't implemented");
        }


        //Parser Expressions
        private Expression? ParseExpression()
        {
            return ParseExpressionLv1(null);
        }

        private Expression? ParseExpressionLv1(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv2(left);
            Expression? newExpression = ParseExpressionLv1Maker(newLeft);
            return newExpression;
        }
        private Expression? ParseExpressionLv1Maker(Expression? left)
        {
            Expression? exp =
                ParseBinaryOp(left, TokenType.Sum, (left, right, location) => new Add(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1(left));


            if (exp != null) return exp;

            exp =
                ParseBinaryOp(left, TokenType.Sum, (left, right, location) => new Sub(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1(left));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv2(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv3(left);
            return ParseExpressionLv2Maker(newLeft);
        }

        private Expression? ParseExpressionLv2Maker(Expression? left)
        {
            Expression? exp =
                ParseBinaryOp(left, TokenType.Mul, (left, right, location) => new Mult(left, right, location),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp == null) return exp;

            exp =
                ParseBinaryOp(left, TokenType.Div, (left, right, location) => new Div(left, right, location),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv3(Expression? left) //not included text
        {
            Expression? exp = ParseNumber();
            return exp;
        }

        private Expression? ParseNumber()
        {
            if (!Reader.OUT(1) && Reader.Match(TokenType.Number))
            {
                return new Number(double.Parse(Reader.Peek().Value), Reader.Peek().Location);
            }
            return null;
        }

        private Expression? ParseBinaryOp
            (Expression? left, TokenType oper, Func<Expression, Expression, CodeLocation, Expression> opBuild,
             Func<Expression?, Expression?> NextlvlParser, Func<Expression?, Expression?> BacklvlMaker)
        {
            CodeLocation location = Reader.Peek().Location;
            if (left == null || !Reader.Match(oper)) return null;

            Expression? right = NextlvlParser(null);

            if (right == null)
            {
                Reader.MoveBack(2);
                return null;
            }

            return BacklvlMaker(opBuild(left, right, location));
        }




    }
}