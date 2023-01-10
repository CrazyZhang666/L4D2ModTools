using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace TestDemo;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==========  程序开始!  ==========");
        Console.WriteLine();

        var filePath = "F:\\My Downloads\\2915322890\\models\\survivors\\survivor_producer.mdl";

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var reader = new BinaryReader(stream, Encoding.ASCII);

        Console.WriteLine(reader.ReadChars(4));
        Console.WriteLine(reader.ReadInt32());

        Console.WriteLine(reader.ReadInt32());
        Console.WriteLine(reader.ReadChars(64));
        Console.WriteLine(reader.ReadInt32());

        reader.Close();
        stream.Close();

        Console.WriteLine();
        Console.WriteLine("==========  程序结束!  ==========");
        Console.ReadLine();
    }
}