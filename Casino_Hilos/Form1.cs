using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casino_Hilos
{
    public partial class Form1 : Form
    {
        private Panel[] paneles;
        private PictureBox[][,] cuadrosImagen;
        private Random aleatorio;
        private int duracionAnimacion = 10000; // Duración de la animación en milisegundos (10 segundos)
        private int tiempoTranscurrido = 0; // Tiempo transcurrido desde el inicio de la animación
        private int anchoPictureBox = 100;
        private int altoPictureBox = 100;
        private int espaciado = 20; // Espacio entre PictureBox
        private int numFilas = 3; // Número de filas
        private int numColumnas = 1; // Número de columnas
        private int numPaneles = 5; // Número de paneles

        public Form1()
        {
            InitializeComponent();
            InicializarPaneles();
            InicializarCuadrosImagen();
            InicializarTemporizador();
        }

        private void InicializarPaneles()
        {
            paneles = new Panel[numPaneles];
            paneles[0] = panel1;
            paneles[1] = panel2;
            paneles[2] = panel3;
            paneles[3] = panel4;
            paneles[4] = panel5;
        }

        private void InicializarCuadrosImagen()
        {
            cuadrosImagen = new PictureBox[numPaneles][,];

            for (int k = 0; k < numPaneles; k++)
            {
                cuadrosImagen[k] = new PictureBox[numColumnas, numFilas]; // 1 columna y 3 filas

                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j] = new PictureBox();
                        cuadrosImagen[k][i, j].Size = new Size(anchoPictureBox, altoPictureBox);
                        cuadrosImagen[k][i, j].Location = new Point(0, 0 + j * (altoPictureBox + espaciado));
                        cuadrosImagen[k][i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                        paneles[k].Controls.Add(cuadrosImagen[k][i, j]);
                    }
                }
            }
        }

        private void InicializarTemporizador()
        {
            timer1 = new Timer();
            timer1.Interval = 50; // Animación más rápida (50 milisegundos)
            timer1.Tick += Timer1_Tick;
            aleatorio = new Random();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            tiempoTranscurrido += timer1.Interval; // Actualizar el tiempo transcurrido

            if (tiempoTranscurrido >= duracionAnimacion)
            {
                timer1.Stop(); // Detener la animación después de 10 segundos
                return;
            }

            // Desplaza las imágenes hacia abajo en cada panel
            for (int k = 0; k < numPaneles; k++)
            {
                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j].Top += 20; // Mover las imágenes más rápido

                        // Si la imagen ha alcanzado la parte inferior, vuelve a la parte superior y cambia la imagen
                        if (cuadrosImagen[k][i, j].Top >= paneles[k].Height)
                        {
                            cuadrosImagen[k][i, j].Top = cuadrosImagen[k][i, j].Top - (altoPictureBox + espaciado) * numFilas;
                            cuadrosImagen[k][i, j].Image = ObtenerImagenAleatoria();
                        }
                    }
                }
            }
        }

        private Image ObtenerImagenAleatoria()
        {
            int numeroAleatorio = aleatorio.Next(1, 4);
            Image imagen = null;

            switch (numeroAleatorio)
            {
                case 1:
                    imagen = Properties.Resources.Limones;
                    break;
                case 2:
                    imagen = Properties.Resources.Naranja;
                    break;
                case 3:
                    imagen = Properties.Resources.Sandia;
                    break;
                default:
                    break;
            }

            return imagen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializa las imágenes con una imagen aleatoria en cada panel
            for (int k = 0; k < numPaneles; k++)
            {
                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j].Image = ObtenerImagenAleatoria();
                    }
                }
            }

            timer1.Start();
        }
    }

}
