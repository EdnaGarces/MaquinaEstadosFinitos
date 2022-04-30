using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaquinaEstadosFinitos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Paquete> paquetes;
        Robot robot;
        EstacionRecarga estacion;
        Random random;
        public MainWindow()
        {
            InitializeComponent();

            //Se incializa la variable de random
            random = new Random();

            //A x y a y se les asigna un valor aleatoria que va de 0 a 700 o 400
            int x = random.Next(0, 700);
            int y = random.Next(0, 400);

            //Se crea una nueva estacion de recarga con las coordenadas generadas
            estacion = new EstacionRecarga(x, y);

            //Se agrega la estacion al terreno
            Terreno.Children.Add(estacion);

            //A x y a y se les asigna un valor aleatoria que va de 0 a 700 o 400
            x = random.Next(0, 700);
            y = random.Next(0, 400);

            //Se crea un robot con las coordenadas generadas
            robot = new Robot(x, y);

            //Se agrega el robot al terreno
            Terreno.Children.Add(robot);

            //Se incializa la lista de paquetes
            paquetes = new List<Paquete>();

            //Genera los paquetes
            for (int i = 0; i < 10; i++)
            {
                //A x y a y se les asigna un valor aleatoria que va de 0 a 700 o 400
                x = random.Next(0, 700);
                y = random.Next(0, 400);

                //Se crea un paquete con las coordenadas generadas
                Paquete paquete = new Paquete(x, y);

                //Se agrega el paquete a la lista
                paquetes.Add(paquete);
            }

            //Indica el nivel de bateria que tiene el robot
            robot.ActualizaDatos += Robot_ActualizaDatos;
            
            //Metodo que inicia la recoleccion de los paquetes
            robot.IniciarRecoleccion(paquetes, estacion);
        }

        private void Robot_ActualizaDatos(object sender, string e)
        {
            //Muestra la informacion en la etiqueta lblEstado
            lblEstado.Content = e;
        }
    }
}
