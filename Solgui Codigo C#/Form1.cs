using System;
using System.Management;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using usercon = controlu;
using decora2 = controlu;
using desconect = controlu;

namespace Solgui
{
    public partial class Form1 : Form, Interface1
    {
        //Declaracion de variables globales
        int i = 0, c = 0, j = 0, nocap = 0, pc = 99, sug = 0, w = 0, persis = 0, correcdia = 1, pase=0;
        double p=0.0, v=0.0;  //Potencia y voltaje
        string m, hx, ver1, ver11, ver2, ver22, ver3, ver33, ver4, ver5, ver55, ver555, ver6, ver66, ver666, ver7, ver77, ver777, decimalx1, entero1;
        string b, texto1, texto2, texto3, texto4, rutaa, rutasinfech, modelNo, manufatureID, signature, totalHeads;
        public string ruta;
        double anox, mesx, diax, horax, minx, segx, anoxr, mesxr, diaxr, horaxr, minxr, segxr, ax, dtbased;
        double mina, mino=59.0, horaa, horao=23.0, axr, dt1baset, dtx, dt2xo, dt2xa, dta, dto, dt1xr, horaminsx, horaminsx1, dt1x;
        int ye = 4, autog = 1, configr = 0, pm1 = 0, pm2 = 0, per=0, confi=0, bat=0;
        double hora1m = 9999.0, hora2m = 9999.0, hm, hs, am, ase, ym, ys, horaa1, horao1;
        double num, decimalx, entero, hora1 = 0.0, min1 = 0.0, hora2 = 12.0, min2 = 0.0, hora3 = 0.0, hora33=0.0, min3 = 0.0;
        double horal1 = 0.0, horal2 = 0.0, dt, t, alfa, y, longitdeci, latideci;
        double horamin, horafalt, lat, lati = 10.29, longit = -68.06, longi, a, h = 9999.0, s, z, sdif, sl, correclocal;
        double y1, h1, horasug, op, horita, horamincam, pprom, vprom, pdia;
        string latistring, longitstring, signaturered, modelNored, latif, latidecif, longitf, longitdecif;
        string tx1, tx2, tx3, tx4, bate, minutos, horamos, dia, mes, ano, ap;
        int tx11, tx22, tx33, tx44, prome=0, scn = 0;
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ConfigSolGUI.txt");
        public bool vacio;//Variable utilizada para saber si hay algun Textbox vacio
        //------------------------------------------------------------------------------

        private void validar(Form1 formulario)
        {
            intentovacio:
            try
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
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentovacio;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        //Función dias faltantes
        void dtfal()
        {
            intentarmdtf:
            try
            {
                //"dt" Dias faltantes desde el 21 de marzo del mediodia
                if (mesx == 1.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0;
                }
                else if (mesx == 2.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0;
                }
                else if (mesx == 3.0)
                {
                    if (diax < 21.0)
                    {
                        dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0 + 30 + 31.0 + 31.0 + (28.0);
                        //--> ojo año bisiesto +1 (Obviar esto, Ya no se suma 1 por considerar 365,25 dias)
                    }
                    else if (diax >= 21.0)
                    {
                        dt = diax - 21.0;
                    }
                }
                else if (mesx == 4.0)
                {
                    dt = diax - 21.0 + 31.0;
                }
                else if (mesx == 5.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0;
                }
                else if (mesx == 6.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0;
                }
                else if (mesx == 7.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0;
                }
                else if (mesx == 8.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0;
                }
                else if (mesx == 9.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0;
                }
                else if (mesx == 10.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0;
                }
                else if (mesx == 11.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0;
                }
                else if (mesx == 12.0)
                {
                    dt = diax - 21.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0 + 30.0;
                }
            }
            catch 
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarmdtf;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        void latitu()
        {
            intentarmlat:
            try
            {
                num = lati;

                if (num >= 0.0)
                {
                    entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                    decimalx = (lati - entero) * (100.0);//para que me quede la parte decimal 0.42 -> 42
                    decimalx = Math.Round(decimalx);
                    lat = entero + (decimalx / 60.0);// convirtiendo a grados
                }
                else if (num < 0.0)
                {
                    num = (-num);
                    entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                    decimalx = (num - entero) * (100.0);//para que me quede la parte decimal 0.42 -> 42
                    decimalx = Math.Round(decimalx);
                    lat = entero + (decimalx / 60.0);// convirtiendo a grados
                    entero = (-entero);
                    lat = -lat;
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarmlat;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        void longitu()
        {
            intentarmlo:
            try
            {
                num = longit;

                if (num >= 0.0)
                {
                    entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                    decimalx = (longit - entero) * (100.0);//para que me quede la parte decimal 0.42 -> 42
                    decimalx = Math.Round(decimalx);
                    longi = entero + (decimalx / (60.0));// convirtiendo a grados
                }
                else if (num < 0.0)
                {
                    num = (-num);
                    entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                    decimalx = (num - entero) * (100.0);//para que me quede la parte decimal 0.42 -> 42
                    decimalx = Math.Round(decimalx);
                    longi = entero + (decimalx / (60.0));// convirtiendo a grados
                    entero = (-entero);
                    longi = (-longi);
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarmlo;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        void calresto()
        {
            intentarmres:
            try
            {
                if ((horamin < 12.0) && (correcdia == 1))//A veces no se cumple el dia (12pm) y debe eliminarse un dia y sumar solo horas
                {
                    correcdia = 0;
                    dtfal();// Cuenta la diferencia de dias desde 21 de marzo a las 12pm hasta hoy (no considera la hora de hoy)
                    dt = dt - (1.0); //Correccion por de dia por horas restantes (considerando la hora de hoy)
                }

                if (horamin < 12.0)
                {
                    horafalt = (horamin + (12.0));
                }
                else if (horamin >= (12.0))
                {
                    horafalt = (horamin - (12.0));
                    if(correcdia == 0)
                    {
                        dtfal();
                        correcdia = 1;
                    }
                }

                // Paso 2 Calculo de la hora sidera local

                s = dt * (3.0 + (55.9084 / 60.0)); //Esto esta en minutos

                sdif = s + (horafalt * 60.0) + ((horafalt / 24.0) * (3.0 + (55.9084 / 60.0))); //Esto esta en minutos

                sdif = ((sdif) / (60.0)); //Llevando a hora

                //llevando de grados minutos' a solo grados
                longitu();

                //Correcion por longitud local segun meridiano de greenwitch
                if (longi > -180.0 && longi <= -165.0)  //hora -12:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (165.0)) / (15.0));
                }
                else if (longi > -165.0 && longi <= -150.0) //hora -11:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (150.0)) / (15.0));
                }
                else if (longi > -150.0 && longi <= -135.0)  //hora -10:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (135.0)) / (15.0));
                }
                else if (longi > -135.0 && longi <= -120.0) //hora -9:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (120.0)) / (15.0));
                }
                else if (longi > -120.0 && longi <= -105.0) //hora -8:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (105.0)) / (15.0));
                }
                else if (longi > -105.0 && longi <= -90.0) //hora -7:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (90.0)) / (15.0));
                }
                else if (longi > -90.0 && longi <= -75.0) //hora -6:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (75.0)) / (15.0));
                }
                else if (longi > -75.0 && longi <= -60.0) //hora -5:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (60.0)) / (15.0));
                }
                else if (longi > -60.0 && longi <= -45.0) //hora -4:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (45.0)) / (15.0));
                }
                else if (longi > -45.0 && longi <= -30.0) //hora -3:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (30.0)) / (15.0));
                }
                else if (longi > -30.0 && longi <= -15.0) //hora -2:00h
                {
                    correclocal = ((((-1.0) * (longi)) - (15.0)) / (15.0));
                }
                else if (longi > -15.0 && longi < 0.0) //hora -1:00h
                {
                    correclocal = (((-1.0) * (longi)) / (15.0));
                }
                else if (longi == 0.0)  //hora 0:00h
                {
                    correclocal = 0.0;
                }
                else if (longi < 15.0 && longi > 0.0) //hora +1:00h
                {
                    correclocal = ((longi) / (15.0));
                }
                else if (longi < 30.0 && longi >= 15.0)  //hora +2:00h
                {
                    correclocal = ((longi - 15.0) / (15.0));
                }
                else if (longi < 45.0 && longi >= 30.0)  //hora +3:00h
                {
                    correclocal = ((longi - 30.0) / (15.0));
                }
                else if (longi < 60.0 && longi >= 45.0)  //hora +4:00h
                {
                    correclocal = ((longi - 45.0) / (15.0));
                }
                else if (longi < 75.0 && longi >= 60.0)  //hora +5:00h
                {
                    correclocal = ((longi - 60.0) / (15.0));
                }
                else if (longi < 90.0 && longi >= 75.0)  //hora +6:00h
                {
                    correclocal = ((longi - 75.0) / (15.0));
                }
                else if (longi < 105.0 && longi >= 90.0) //hora +7:00h
                {
                    correclocal = ((longi - 90.0) / (15.0));
                }
                else if (longi < 120.0 && longi >= 105.0) //hora +8:00h
                {
                    correclocal = ((longi - 105.0) / (15.0));
                }
                else if (longi < 135.0 && longi >= 120.0) //hora +9:00h
                {
                    correclocal = ((longi - 120.0) / (15.0));
                }
                else if (longi < 150.0 && longi >= 135.0) //hora +10:00h
                {
                    correclocal = ((longi - 135.0) / (15.0));
                }
                else if (longi < 165.0 && longi >= 150.0) //hora +11:00h
                {
                    correclocal = ((longi - 150.0) / (15.0));
                }
                else if (longi < 180.0 && longi >= 165.0) //hora +12:00h
                {
                    correclocal = ((longi - 165.0) / (15.0));
                }

