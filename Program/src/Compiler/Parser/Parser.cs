using System;
using System.IO;
using GameProgram;
using AST;

namespace Compiler
{
    public class Parser
    {
        public TokenStream Reader { get; private set; }
        private List<Error> CompilerErrors;

        public Parser(TokenStream reader)
        {
            Reader = reader;
            CompilerErrors = new List<Error>();
        }

        // 

        public Effector? ParseEffectorCall(out List<Error> compileErrors)
        {
            compileErrors = CompilerErrors;

            if (Reader.OUT(1)) return null;

            return ParseEffector();
        }
        public Power? ParsePowerCall(out List<Error> compileErrors)
        {
            compileErrors = CompilerErrors;

            if (Reader.OUT(1)) return null;

            if (CheckToken(TokenType.Power)) return ParsePower();

            return null;
        }

        public WarCardProgram ParseProgram()
        {
            WarCardProgram program = new WarCardProgram(new CodeLocation());

            while (!Reader.OUT(1))
            {
                if (CheckToken(TokenType.Card))
                {
                    Card? card = ParseCard();
                    if (card != null) program.AddCard(card);
                }
                
            }

            foreach(Error error in CompilerErrors)
            {
                System.Console.WriteLine(error);
            }

            return program;
        }

        public Card? ParseCard()
        {
            string name = "";

            CodeLocation location = Reader.Peek().Location;

            List<Effector> effects = new List<Effector>();

            // Current Token is Card

            // Name
            if (CheckToken(TokenType.ID)) name = Reader.Peek().Value;


            // Add cycle to read multiple effects
            // {
            CheckToken(TokenType.LBracket);
            do
            {

                Effector? effect = ParseEffector();
                if(effect != null)
                {
                    effects.Add(effect);
                }

            }
            while(Reader.Match(TokenType.Comma));

            // }
            CheckToken(TokenType.RBracket);
            
            if (effects.Count != 0)
            {
                return new Card(name, effects, Reader.Peek().Location);
            }
            return null;
        }

        public Effector? ParseEffector()
        {
            if (!CheckToken(TokenType.LSquareBracket)) return null; // exception [ ]

            //  Current Token is [

            CodeLocation location = Reader.Peek().Location;

            List<Objective> objectives = new List<Objective>();

            List<Power> powers = new List<Power>();

            // Parse Objectives and add them to the list
            do
            {
                if (CheckToken(TokenType.Objective))
                {
                    Objective? objective = ParseObjective();
                    if (objective == null) CompilerErrors.Add(new Error(ErrorCode.Expected, location, "An Objective"));
                    else objectives.Add(objective);
                }
            }
            while (Reader.Match(TokenType.Comma));

            CheckToken(TokenType.Breaker);

            // Parse Powers and add them to the list
            do
            {
                if (CheckToken(TokenType.Power))
                {
                    Power? power = ParsePower();
                    if (power == null) CompilerErrors.Add(new Error(ErrorCode.Expected, location, "An power"));
                    else
                    {
                        powers.Add(power);
                    }
                }
            }
            while (Reader.Match(TokenType.Comma));

            CheckToken(TokenType.RSquareBracket);

            return new Effector(objectives, powers, location);
        }

        public Objective ParseObjective()
        {
            //current token is Objective
            CodeLocation location = Reader.Peek().Location;
            string objectiveName = Reader.Peek().Value;

            List<Expression> parameters = new List<Expression>();

            CheckToken(TokenType.LParen);

            if (!Reader.Match(TokenType.RParen, false))
            {
                do
                {
                    Expression? parameter = ParseExpression();

                    if (parameter == null) CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, $"An Expression"));
                    else parameters.Add(parameter);

                }
                while (Reader.Match(TokenType.Comma));
            }

            CheckToken(TokenType.RParen);

