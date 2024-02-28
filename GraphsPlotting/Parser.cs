using System;

namespace GraphsPlotting
{
    class Expression
    {
        public string Token;
        //public Expression arg1;
        //public Expression arg2;
        public Expression[] Args = new Expression[0];
        public Expression(string token) { this.Token = token; }
        public Expression(string token, Expression a)
        {
            this.Token = token;
            this.Args = new Expression[1];
            this.Args[0] = a;
        }
        public Expression(string token, Expression a, Expression b)
        {
            this.Token = token;
            this.Args = new Expression[2];
            this.Args[0] = a;
            this.Args[1] = b;
        }
    }
    class Parcer
    {
        private string _input;
        private int _i;
        public Parcer(string expression)
        {
            _input = expression;
        }

        private string Parse_token()
        {
            if (_i > _input.Length - 1) return "";

            while (_input[_i] == ' ')
                if (_i < _input.Length - 1) ++_i; else break;

            if (Char.IsDigit(_input, _i))
            {
                string number = "";
                while (Char.IsDigit(_input, _i) || _input[_i] == '.')
                {
                    number += _input[_i];
                    ++_i;
                    if (_i > _input.Length - 1)
                        break;
                }
                return number;
            }

            switch (_input[_i])
            {
                case 'x':
                    ++_i;
                    return "x";
                case '+':
                    ++_i;
                    return "+";
                case '-':
                    ++_i;
                    return "-";
                case '*':
                    ++_i;
                    return "*";
                case '/':
                    ++_i;
                    return "/";
                case '^':
                    ++_i;
                    return "^";
                case 's':
                    if (_input[_i + 1] == 'i')
                    {
                        _i += 2;
                        goto case 'n';
                    }
                    if (_input[_i + 1] == '(')
                    {
                        ++_i;
                        return "cos";
                    }
                    break;
                case 'n':
                    if (_input[_i] == 'n')
                    {
                        ++_i;
                        return "sin";
                    }
                    break;

                case 'c':
                    if (_input[_i + 1] == 'o' && _input[_i + 2] == 's')
                        _i += 3;
                    return "cos";

                case '(':
                    ++_i;
                    return "(";
                case ')':
                    ++_i;
                    return ")";
            }
            return "";
        }


        private Expression Parse_simple_expression()
        {
            var token = Parse_token();
            if (token == "")
                return new Expression("Ivaleb");

            if (token == "(")
            {
                var result = Parse();
                if (Parse_token() != ")") return new Expression("Expexted ) , dalba'b");
                return result;
            }

            if (Char.IsDigit(token[0]) || token[0] == 'x')
                return new Expression(token);

            var arg = Parse_simple_expression();

            return new Expression(token, arg);
        }

        private int Get_priority(string op)
        {
            switch (op)
            {
                case "+":
                    return 1;
                case "-":
                    return 1;
                case "*":
                    return 2;
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 0;
            }
        }
        Expression Parse_binary_expression(int minPriority)
        {
            var leftExpr = Parse_simple_expression();

            while (true)
            {
                var op = Parse_token();
                var priority = Get_priority(op);
                if (priority <= minPriority)
                {
                    _i -= op.Length;
                    return leftExpr;
                }

                var rightExpr = Parse_binary_expression(priority);
                leftExpr = new Expression(op, leftExpr, rightExpr);
            }
        }

        public Expression Parse()
        {
            return Parse_binary_expression(0);
        }

        public double Calculate(Expression expr, double x)
        {
            switch (expr.Args.Length)
            {
                case 2:
                    var a = Calculate(expr.Args[0], x);
                    var b = Calculate(expr.Args[1], x);
                    if (expr.Token == "+") return a + b;
                    if (expr.Token == "-") return a - b;
                    if (expr.Token == "*") return a * b;
                    if (expr.Token == "/") return a / b;
                    if (expr.Token == "^") return Math.Pow(a, b);
                    if (expr.Token == "mod") return (int)a % (int)b;
                    return -1;

                case 1:
                    var c = Calculate(expr.Args[0], x);
                    if (expr.Token == "+") return +c;
                    if (expr.Token == "-") return -c;
                    if (expr.Token == "abs") return Math.Abs(c);
                    if (expr.Token == "sin") return Math.Sin(c);
                    if (expr.Token == "cos") return Math.Cos(c);
                    return -3;

                case 0:
                    if (expr.Token == "x")
                        return (double)x;
                    return Double.Parse(expr.Token);
            }

            return -2;
        }
    }
}
