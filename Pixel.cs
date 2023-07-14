using System;

namespace Snakie
{
    public readonly struct Pixel
    {
        // Caracter principal para dibujar
        private const char PixelChar = '█';

        // Método constructor
        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        // Métodos geters para establecer atributos X y Y
        public int X { get; }
        public int Y { get; }

        // Método geter para establecer el color a dibujar
        public ConsoleColor Color { get; }

        // Método para dibujar en pantalla
        public void Dibujar()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(left: X, top: Y);
            Console.Write(PixelChar);
        }

        // Método para limpiar pantalla
        public void Limpiar()
        {
            Console.SetCursorPosition(left: X, top: Y);
            Console.Write(' ');
        }
    }
}
