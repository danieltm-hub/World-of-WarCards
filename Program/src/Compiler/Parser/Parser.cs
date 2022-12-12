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

        public Expression? ParseExpressionCall(List<Error> CompileErrors)
        {
            return ParseExpression();
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

            // Current Token is Card

            // Name
            if (CheckToken(TokenType.ID, CompilerErrors)) name = Reader.Peek().Value;


            // Add cycle to read multiple effects
            // {
            CheckToken(TokenType.LSquareBracket, CompilerErrors);

            // Effect : Check to stop when missing ;
            effect = ParseEffector(CompilerErrors);

            // }
            CheckToken(TokenType.RSquareBracket, CompilerErrors);

            return new Card(name, effect, Reader.Peek().Location);
        }

        public Effector ParseEffector(List<Error> CompilerErrors)
        {
            CodeLocation location = Reader.Peek().Location;

            Objective objective = new NullObjective(new CodeLocation());

            Power power = new NullPower(new CodeLocation());
            
            //first token is Objective, add cycle to read multiple objectives
            if (CheckToken(TokenType.Objective, CompilerErrors)) objective = ParseObjective(CompilerErrors);

            //second token is Power, add cycle to read multiple powers
            if (CheckToken(TokenType.Power, CompilerErrors)) power = ParsePower(CompilerErrors);

            return new Effector(new List<Objective> { objective }, new List<Power> { power }, location);
        }

        public Objective ParseObjective(List<Error> CompilerErrors)
        {
            //current token is Objective
            CodeLocation location = Reader.Peek().Location;
            Reader.MoveNext();

            //Specific Objective to Parsed
            throw new Exception("ParseObjective was't implemented");
        }
        public Power ParsePower(List<Error> CompilerErrors)
        {
            //current token is Power
            CodeLocation location = Reader.Peek().Location;
            Reader.MoveNext();

            //Specific Power to Parsed

            throw new Exception("ParsePower was't implemented");
        }

        public bool CheckToken(TokenType token, List<Error> CompilerErrors, bool pass = true)
        {
            if (Reader.Match(token, pass)) return true;
           
            CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, Reader.Peek().Value));
            return false;
        }


        #region Expression
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
    #endregion
}