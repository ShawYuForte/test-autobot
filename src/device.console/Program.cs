using System;
using forte.device.services;

namespace device
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var svc = new VMixService();
            var state = svc.FetchState();
            Console.WriteLine($"Fetched state for version: {state.Version}");
        }
    }
}