namespace Snakie
{
    class Snake
    {
        // Class attributes to receive snake colors
        private readonly ConsoleColor _headColor;
        private readonly ConsoleColor _bodyColor;

        // Constructor method to initialize the snake object with corresponding attributes
        public Snake(int initialX, int initialY, ConsoleColor headColor, ConsoleColor bodyColor, int bodyLength = 0)
        {
            _headColor = headColor;
            _bodyColor = bodyColor;
            Head = new Pixel(initialX, initialY, _headColor);

            for (int i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(x: Head.X - i - 1, initialY, _bodyColor));
            }

            Draw();
        }

        // Get/set method to create the head
        public Pixel Head { get; private set; }

        // Getter method to create the body
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        // Method to move the snake
        public void Move(Direction direction, bool eat = false)
        {
            // Clear the snake
            Clear();
            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue();

            Head = direction switch
            {
                Direction.Up => new Pixel(Head.X, y: Head.Y - 1, _headColor),
                Direction.Down => new Pixel(Head.X, y: Head.Y + 1, _headColor),
                Direction.Right => new Pixel(x: Head.X + 1, Head.Y, _headColor),
                Direction.Left => new Pixel(x: Head.X - 1, Head.Y, _headColor),
                _ => Head
            };

            Draw();
        }

        // Method to draw the snake
        public void Draw()
        {
            Head.Draw();

            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        // Method to clear the snake
        public void Clear()
        {
            Head.Clear();
            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }
}
