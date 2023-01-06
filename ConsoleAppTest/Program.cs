using System;
using System.Net;

namespace ConsoleAppTest;

internal class Program
{
    static void Main(string[] args)
    {
        var iPAddress = IPAddress.Parse("127.0.0.1");

        //Console.WriteLine(iPAddress.Address);
        Console.WriteLine(iPAddress.ToString());

        Console.WriteLine(iPAddress.GetHashCode());
        var addressBytes = iPAddress.GetAddressBytes();
        Console.WriteLine(BitConverter.ToUInt32(addressBytes));

        var iPAddress2 = new IPAddress(16777343);
        Console.WriteLine(iPAddress2);

        Console.WriteLine("程序结束!");
        Console.ReadLine();
    }
}