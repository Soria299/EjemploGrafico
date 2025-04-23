using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjemploGrafico
{
    public partial class Form1 : Form
    {
        // Vector para almacenar los puntos que formarán la figura 
        private Point[] puntos;
        // Matriz de ejemplo (podría representar una transformación) 
        private int[,] matriz;
        // Variable para almacenar el tipo de figura seleccionada
        private string figuraSeleccionada;

        public Form1()
        {
            InitializeComponent();
            InicializarComponentesPersonalizados();
        }

        private void InicializarComponentesPersonalizados()
        {
            // Configuración básica del formulario 
            this.Text = "Aplicación Gráfica en .NET";
            this.Size = new Size(800, 600);

            // TextBox para ingresar el tamaño de la figura
            TextBox txtTamano = new TextBox();
            txtTamano.Location = new Point(20, 20);
            txtTamano.Width = 100;
            txtTamano.Name = "txtTamano";
            txtTamano.Text = ""; // Valor predeterminado
                                    // Validación: solo se permiten dígitos 
            txtTamano.KeyPress += TxtTamano_KeyPress;
            this.Controls.Add(txtTamano);

            Label lblTamano = new Label();
            lblTamano.Text = "Tamaño:";
            lblTamano.Location = new Point(20, 5);
            this.Controls.Add(lblTamano);

            // ComboBox para seleccionar el tipo de figura 
            ComboBox cmbFigura = new ComboBox();
            cmbFigura.Location = new Point(140, 20);
            cmbFigura.Width = 150;
            cmbFigura.Name = "cmbFigura";
            cmbFigura.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFigura.Items.Add("Triángulo");
            cmbFigura.Items.Add("Cuadrado");
            cmbFigura.Items.Add("Círculo");
            cmbFigura.SelectedIndex = 0; // Seleccionar triángulo por defecto
            figuraSeleccionada = "Triángulo";

            // Guardar la selección cuando cambia
            cmbFigura.SelectedIndexChanged += (sender, e) => {
                figuraSeleccionada = cmbFigura.SelectedItem.ToString();
            };

            this.Controls.Add(cmbFigura);

            Label lblFigura = new Label();
            lblFigura.Text = "Figura:";
            lblFigura.Location = new Point(140, 5);
            this.Controls.Add(lblFigura);

            // Botón para generar y dibujar la figura 
            Button btnDibujar = new Button();
            btnDibujar.Location = new Point(310, 20);
            btnDibujar.Text = "Dibujar";
            btnDibujar.Width = 80;
            btnDibujar.Click += (sender, e) =>
            {
                // Validación: se debe ingresar un valor
                if (string.IsNullOrEmpty(txtTamano.Text))
                {
                    MessageBox.Show("Ingrese el tamaño de la figura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Conversión y validación del número ingresado 
                if (!int.TryParse(txtTamano.Text, out int tamano))
                {
                    MessageBox.Show("El tamaño debe ser numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (tamano <= 0)
                {
                    MessageBox.Show("El tamaño debe ser mayor que cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Punto central de la figura (centro del formulario)
                int centroX = this.ClientSize.Width / 2;
                int centroY = this.ClientSize.Height / 2;

                // Crear puntos según la figura seleccionada
                switch (figuraSeleccionada)
                {
                    case "Triángulo":
                        // Crear un triángulo equilátero
                        puntos = new Point[3];
                        puntos[0] = new Point(centroX, centroY - tamano); // Vértice superior
                        puntos[1] = new Point(centroX - (int)(tamano * 0.866), centroY + (tamano / 2)); // Vértice inferior izquierdo
                        puntos[2] = new Point(centroX + (int)(tamano * 0.866), centroY + (tamano / 2)); // Vértice inferior derecho
                        break;

                    case "Cuadrado":
                        // Crear un cuadrado
                        puntos = new Point[4];
                        puntos[0] = new Point(centroX - tamano / 2, centroY - tamano / 2); // Esquina superior izquierda
                        puntos[1] = new Point(centroX + tamano / 2, centroY - tamano / 2); // Esquina superior derecha
                        puntos[2] = new Point(centroX + tamano / 2, centroY + tamano / 2); // Esquina inferior derecha
                        puntos[3] = new Point(centroX - tamano / 2, centroY + tamano / 2); // Esquina inferior izquierda
                        break;

                    case "Círculo":
                        // Para el círculo, generamos múltiples puntos en su circunferencia
                        int numPuntos = 36; // Para hacer un círculo suave
                        puntos = new Point[numPuntos];
                        for (int i = 0; i < numPuntos; i++)
                        {
                            double angulo = 2 * Math.PI * i / numPuntos;
                            puntos[i] = new Point(
                                centroX + (int)(tamano * Math.Cos(angulo)),
                                centroY + (int)(tamano * Math.Sin(angulo))
                            );
                        }
                        break;
                }

                // Creación de una matriz de 3x3 (matriz identidad) 
                matriz = new int[3, 3]
                {
            {1, 0, 0},
            {0, 1, 0},
            {0, 0, 1}
                };

                // Forzar el repintado del formulario para mostrar la figura 
                this.Invalidate();
            };
            this.Controls.Add(btnDibujar);
            // Botón para limpiar los datos
            Button btnLimpiar = new Button();
            btnLimpiar.Location = new Point(400, 20);
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Width = 80;
            btnLimpiar.Click += (sender, e) =>
            {
                // Limpiar la figura
                puntos = null;

                // Opcional: Restaurar valores predeterminados
                txtTamano.Text = "";
                cmbFigura.SelectedIndex = 0;
                figuraSeleccionada = "Triángulo";

                // Forzar el repintado del formulario para quitar la figura
                this.Invalidate();
            };
            this.Controls.Add(btnLimpiar);
        }

        // Validación en el TextBox: permite solo dígitos y teclas de control 
        private void TxtTamano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Método para dibujar la figura en el formulario 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (puntos != null && puntos.Length > 0)
            {
                // Configurar la calidad del dibujo
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (figuraSeleccionada == "Círculo")
                {
                    // Para el círculo, calculamos su rectángulo delimitador
                    int centroX = this.ClientSize.Width / 2;
                    int centroY = this.ClientSize.Height / 2;
                    int tamano = (int)Math.Sqrt(Math.Pow(puntos[0].X - centroX, 2) + Math.Pow(puntos[0].Y - centroY, 2));

                    // Dibujar el círculo
                    e.Graphics.DrawEllipse(
                        new Pen(Color.Blue, 2),
                        centroX - tamano,
                        centroY - tamano,
                        tamano * 2,
                        tamano * 2
                    );
                }
                else
                {
                    // Dibujar un polígono uniendo los puntos para el triángulo y cuadrado
                    e.Graphics.DrawPolygon(new Pen(Color.Blue, 2), puntos);
                }
                // Dibujar cada punto con un pequeño círculo rojo 
                foreach (var punto in puntos)
                {
                    e.Graphics.FillEllipse(Brushes.Red, punto.X - 3, punto.Y - 3, 6, 6);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