                sl = sdif - correclocal; //corrigiendo hora sidera segun hora local por zona horaria segun meridiano de greenwitch


                //llevando de grados minutos' a solo grados
                latitu();

                // Paso 3 calculo del angulo t

                alfa = ((360.0 / 365.25) * (dt)) + ((horafalt) * (360.0 / 8766.0));     //1 hora son 15 grados   (360grados/365.25 dias * cantidad de dias trasncurridos)
                alfa = ((alfa * (Math.PI)) / 180.0); //transformando alfa de grados a radianes

                y = ((23.45) * (Math.Sin(alfa)));  //alfa en radianes "y" es la declinacion esta en grados

                alfa = ((alfa * (180.0)) / (Math.PI)); //Llevando de radianes a grados
                alfa = ((alfa) / (15.0)); //llevado de grados a horas

                t = sl - alfa;

                t = (t * 15.0); //llevado a grados

                if(t>180)
                {
                    t = t - 360;
                }

                // Paso 4 altura del sol

                //transformando todo a radianes

                t = ((t * (Math.PI)) / 180.0); //transformando de grados a radianes
                lat = ((lat * (Math.PI)) / 180.0);
                y = ((y * (Math.PI)) / 180.0);

                h = Math.Acos(((Math.Sin(y)) * (Math.Sin(lat))) + ((Math.Cos(y)) * (Math.Cos(lat)) * (Math.Cos(t))));
                z = h;
                // Paso 5 Calculo del azimut
                t = ((t * 180.0) / (Math.PI)); // radianes a grados
                if (t < 0.0) // t negativo
                {
                    t = ((t * (Math.PI)) / 180.0); // grados a radianes
                    a = Math.Acos((((Math.Sin(y)) * (Math.Cos(lat))) - ((Math.Cos(y)) * (Math.Sin(lat)) * (Math.Cos(t)))) / (Math.Sin(z)));
                    a = ((a * 180.0) / (Math.PI)); // radianes a grados
                }
                else if (t >= 0.0) // t positivo
                {
                    t = ((t * (Math.PI)) / 180.0); // grados a radianes
                    a = Math.Acos((((-1 * (Math.Sin(y))) * (Math.Cos(lat))) + ((Math.Cos(y)) * (Math.Sin(lat)) * (Math.Cos(t)))) / (Math.Sin(z)));
                    a = ((a * 180.0) / (Math.PI)); // radianes a grados
                    a = a + 180.0;
                }
                y = ((y * (180.0)) / (Math.PI)); //Llevando de radianes a grados y
                //Esto es parte del paso 4
                h = ((h * 180.0) / (Math.PI)); // radianes a grados
                lat = ((lat * 180.0) / (Math.PI));
                h = (90.0) - h;

                //Cambiando altitud del sol de grados a grados°min'seg''
                num = h;
                h1 = h;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (h - entero);//para que me quede la parte decimal 0.42 
                h = entero;//grados
                hm = (decimalx * 60.0); //minutos
                num = hm;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (hm - entero);//para que me quede la parte decimal 0.42 
                hm = Math.Truncate(hm); //new
                hs = (decimalx * 60.0);
                hs = Math.Truncate(hs); //new
                if (hm < 0.0)
                {
                    hm = -hm;
                }
                if(hs < 0.0)
                {
                    hs = -hs;
                }
                
                //Cambiando azimut del sol de grados a grados°min'seg''
                num = a;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (a - entero);//para que me quede la parte decimal 0.42 -> 42
                a = entero;//grados
                am = (decimalx * 60.0); //minutos
                num = am;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (am - entero);//para que me quede la parte decimal 0.42 -> 42
                am = Math.Truncate(am); ;
                ase = (decimalx * 60.0);//Segundos
                ase = Math.Truncate(ase);

