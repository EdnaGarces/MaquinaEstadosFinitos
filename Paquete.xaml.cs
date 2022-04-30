using System;
using System.Collections.Generic;
using System.Text;
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
    /// Lógica de interacción para Paquete.xaml
    /// </summary>
    public partial class Paquete : UserControl
    {
        //Propiedades las cuales pueden obtenerse pero no escribirse desde otra clase
        public int X { get; private set; }
        public int Y { get; private set; }

        //Metodo que obtiene el valor de x y y
        public Paquete(int x, int y)
        {
            InitializeComponent();
            //Se asigna dichos valores a sus respectivas variables
            X = x;
            Y = y;

            //Tranlacion en x y y
            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate;

            //Se asigna el valor de alto y ancho de este elemento es de 20
            this.Height = 20;
            this.Width = 20;
        }

        //Metodo que realiza acciones cuando un objeto fue recollectado
        public void Recolectado()
        {
            //Cuando el elemento fue recolectado, este se oculta
            this.Visibility = Visibility.Hidden;
        }
    }
}
