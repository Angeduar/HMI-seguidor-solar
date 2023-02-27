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
    public partial class Form3 : Form
    {
        //Declaración de variables
        string tx1, tx2, tx3, tx4;
        Double tx11, tx22, tx33, tx44;
        int eer = 0;

        public Form3(string texto1, string texto2, string texto3, string texto4)
        {
            InitializeComponent();
            textBox1.Text = texto1;
            textBox2.Text = texto2;
            textBox3.Text = texto3;
            textBox4.Text = texto4;
            timer1.Enabled = true;
            timer2.Enabled = true;
            textBox1.MaxLength = 2;
            textBox2.MaxLength = 2;
            textBox3.MaxLength = 2;
            textBox4.MaxLength = 2;
        }

        public bool vacio;//Variable utilizada para saber si hay algun Textbox vacio

        private void validar(Form3 formulario)
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
                vacio = false;
            }
        }
        public Interface1 contrato { get; set; }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                tx1 = textBox1.Text;
                tx11 = Convert.ToDouble(tx1);
                if (tx11 >= 24)
                {
                    tx11 = 23;
                    tx1 = Convert.ToString(tx11);
                    textBox1.Text = tx1;
                }
                tx2 = textBox2.Text;
                tx22 = Convert.ToDouble(tx2);
                if (tx22 >= 60)
                {
                    tx22 = 59;
                    tx2 = Convert.ToString(tx22);
                    textBox2.Text = tx2;
                }
                tx3 = textBox3.Text;
                tx33 = Convert.ToDouble(tx3);
                if (tx33 >= 24)
                {
                    tx33 = 23;
                    tx3 = Convert.ToString(tx33);
                    textBox3.Text = tx3;
                }
                tx4 = textBox4.Text;
                tx44 = Convert.ToDouble(tx4);
                if (tx44 >=60)
                {
                    tx44 = 59;
                    tx4 = Convert.ToString(tx44);
                    textBox4.Text = tx4;
                }
                //Comparar entre rango inicial y final
                tx1 = textBox1.Text;
                tx11 = Convert.ToDouble(tx1);
                tx2 = textBox2.Text;
                tx22 = Convert.ToDouble(tx2);
                tx3 = textBox3.Text;
                tx33 = Convert.ToDouble(tx3);
                tx4 = textBox4.Text;
                tx44 = Convert.ToDouble(tx4);

                tx11= tx11+(tx22/60.0);
                tx33= tx33+(tx44/60.0);
                eer = 0;
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

                    if (tx11 >= tx33)
                    {
                        tx11 = 0;
                        tx1 = Convert.ToString(tx11);
                        textBox1.Text = tx1;

                        tx22 = 0;
                        tx2 = Convert.ToString(tx22);
                        textBox2.Text = tx2;

                        tx33 = 23;
                        tx3 = Convert.ToString(tx33);
                        textBox3.Text = tx3;

                        tx44 = 59;
                        tx4 = Convert.ToString(tx44);
                        textBox4.Text = tx4;
                        MessageBox.Show("El rango de inicio no puede ser mayor que el rango final", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        eer = 1;
                    }
                    if (eer == 0)
                    {
                        contrato.ejecutar3(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                        this.Close();
                    }
                }
            }
            catch
            {
                //falta mensaje por desconexion
            }
        }
    }
}
