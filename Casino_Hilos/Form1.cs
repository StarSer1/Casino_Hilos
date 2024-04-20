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
        private int anchoPictureBox = 100;
        private int altoPictureBox = 100;
        private int espaciado = 20; // Espacio entre PictureBox
        private int numFilas = 3; // Número de filas
        private int numColumnas = 1; // Número de columnas
        private int numPaneles = 5; // Número de paneles
        private Thread[] hilos;
        private ManualResetEvent[] handleCreatedEvents;
        private bool detenerAnimacion = true; // Bandera para detener la animación
        private bool animacionIniciada = false; // Bandera para verificar si la animación ya ha comenzado
        private DateTime tiempoInicio; // Hora de inicio de la animación

        public Form1()
        {
            InitializeComponent();
            aleatorio = new Random(); // Inicializar la variable aleatorio
            hilos = new Thread[numPaneles]; // Inicializar la variable hilos
            InicializarPaneles();
            InicializarCuadrosImagen();
            InicializarImagenes(); // Llama al método para inicializar las imágenes en los paneles
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

        private void InicializarImagenes()
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
        }

        private void AjustarTamañoImagenes()
        {
            for (int k = 0; k < numPaneles; k++)
            {
                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j].Size = new Size(anchoPictureBox, altoPictureBox);
                    }
                }
            }
        }

        private void IniciarHilos()
        {
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
        }

        private void AnimarColumna(int panelIndex)
        {
            while (true && !detenerAnimacion) // Verifica si se debe detener la animación
            {
                if (animacionIniciada)
                {
                    TimeSpan tiempoTranscurrido = DateTime.Now - tiempoInicio;
                    if (tiempoTranscurrido.TotalMilliseconds >= 4200)
                    {
                        detenerAnimacion = true;
                        break;
                    }
                }

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

            // Al finalizar la animación, ajustar el tamaño de las imágenes
            AjustarTamañoImagenes();
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
                    imagen = Properties.Resources.Lima;
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

        private void btnPalanca_Click(object sender, EventArgs e)
        {
            if (detenerAnimacion)
            {
                detenerAnimacion = false; // Reiniciar la bandera para detener la animación
                animacionIniciada = true; // Indicar que la animación ha comenzado
                tiempoInicio = DateTime.Now; // Registrar el tiempo de inicio de la animación
                IniciarHilos(); // Iniciar la animación
            }
            else
            {
                detenerAnimacion = true; // Detener la animación
                foreach (var hilo in hilos)
                {
                    hilo.Join(); // Esperar a que todos los hilos terminen
                }
                AjustarTamañoImagenes(); // Ajustar el tamaño de las imágenes
            }
        }
    }
}