                //Cambiando declinacion del sol de grados a grados°min'seg''
                num = y;
                y1 = y;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (y - entero);//para que me quede la parte decimal 0.42 -> 42
                y = entero;//grados
                ym = (decimalx * 60.0); //minutos
                num = ym;
                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (ym - entero);//para que me quede la parte decimal 0.42 -> 42
                ym = Math.Truncate(ym);
                ys = (decimalx * 60.0);
                ys = Math.Truncate(ys);
                if (ym < 0.0)
                {
                    ym = -ym;
                }
                if (ys < 0.0)
                {
                    ys = -ys;
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarmres;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        void inicio()
        {
            intentarmi:
            try
            {
                longitu();
                entero1 = Convert.ToString(entero);
                longitf = entero1;
                decimalx1 = decimalx.ToString("00");
                longitdecif = decimalx1;
                label17.Text = ("Longitud actual:");
                label28.Text = (entero1 + "°" + decimalx1 + "'");
                latitu();
                entero1 = Convert.ToString(entero);
                latif = entero1;
                decimalx1 = decimalx.ToString("00");
                latidecif = decimalx1;
                label20.Text = ("Latitud actual:");
                label27.Text = (entero1 + "°" + decimalx1 + "'");

                if (ye == 4)
                {
                    //"dtx" Dias faltantes desde el 1 de enero de la media noche 00:00
                    if (mesx == 1.0)
                    {
                        dtx = diax;
                    }
                    else if (mesx == 2.0)
                    {
                        dtx = diax + 30.0;
                    }
                    else if (mesx == 3.0)
                    {
                        dtx = diax + 30.0 + 28.0;//--> ojo año bisiesto +1 a partir de marzo
                    }
                    else if (mesx == 4.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0;
                    }
                    else if (mesx == 5.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0;
                    }
                    else if (mesx == 6.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0;
                    }
                    else if (mesx == 7.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0;
                    }
                    else if (mesx == 8.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0;
                    }
                    else if (mesx == 9.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0;
                    }
                    else if (mesx == 10.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0;
                    }
                    else if (mesx == 11.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0;
                    }
                    else if (mesx == 12.0)
                    {
                        dtx = diax + 30.0 + 28.0 + 31.0 + 30.0 + 31.0 + 30.0 + 31.0 + 31.0 + 30.0 + 31.0 + 30.0;
                    }
                }

                //Calculo de altura inicial del sol h aprox cero (obtener la hora y min amanecer)
                min1 = 0.0;
                hora1 = 0.0;
                min2 = 0.0;
                hora2 = 12.0;
                h = 9999.0;
                for (int k = 1; k < 86400; k++)
                {
                    if ((h > -0.5) && (h < 0.5))
                    {
                        break;
                    }

                    if (min1 == 60.0)
                    {
                        min1 = 0.0;
                        hora1 = hora1 + 1.0;
                        if (hora1 == 24.0)
                        {
                            break;
                        }
                    }
                    //-----------------------------------------------------------------------------
                    // Paso 1 Calculo de dias faltantes desde el 21 de marzo del mediodia
                    dtfal();  //funcion dias faltantes

                    //conversion de minutos a hora
                    horamin = hora1 + ((min1) / (60.0));

                    calresto(); //correccion de hora, y demas pasos para calcular altura y azimut
                    //-----------------------------------------------------------------------------
                    min1++;
                }
                //Calculo de altura final del sol h aprox cero (obtener la hora y min)
                h = 9999.0;
                for (int k = 43200; k < 86400; k++)
                {
                    if ((h > -0.5) && (h < 0.5))
                    {
                        break;
                    }

                    if (min2 == 60.0)
                    {
                        min2 = 0.0;
                        hora2 = hora2 + 1.0;
                        if (hora2 == 24.0)
                        {
                            break;
                        }
                    }
                    //-----------------------------------------------------------------------------
                    dtfal();  //funcion dias faltantes

                    //conversion de minutos a hora
                    horamin = hora2 + ((min2) / (60.0));

                    calresto(); //correccion de hora, y demas pasos para calcular altura y azimut
                    //-----------------------------------------------------------------------------
                    min2++;
                }
                // Paso 1 Calculo de dias faltantes desde el 21 de marzo del mediodia

                dtfal();  //funcion dias faltantes

                //conversion de minutos a hora
                horamin = horax + ((minx) / (60.0)) + (segx / (3600.0));

                calresto(); //correccion de hora, y demas pasos para calcular altura y azimut

                //Cantidad de horas de luz por dia
                hora3 = 0.0;
                min3 = 0.0;
                horal1 = 0.0;
                horal2 = 0.0;
                horal1 = hora1 + ((min1) / (60.0));
                horal2 = hora2 + ((min2) / (60.0));

                hora3 = horal2 - horal1; //La resta me da la diferencia de horas de luz
                num = hora3;

                entero = Math.Truncate(num); //tomando el numero entero 12.42 -> 12
                decimalx = (hora3 - entero);//para que me quede la parte decimal 0.42 estos se convierte en minutos
                decimalx = (decimalx) * (60.0);
                decimalx = Math.Truncate(decimalx);
                hora3 = entero;
                min3 = decimalx;

                //Cambio de p.m. y a.m. hora1
                if (hora1 >= 12.0)
                {
                    pm1 = 1; //Activar p.m.
                    hora1m = hora1;
                    if (hora1 >= 13.0)
                    {
                        hora1m = hora1 - 12.0; // hora p.m.
                    }
                }
                else if (hora1 < 12.0)
                {
                    pm1 = 0; //Activar a.m.
                    if (hora1 == 0.0)
                    {
                        hora1m = hora1 + 12.0; //Esto es para que sea 12:30 am y no 0:30 am
                    }
                    else
                    {
                        hora1m = hora1;
                    }
                }
                //Cambio de p.m. y a.m. hora2
                if (hora2 >= 12.0)
                {
                    pm2 = 1; //Activar p.m.
                    hora2m = hora2;
                    if (hora2 >= 13.0)
                    {
                        hora2m = hora2 - 12.0; // hora p.m.
                    }
                }
                else if (hora2 < 12.0)
                {
                    pm2 = 0; //Activar a.m.
                    if (hora2 == 0.0)
                    {
                        hora2m = hora2 + 12.0; //Esto es para que sea 12:30 am y no 0:30 am
                    }
                    else
                    {
                        hora2m = hora2;
                    }
                }

                ax = anox;
                c = 0;
                while (2012.0 <= ax)
                {
                    ax = ax - 4.0;
                    c = c + 1; //contador de años biciestos
                }

                ax = anox - 2012.0; //Diferencia de años
                dt1x = (365.0 * ax) + dtx + c; //"c" es los dias faltantes para sumar a dias de diferencia total a sumar con 40909
                dt1x = dt1x + 40909.0; //este valor va en minium AxisX.Minimum
                dtbased = dt1x;
                dt2xo = dt1x;
                dt1xr = dt1x;//Esto es para hacer un rango maximo autoajustable

                horaminsx = horax + ((minx) / (60.0)) + ((segx) / (3600.0)); //conversion de minutos a hora
                dt1x = ((horaminsx) / (24.0)) + dt1x;

                horaminsx = hora2 + ((min2) / (60.0));
                dt2xo = ((horaminsx) / (24.0)) + dt2xo;

                if(ye==4)
                {
                    horaminsx1 = (horax) + ((minx) / (60.0)) + ((segx + 1.0) / (3600.0)); //conversion de minutos a hora
                    dt1xr = ((horaminsx1) / (24.0)) + dt1xr;
                }

                chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dt1x;  //Capta los datos XY al abrir el programa
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dt1xr;

                chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chart2.ChartAreas["ChartArea1"].AxisX.Minimum = dt1x;  //Capta los datos XY al abrir el programa
                chart2.ChartAreas["ChartArea1"].AxisX.Maximum = dt1xr;

                if (ye == 4)//Esto es para que no haga un salto en la grafica
                {
                    ye = 1;
                    if(pc==0)
                    {
                        chart1.Series["P vs t"].Points.AddXY(dt1x, i);
                        chart2.Series["V vs t"].Points.AddXY(dt1x, i);
                    }
                }
                //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dt2xo; //ajuste automatico  de rango
                timer1.Enabled = true;
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarmi;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        public Form1()
        {   
            intentar1:
            try
            {
                if (ye == 4) //ye=4 es usado solo para no inicializar mas de una vez
                {
                    InitializeComponent();

                    timer1.Enabled = false;
                    timer3.Enabled = false;
                    menuStrip1.Enabled = false;
                    menuStrip1.Visible = false;
                    chart1.Size = new Size(0, 0);
                    chart2.Size = new Size(0, 0);

                    //Ocultar pantalla interfaz
                    label1.Visible = false;
                    label2.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label8.Visible = false;
                    label20.Visible = false;
                    label17.Visible = false;
                    label25.Visible = false;
                    label39.Visible = false;

                    //Obteniendo datos del discoduro
                    modelNo = identifier("Win32_DiskDrive", "Model");//Modelo
                    manufatureID = identifier("Win32_DiskDrive", "Manufacturer");
                    signature = identifier("Win32_DiskDrive", "Signature");//Numero
                    totalHeads = identifier("Win32_DiskDrive", "TotalHeads");;

                    if (!File.Exists(path))//Verificar si existe el documento .txt sino lo crea y escribe
                    {
                        File.Create(path).Dispose();
                        using (TextWriter escribir = new StreamWriter(path))
                        {
                            escribir.WriteLine("N");
                            escribir.WriteLine("N");
                            escribir.WriteLine("N");
                            escribir.WriteLine("N");
                            escribir.WriteLine("N");
                            escribir.Close();
                        }
                    }
                    else if (File.Exists(path))
                    {
                        using (StreamReader leer = new StreamReader(path))//ojo debe ser la misma ruta q el instalado
                        {
                            //Obtener datos guardados (Variables, etc)
                            latistring = leer.ReadLine();
                            longitstring = leer.ReadLine();
                            signaturered = leer.ReadLine();
                            modelNored = leer.ReadLine();
                            ruta = leer.ReadLine();
                        }
                    }

                    if ((modelNored == modelNo) && (signaturered == signature))
                    {
                        //Cambiando parte visual 2da vez
                        lati = Convert.ToDouble(latistring);
                        longit = Convert.ToDouble(longitstring);
                        
                        //Ocultando pantalla 1era vez
                        label14.Visible = false;
                        label15.Visible = false;
                        label16.Visible = false;
                        label12.Visible = false;
                        label13.Visible = false;
                        label4.Visible = false;
                        label3.Visible = false;
                        label24.Visible = false;
                        label23.Visible = false;
                        label22.Visible = false;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        button1.Visible = false;

                        //Mostrando pantalla 2da vez
                        bconectar.Visible = true;
                        spuerto.Visible = true;
                        rconexion.Visible = true;
                        button2.Visible = true;
                        button2.Enabled = false;
                        button3.Visible = true;
                        this.Size = new Size(322, 180);
                        this.MaximumSize = new Size(318, 201);
                        this.MinimumSize = new Size(318, 201);
                    }
                    else
                    {
                        //Cambiando parte visual 1era vez
                        ruta = ("C:\\GraficasSolgui");
                        this.Size = new Size(323, 278);
                        this.MaximumSize = new Size(323, 278);
                        this.MinimumSize = new Size(323, 278);
                        bconectar.Visible = false;
                        spuerto.Visible = false;
                        rconexion.Visible = false;
                        button2.Visible = false;
                        button3.Visible = false;
                    }
                }

                m = DateTime.Now.ToString();
                anox = Int64.Parse(DateTime.Now.ToString("yyyy"));
                mesx = Int64.Parse(DateTime.Now.ToString("MM"));
                diax = Int64.Parse(DateTime.Now.ToString("dd"));
                horax = Int64.Parse(DateTime.Now.ToString("HH"));
                minx = Int64.Parse(DateTime.Now.ToString("mm"));
                segx = Int64.Parse(DateTime.Now.ToString("ss"));

                label2.Visible = false;
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentar1;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            intentarm:
            try
            {
                //m = DateTime.Now.ToString(); //Fecha hora y dia

                anoxr = Int64.Parse(DateTime.Now.ToString("yyyy"));//Solicitar año mes dia hora minutos y segundos para el auto rango
                mesxr = Int64.Parse(DateTime.Now.ToString("MM"));
                diaxr = Int64.Parse(DateTime.Now.ToString("dd"));
                horaxr = Int64.Parse(DateTime.Now.ToString("HH"));
                minxr = Int64.Parse(DateTime.Now.ToString("mm"));
                segxr = Int64.Parse(DateTime.Now.ToString("ss"));

                //Conversion de minutos a hora
                horamin = horaxr + ((minxr) / (60.0)) + (segxr / (3600.0));

                horasug = horaxr + (minxr / 60.0) + (segxr / 3600.0);

                calresto(); //correccion de hora, y demas pasos para calcular altura y azimut

                axr = anoxr - 2012.0; //Diferencia de años
                dt1xr = (365.0 * axr) + dtx + c; //"c" es los dias faltantes para sumar a dias de diferencia total a sumar con 40909
                dt1xr = dt1xr + 40909.0; //este valor va en minium AxisX.Minimum

                horaminsx1 = (horaxr) + ((minxr) / (60.0)) + ((segxr + 1.0) / (3600.0)); //conversion de minutos a hora
                dt1xr = ((horaminsx1) / (24.0)) + dt1xr;

                if (ye == 1)
                {
                    ye = 0;//ye=0 y ye=1 estan reservados solo para esto
                    dt1baset = ((0.5 / (24.0)) + dt1x); //Valor para que cambie de intervalo a horas
                }

                if (configr == 0) //Por defecto
                {
                    if (dt1xr > dt2xo)
                    {
                        nocap = 1;
                        chart1.Enabled = false;
                        chart1.Visible = false;
                        chart2.Enabled = false;
                        chart2.Visible = false;
                        label2.Text = "No disponible, ¡Es de noche espera el proximo dia!";
                        label2.Visible = true;

                    }
                    else if (dt1xr < dt2xo)
                    {
                        nocap = 0;
                        chart1.Enabled = true;
                        chart1.Visible = true;
                        chart2.Enabled = true;
                        chart2.Visible = true;
                        label2.Visible = false;
                    }

                    /*if(dt1xr==dt2xo)
                    {
                        configr = 9999;
                    }*/

                    if (dt1baset <= dt1xr)
                    {
                        chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                        chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    }
                    else if (configr == 0 && ye == 2)
                    {
                        ye = 3; //ye=2 y ye=3 estan reservados solo para cambiar a minutos
                        chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                        chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                    }
                }
                else if (configr == 1)//Por amanecer y oscurecer
                {
                    if ((dt1xr < dt2xa) || (dt1xr > dt2xo))
                    {
                        nocap = 1;
                        chart1.Enabled = false;
                        chart1.Visible = false;
                        chart2.Enabled = false;
                        chart2.Visible = false;
                        label2.Text = "No disponible, ¡Aun no amanece!";
                        label2.Visible = true;
                    }
                    else if (dt1xr >= dt2xa)
                    {
                        nocap = 0;
                        chart1.Enabled = true;
                        chart1.Visible = true;
                        chart2.Enabled = true;
                        chart2.Visible = true;
                        label2.Visible = false;
                    }

                }
                else if (configr == 2)//Por rango personalizado
                {
                    if(per==0)
                    {
                        if(confi == 0)
                        {
                            porDefectoToolStripMenuItem.Checked = true;
                            amanecerAtardecerToolStripMenuItem.Checked = false;
                            personalizadoToolStripMenuItem.Checked = false;
                        }
                        else if (confi == 1)
                        {
                            porDefectoToolStripMenuItem.Checked = false;
                            amanecerAtardecerToolStripMenuItem.Checked = true;
                            personalizadoToolStripMenuItem.Checked = false;
                        }
                    }
                    else if (per == 1)
                    {
                        nocap = 0;
                        chart1.Enabled = true;
                        chart1.Visible = true;
                        chart2.Enabled = true;
                        chart2.Visible = true;
                        label2.Visible = false;
                        porDefectoToolStripMenuItem.Checked = false;
                        amanecerAtardecerToolStripMenuItem.Checked = false;
                        personalizadoToolStripMenuItem.Checked = true;
                    }
                }

                hx = Convert.ToString(m);

                dia = diaxr.ToString("00");
                mes = mesxr.ToString("00");
                ano = anoxr.ToString("00");

                label1.Text = (dia+"/"+mes+"/"+ano);
                ver1 = hora1m.ToString("00");
                ver11 = min1.ToString("00");
                if (pm1 == 1)
                {
                    label5.Text = ("Amanecer:");
                    label29.Text = (ver1 + ":" + ver11 + " p.m.");
                }
                else if (pm1 == 0)
                {
                    label5.Text = ("Amanecer:");
                    label29.Text = (ver1 + ":" + ver11 + " a.m.");
                }
                ver2 = hora2m.ToString("00");
                ver22 = min2.ToString("00");
                if (pm2 == 1)
                {
                    label6.Text = ("Atardecer:");
                    label30.Text = (ver2 + ":" + ver22 + " p.m.");
                }
                else if (pm2 == 0)
                {
                    label6.Text = ("Atardecer:");
                    label30.Text = (ver2 + ":" + ver22 + " a.m.");
                }
                ver3 = Convert.ToString(hora3);
                ver33 = Convert.ToString(min3);
                label7.Text = ("Horas de luz para hoy:");
                label36.Text = (ver3 + "hr" + ":" + ver33 + "min");
                //ver4 = Convert.ToString();//Watts 
                label8.Text = ("Vatios estimados para hoy:");
                hora33= hora3 + (min3 / 60.0);
                pdia=(hora33*(4.2)); // Los 4.2 son Wattios
                label37.Text = (pdia + "W");
                ver5 = Convert.ToString(h);
                ver55 = hm.ToString("00");
                ver555 = hs.ToString("00");
                if (h1 >= 0.0)
                {
                    label9.Text = ("Altitud del sol:");
                    label31.Text = (ver5 + "°" + ver55 + "'" + ver555 + "''");

                }
                else if (h1 < 0.0)
                {
                    if (h1 < -1.0)
                    {
                        label9.Text = ("Altitud del sol:");
                        label31.Text = (ver5 + "°" + ver55 + "'" + ver555 + "''");
                    }
                    else
                    {
                        label9.Text = ("Altitud del sol:");
                        label31.Text = ("-" + ver5 + "°" + ver55 + "'" + ver555 + "''");
                    }
                }
                ver6 = Convert.ToString(a);
                ver66 = am.ToString("00");
                ver666 = ase.ToString("00");
                label10.Text = ("Azimut del sol:");
                label32.Text = ( ver6 + "°" + ver66 + "'" + ver666 + "''");
                ver7 = Convert.ToString(y);
                ver77 = ym.ToString("00");
                ver777 = ys.ToString("00");
                if(y1 >= 0.0)
                {
                    label11.Text = ("Declinacion del sol:");
                    label33.Text = (ver7 + "°" + ver77 + "'" + ver777 + "''");
                }
                else if (y1 < 0.0)
                {
                    if(y1<-1.0)
                    {
                        label11.Text = ("Declinacion del sol:");
                        label33.Text = (ver7 + "°" + ver77 + "'" + ver777 + "''");
                    }
                    else
                    {
                        label11.Text = ("Declinacion del sol:");
                        label33.Text = (ver7 + "°" + ver77 + "'" + ver777 + "''");
                    }
                }
                //ver4 = Convert.ToString(v);
                if (v <= 999.99)
                {
                    if (v <= 9.99)
                    {
                        ver77 = v.ToString("0.00");
                        label18.Text = ("Voltaje generado: " + ver77 + "V");
                    }
                    else if (v <= 99.99)
                    {
                        ver77 = v.ToString("00.00");
                        label18.Text = ("Voltaje generado: " + ver77 + "V");
                    }
                    else if (v <= 999.99)
                    {
                        ver77 = v.ToString("000.00");
                        label18.Text = ("Voltaje generado: " + ver77 + "V");
                    }
                }

                if (vprom <= 999.9)
                {
                    if (vprom <= 9.99)
                    {
                        ver77 = vprom.ToString("0.00");
                        label39.Text = ("Voltaje promedio generado: " + ver77 + "V");
                    }
                    else if (vprom <= 99.99)
                    {
                        ver77 = vprom.ToString("00.00");
                        label39.Text = ("Voltaje promedio generado: " + ver77 + "V");
                    }
                    else if (vprom <= 999.99)
                    {
                        ver77 = vprom.ToString("000.00");
                        label39.Text = ("Voltaje promedio generado: " + ver77 + "V");
                    }
                }

                //ver4 = Convert.ToString(p);
                if(p>=1000.0)
                {
                    double temporal77;
                    temporal77 = (p * 0.001);
                    if (p <= 9999.99)
                    {
                        ver77 = temporal77.ToString("0.00");
                        label19.Text = ("Potencia generada: " + ver77 + "W");
                    }
                    else if (p <= 99999.99)
                    {
                        ver77 = temporal77.ToString("00.00");
                        label19.Text = ("Potencia generada: " + ver77 + "W");
                    }
                }
                else if (p<1000.0)
                {
                    if (p <= 9.99)
                    {
                        ver77 = p.ToString("0.00");
                        label19.Text = ("Potencia generada: " + ver77 + "mW");
                    }
                    else if (p <= 99.99)
                    {
                        ver77 = p.ToString("00.00");
                        label19.Text = ("Potencia generada: " + ver77 + "mW");
                    }
                    else if (p <= 999.99)
                    {
                        ver77 = p.ToString("000.00");
                        label19.Text = ("Potencia generada: " + ver77 + "mW");
                    }
                }

                if (pprom >= 1000.0)
                {
                    double temporal77;
                    temporal77 = (pprom * 0.001);
                    if (pprom <= 9999.99)
                    {
                        ver77 = temporal77.ToString("0.00");
                        label38.Text = ("Potencia promedio generada: " + ver77 + "W");
                    }
                    else if (pprom <= 99999.99)
                    {
                        ver77 = temporal77.ToString("00.00");
                        label38.Text = ("Potencia promedio generada: " + ver77 + "W");
                    }
                }
                else if (pprom < 1000.0)
                {
                    if (pprom <= 9.99)
                    {
                        ver77 = pprom.ToString("0.00");
                        label38.Text = ("Potencia promedio generada: " + ver77 + "mW");
                    }
                    else if (pprom <= 99.99)
                    {
                        ver77 = pprom.ToString("00.00");
                        label38.Text = ("Potencia promedio generada: " + ver77 + "mW");
                    }
                    else if(pprom <= 999.99)
                    {
                        ver77 = pprom.ToString("000.00");
                        label38.Text = ("Potencia promedio generada: " + ver77 + "mW");
                    }
                }
                //ver4 = Convert.ToString(bat);
                label26.Text=(bat+"%");

                if (configr == 0 || configr == 1)
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dt1xr; //ajuste automatico  de rango
                    chart2.ChartAreas["ChartArea1"].AxisX.Maximum = dt1xr; //ajuste automatico  de rango
                }

                if(pc==0)
                {
                    i++;
                    chart1.Series["P vs t"].Points.AddXY(dt1xr, i);
                    chart2.Series["V vs t"].Points.AddXY(dt1xr, i);
                }
                else if(pc==1)
                {
                    chart1.Series["P vs t"].Points.AddXY(dt1xr, p);
                    chart2.Series["V vs t"].Points.AddXY(dt1xr, v);
                }


                if (dt1xr == dt2xo)//Captura automatica de graficas
                {
                    rutasinfech = ruta;
                    texto2 = Convert.ToString(anoxr);
                    texto3 = Convert.ToString(mesxr);
                    texto4 = Convert.ToString(diaxr);
                    ruta = (ruta + "\\" + texto4 + "." + texto3 + "." + texto2);
                    b = Convert.ToString(j);
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                        chart1.Size = new Size(944, 271);
                        chart2.Size = new Size(944, 271);
                        this.chart1.SaveImage(ruta + "\\" + "GraficaPvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        this.chart2.SaveImage(ruta + "\\" + "GraficaVvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        j++;
                    }
                    else if (Directory.Exists(ruta))
                    {
                        chart1.Size = new Size(944, 271);
                        chart2.Size = new Size(944, 271);
                        this.chart1.SaveImage(ruta + "\\" + "GraficaPvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        this.chart2.SaveImage(ruta + "\\" + "GraficaVvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        j++;
                    }
                    ruta =rutasinfech;
                    chart1.Size = new Size(465, 226);
                    chart2.Size = new Size(465, 226);
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentarm;
                }
                else
                {
                    Application.Restart();
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                horita = Int64.Parse(DateTime.Now.ToString("hh"));
                horaxr = Int64.Parse(DateTime.Now.ToString("HH"));
                minxr = Int64.Parse(DateTime.Now.ToString("mm"));
                segxr = Int64.Parse(DateTime.Now.ToString("ss"));

                //Conversion de minutos a hora
                horamincam = horaxr + ((minxr) / (60.0)) + (segxr / (3600.0));

                ap = DateTime.Now.ToString("tt");

                minutos = minxr.ToString("00");
                horamos = horita.ToString("00");
                label34.Text = (horamos + ":" + minutos);
                label35.Text = (ap);

                if (horamincam >= 23.999)//Esto ocurre si cambia de dias
                {
                    timer1.Enabled = false;
                    pase = 1;
                    //Por ultimo cambio de año ver como se modifica todo
                }
                else if ((horamincam < 23.999) && (pase == 1))
                {
                    pase = 0;
                    j = 0;
                    anox = Int64.Parse(DateTime.Now.ToString("yyyy"));
                    mesx = Int64.Parse(DateTime.Now.ToString("MM"));
                    diax = Int64.Parse(DateTime.Now.ToString("dd"));
                    horax = Int64.Parse(DateTime.Now.ToString("HH"));
                    minx = Int64.Parse(DateTime.Now.ToString("mm"));
                    segx = Int64.Parse(DateTime.Now.ToString("ss"));
                    ye = 4;
                    inicio();
                }

                if((ye==0 || ye==2) && (horamin < 23.99))
                {
                    //Detección de cambio de hora para sugerir reinicio
                    horaxr = Int64.Parse(DateTime.Now.ToString("HH"));
                    minxr = Int64.Parse(DateTime.Now.ToString("mm"));
                    segxr = Int64.Parse(DateTime.Now.ToString("ss"));
                    if (((horasug - (1.0 / 60.0)) > (horaxr + (minxr / 60.0) + (segxr / 3600.0))) || 
                        ((horasug + (1.0 / 60.0)) < (horaxr + (minxr / 60.0) + (segxr / 3600.0))) || 
                        sug == 1 || diax!=diaxr || mesx != mesxr || anox != anoxr)
                    {
                        //Mostrar sugerencia para reiniciar
                        sug = 1;
                        label25.Visible = true;
                    }
                }

                desconect.desconect desof = elementHost4.Child as desconect.desconect;
                if (desof.des == 1)//Boton desconectar
                {
                    desof.des = 0;
                    scn = 1;
                    serialPort1.Close();
                    MessageBox.Show("Desconectado con exito!", "Información" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pc = 0;
                    Application.Restart();//Es temporal debe ser una grafica sin marcar descontinuada
                }

                usercon.usercon res = elementHost1.Child as usercon.usercon;
                if (res.re == 1)//Boton conectar
                {
                    res.re = 0;
                    Application.Restart();
                }

                decora2.decora2 btn = elementHost3.Child as decora2.decora2;
                if (btn.bt == 1)//Boton cambiar lat y long
                {
                    intentolat1:
                    try
                    {
                        btn.bt = 0;
                        Form4 modilatlong = new Form4(latif, latidecif, longitf, longitdecif);
                        modilatlong.contrato = this;
                        modilatlong.ShowDialog();
                    }
                    catch
                    {
                        p = 0.0;
                        v = 0.0;
                        DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (result == DialogResult.Retry)
                        {
                            goto intentolat1;
                        }
                        else
                        {
                            Application.Restart();
                        }
                    }
                }

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
                }
                else if (tx44 < -90)
                {
                    tx44 = -90;
                    tx4 = Convert.ToString(tx44);
                    textBox4.Text = tx4;
                }
            }
            catch
            {

            }
        }

        private void porDefectoToolStripMenuItem_Click(object sender, EventArgs e)//Modo rango por defecto, desde hora de encendido de software hasta oscurecer el dia 
        {
            intentodefecto:
            try
            {
                confi = 0;
                per = 0;
                if (configr == 1 || configr == 2)
                {
                    configr = 0;
                    porDefectoToolStripMenuItem.Checked = true;
                    amanecerAtardecerToolStripMenuItem.Checked = false;
                    personalizadoToolStripMenuItem.Checked = false;
                }

                if (dt1x > dt2xo || dt1xr > dt2xo)
                {
                    nocap = 1;
                    chart1.Enabled = false;
                    chart1.Visible = false;
                    chart2.Enabled = false;
                    chart2.Visible = false;
                    label2.Text = "No disponible, ¡Es de noche espera el proximo dia!";
                    label2.Visible = true;
                }
                else if (dt1x <= dt2xo || dt1xr <= dt2xo)
                {
                    nocap = 0;
                    chart1.Enabled = true;
                    chart1.Visible = true;
                    chart2.Enabled = true;
                    chart2.Visible = true;
                    label2.Visible = false;
                }
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dt1x;  //Capta los datos XY al abrir el programa
                chart2.ChartAreas["ChartArea1"].AxisX.Minimum = dt1x;  //Capta los datos XY al abrir el programa
                //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dt2xo; //Puesta de sol
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentodefecto;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void amanecerAtardecerToolStripMenuItem_Click(object sender, EventArgs e)//Modo rango desde amanecer hasta oscurecer
        {
            intentoamanece:
            try
            {
                confi = 1;
                per = 0;
                if (configr == 0 || configr == 2)
                {
                    ye = 2;
                    configr = 1;
                    porDefectoToolStripMenuItem.Checked = false;
                    amanecerAtardecerToolStripMenuItem.Checked = true;
                    personalizadoToolStripMenuItem.Checked = false;
                }
                ax = anox;
                c = 0;
                while (2012.0 <= ax)
                {
                    ax = ax - 4.0;
                    c = c + 1; //contador de años biciestos
                }
                ax = anox - 2012.0; //Diferencia de años
                dt2xa = (365.0 * ax) + dtx + c; //"c" es los dias faltantes para sumar a dias de diferencia total a sumar con 40909
                dt2xa = dt2xa + 40909.0; //este valor va en minium AxisX.Minimum
                dt2xo = dt2xa;

                horaminsx = hora1 + ((min1) / (60.0)); //conversion de minutos a hora y sumar todo
                dt2xa = ((horaminsx) / (24.0)) + dt2xa;

                horaminsx = hora2 + ((min2) / (60.0));
                dt2xo = ((horaminsx) / (24.0)) + dt2xo;

                if (((dt1x < dt2xa) || (dt1x > dt2xo)) || ((dt1xr < dt2xa) || (dt1xr > dt2xo)))
                {
                    nocap = 1;
                    chart1.Enabled = false;
                    chart1.Visible = false;
                    chart2.Enabled = false;
                    chart2.Visible = false;
                    label2.Text = "No disponible, ¡Aun no amanece!";
                    label2.Visible = true;
                }
                else if (dt1x >= dt2xa || dt1xr >= dt2xa)
                {
                    nocap = 0;
                    chart1.Enabled = true;
                    chart1.Visible = true;
                    chart2.Enabled = true;
                    chart2.Visible = true;
                    label2.Visible = false;
                }
                chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dt2xa;  //Capta los datos XY desde amanecer
                chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chart2.ChartAreas["ChartArea1"].AxisX.Minimum = dt2xa;  //Capta los datos XY desde amanecer
                //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dt2xo;  //Puesta de sol
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoamanece;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void personalizadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentoperson:
            try
            {
                if (configr == 0 || configr == 1)
                {
                    ye = 2;
                    configr = 2;
                    porDefectoToolStripMenuItem.Checked = false;
                    amanecerAtardecerToolStripMenuItem.Checked = false;
                    personalizadoToolStripMenuItem.Checked = true;
                }
                string horaxst, minxst, texhor, texmin;

                texhor = Convert.ToString(horao);
                texmin = Convert.ToString(mino);
                horaxst = Convert.ToString(horax);
                minxst = Convert.ToString(minx);

                if (persis==0)
                {
                    Form3 persona = new Form3(horaxst, minxst, texhor, texmin);
                    persona.contrato = this;
                    persona.ShowDialog();
                }
                else
                {
                    horaxst = Convert.ToString(horaa);
                    minxst = Convert.ToString(mina);
                    Form3 persona = new Form3(horaxst, minxst, texhor, texmin);
                    persona.contrato = this;
                    persona.ShowDialog();
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoperson;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void autoactivadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentoauto:
            try
            {
                if (autog == 1)
                {
                    autog = 0;
                    autoactivadoToolStripMenuItem.Text = "Automatico (desactivado)";
                }
                else if (autog == 0)
                {
                    autog = 1;
                    autoactivadoToolStripMenuItem.Text = "Automatico (activado)";
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoauto;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void ayudaYSoporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentoayuda:
            try
            {
                Form2 ayuda = new Form2();
                ayuda.ShowDialog();
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoayuda;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void modificarLatatitudYLongitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentolat:
            try
            {
                Form4 modilatlong = new Form4(latif, latidecif, longitf, longitdecif);
                modilatlong.contrato = this;
                modilatlong.ShowDialog();
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentolat;
                }
                else
                {
                    Application.Restart();
                }
            }
        }
        public void ejecutar(string texto1, string texto2, string texto3, string texto4)
        {
            try
            {
                longit = Convert.ToDouble(texto1);
                longitf = texto1;
                longitdecif = texto2;
                longitdeci = Convert.ToDouble(texto2);
                lati = Convert.ToDouble(texto4);
                latif = texto4;
                latidecif = texto3;
                latideci = Convert.ToDouble(texto3);
                if(lati >=0.0)
                {
                    lati = lati + (latideci / 100.0);
                }
                else if(lati < 0.0)
                {
                    lati = lati - (latideci / 100.0);
                }
                if (longit >= 0.0)
                {
                    longit = longit + (longitdeci / 100.0);
                }
                else if (longit < 0.0)
                {
                    longit = longit - (longitdeci / 100.0);
                }
                
                texto2 = longitdeci.ToString("00");
                texto3 = lati.ToString("00");
                label17.Text = ("Longitud actual:");
                label28.Text = (texto1 + "°" + texto2 + "'");
                label20.Text = ("Latitud actual:");
                label27.Text = (texto4 + "°" + texto3 + "'");
                //Guardar datos de la latitud y longitud 
                latistring = Convert.ToString(lati);
                longitstring = Convert.ToString(longit);

                StreamWriter guardar = new StreamWriter(path);//ojo debe ser la misma ruta q el instalado
                guardar.WriteLine(latistring); //*
                guardar.WriteLine(longitstring);//*
                guardar.WriteLine(signature);
                guardar.WriteLine(modelNo);
                guardar.WriteLine(ruta);
                guardar.Close();
            }
            catch
            {
                MessageBox.Show("¡Error en el llenado de los campos!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            inicio();
        }
        public void ejecutar3(string texto1, string texto2, string texto3, string texto4)
        {
            try
            {
                persis = 1;
                mino = Convert.ToDouble(texto4);//Todo esto es para el rango variable
                horao = Convert.ToDouble(texto3);
                mina = Convert.ToDouble(texto2);
                horaa = Convert.ToDouble(texto1);

                horaa1 = horaa + (mina / 60.0);
                horao1 = horao + (mino / 60.0);

                per = 1;

                if (horao1 - horaa1 > 0.5) //convertir rango a horas
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                }
                else if (horao1 - horaa1 <= 0.5) //convertir rango a minutos
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                    chart2.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                }

                ax = anox - 2012.0; //Diferencia de años
                dta = (365.0 * ax) + dtx + c; //"c" es los dias faltantes para sumar a dias de diferencia total a sumar con 40909
                dta = dta + 40909.0; //este valor va en minium AxisX.Minimum
                dto = dta;

                horaminsx = horaa + ((mina) / (60.0)); //conversion de minutos a hora y sumar todo
                dta = ((horaminsx) / (24.0)) + dta;

                horaminsx = horao + ((mino) / (60.0));
                dto = ((horaminsx) / (24.0)) + dto;

                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = dta;  //Capta los datos XY al abrir el programa
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = dto; //Puesta de sol
                chart2.ChartAreas["ChartArea1"].AxisX.Minimum = dta;  //Capta los datos XY al abrir el programa
                chart2.ChartAreas["ChartArea1"].AxisX.Maximum = dto; //Puesta de sol
            }
            catch
            {
                MessageBox.Show("¡Error en el llenado de los campos!");
            }
        }
        public void ejecutar5(string texto1g)
        {
            intentoguar:
            try
            {
                texto2 = Convert.ToString(anoxr);
                texto3 = Convert.ToString(mesxr);
                texto4 = Convert.ToString(diaxr);
                rutaa = ruta;
                ruta = texto1g;
                rutasinfech = ruta;
                ruta = (ruta + "\\" + texto4 + "." + texto3 + "." + texto2);
                intentar:
                try
                {
                    if (!Directory.Exists(ruta))
                    {
                        DialogResult result = MessageBox.Show("La carpeta no existe ¿desea crearla?", "Carpeta no existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Yes)
                        {
                            texto2 = Convert.ToString(anoxr);
                            texto3 = Convert.ToString(mesxr);
                            texto4 = Convert.ToString(diaxr);
                            b = Convert.ToString(j);

                            Directory.CreateDirectory(ruta);
                            chart1.Size = new Size(944, 271);
                            chart2.Size = new Size(944, 271);
                            //this.chart1.SaveImage(ruta + "\\" + texto4 + "." + texto3 + "." + texto2 + "\\" + "GraficaPvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                            this.chart1.SaveImage(ruta + "\\" + "GraficaPvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                            this.chart2.SaveImage(ruta + "\\" + "GraficaVvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                            chart1.Size = new Size(465, 226);
                            chart2.Size = new Size(465, 226);
                            MessageBox.Show("Las graficas han sido guardadas con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            j++;
                        }
                        else if (result == DialogResult.No)
                        {
                            ruta = rutaa;
                        }
                    }
                    else if (Directory.Exists(ruta))
                    {
                        texto2 = Convert.ToString(anoxr);
                        texto3 = Convert.ToString(mesxr);
                        texto4 = Convert.ToString(diaxr);
                        b = Convert.ToString(j);

                        chart1.Size = new Size(944, 271);
                        chart2.Size = new Size(944, 271);
                        this.chart1.SaveImage(ruta + "\\" + "GraficaPvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        this.chart2.SaveImage(ruta + "\\" + "GraficaVvst" + b + ".Jpeg", ChartImageFormat.Jpeg);
                        chart1.Size = new Size(465, 226);
                        chart2.Size = new Size(465, 226);
                        MessageBox.Show("Las graficas han sido guardadas con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        j++;
                    }
                    ruta = rutasinfech;
                }
                catch
                {
                    DialogResult result = MessageBox.Show("No se pudo guardar la grafica, error de directorio o disponibilidad de la grafica.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Retry)
                    {
                        goto intentar;
                    }
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoguar;
                }
                else
                {
                    Application.Restart();
                }
            }
        }
        public void ejecutar6(string texto1a)//Actualizar ruta de guardado graficas
        {
            intentoactual:
            try
            {
                rutaa = ruta; //Guardar ruta anterior
                ruta = texto1a;
                rutasinfech = ruta;
                texto2 = Convert.ToString(anoxr);
                texto3 = Convert.ToString(mesxr);
                texto4 = Convert.ToString(diaxr);
                ruta = (ruta + "\\" + texto4 + "." + texto3 + "." + texto2);
                intentar1:
                try
                {
                    if (!Directory.Exists(ruta))
                    {
                        DialogResult result = MessageBox.Show("La carpeta no existe, ¿desea crearla?", "Carpeta no existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Yes)
                        {
                            Directory.CreateDirectory(ruta);
                            MessageBox.Show("Carpeta y dirección creadas con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ruta = rutasinfech;
                        }
                        else if (result == DialogResult.No)
                        {
                            ruta = rutaa;
                        }
                    }
                    else if (Directory.Exists(ruta))
                    {
                        ruta = rutasinfech; //Para que no se vea la carpeta de fecha
                        MessageBox.Show("Dirección actualizada con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //Guardar datos de ruta
                    latistring = Convert.ToString(lati);
                    longitstring = Convert.ToString(longit);

                    StreamWriter guardar = new StreamWriter(path);//ojo debe ser la misma ruta q el instalado
                    guardar.WriteLine(latistring);
                    guardar.WriteLine(longitstring);
                    guardar.WriteLine(signature);
                    guardar.WriteLine(modelNo);
                    guardar.WriteLine(ruta);//*
                    guardar.Close();
                }
                catch
                {
                    DialogResult result = MessageBox.Show("Error en dirección, intente nuevamente", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Retry)
                    {
                        goto intentar1;
                    }
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoactual;
                }
                else
                {
                    Application.Restart();
                }
            }
        }
        private void capturaActualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentocaptu:
            try
            {
                if (nocap == 0)
                {
                    Form5 guardar = new Form5(ruta);
                    guardar.le = 0;
                    guardar.contrato = this;
                    guardar.ShowDialog();
                }
                else
                {
                    DialogResult result = MessageBox.Show("Graficas no disponible, por favor intente luego.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentocaptu;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void modificarRutaDeGuardadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intentoguarut:
            try
            {
                Form5 guardar = new Form5(ruta);
                guardar.le = 1;
                guardar.contrato = this;
                guardar.ShowDialog();
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoguarut;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                validar(this);
                string t1, t2, t3, t4;

                t1 = textBox1.Text;
                t2 = textBox2.Text;
                t3 = textBox3.Text;
                t4 = textBox4.Text;

                longit = Convert.ToDouble(t1);
                longitf = t1;
                longitdecif = t2;
                longitdeci = Convert.ToDouble(t2);
                lati = Convert.ToDouble(t4);
                latif = t4;
                latidecif = t3;
                latideci = Convert.ToDouble(t3);
                if (lati >= 0.0)
                {
                    lati = lati + (latideci / 100.0);
                }
                else if (lati < 0.0)
                {
                    lati = lati - (latideci / 100.0);
                }
                if (longit >= 0.0)
                {
                    longit = longit + (longitdeci / 100.0);
                }
                else if (longit < 0.0)
                {
                    longit = longit - (longitdeci / 100.0);
                }

                t2 = longitdeci.ToString("00");
                t3 = lati.ToString("00");
                label17.Text = ("Longitud actual:");
                label28.Text = (t1 + "°" + t2 + "'");
                label20.Text = ("Latitud actual:");
                label27.Text = (t4 + "°" + t3 + "'");

                latistring = Convert.ToString(lati);
                longitstring = Convert.ToString(longit);

                //Guardar datos del discoduro, latitud, longitud y ruta por defecto
                StreamWriter guardar = new StreamWriter(path);//ojo debe ser la misma ruta q el instalado
                guardar.WriteLine(latistring);
                guardar.WriteLine(longitstring);
                guardar.WriteLine(signature);
                guardar.WriteLine(modelNo);
                guardar.WriteLine(ruta);
                guardar.Close();

                Application.Restart();
            }
            catch
            {
                MessageBox.Show("¡Error en el llenado de los campos!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reiniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)//Boton conectar
        {
            intentoconect:
            try
            {
                //spuerto es un menu desplegable
                //si conecta entonces habilitar timer1, pc=1 y mostrar todo
                serialPort1.BaudRate = 9600;
                serialPort1.DataBits = 8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;
                serialPort1.PortName = spuerto.Text;
                try
                {
                    serialPort1.Open();
                    pc = 1;
                    serialPort1.Write("0");

                    timer3.Enabled = true;
                    menuStrip1.Enabled = true;
                    menuStrip1.Visible = true;
                    this.Size = new Size(984, 554);
                    this.MaximumSize = new Size(984, 554);
                    this.MinimumSize = new Size(984, 554);
                    chart1.Size = new Size(465, 226);
                    chart2.Size = new Size(465, 226);

                    //Verificacion para mostrar o no el boton conectar o desconectar
                    if (pc == 0)//Desconectado
                    {
                        elementHost4.Visible = false;//Ocultar boton desconectar
                        elementHost1.Visible = true;//Mostrar boton conectar
                    }
                    else if (pc == 1)//Conectado
                    {
                        elementHost4.Visible = true;//Mostrar boton desconectar
                        elementHost1.Visible = false;//Ocultar boton conectar
                    }

                    //Ocultar pantalla 2da vez
                    button2.Visible = false;
                    button3.Visible = false;
                    label21.Visible = false;
                    bconectar.Visible = false;
                    spuerto.Visible = false;
                    rconexion.Visible = false;
                    
                    //Mostrar pantalla interfaz
                    label1.Visible = true;
                    label2.Visible = true;
                    label18.Visible = true;
                    label19.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label8.Visible = true;
                    label20.Visible = true;
                    label17.Visible = true;
                    label39.Visible = true;
                    inicio();
                }
                catch (Exception exc)
                {
                    pc = 0;
                    MessageBox.Show(exc.Message.ToString());
                }
            }
            catch
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoconect;
                }
                else
                {
                    Application.Restart();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) //Boton offline
        {
            pc = 0;
            timer1.Enabled = true;
            elementHost4.Visible = false;//Ocultar boton de desconectar
            menuStrip1.Enabled = true;
            menuStrip1.Visible = true;
            this.Size = new Size(984, 554);
            this.MaximumSize = new Size(984, 554);
            this.MinimumSize = new Size(984, 554);
            chart1.Size = new Size(465, 226);
            chart2.Size = new Size(465, 226);

            //Ocultar pantalla 2da vez
            button2.Visible = false;
            button3.Visible = false;
            label21.Visible = false;
            bconectar.Visible = false;
            spuerto.Visible = false;
            rconexion.Visible = false;

            //Mostrar pantalla interfaz
            label1.Visible = true;
            label2.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label8.Visible = true;
            label20.Visible = true;
            label17.Visible = true;
            label39.Visible = true;
            inicio();
        }

        private void bconectar_Click(object sender, EventArgs e)
        {
            spuerto.DataSource = null;
            spuerto.Items.Clear();

            try
            {
                //Esto va para un boton para buscar puertos y lo muestra en un label
                spuerto.DataSource = null;
                spuerto.Items.Clear();

                string[] puertosdisp = SerialPort.GetPortNames();

                foreach (string puertosimple in puertosdisp)
                {
                    spuerto.Items.Add(puertosimple);
                }
                if (spuerto.Items.Count > 0)
                {
                    spuerto.SelectedIndex = 0;
                    rconexion.ForeColor = Color.LimeGreen;
                    rconexion.Text = "Puertos disponibles";
                    button2.Enabled = true;
                }
                else
                {
                    rconexion.ForeColor = Color.Red;
                    rconexion.Text = "No hay puertos disponibles";
                    spuerto.Text = "                    ";
                    button2.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {//Datos recibidos, mandar confirmacion
            intentoconec:
            try 
            {
                if (pc == 1)
                {
                    string lectur = serialPort1.ReadLine();

                    if (w == 0)
                    {
                        double temporal = Convert.ToDouble(lectur);
                        p = ((temporal)*(0.01));

                        if(prome==0)
                        {
                            pprom = p;
                        }

                        if(prome==1)
                        {
                            pprom= (p+pprom)/2.0;
                        }
                        serialPort1.Write("1");
                        w = 1;
                    }
                    else if (w == 1)
                    {
                        double temporal = Convert.ToDouble(lectur);
                        v = ((temporal) * (0.01));

                        if (prome == 0)
                        {
                            vprom = v;
                        }

                        if (prome == 1)
                        {
                            vprom = (v + vprom) / 2.0;
                        }
                        w = 0;
                        prome = 1;
                    }
                    //Invoke(new EventHandler(progressBar1_Click)); podria usarlo para invocar graficas y rayarla
                }
            }
            catch 
            {
                p = 0.0;
                v = 0.0;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    goto intentoconec;
                }
                else
                {
                    Application.Restart();
                }
            }      
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            intentoconec3:
            try
            {
                if ((pc == 1) && (scn == 0))
                {
                    serialPort1.Write("0"); //Solicitar datos V, P y bateria al arduino
                    timer3.Enabled = true;
                }
            }
            catch
            {
                scn = 1;
                p = 0.0;
                v = 0.0;
                timer3.Enabled = false;
                DialogResult result = MessageBox.Show("Error en la fecha o conexión, por favor verificar.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    try
                    {
                        serialPort1.BaudRate = 9600;
                        serialPort1.DataBits = 8;
                        serialPort1.Parity = Parity.None;
                        serialPort1.StopBits = StopBits.One;
                        serialPort1.Handshake = Handshake.None;
                        serialPort1.PortName = spuerto.Text;

                        serialPort1.Open();
                        serialPort1.Write("0");
                        scn = 0;
                        //inicio();
                    }
                    catch
                    {
                        goto intentoconec3;
                    }
                }
                else
                {
                    Application.Restart();
                }
            }
        }
    }
}