            return (Objective)Reflection.Reflect(objectiveName, parameters, location);
        }
        public Power ParsePower()
        {
            //current token is Power
            CodeLocation location = Reader.Peek().Location;
            string powerName = Reader.Peek().Value;

            List<Expression> parameters = new List<Expression>();

            CheckToken(TokenType.LParen);

            if (!Reader.Match(TokenType.RParen, false))
            {
                do
                {
                    Expression? parameter = ParseExpression();

                    if (parameter == null) CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, $"An Expression"));
                    else parameters.Add(parameter);
                }
                while (Reader.Match(TokenType.Comma));
            }

            CheckToken(TokenType.RParen);

            return (Power)Reflection.Reflect(powerName, parameters, location);
        }

        public bool CheckToken(TokenType token, bool pass = true)
        {
            if (Reader.Match(token, pass)) return true;

            if (Reader.OUT(1)) CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, token.ToString()));

            else CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek(1).Location, token.ToString()));

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

        private Expression? ParseExpressionLv1Maker(Expression? left) // + - > < == !=  >= <=
        {
            Expression? exp = // +
                ParseBinaryOp(left, TokenType.Sum, (left, right, location) => new Add(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // -
                ParseBinaryOp(left, TokenType.Sub, (left, right, location) => new Sub(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            // booleans
            exp = // <
                ParseBinaryOp(left, TokenType.Smaller, (left, right, location) => new SmallerThan(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // >
                ParseBinaryOp(left, TokenType.Greater, (left, right, location) => new GreaterThan(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // == 
                ParseBinaryOp(left, TokenType.Equal, (left, right, location) => new Equal(left, right, location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // !=
                ParseBinaryOp(left, TokenType.NotEqual, (left, right, location) => new Not(new Equal(left, right, location), location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // <=
                ParseBinaryOp(left, TokenType.SmallerEqual, (left, right, location) => new Or(new SmallerThan(left, right, location), new Equal(left, right, location), location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            exp = // >=
                ParseBinaryOp(left, TokenType.GreaterEqual, (left, right, location) => new Or(new GreaterThan(left, right, location), new Equal(left, right, location), location),
                (left) => ParseExpressionLv2(left), (left) => ParseExpressionLv1Maker(left));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv2(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv3(left);
            return ParseExpressionLv2Maker(newLeft);
        }

        private Expression? ParseExpressionLv2Maker(Expression? left) // * / && ||
        {
            Expression? exp = // *
                ParseBinaryOp(left, TokenType.Mul, (left, right, location) => new Mult(left, right, location),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp != null) return exp;

            exp = // /
                ParseBinaryOp(left, TokenType.Div, (left, right, location) => new Div(left, right, location),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp != null) return exp;

            exp = // &&
                ParseBinaryOp(left, TokenType.And, (left, right, location) => (new And(left, right, location)),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp != null) return exp;

            exp = // ||
                ParseBinaryOp(left, TokenType.Or, (left, right, location) => (new Or(left, right, location)),
                (left) => (ParseExpressionLv3(left)), (left) => (ParseExpressionLv2Maker(left)));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv3(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv4(left);
            return ParseExpressionLv3Maker(newLeft);
        }

        private Expression? ParseExpressionLv3Maker(Expression? left) // pow
        {
            Expression? exp =
                ParseBinaryOp(left, TokenType.Pow, (left, right, location) => new Pow(left, right, location),
                (left) => (ParseExpressionLv4(left)), (left) => (ParseExpressionLv3Maker(left)));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv4(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv5(left);
            return ParseExpressionLv4Maker(newLeft);
        }

        private Expression? ParseExpressionLv4Maker(Expression? left) //sin cos !
        {
            Expression? exp = //Sen
                ParseUnaryOp(left, TokenType.Sin, (right, location) => new Sin(right, location),
                (left) => (ParseExpressionLv5(left)), (left) => (ParseExpressionLv4Maker(left)));

            if (exp != null) return exp;

            exp = // Cos
                ParseUnaryOp(left, TokenType.Cos, (right, location) => new Cos(right, location),
                (left) => (ParseExpressionLv5(left)), (left) => (ParseExpressionLv4Maker(left)));

            if (exp != null) return exp;

            exp = // !
                ParseUnaryOp(left, TokenType.Not, (right, location) => new Not(right, location),
                (left) => (ParseExpressionLv5(left)), (left) => (ParseExpressionLv4Maker(left)));

            if (exp != null) return exp;

            return left;
        }

        private Expression? ParseExpressionLv5(Expression? left)
        {
            Expression? newLeft = ParseExpressionLv6(left);
            return ParseExpressionLv5Maker(newLeft);
        }

        private Expression? ParseExpressionLv5Maker(Expression? left) // ()
        {
            if (!Reader.Match(TokenType.LParen)) return left;

            Expression? exp = ParseExpression();
            CheckToken(TokenType.RParen);

            return (exp != null) ? exp : left;
        }

        private Expression? ParseExpressionLv6(Expression? left) //not included text, number bool
        {
            Expression? exp = // number
                ParseAtomicExpression(TokenType.Number, (atomic) => (new Number(double.Parse(atomic.Value), atomic.Location)));

            if (exp != null) return exp;

            exp = // true false
                ParseAtomicExpression(TokenType.Bool, (atomic) => (new Bool(bool.Parse(atomic.Value), atomic.Location)));

            if (exp != null) return exp;

            exp = // text
                ParseAtomicExpression(TokenType.Text, (atomic) => (new Text(atomic.Value, atomic.Location)));

            return (exp != null) ? exp : left;
        }

        private Expression? ParseAtomicExpression(TokenType atomicType, Func<Token, Expression> getExpression)
        {
            if (Reader.Match(atomicType))
            {
                return getExpression(Reader.Peek());
            }
            return null;
        }

        private Expression? ParseBinaryOp
            (Expression? left, TokenType oper, Func<Expression, Expression, CodeLocation, Expression> opBuild,
             Func<Expression?, Expression?> NextlvlParser, Func<Expression?, Expression?> BacklvlMaker)
        {
            if (left == null || !Reader.Match(oper)) return null;

            CodeLocation location = Reader.Peek().Location;

            Expression? right = NextlvlParser(null);

            //if (right != null) System.Console.WriteLine(right);

            if (right == null)
            {
                System.Console.WriteLine(Reader.Peek().Type);
                CompilerErrors.Add(new Error(ErrorCode.Invalid, location, $"Expected expression after {oper}"));
                Reader.MoveBack(2);
                return null;
            }

            return BacklvlMaker(opBuild(left, right, location));
        }

        private Expression? ParseUnaryOp(Expression? left, TokenType oper, Func<Expression, CodeLocation, Expression> opBuild,
                 Func<Expression?, Expression?> NextlvlParser, Func<Expression?, Expression?> BacklvlMaker)
        {
            if (left != null || !Reader.Match(oper)) return null;

            CodeLocation location = Reader.Peek().Location;
            Expression? right = NextlvlParser(null);

            if (right == null)
            {
                CompilerErrors.Add(new Error(ErrorCode.Invalid, location, $"Expected expression after {oper}"));
                Reader.MoveBack(1);
                return null;
            }

            return BacklvlMaker(opBuild(right, location));
        }
        #endregion
    }
}