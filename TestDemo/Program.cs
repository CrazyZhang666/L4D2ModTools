using System;

namespace TestDemo;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==========  程序开始!  ==========");
        Console.WriteLine();

        Console.WriteLine("hello");

        Student student1 = new Student();
        student1.Name = "Tom";
        student1.Age = 1;

        Student student2 = new Student()
        {
            Name = "Tom",
            Age = 1
        };

        Console.WriteLine(student1.Age);

        ChangeValue(student1);

        Console.WriteLine(student1.Age);

        Console.WriteLine();
        Console.WriteLine("==========  程序结束!  ==========");
        Console.ReadLine();
    }

    static void ChangeValue(Student student)
    {
        student.Age = 100;
    }
}

internal class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
}