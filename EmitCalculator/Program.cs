using System;
using System.Reflection.Emit;
    
namespace EmitCalculator
{
    class Calculator
    {
        private string s;
        private int position;
        private ILGenerator gen;
        private DynamicMethod method;
        private char[] Sign = new char[] { '*', '+' };
        private OpCode[] Code = new OpCode[] { OpCodes.Mul, OpCodes.Add };
    
        public Calculator(string expression)
        {
            s = expression.Replace(" ", "");
        }
    
        public Func<T, T, T, T, T> FuncGenerate<T>()
        {
            GenerateMethod(typeof(T));
            Delegate result = method.CreateDelegate(typeof(Func<T, T, T, T, T>));
            return (Func<T, T, T, T, T>)result;
        }
    
        private void GenerateMethod(Type t)
        {
            method = new DynamicMethod("Caclulate", 
                                       t,
                                       new Type[] {t, t, t, t},
                                       typeof(Calculator));
            gen = method.GetILGenerator();
            position = 0;
            GenerateCode();
            gen.Emit(OpCodes.Ret);
        }
        
        private void GenerateCode(int v = 1)
        {
            if (v < 0)
            {
                gen.Emit(OpCodes.Ldarg, s[position] - 'a');
                position++;
                return;
            }
            GenerateCode(v - 1);
            while (position < s.Length && s[position] == Sign[v])
            {
                position++;
                GenerateCode(v - 1);
                gen.Emit(Code[v]);
            }
        }
    }
        
    class Program
    {
        static void Main(string[] args)
        {
            string expression = Console.ReadLine();
            Calculator expr = new Calculator(expression);
                
            Func<int, int, int, int, int> 
                x = expr.FuncGenerate<int>();
            Func<double, double, double, double, double> 
                y = expr.FuncGenerate<double>();
    
                
            Console.WriteLine(x(1, 3, 3, 7));
            Console.WriteLine(x(2, 4, 6, 8));
            Console.WriteLine(y(1.2, 3.11, 5.8, 5.33));
            Console.WriteLine(y(2.1, 1.2, 3.5, 4.1));
            Console.ReadKey();
        }
    }
}