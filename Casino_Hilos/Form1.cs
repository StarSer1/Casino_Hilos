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
using System.Media;


namespace Casino_Hilos
{
    public partial class Form1 : Form
    {
        private Panel[] paneles;
        private PictureBox[][,] cuadrosImagen;
        private Thread[] hilos;
        private ManualResetEvent[] handleCreatedEvents;
        private Random aleatorio;
        private int anchoPictureBox = 100;
        private int altoPictureBox = 100;
        private int espaciado = 20; // Espacio entre PictureBox
        private int numFilas = 3; // Número de filas
        private int numColumnas = 1; // Número de columnas
        private int numPaneles = 5; // Número de paneles
        private int tiempoInactivo = 5000;
        private int Apuesta = 1;
        private int SaldoDeCuenta = 10000;
        private bool detenerAnimacion = true; // Bandera para detener la animación
        private bool animacionIniciada = false; // Bandera para verificar si la animación ya ha comenzado
        private bool botonDisponible = true;
        private bool ApuestaValida = false;
        private DateTime tiempoInicio; // Hora de inicio de la animación
        private DateTime ultimoClick = DateTime.MinValue;

        private int[] data = new int[5]; //LLENAR ESTE ARREGLO CON LOS VALORES DE LAS IMAGENES (DEL 1 AL 6)
        private int[][] posicionesIniciales;
        private int contador;
        private int valorS;
        private int valorA;
        private int Dinero;
        private bool primeravuelta = true;
        private SoundPlayer playerButtonSound;

