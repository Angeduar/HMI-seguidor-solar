using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using actualizar1 = controlu;
using guardar1 = controlu;

namespace Solgui
{
    public partial class Form5 : Form
    {
        Form1 rut = new Form1();
        public int le;
        public string rutat;
        string texto1g, texto1a, ruta5;
        double anox, mesx, diax;

        public Interface1 contrato { get; set; }

        public Form5(string texto1)
        {
            InitializeComponent();
            timer1.Enabled = true;
            ruta5 = texto1;
            textBox1.Text = texto1;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            guardar1.guardar1 g = elementHost1.Child as guardar1.guardar1;
            actualizar1.actualizar1 a = elementHost2.Child as actualizar1.actualizar1;
            if (le == 1)
            {
                g.btng1.IsEnabled = false;//boton guardar
                a.btna.IsEnabled = true;//boton actualizar
            }
            else if (le == 0)
            {
                g.btng1.IsEnabled = true;//boton guardar
                a.btna.IsEnabled = false;//boton actualizar
            }
            radioButton2.Checked = true;
            radioButton1.Checked = false;
            anox = Int64.Parse(DateTime.Now.ToString("yyyy"));
            mesx = Int64.Parse(DateTime.Now.ToString("MM"));
            diax = Int64.Parse(DateTime.Now.ToString("dd"));
        }

        public bool vacio;//Variable utilizada para saber si hay algun Textbox vacio
        private void validar(Form5 formulario)
        {
            foreach (Control oControls in formulario.Controls)//Buscamos en cada Textbox de nuestro Formulario
            {
                if (oControls is TextBox & oControls.Text == String.Empty)//Verificamos que no existe vacio
                {
                    vacio = true;
                }
            }
            if (vacio == true)
            {
                MessageBox.Show("¡Por favor, rellene todos los campos!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            radioButton1.Checked = true;
            textBox1.Enabled = true;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            textBox1.Enabled = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                guardar1.guardar1 gua = elementHost1.Child as guardar1.guardar1;
                if(gua.gu == 1) //Guardar
                {
                    gua.gu=0;

                    validar(this);

                    if(vacio == false)
                    {
                        if (radioButton2.Checked == true)
                        {
                            ruta5= ("C:\\GraficasSolgui");
                            contrato.ejecutar5(ruta5);
                        }
                        else if (radioButton1.Checked == true)
                        {
                            texto1g = (textBox1.Text);
                            contrato.ejecutar5(texto1g);
                        }
                    }
                    else if(vacio == true)
                    {
                        vacio = false;
                    }
                    this.Close();
                }
                actualizar1.actualizar1 ac = elementHost2.Child as actualizar1.actualizar1;
                if (ac.actu == 1) //Actualizar
                {
                    ac.actu = 0;

                    if (radioButton2.Checked == true)
                    {
                        ruta5 = ("C:\\GraficasSolgui");
                        contrato.ejecutar6(ruta5); 
                        this.Close();
                    }
                    else if (radioButton1.Checked == true)
                    {
                            
                        validar(this);
                        if(vacio==false)
                        {
                            texto1a = (textBox1.Text);
                            contrato.ejecutar6(texto1a);
                            this.Close();
                        }
                        else if (vacio == true)
                        {
                            vacio = false;
                        }
                    }
                }
            }
            catch
            {
                //Falta mensaje de desconexion
            }
        }
    }
}
