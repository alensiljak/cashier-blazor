namespace Cashier.Lib
{
    public static class Log
    {
        public static void log(string message, params object?[] args)
        {
            Console.WriteLine(message, args);
        }
        
        public static void log(int value)
        {
            Console.WriteLine(value);
        }

        public static void debug(object? obj)
        {
            DebugPrinter.PrintJson(obj);
        }
    }
}
