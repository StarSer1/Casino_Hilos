using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casino_Hilos
{
    public partial class Form1 : Form
    {
        private Panel[] paneles;
        private PictureBox[][,] cuadrosImagen;
        private Random aleatorio;
        private int duracionAnimacion = 4000; // Duración de la animación en milisegundos (10 segundos)
        private int anchoPictureBox = 100;
        private int altoPictureBox = 100;
        private int espaciado = 20; // Espacio entre PictureBox
        private int numFilas = 3; // Número de filas
        private int numColumnas = 1; // Número de columnas
        private int numPaneles = 5; // Número de paneles
        private Thread[] hilos;
        private ManualResetEvent[] handleCreatedEvents;

        public Form1()
        {
            InitializeComponent();
            InicializarPaneles();
            InicializarCuadrosImagen();
            IniciarHilos();
            this.FormClosing += Form1_FormClosing; // Suscribirse al evento FormClosing
        }

        private void InicializarPaneles()
        {
            paneles = new Panel[numPaneles];
            handleCreatedEvents = new ManualResetEvent[numPaneles];

            for (int k = 0; k < numPaneles; k++)
            {
                paneles[k] = Controls.Find($"panel{k + 1}", true).FirstOrDefault() as Panel;
                handleCreatedEvents[k] = new ManualResetEvent(false);
                paneles[k].HandleCreated += (sender, e) =>
                {
                    int index = Array.IndexOf(paneles, sender);
                    handleCreatedEvents[index].Set();
                };
            }
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

        private void IniciarHilos()
        {
            hilos = new Thread[numPaneles];
            for (int k = 0; k < numPaneles; k++)
            {
                int index = k;
                hilos[k] = new Thread(() =>
                {
                    handleCreatedEvents[index].WaitOne(); // Esperar hasta que el panel esté completamente inicializado
                    AnimarColumna(index);
                });
                hilos[k].Start();
            }
            aleatorio = new Random();
        }

        private void AnimarColumna(int panelIndex)
        {
            while (true)
            {
                for (int i = 0; i < numFilas; i++)
                {
                    if (!IsHandleCreated)
                        return; // Salir si el formulario ya no está creado

                    paneles[panelIndex].Invoke(new Action(() =>
                    {
                        if (!IsDisposed)
                        {
                            cuadrosImagen[panelIndex][0, i].Top += 20; // Mover las imágenes más rápido

                            // Si la imagen ha alcanzado la parte inferior, vuelve a la parte superior y cambia la imagen
                            if (cuadrosImagen[panelIndex][0, i].Top >= paneles[panelIndex].Height)
                            {
                                cuadrosImagen[panelIndex][0, i].Top = cuadrosImagen[panelIndex][0, i].Top - (altoPictureBox + espaciado) * numFilas;
                                cuadrosImagen[panelIndex][0, i].Image = ObtenerImagenAleatoria();
                            }
                        }
                    }));
                }
                Thread.Sleep(50); // Espera 50 milisegundos antes de la siguiente iteración
            }
        }

        private Image ObtenerImagenAleatoria()
        {
            int numeroAleatorio = aleatorio.Next(1, 7);
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
                case 4:
                    imagen = Properties.Resources.Cereza_Grande;
                    break;
                case 5:
                    imagen = Properties.Resources.Cereza;
                    break;
                case 6:
                    imagen = Properties.Resources.Lima_V2;
                    break;
                default:
                    break;
            }

            return imagen;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detener todos los hilos cuando el formulario se esté cerrando
            foreach (var hilo in hilos)
            {
                hilo.Abort();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // No es necesario iniciar las imágenes aquí, ya que serán actualizadas por los hilos
        }
    }
}
