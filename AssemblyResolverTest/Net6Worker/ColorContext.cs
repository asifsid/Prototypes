namespace Net6Worker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ColorContext : IDisposable
    {
        public static ColorContext Forground(ConsoleColor color)
        {
            return new ColorContext(color);
        }

        private ConsoleColor _oldColor;

        private ColorContext(ConsoleColor newColor)
        {
            _oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
        }

        void IDisposable.Dispose()
        {
            Console.ForegroundColor = _oldColor;
        }
    }
}
