using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using actualizar1 = controlu;

namespace Solgui
{
    public partial class Form4 : Form
    {
        //Declaración de variables
        string tx1, tx2, tx3, tx4;
        Int32 tx11, tx22, tx33, tx44;

        public Form4(string texto1, string texto2, string texto3, string texto4)
        {
            InitializeComponent();
            textBox1.Text = texto3; //longitud
            textBox2.Text = texto4;
            textBox4.Text = texto1; //latitud
            textBox3.Text = texto2;
            timer1.Enabled = true;
            timer2.Enabled = true;
            textBox1.MaxLength = 4;
            textBox2.MaxLength = 2;
            textBox3.MaxLength = 2;
            textBox4.MaxLength = 4;
        }

        public bool vacio;//Variable utilizada para saber si hay algun Textbox vacio

        private void validar(Form4 formulario)
        {
            foreach (Control oControls in formulario.Controls)//Buscamos en cada Textbox de nuestro Formulario
            {
                if(oControls is TextBox & oControls.Text == String.Empty)//Verificamos que no existe vacio
                {
                    vacio = true;
                }
            }
            if(vacio==true)
            {
                MessageBox.Show("¡Por favor, rellene todos los campos!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                vacio = false;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '-') && (sender as TextBox).Text.IndexOf('-') > -1) //Para que no se repita el "-"
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '-') && (sender as TextBox).Text.IndexOf('-') > -1) //Para que no se repita el "-"
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public Interface1 contrato { get; set; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                tx1 = textBox1.Text;
                tx11 = Convert.ToInt32(tx1);
                if (tx11 > 180)
                {
                    tx11 = 180;
                    tx1 = Convert.ToString(tx11);
                    textBox1.Text = tx1;
                }
                else if (tx11 < -180)
                {
                    tx11 = -180;
                    tx1 = Convert.ToString(tx11);
                    textBox1.Text = tx1;
                }
                tx2 = textBox2.Text;
                tx22 = Convert.ToInt32(tx2);
                if (tx22 >= 60)
                {
                    tx22 = 59;
                    tx2 = Convert.ToString(tx22);
                    textBox2.Text = tx2;
                }
                tx3 = textBox3.Text;
                tx33 = Convert.ToInt32(tx3);
                if (tx33 >= 60)
                {
                    tx33 = 59;
                    tx3 = Convert.ToString(tx33);
                    textBox3.Text = tx3;
                }
                tx4 = textBox4.Text;
                tx44 = Convert.ToInt32(tx4);
                if (tx44 > 90)
                {
                    tx44 = 90;
                    tx4 = Convert.ToString(tx44);
                    textBox4.Text = tx4;
                }else if (tx44 < -90)
                {
                    tx44 = -90;
                    tx4 = Convert.ToString(tx44);
                    textBox4.Text = tx4;
                }
            }
            catch
            {
                //falta mensaje por desconexion
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                actualizar1.actualizar1 ac = elementHost1.Child as actualizar1.actualizar1;

                if (ac.actu == 1)
                {
                    ac.actu = 0;
                    validar(this);

                    contrato.ejecutar(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    this.Close();
                }
            }
            catch 
            {
                //falta mensaje por desconexion
            }
        }
    }
}
