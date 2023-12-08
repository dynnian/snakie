using System.Diagnostics;

namespace Snakie
{
    class Game
    {
        // Define the size of the board
        static readonly int boardWidth = 40;
        static readonly int boardHeight = 20;

        // Define colors to be used in the game
        private const ConsoleColor BorderColor = ConsoleColor.Magenta;
        private const ConsoleColor HeadColor = ConsoleColor.Cyan;
        private const ConsoleColor BodyColor = ConsoleColor.Red;
        private const ConsoleColor FoodColor = ConsoleColor.Green;

        // Define refresh frames
        private const int MinimumFrames = 50;
        private const int InitialFrames = 150;
        static int frames = 150;

        // Game initialization method
        static void StartGame()
        {
            // Clear the screen
            Console.Clear();

            // Draw the board
            DrawBoard();

            // Default direction is upwards
            Direction movement = Direction.Up;

            // Create a snake with default parameters
            var snake = new Snake(initialX: boardWidth / 2, initialY: boardHeight / 2, HeadColor, BodyColor);
            Stopwatch stopwatch = new();

            // Initialize the score to 0
            int score = 0;

            // Draw food on the screen
            Pixel food = NewFood(snake);
            food.Draw();

            // Snake movement loop
            while (true)
            {
                stopwatch.Restart();

                // Move upwards by default
                Direction previousMovement = movement;

                // Continue moving the snake at the frame speed
                while (stopwatch.ElapsedMilliseconds <= frames)
                {
                    if (movement == previousMovement)
                    {
                        movement = Move(movement);
                    }
                }

                // If the snake eats food, increase the snake size and score
                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    Console.Beep();
                    snake.Move(movement, eat: true);
                    food = NewFood(snake);
                    food.Draw();
                    score++;
                    frames -= 5;
                    if (frames < MinimumFrames)
                    {
                        frames = MinimumFrames;
                    }
                }
                // Otherwise, keep moving
                else
                {
                    snake.Move(movement);
                }

                // If the snake collides with itself or the board, end the loop
                if (snake.Head.X == boardWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == boardHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(i => i.X == snake.Head.X && i.Y == snake.Head.Y))
                {
                    Console.Beep();
                    break;
                }
            }

            // Clear the snake
            snake.Clear();
            // Set the cursor position to the center of the board to display the GAME OVER message
            Console.SetCursorPosition(left: 9, top: 10);
            // Set the text color to green
            Console.ForegroundColor = ConsoleColor.Green;
            // Display the GAME OVER message
            Console.WriteLine("GAME OVER");
            // Set the cursor position below the GAME OVER message to display the score
            Console.SetCursorPosition(left: 7, top: 13);
            // Display the obtained score
            Console.WriteLine($"Your score is: {score}");
            // Display the message to return to the main menu
            Console.SetCursorPosition(left: 7, top: 14);
            Console.WriteLine("Press any key to return");
            Console.SetCursorPosition(left: 7, top: 15);
            Console.WriteLine("to the main menu");
            frames = InitialFrames;
        }

        // Method to get the new position of the food on the screen, depending on the snake's position
        static Pixel NewFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(new Random().Next(1, boardWidth - 2), new Random().Next(1, boardHeight - 2), FoodColor);
            }
            while (snake.Head.X == food.X && snake.Head.Y == food.Y || snake.Body.Any(i => i.X == snake.Head.X && i.Y == snake.Head.Y));

            return food;
        }

        // Method for snake movement with WASD keys
        static Direction Move(Direction currentDirection)
        {
            if (!Console.KeyAvailable) return currentDirection;

            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            currentDirection = key switch
            {
                ConsoleKey.W when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.S when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.D when currentDirection != Direction.Left => Direction.Right,
                ConsoleKey.A when currentDirection != Direction.Right => Direction.Left,
                _ => currentDirection
            };
            return currentDirection;
        }

        // Method to draw the board
        static void DrawBoard()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                new Pixel(x: i, y: 0, BorderColor).Draw();
                new Pixel(x: i, y: boardHeight - 1, BorderColor).Draw();
            }
            for (int i = 0; i < boardHeight; i++)
            {
                new Pixel(x: 0, y: i, BorderColor).Draw();
                new Pixel(x: boardWidth - 1, y: i, BorderColor).Draw();
            }
        }

        static void MainMenu()
        {
            // Draw the main menu
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ");
            Console.WriteLine(@"MP'''''''MM                   dP       oo          dP");
            Console.WriteLine(@"M  mmmmm..M                   88                   88");
            Console.WriteLine(@"M.      `YM 88d888b. .d8888b. 88  .dP  dP .d8888b. 88");
            Console.WriteLine(@"MMMMMMM.  M 88'  `88 88'  `88 888888   88 88ooood8 dP");
            Console.WriteLine(@"M. .MMM'  M 88    88 88.  .88 88  `8b. 88 88.  ...   ");
            Console.WriteLine(@"Mb.     .dM dP    dP `88888P8 dP   `YP dP `88888P' oo");
            Console.WriteLine(@"MMMMMMMMMMM                                          ");
            Console.WriteLine(" ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press 1 to start the game, 2 to exit");
            Console.WriteLine("Movement keys: W for up, S for down, A for left, D for right");
        }

        static void Main()
        {
            // Make the cursor not visible in the console
            Console.CursorVisible = false;

            // Game loop
            while (true)
            {
                MainMenu();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char choice = keyInfo.KeyChar;
                if (choice == '1')
                {
                    StartGame();
                    Console.ReadKey();
                }
                else if (choice == '2')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.CursorVisible = true;
                    Environment.Exit(1);
                }
            }
        }
    }
}