        public Form1()
        {
            InitializeComponent();
            aleatorio = new Random(); // Inicializar la variable aleatorio
            hilos = new Thread[numPaneles]; // Inicializar la variable hilos
            InicializarPaneles();
            InicializarCuadrosImagen();
            InicializarImagenes(); // Llama al método para inicializar las imágenes en los paneles
            this.FormClosing += Form1_FormClosing; // Suscribirse al evento FormClosing
            this.Text = String.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            playerButtonSound = new SoundPlayer();
            playerButtonSound.SoundLocation = "C:\\Users\\user\\Desktop\\Programas 2\\Casino_Hilos\\Casino_Hilos\\Resources\\Efecto de Dinero.wav"; // Ruta del archivo de sonido del botón
        }
        private void AlinearImagenes(int panelIndex)
        {
            // Ajustar el tamaño de las imágenes
            AjustarTamañoImagenes();

            // Alinear las imágenes en el panel dado a sus posiciones iniciales
            for (int i = 0; i < numFilas; i++)
            {
                cuadrosImagen[panelIndex][0, i].Top = posicionesIniciales[panelIndex][i];
            }
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
            posicionesIniciales = new int[numPaneles][];

            for (int k = 0; k < numPaneles; k++)
            {
                cuadrosImagen[k] = new PictureBox[numColumnas, numFilas]; // 1 columna y 3 filas
                posicionesIniciales[k] = new int[numFilas]; // Inicializar las posiciones iniciales

                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j] = new PictureBox();
                        cuadrosImagen[k][i, j].Size = new Size(anchoPictureBox, altoPictureBox);
                        posicionesIniciales[k][j] = 0 + j * (altoPictureBox + espaciado); // Asignar las posiciones iniciales
                        cuadrosImagen[k][i, j].Location = new Point(0, posicionesIniciales[k][j]);
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
            // Guardar la posición inicial de cada imagen
            int[] posicionesIniciales = new int[numFilas];
            for (int i = 0; i < numFilas; i++)
            {
                posicionesIniciales[i] = cuadrosImagen[panelIndex][0, i].Top;
            }

            bool animacionCompleta = false;

            while (!animacionCompleta) // Se ejecuta hasta que la animación esté completa
            {
                if (animacionIniciada)
                {
                    TimeSpan tiempoTranscurrido = DateTime.Now - tiempoInicio;
                    if (tiempoTranscurrido.TotalMilliseconds >= 4200)
                    {
                        detenerAnimacion = true;
                    }
                }

                bool allAligned = true; // Variable para controlar si todas las imágenes están alineadas

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

                            // Verificar si la imagen ha vuelto a su posición inicial
                            if (cuadrosImagen[panelIndex][0, i].Top != posicionesIniciales[i])
                            {
                                allAligned = false;
                            }
                        }
                    }));
                }

                if (detenerAnimacion && allAligned)
                {
                    animacionCompleta = true; // La animación está completa
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
            contador++;
            if(primeravuelta == true)
            {
                switch (numeroAleatorio)
                {
                    case 1:
                        imagen = Properties.Resources.Naranja;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 1;
                            data[valorA] = valorS;
                            valorA++;

                        }
                        break;
                    case 2:
                        imagen = Properties.Resources.Limones;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 2;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 3:
                        imagen = Properties.Resources.Lima;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 3;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 4:
                        imagen = Properties.Resources.Sandia;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 4;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 5:
                        imagen = Properties.Resources.Cereza_Grande;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 5;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 6:
                        imagen = Properties.Resources.Cereza;
                        if (contador == 66 || contador == 67 || contador == 68 || contador == 69 || contador == 70)
                        {
                            valorS = 6;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    default:
                        break;
                }
                if ( contador == 75)
                {
                    Dinero = Calcular_Premio(data, Apuesta);
                    SaldoDeCuenta = SaldoDeCuenta + Dinero;
                    label2.Text = SaldoDeCuenta.ToString();
                    label3.Text = Dinero.ToString();
                    ApuestaValida = false;
                    contador = 0;
                    primeravuelta = false;
                    valorA = 0;
                }
                

            }
            else if (primeravuelta == false)
            {
                switch (numeroAleatorio)
                {
                    case 1:
                        imagen = Properties.Resources.Naranja;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 1;
                            data[valorA] = valorS;
                            valorA++;

                        }
                        break;
                    case 2:
                        imagen = Properties.Resources.Limones;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 2;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 3:
                        imagen = Properties.Resources.Lima;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 3;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 4:
                        imagen = Properties.Resources.Sandia;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 4;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 5:
                        imagen = Properties.Resources.Cereza_Grande;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 5;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    case 6:
                        imagen = Properties.Resources.Cereza;
                        if (contador == 51 || contador == 52 || contador == 53 || contador == 54 || contador == 55)
                        {
                            valorS = 6;
                            data[valorA] = valorS;
                            valorA++;
                        }
                        break;
                    default:
                        break;
                }

                if (contador == 60)
                {
                    Dinero = Calcular_Premio(data, Apuesta);
                    SaldoDeCuenta = SaldoDeCuenta + Dinero;
                    label2.Text = SaldoDeCuenta.ToString();
                    label3.Text = Dinero.ToString();
                    ApuestaValida = false;
                    contador = 0;
                    primeravuelta = false;
                    valorA = 0;
                }
            }
            
            
            return imagen;
        }
        private int Calcular_Premio(int[] valores, int Apuesta)
        {
            int Total = 0;
            int Opcion = 1;
            bool Aprovado = false;
            for (int h = 0; h < 6; h++)
            {
                for (int i = 0; i < valores.Length; i++)
                {
                    if (valores[i] != Opcion)
                    {
                        Aprovado = false;
                        break;
                    }
                    Aprovado = true;
                }

                if (Aprovado)
                {
                    Total = (Opcion * 2000) * Apuesta;
                    break;
                }
                Opcion++;
            }
            if (Aprovado == false)
            {
                Array.Sort(valores);

                // Recorremos el arreglo para encontrar números repetidos
                for (int i = 0; i < valores.Length - 2; i++)
                {
                    // Si encontramos un número que se repite exactamente 3 veces
                    if (valores[i] == valores[i + 1] && valores[i] == valores[i + 2])
                    {
                        Total = (valores[i] * 1000) * Apuesta;
                    }
                }
            }

            return Total;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detener todos los hilos cuando el formulario se esté cerrando
            foreach (var hilo in hilos)
            {
                if (hilo != null)
                {
                    hilo.Abort();
                }
            }
        }
        #region Botones
        private void btnPalanca_Click(object sender, EventArgs e)
        {
            // Deshabilitar el botón mientras se ejecuta la animación
            btnPalanca.Enabled = false;

            int Apostado = int.Parse(label1.Text);
            int Saldo = int.Parse(label2.Text);
            int DineroInicial = SaldoDeCuenta;
            SaldoDeCuenta = (Saldo - Apostado);
            if (SaldoDeCuenta < 0)
            {
                ApuestaValida = false;
            }
            else
            {
                ApuestaValida = true;
            }
            if (botonDisponible)
            {
                if (ApuestaValida)
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
                    botonDisponible = false;

                    // Obtener la hora actual
                    ultimoClick = DateTime.Now;

                    // Reactivar el botón después de cierto tiempo
                    Task.Delay(tiempoInactivo).ContinueWith(_ =>
                    {
                        // Verificar si ha pasado suficiente tiempo desde el último clic
                        if ((DateTime.Now - ultimoClick).TotalMilliseconds >= tiempoInactivo)
                        {
                            // Reactivar el botón en el hilo de la interfaz de usuario
                            Invoke((Action)(() =>
                            {
                                botonDisponible = true;
                                // Habilitar el botón una vez que la animación haya terminado
                                btnPalanca.Enabled = true;
                            }));
                        }
                    });

                }
                else
                {
                    DialogResult resultado = MessageBox.Show("Su cuenta no tiene el saldo suficiente, ¿Desea volver al menu principal?", "Confirmación", MessageBoxButtons.YesNo);

                    // Verificar qué botón fue presionado
                    if (resultado == DialogResult.Yes)
                    {
                        // Si se presionó "Sí", crear una instancia del otro formulario y mostrarlo
                        Menu formularioOtro = new Menu();
                        formularioOtro.Show();
                    }
                    SaldoDeCuenta = DineroInicial;
                    botonDisponible = true;
                    btnPalanca.Enabled = true;
                }

            }
        }

        private void DetenerHilo(int N)
        {
            if (hilos.Length >= N && hilos[N - 1] != null && hilos[N - 1].IsAlive)
            {
                hilos[N - 1].Abort();

                // Esperar a que el hilo se detenga completamente
                while (hilos[N - 1].IsAlive)
                {
                    Thread.Sleep(100);
                }

                // Alinear las imágenes en el panel correspondiente
                AlinearImagenes(panelIndex: N - 1);
            }
        }
        private void BtnPararHilo1_Click(object sender, EventArgs e)
        {
            DetenerHilo(1);
        }

        private void BtnPararHilo2_Click(object sender, EventArgs e)
        {
            DetenerHilo(2);
        }

        private void BtnPararHilo3_Click(object sender, EventArgs e)
        {
            DetenerHilo(3);
        }

        private void BtnPararHilo4_Click(object sender, EventArgs e)
        {
            DetenerHilo(4);
        }

        private void BtnPararHilo5_Click(object sender, EventArgs e)
        {
            DetenerHilo(5);
        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            if (SaldoDeCuenta > 0)
            {
                playerButtonSound.Play();
            }

            else
            {
                MessageBox.Show("La apuesta excede el saldo de su cuenta");
            }
        }

        private void BtnApuesta1000_Click(object sender, EventArgs e)
        {
            Apuesta = 1;
            label1.Text = "1000";
        }

        private void BtnApuesta2000_Click(object sender, EventArgs e)
        {
            Apuesta = 2;
            label1.Text = "2000";
        }

        private void BtnApuesta3000_Click(object sender, EventArgs e)
        {
            Apuesta = 3;
            label1.Text = "3000";
        }

        private void BtnApuesta4000_Click(object sender, EventArgs e)
        {
            Apuesta = 4;
            label1.Text = "4000";
        }

        private void BtnApueSta5000_Click(object sender, EventArgs e)
        {
            Apuesta = 5;
            label1.Text = "5000";
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}