using System;
using System.Collections.Generic;

class Program
{   static void Main()
    {
        string expression = "20+(4+3*2)/5";
        double result = Evaluate(expression);
        Console.WriteLine($"Výsledek výrazu '{expression}' je: {result}");
    }

    static double Evaluate(string expression)
    {
        var outputQueue = new Queue<string>();
        var operatorStack = new Stack<string>();
        var tokens = Tokenize(expression);

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out double number))
            {
                outputQueue.Enqueue(token);
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }
                operatorStack.Pop(); 
            }
            else
            {
                while (operatorStack.Count > 0 && GetPrecedence(token) <= GetPrecedence(operatorStack.Peek()))
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
        }

        while (operatorStack.Count > 0)
        {
            outputQueue.Enqueue(operatorStack.Pop());
        }

        return EvaluatePostfix(outputQueue);
    }

    static Queue<string> Tokenize(string expression)
    {
        var tokens = new Queue<string>();
        var currentNumber = "";

        foreach (char c in expression)
        {
            if (char.IsDigit(c) || c == '.')
            {
                currentNumber += c; 
            }
            else
            {
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    tokens.Enqueue(currentNumber);
                    currentNumber = "";
                }

                if ("+-*/()".Contains(c))
                {
                    tokens.Enqueue(c.ToString());
                }
            }
        }
        if (!string.IsNullOrEmpty(currentNumber))
        {
            tokens.Enqueue(currentNumber);
        }

        return tokens;
    }

    static int GetPrecedence(string op)
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
                return 2;
            default:
                return 0;
        }
    }
    static double EvaluatePostfix(Queue<string> postfix)
    {
        var stack = new Stack<double>();

        while (postfix.Count > 0)
        {
            string token = postfix.Dequeue();
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else
            {
                double b = stack.Pop();
                double a = stack.Pop();
                double result = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    _ => throw new InvalidOperationException("Neplatný operátor")
                };
                stack.Push(result);
            }
        }

        return stack.Pop();
    }
}




