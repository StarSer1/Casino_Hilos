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
using System.IO;
using System.Runtime.InteropServices;



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
        private int espaciado = 20; 
        private int numFilas = 3; 
        private int numColumnas = 1; 
        private int numPaneles = 5; 
        private int tiempoInactivo = 5000;
        private int Apuesta = 1;
        private int SaldoDeCuenta = 10000;
        private bool detenerAnimacion = true; 
        private bool animacionIniciada = false; 
        private bool botonDisponible = true;
        private bool ApuestaValida = false;
        private DateTime tiempoInicio; 
        private DateTime ultimoClick = DateTime.MinValue;

        private int[] data = new int[5]; 
        private int[][] posicionesIniciales;
        private int contador;
        private int valorS;
        private int valorA;
        private int Dinero;
        private bool primeravuelta = true;

        public Form1()
        {
            InitializeComponent();
            aleatorio = new Random();
            hilos = new Thread[numPaneles];
            InicializarPaneles();
            InicializarCuadrosImagen();
            InicializarImagenes(); 


            this.FormClosing += Form1_FormClosing; 
            this.Text = String.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
          
        }

        private void ReproducirAudio()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.Efecto_de_Dinero);

                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir el archivo de audio: " + ex.Message);
            }
        }

        private void AlinearImagenes(int panelIndex)
        {
            AjustarTamañoImagenes();

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
                cuadrosImagen[k] = new PictureBox[numColumnas, numFilas]; 
                posicionesIniciales[k] = new int[numFilas]; 

                for (int i = 0; i < numColumnas; i++)
                {
                    for (int j = 0; j < numFilas; j++)
                    {
                        cuadrosImagen[k][i, j] = new PictureBox();
                        cuadrosImagen[k][i, j].Size = new Size(anchoPictureBox, altoPictureBox);
                        posicionesIniciales[k][j] = 0 + j * (altoPictureBox + espaciado); 
                        cuadrosImagen[k][i, j].Location = new Point(0, posicionesIniciales[k][j]);
                        cuadrosImagen[k][i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                        paneles[k].Controls.Add(cuadrosImagen[k][i, j]);
                    }
                }
            }
        }


        private void InicializarImagenes()
        {
            
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
                    handleCreatedEvents[index].WaitOne(); 
                    AnimarColumna(index);
                });
                hilos[k].Start();
            }
        }

        private void AnimarColumna(int panelIndex)
        {
            int[] posicionesIniciales = new int[numFilas];
            for (int i = 0; i < numFilas; i++)
            {
                posicionesIniciales[i] = cuadrosImagen[panelIndex][0, i].Top;
            }

            bool animacionCompleta = false;

            while (!animacionCompleta) 
            {
                if (animacionIniciada)
                {
                    TimeSpan tiempoTranscurrido = DateTime.Now - tiempoInicio;
                    if (tiempoTranscurrido.TotalMilliseconds >= 4200)
                    {
                        detenerAnimacion = true;
                    }
                }

                bool allAligned = true; 

                for (int i = 0; i < numFilas; i++)
                {
                    if (!IsHandleCreated)
                        return; 

                    paneles[panelIndex].Invoke(new Action(() =>
                    {
                        if (!IsDisposed)
                        {
                            cuadrosImagen[panelIndex][0, i].Top += 20;

                
                            if (cuadrosImagen[panelIndex][0, i].Top >= paneles[panelIndex].Height)
                            {
                                cuadrosImagen[panelIndex][0, i].Top = cuadrosImagen[panelIndex][0, i].Top - (altoPictureBox + espaciado) * numFilas;
                                cuadrosImagen[panelIndex][0, i].Image = ObtenerImagenAleatoria();
                            }

                            if (cuadrosImagen[panelIndex][0, i].Top != posicionesIniciales[i])
                            {
                                allAligned = false;
                            }
                        }
                    }));
                }

                if (detenerAnimacion && allAligned)
                {
                    animacionCompleta = true; 
                }

                Thread.Sleep(50);
            }

 
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

                for (int i = 0; i < valores.Length - 2; i++)
                {
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
                        detenerAnimacion = false;
                        animacionIniciada = true; 
                        tiempoInicio = DateTime.Now; 
                        IniciarHilos(); 
                    }
                    else
                    {
                        detenerAnimacion = true; 
                        foreach (var hilo in hilos)
                        {
                            hilo.Join();
                        }
                        AjustarTamañoImagenes(); 
                    }
                    botonDisponible = false;

                    ultimoClick = DateTime.Now;

                    Task.Delay(tiempoInactivo).ContinueWith(_ =>
                    {
                        if ((DateTime.Now - ultimoClick).TotalMilliseconds >= tiempoInactivo)
                        {
                            Invoke((Action)(() =>
                            {
                                botonDisponible = true;
                                btnPalanca.Enabled = true;
                            }));
                        }
                    });

                }
                else
                {
                    DialogResult resultado = MessageBox.Show("Su cuenta no tiene el saldo suficiente, ¿Desea volver al menu principal?", "Confirmación", MessageBoxButtons.YesNo);

                    if (resultado == DialogResult.Yes)
                    {
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

                while (hilos[N - 1].IsAlive)
                {
                    Thread.Sleep(100);
                }

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
                ReproducirAudio();
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
            this.Hide();
            Menu menu = new Menu();
            menu.Show();
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void panelMover_Paint(object sender, PaintEventArgs e)
        {

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]


        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelMover_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}