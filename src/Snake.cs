using System;
using System.Collections.Generic;

namespace Snakie
{
    class Snake
    {
        // Atributos de la clase para recibir los colores de la serpiente
        private readonly ConsoleColor _colorCabeza;
        private readonly ConsoleColor _colorCuerpo;

        // Método constructor para inicializar el objeto serpiente con los correspondientes atributos
        public Snake(int Xinicial, int Yinicial, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, int longitudCuerpo = 0)
        {
            _colorCabeza = colorCabeza;
            _colorCuerpo = colorCuerpo;
            Cabeza = new Pixel(Xinicial, Yinicial, _colorCabeza);

            for(int i = longitudCuerpo; i >= 0; i--)
            {
                Cuerpo.Enqueue(item: new Pixel(x: Cabeza.X - i - 1, Yinicial, _colorCuerpo));
            }

            Dibujar();
        }

        // Método get/set para crear la cabeza
        public Pixel Cabeza { get; private set; }

        // Método geter para crear el cuerpo
        public Queue<Pixel> Cuerpo { get; } = new Queue<Pixel>();

        // Método para mover la serpiente
        public void Mover(Direccion direccion, bool eat = false)
        {
            // Limpia la serpiente
            Limpiar();
            Cuerpo.Enqueue(item: new Pixel(Cabeza.X, Cabeza.Y, _colorCuerpo));
            if (!eat)
               Cuerpo.Dequeue();

            Cabeza = direccion switch
            {
                Direccion.Arriba => new Pixel(Cabeza.X, y: Cabeza.Y - 1, _colorCabeza),
                Direccion.Abajo => new Pixel(Cabeza.X, y: Cabeza.Y + 1, _colorCabeza),
                Direccion.Derecha => new Pixel(x: Cabeza.X + 1, Cabeza.Y, _colorCabeza),
                Direccion.Izquierda => new Pixel(x: Cabeza.X - 1, Cabeza.Y, _colorCabeza),
                _=> Cabeza
            };

            Dibujar();
        }
        
        // Método para dibujar la serpiente
        public void Dibujar()
        {
            Cabeza.Dibujar();

            foreach(Pixel pixel in Cuerpo)
            {
                pixel.Dibujar();
            }
        }

        // Método para limpiar la serpiente
        public void Limpiar()
        {
            Cabeza.Limpiar();
            foreach (Pixel pixel in Cuerpo)
            {
                pixel.Limpiar();
            }
        }
    }
}
