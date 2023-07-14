using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Snakie
{
    class Juego
    {
        // Define el tamaño del tablero
        static readonly int anchoTablero = 40;
        static readonly int alturaTablero = 20;

        // Define los colores a utilizar en el juego
        private const ConsoleColor ColorBorde = ConsoleColor.Magenta;
        private const ConsoleColor ColorCabeza = ConsoleColor.Cyan;
        private const ConsoleColor ColorCuerpo = ConsoleColor.Red;
        private const ConsoleColor ColorComida = ConsoleColor.Green;

        // Define los frames de refrescado
        private const int FramesMinimos = 50;
        private const int FramesIniciales = 150;
        static int frames = 150;

        // Método main
        static void Main()
        {
            // No hacer visible el cursor en la consola
            Console.CursorVisible = false;
            
            // Bucle del juego
            while (true)
            {
                MenuPrincipal();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char eleccion = keyInfo.KeyChar;
                if (eleccion == '1')
                {
                    IniciarJuego();
                    Console.ReadKey();
                }
                else if (eleccion == '2')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.CursorVisible = true;
                    System.Environment.Exit(1); 
                }
            }
        }

        // Método de inicialización del juego
        static void IniciarJuego()
        {
            // Se limpia la pantalla
            Console.Clear();

            // Se dibuja el tablero
            DibujarTablero();

            // La dirección por defecto es hacia arriba
            Direccion movimiento = Direccion.Arriba;

            // Crea una serpiente con los parametros por defecto
            var snake = new Snake(Xinicial: anchoTablero / 2, Yinicial: alturaTablero / 2, ColorCabeza, ColorCuerpo);
            Stopwatch stw = new();

            // Inicia la puntuación en 0
            int puntaje = 0;

            // Dibuja la comida en la pantalla
            Pixel food = NuevaComida(snake);
            food.Dibujar();

            // Bucle de movimiento de la serpiente
            while (true)
            {
                stw.Restart();

                // Se mueve hacia arriba por defecto
                Direccion movimientoAnterior = movimiento;
                
                // Sigue moviendo la serpiente a la velocidad de los frames
                while (stw.ElapsedMilliseconds <= frames)
                {
                    if (movimiento == movimientoAnterior)
                    {
                        movimiento = Movimiento(movimiento);
                    }

                }
                // Si la serpiente come una comida, aumenta el tamaño de la serpiente y aumenta el puntaje
                if(snake.Cabeza.X == food.X && snake.Cabeza.Y == food.Y)
                {
                    Console.Beep();
                    snake.Mover(movimiento, eat: true);
                    food = NuevaComida(snake);
                    food.Dibujar();
                    puntaje++;
                    frames -= 5;
                    if (frames < FramesMinimos)
                    {
                        frames = FramesMinimos;
                    }
                }
                // De lo contrario, sigue moviendose
                else
                {
                    snake.Mover(movimiento);
                }

                // Si la serpiente colisiona con sigo misma o el tablero, termina el bucle 
                if (snake.Cabeza.X == anchoTablero - 1
                    || snake.Cabeza.X == 0
                    || snake.Cabeza.Y == alturaTablero - 1
                    || snake.Cabeza.Y == 0
                    || snake.Cuerpo.Any(i => i.X == snake.Cabeza.X && i.Y == snake.Cabeza.Y))
                    {
                        Console.Beep();
                        break;
                    }
            }

            // Limpia la serpiente
            snake.Limpiar();
            // Setea la posicion del cursor en el centro del tablero para mostrar el mensaje de GAME OVER
            Console.SetCursorPosition(left: 9, top: 10);
            // Setea el color del texto en verde
            Console.ForegroundColor = ConsoleColor.Green;
            // Muestra el mensaje de GAME OVER
            Console.WriteLine("GAME OVER");
            // Setea la posición del cursor debajo del mensaje de GAME OVER para mostrar el puntaje
            Console.SetCursorPosition(left: 7, top: 13);
            // Muestra el puntaje obtenido
            Console.WriteLine($"Tu puntuación ha sido: {puntaje}");
            // Muestra el mensaje de volver al menu
            Console.SetCursorPosition(left: 7, top: 14);
            Console.WriteLine("Presiona cualquier tecla para volver");
            Console.SetCursorPosition(left: 7, top: 15);
            Console.WriteLine("al menu principal");
            frames = FramesIniciales;
        }

        // Método para obtener la nueva posición de la comida en pantalla, dependiendo de la posición de la serpiente
        static Pixel NuevaComida(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(new Random().Next(1, anchoTablero - 2), new Random().Next(1, alturaTablero - 2), ColorComida);
            }
            while (snake.Cabeza.X == food.X && snake.Cabeza.Y == food.Y || snake.Cuerpo.Any(i => i.X == snake.Cabeza.X && i.Y == snake.Cabeza.Y));

            return food;
        }

        // Método para el movimiento de la serpiente con las teclas WASD
        static Direccion Movimiento(Direccion direccionActual)
        {
            if (!Console.KeyAvailable) return direccionActual;

            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            direccionActual = key switch
            {
                ConsoleKey.W when direccionActual != Direccion.Abajo => Direccion.Arriba,
                ConsoleKey.S when direccionActual != Direccion.Arriba => Direccion.Abajo,
                ConsoleKey.D when direccionActual != Direccion.Izquierda => Direccion.Derecha,
                ConsoleKey.A when direccionActual != Direccion.Derecha => Direccion.Izquierda,
                _ => direccionActual
            };
            return direccionActual;
        }

        // Método para dibujar el tablero
        static void DibujarTablero()
        {
            for (int i = 0; i < anchoTablero; i++)
            {
                new Pixel(x: i, y: 0, ColorBorde).Dibujar();
                new Pixel(x: i, y: alturaTablero -1, ColorBorde).Dibujar();
            }
            for (int i = 0; i < alturaTablero; i++)
            {
                new Pixel(x: 0, y: i, ColorBorde).Dibujar();
                new Pixel(x: anchoTablero - 1, y: i, ColorBorde).Dibujar();
            }
        }

        static void MenuPrincipal()
        {
            // Dibuja el menu principal
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
            Console.WriteLine("Presiona 1 para iniciar el juego, 2 para salir");
            Console.WriteLine("Teclas de movimiento: W para arriba, S para abajo, A para izquierda, D para derecha");
        }
    }
}