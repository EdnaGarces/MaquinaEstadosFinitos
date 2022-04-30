using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MaquinaEstadosFinitos
{
    /// <summary>
    /// Lógica de interacción para Robot.xaml
    /// </summary>

    //Conjunto de estados que realiza el robot
    public enum EstadoEnum
    {
        Busqueda,
        NuevaBusqueda,
        IrBateria,
        Recargar,
        Muerto,
        Aleatorio
    }

    public partial class Robot : UserControl
    {
        //Evento que actualiza los datos
        public event EventHandler<string> ActualizaDatos;

        Random random;
        int PaqueteActual = 0;
        List<Paquete> paquetes;
        EstacionRecarga estacion;

        //Controla en que momento se va a lanzar el evento que va a cambiar los estado y mover al robot
        readonly DispatcherTimer timer;


        public int Bateria { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public EstadoEnum Estado { get; set; }

        public Robot(int x, int y)
        {
            InitializeComponent();

            //Se incializa la variable random
            random = new Random();

            //Se llama al metodo recargar bateria
            RecargarBateria();

            //Indica el valor del ancho y alto del robot
            this.Height = 40;
            this.Width = 30;

            //Mueve el elemento a las coordenadas x y y
            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate;

            //Si la bateria es menor a 30, el color de esta cambiara a rojo
            indicador.Fill = new SolidColorBrush(Bateria < 350 ? Colors.Red : Colors.Green);

            //Inicia el estado de busqueda
            Estado = EstadoEnum.Busqueda;

            //Se inicializa el timer
            timer = new DispatcherTimer();

            //Cada milisegundo da un paso
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);

            //Se subscribe al elemento tick, que gestiona la perte de la maquina de estado
            timer.Tick += Timer_Tick;
        }

        //Metodo que recarga la materia
        private void RecargarBateria()
        {
            Bateria = 1000;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            switch (Estado)
            {
                //Si el estado que se esta ejecuntando es Busqueda
                case EstadoEnum.Busqueda:
                    //Se crean nuevas variables para X y Y
                    int newX = X, newY = Y;
                    //Si el valor de X es mayor que el valor de X del paquete actual
                    if (X > paquetes[PaqueteActual].X)
                        //Se le resta a X, 1
                        newX = X - 1;
                    //Si el valor de X es menor que el valor de X del paquete actual
                    if (X < paquetes[PaqueteActual].X)
                        //Se le suma a X, 1
                        newX = X + 1;
                    //Si el valor de Y es mayor que el valor de Y del paquete actual
                    if (Y > paquetes[PaqueteActual].Y)
                        //Se le resta a Y, 1
                        newY = Y - 1;
                    //Si el valor de Y es menor que el valor de Y del paquete actual
                    if (Y < paquetes[PaqueteActual].Y)
                        //Se le suma a Y, 1
                        newY = Y + 1;
                    //Se llama al metodo ActualizaPosicion enviandole los valores de X y Y
                    ActualizaPosicion(newX, newY);
                    //Si el valor de X y Y del paquete actual son iguales a X y Y
                    if (X == paquetes[PaqueteActual].X && Y == paquetes[PaqueteActual].Y)
                    {
                        //Se recolecta el paquete
                        paquetes[PaqueteActual].Recolectado();
                        //Se aumenta el valor de paquete actual
                        PaqueteActual++;
                        //Se ejecuta el estado nueva busqueda
                        Estado = EstadoEnum.NuevaBusqueda;
                    }
                    //Si la bateria es menor a 30
                    if (Bateria < 350)
                    {
                        //Se ejecuta el estado IrBateria
                        Estado = EstadoEnum.IrBateria;
                    }
                    break;

                //Si el estado que se esta ejecuntando es NuevaBusqueda
                case EstadoEnum.NuevaBusqueda:
                    //Si el total de paquetes actuales es menor al total de paquetes generados
                    if (PaqueteActual < paquetes.Count)
                    {
                        //Se ejecuta el estado de Busqueda
                        Estado = EstadoEnum.Busqueda;
                    }
                    else
                    {
                        //Se ejecuta el estado Aleatorio
                        Estado = EstadoEnum.Aleatorio;
                    }
                    break;

                //Si el estado que se esta ejecuntando es IrBateria
                case EstadoEnum.IrBateria:
                    newX = X;
                    newY = Y;
                    //Si el valor de X es menor al valor de X de la estacion
                    if (X > estacion.X)
                        //Se le resta a X, 1
                        newX = X - 1;
                    //Si el valor de X es menor que el valor de X de la estacion
                    if (X < estacion.X)
                        //Se le suma a X, 1
                        newX = X + 1;
                    //Si el valor de Y es mayor que el valor de Y de la estacion
                    if (Y > estacion.Y)
                        //Se le resta a Y, 1
                        newY = Y - 1;
                    //Si el valor de Y es menor que el valor de Y de la estacion
                    if (Y < estacion.Y)
                        //Se le suma a Y, 1
                        newY = Y + 1;
                    //Se llama al metodo ActualizaPosicion enviandole los valores de X y Y
                    ActualizaPosicion(newX, newY);
                    // Si el valor de X y Y de la estacion son iguales a X y Y
                    if (X == estacion.X && Y == estacion.Y)
                    {
                        //Se ejecuta el estado Recargar
                        Estado = EstadoEnum.Recargar;
                    }
                    //Si la bateria es igual a 0
                    if (Bateria == 0)
                    {
                        //Se ejecuta el estado Muerto
                        Estado = EstadoEnum.Muerto;
                    }
                    break;

                //Si el estado que se esta ejecuntando es Recargar
                case EstadoEnum.Recargar:
                    //Se ejecuta el metodo RecargarBateria
                    RecargarBateria();
                    //Realiza una espera medio segundo
                    Thread.Sleep(500);
                    //Se ejecuta el metodo de Busqueda
                    Estado = EstadoEnum.Busqueda;
                    break;

                //Si el estado que se esta ejecuntando es Muerto
                case EstadoEnum.Muerto:
                    //Se detiene el timer para que no genere mas iteraciones
                    timer.Stop();
                    //Manda un mensaje al usuario de que ha muerto el robot
                    MessageBox.Show("Ha muerto el robot");
                    break;

                //Si el estado que se esta ejecuntando es Aleatorio
                case EstadoEnum.Aleatorio:
                    //Genera numeros aleatorios de X y Y
                    ActualizaPosicion(random.NextDouble() > 0.5 ? X - 1 : X + 1, random.NextDouble() > 0.5 ? Y - 1 : Y + 1);
                    //Si la bateria es igual a 0
                    if (Bateria == 0)
                    {
                        //Se ejecuta el estado Muerto
                        Estado = EstadoEnum.Muerto;
                    }
                    break;

                    //Caso por defecto
                    default:
                    break;
            }
            //Actualiza el estado en el que se encuentra el robot y la bateria que tiene
            ActualizaDatos(null, "Estado: " + Estado.ToString() + " Bateria: " + Bateria);
        }

        //Metodo que actualiza la posicion dependiendo del valor de las coordenadas obtenidas
        private void ActualizaPosicion(int x, int y)
        {
            Bateria--;
            X = x;
            Y = y;

            //Mueve el elemento a las coordenadas x y y
            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate;

            //Si la bateria es menor a 30, el color de esta cambiara a rojo
            indicador.Fill = new SolidColorBrush(Bateria < 350 ? Colors.Red : Colors.Green);
        }

        //Metodo para i
        public void IniciarRecoleccion(List<Paquete> paquetes, EstacionRecarga estacionRecarga)
        {
            this.paquetes = paquetes;
            this.estacion = estacionRecarga;
            timer.Start();
        }
    }
}

