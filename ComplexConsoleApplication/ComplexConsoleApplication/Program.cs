using System;

class Program
{
    static void Main(string[] args)
    {
        Complex A = new Complex(1, 1);
        Complex B = new Complex(1, 1);

        Console.WriteLine(A + B);
        Console.ReadLine();
    }
}

class Complex
{
    int real;

    public int Real
    {
        get { return real; }
        private set { real = value; }
    }
    int imaginary;

    public int Imaginary
    {
        get { return imaginary; }
        private set { imaginary = value; }
    }

    public Complex(int real, int imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;
    }

    public static Complex operator +(Complex operandA, Complex operandB)
    {
        return new Complex(operandA.real + operandB.real, operandA.imaginary + operandB.imaginary);
    }

    public override string ToString()
    {
        return String.Format("{0} + {1}i", real, imaginary);
    }
}
