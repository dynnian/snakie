namespace Snakie
{
    public readonly struct Pixel
    {
        // Main character for drawing
        private const char PixelChar = '█';

        // Constructor method
        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        // Getter methods to set X and Y attributes
        public int X { get; }
        public int Y { get; }

        // Getter method to set the color to draw
        public ConsoleColor Color { get; }

        // Method to draw on the screen
        public void Draw()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(left: X, top: Y);
            Console.Write(PixelChar);
        }

        // Method to clear the screen
        public void Clear()
        {
            Console.SetCursorPosition(left: X, top: Y);
            Console.Write(' ');
        }
    }
}
