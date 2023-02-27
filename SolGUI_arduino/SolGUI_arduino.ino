/*--------------------------------------------------------------------------------------------------------------------------
                                                     Programa SolGUI 

AUTORES:
ANGEL MELENDEZ
DANIEL RODRIGUEZ
--------------------------------------------------------------------------------------------------------------------------*/
//----------------------------------------------------**LIBRERIAS**---------------------------------------------------------
#include <math.h>
#include <Wire.h>
#include <SPI.h>
#include <Servo.h>
#include <Adafruit_INA219.h>
#include <RTClib.h>//Ajustar fecha y hora con la hora en la que se subio el programa.
#include "BasicStepperDriver.h"

#define VSENSE A0
#define ISENSE A1
#define PWM 5
#define MUESTRAS 200
#define I_MAX 0.4
#define MOTOR_STEPS 200
#define RPM 300
#define MICROSTEPS 1
#define DIR 42 //Pin digital
#define STEP 4 //PWM
#define SLEEP 40 //Pin digital

Adafruit_INA219 ina219; //Nombro una variable resumida del nombre "Adafruit_INA219" a solo "ina219" 
RTC_DS3231 RTC; //Nombro una variable resumida del nombre "RTC_DS3231" a solo "RTC" 
Servo myservo;  // Crea el objeto servo

//--------------------------------------------------------------------------------------------------------------------------
//Declaracion de variables
double p=0.0, v=0.0, hmax=0.0, hora1, min1, seg1,  pasosgrad=0.0;  //Potencia y voltaje
double num, lat, lati = 10.20, longit = -68.43, longi, dt, anox, mesx, diax, horax, minx, segx, horamin;
double horafalt, sdif, s, correclocal, sl, alfa, y, yl, t, z, h, a, entero, decimalx, horaminmax;
int correcdia = 1, unavez=0, k, nomove=1, rew=0, lm=0, verifi=0, conteo=0;
String mostrar;
char c;

int RawValue= 0;
double tensionAmp = 0;
double amperes = 0;
float corriente = 0;
float r_shunt = 1.0;
int valorPWM = 0;

//Motor servo
//int grados = 0; 
int pos = 89;    // Posicion del servo
int  i = 0;
int aux=0;

//Motor paso a paso
volatile unsigned int cuenta = 0;
bool ESTADO = false;

double ini = 4300.0; //Ojo lo estamos tomando de 0° a 180° para 41.5°
double fnal = 13000.0; //13200
double posactual = 0.0, posnueva = 0.0, posanterior=4200.0;
int x=99;

//Variables del medidor
double voltage_V = 0.0,shuntVoltage_mV,busVoltage_V;
double current_mA = 0.0;
double power_mW = 0.0;
double energy_Wh=0.0;
long time_s=0;

//Variables y Constantes de Medicion de Tension
#define V_MAX 7.25
double volts = 0;
double tensionBAT = 0;
float tension = 0;
float divisor = 0.4444;
float v_diodo = 0.5;
float v_shunt = 0;

//--------------------------------------------------------------------------------------------------------------------------

BasicStepperDriver stepper(MOTOR_STEPS, DIR, STEP, SLEEP);

  void setup()
  {
    pinMode(13,OUTPUT);
    pinMode(44,INPUT);
    SREG = (SREG & 0b01111111); //Desabilitar interrupciones
    TIMSK2 = TIMSK2|0b00000001; //Habilita la interrupcion por desbordamiento
    TCCR2B = 0b00000111; //Configura preescala para que FT2 sea de 7812.5Hz
    SREG = (SREG & 0b01111111) | 0b10000000; //Habilitar interrupciones //Desabilitar interrupciones

    pinMode(PWM, OUTPUT);
    valorPWM=30;
    analogWrite(PWM, valorPWM);
    delay(50);
    myservo.attach(3);  // vincula el servo al pin digital 3
    Serial.begin(9600);// inicializamos la comunicación serial
    uint32_t currentFrequency;
    ina219.begin();

    stepper.begin(RPM, MICROSTEPS);
    stepper.disable();
    //pinMode(10,OUTPUT); //definimos el PIN 10 como salida
        
    //Primero obtener datos de la interfaz en caso de estar conectada con esta sino buscar datos en EEPROM
    
    //Segundo obtener fecha y hora para realizar los calculos
    Wire.begin();
    RTC.begin();
    DateTime now = RTC.now();
    //Fecha
    anox=now.year();//Asi se extrae la hora fecha y lo que sea de datos del RTC
    mesx=now.month();
    diax==now.day();
    //Hora
    horax=now.hour();
    minx=now.minute();
    segx=now.second();
  }
  
  ISR(TIMER2_OVF_vect)
  {
      cuenta++;
      if(cuenta > 29) 
      {
        digitalWrite(13,ESTADO);
        ESTADO = !ESTADO;
        cuenta=0;
      }
  }

   void getData()
   { 
   time_s=millis()/(1000); // convert time to sec
   busVoltage_V = ina219.getBusVoltage_V();
   shuntVoltage_mV = ina219.getShuntVoltage_mV();
   voltage_V = busVoltage_V + (shuntVoltage_mV / 1000);
   current_mA = ina219.getCurrent_mA();
   //power_mW = ina219.getPower_mW(); 
   power_mW=current_mA*voltage_V; 
   energy_Wh=(power_mW*time_s)/3600;   //energy in watt hour
    
    //Serial.print("Bus Voltage:   "); Serial.print(busVoltage_V); Serial.println(" V");
    //Serial.print("Shunt Voltage: "); Serial.print(shuntVoltage_mV); Serial.println(" mV");
    //Serial.print("Load Voltage:  "); Serial.print(voltage_V); Serial.println(" V");
    v=voltage_V;
    //Serial.print("Current:       "); Serial.print(current_mA); Serial.println(" mA");
    //Serial.print("Power:         "); Serial.print(power_mW); Serial.println(" mW");
    p=power_mW;
    //Serial.print("Energy:        "); Serial.print(energy_Wh); Serial.println(" mWh");
    //Serial.println("----------------------------------------------------------------------------");
  }  
  
  void latitu()
  {
    num = lati;
  
    if (num >= 0.0)
    {
      entero = int(num); //tomando el numero entero 12.42 -> 12
      decimalx = int((lati - entero) * (100.0));//para que me quede la parte decimal 0.42 -> 42
      lat = entero + (decimalx / 60.0);// convirtiendo a grados
    }
    else if (num < 0.0)
    {
        num = (-num);
        entero = int(num); //tomando el numero entero 12.42 -> 12
        decimalx = int((num - entero) * (100.0));//para que me quede la parte decimal 0.42 -> 42
        lat = entero + (decimalx / 60.0);// convirtiendo a grados
        //entero = (-entero);//Borrar esto
        lat = -lat;
    }
  }
  
  float mideCorrienteCarga()
  {
     amperes = 0;
     for(i=0;i<MUESTRAS;i++){
        RawValue = analogRead(ISENSE);
        tensionAmp = (RawValue / 1023.0) * 5;
        amperes = amperes + tensionAmp;
        delay(1);
     }
     corriente = ((amperes / MUESTRAS)) / r_shunt;
     return corriente;
  }
    
  float mideTensionBat() 
  {
    volts = 0;
    for(i=0;i<MUESTRAS;i++){
      RawValue = analogRead(VSENSE);
      tensionBAT = (RawValue / 1023.0) * 5;
      volts = volts + tensionBAT;
      delay(1);
   }
   tension = (((volts / MUESTRAS) ) / divisor) - v_diodo - v_shunt;
   return tension;
  }
  void ajustaPWMcargador() 
  {
     if(corriente < I_MAX){
        if(tension < V_MAX){
           if(valorPWM<255)
              valorPWM++;
           delay(1);
           analogWrite(PWM, valorPWM); 
        } 
        else{
           if(valorPWM>0)
              valorPWM--;
           delay(1);
           analogWrite(PWM, valorPWM);
        }
     }
  } 
  
  void longitu()
  {
    num = longit;
  
    if (num >= 0.0)
    {
        entero = int(num); //tomando el numero entero 12.42 -> 12
        decimalx = int((longit - entero) * (100.0));//para que me quede la parte decimal 0.42 -> 42
        longi = entero + (decimalx / (60.0));// convirtiendo a grados
    }
    else if (num < 0.0)
    {
        num = (-num);
        entero = int(num); //tomando el numero entero 12.42 -> 12
        decimalx = int((num - entero) * (100.0));//para que me quede la parte decimal 0.42 -> 42
        longi = entero + (decimalx / (60.0));// convirtiendo a grados
        //entero = (-entero);
        longi = (-longi);
    }
  }
  
  void dtfal()
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

  void calresto()
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
    
    alfa = ((360.0 / 365.25) * (dt)) + ((horafalt) * (360.0 / 8766.0)); //1 hora son 15 grados   (360grados/365.25 dias * cantidad de dias trasncurridos)
    alfa = ((alfa * (PI)) / 180.0); //transformando alfa de grados a radianes
    y = ((23.45) * (sin(alfa))); //alfa en radianes "y" es la declinacion esta en grados

    yl = ((23.45) * (sin(alfa))) - lat; //Declinacion local
    
    alfa = ((alfa * (180.0)) / (PI)); //Llevando de radianes a grados
    alfa = ((alfa) / (15.0)); //llevado de grados a horas
    
    t = sl - alfa;
    
    t = (t * 15.0); //llevado a grados
    
    if(t>180)
    {
        t = t - 360;
    }
    
    // Paso 4 altura del sol
    
    //transformando todo a radianes
    
    t = ((t * (PI)) / 180.0); //transformando de grados a radianes
    lat = ((lat * (PI)) / 180.0);
    y = ((y * (PI)) / 180.0);
    
    h = acos(((sin(y)) * (sin(lat))) + ((cos(y)) * (cos(lat)) * (cos(t))));
    z = h;
    // Paso 5 Calculo del azimut
    t = ((t * 180.0) / (PI)); // radianes a grados
    if (t < 0.0) // t negativo
    {
        t = ((t * (PI)) / 180.0); // grados a radianes
        a = acos((((sin(y)) * (cos(lat))) - ((cos(y)) * (sin(lat)) * (cos(t)))) / (sin(z)));
        a = ((a * 180.0) / (PI)); // radianes a grados
    }
    else if (t >= 0.0) // t positivo
    {
        t = ((t * (PI)) / 180.0); // grados a radianes
        a = acos((((-1 * (sin(y))) * (cos(lat))) + ((cos(y)) * (sin(lat)) * (cos(t)))) / (sin(z)));
        a = ((a * 180.0) / (PI)); // radianes a grados
        a = a + 180.0;
    }
    y = ((y * (180.0)) / (PI)); //Llevando de radianes a grados y
    //Esto es parte del paso 4
    h = ((h * 180.0) / (PI)); // radianes a grados
    lat = ((lat * 180.0) / (PI));
    h = (90.0) - h;
  }

  void calcuastro()
  {
    //--------------------------------------------------**CALCULOS ASTRONOMICOS**-------------------------------------------------------------
    //paso1' antes de hacer esto es necesario conocer fecha hora min seg(RTC) y lat y long (Sincronizacion ó memoria)
    longitu();
    latitu();
  
    // Paso 1 Calculo de dias faltantes desde el mediodia del 21 de marzo 
  
    dtfal();  //funcion dias faltantes
    //Función dias faltantes
    
    //conversion de minutos a hora
    horamin = horax + ((minx) / (60.0)) + (segx / (3600.0));
    
    calresto(); //correccion de hora, y demas pasos para calcular altura y azimut
   //------------------------------------------------------------------------------------------------------------------------------------- 
  }
  
  void loop()
  { 
    if((digitalRead(44))==HIGH)
    {
      mideCorrienteCarga();
      mideTensionBat();
      ajustaPWMcargador();
      digitalWrite(13,HIGH);
      getData();
      DateTime now = RTC.now();
      //Fecha
      anox=now.year();//Asi se extrae la hora fecha y lo que sea de datos del RTC
      mesx=now.month();
      diax=now.day();
      //Hora
      horax=now.hour();
      minx=now.minute();
      segx=now.second(); 

      longitu();
      latitu();
      
      //Algoritmo para determinar altura maxima
      if(unavez==0)
      {      
        //Calculo de altura maxima 
        min1 = 0.0;
        hora1 = 12.0;
        seg1 = 0.0;
        h = 0.0;
        for (k = 1; k < 5000; k++)
        {         
            if (seg1 == 60.0)
            {
                seg1 = 0.0;
                min1 = min1 + 1.0;
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
            horamin = hora1 + ((min1) / (60.0)) + ((seg1) / (3600.0));
    
            calresto(); //correccion de hora, y demas pasos para calcular altura y azimut
            //-----------------------------------------------------------------------------
            if (h >= hmax )
            {
              hmax = h;
            }
            else if (h < hmax )
            {
              break;
            }
            seg1++;
        }
        horaminmax = hora1 + ((min1) / (60.0)) + ((seg1) / (3600.0));
      }
      
      calcuastro();
  //------------------------------------------------------------------------------------------------------------------------------------- 
      if(h>=0.0)
      {     
        //Interpolacion para obtener el equivalente en grados de hmax para el motor paso a paso
        if(horaminmax>=horamin)
        {
          posnueva = (((h-0.0)/(hmax-0.0))*(90.0-0.0)) + 0.0; //Si es positivo empieza por el este
        }
        else if(horaminmax<horamin)
        {
          posnueva = (((180.0-90.0)/(0.0-hmax))*(h-hmax)) + 90.0; //Si es negativo cae por el oeste
        }

        posnueva=int(posnueva*100.0);
        
        if(posnueva>posanterior)
        { 
          posanterior=int(posanterior);
          pasosgrad = int(pasosgrad + posnueva - posanterior);
          pasosgrad=(pasosgrad/100.0);
          /*else if(posnueva<posanterior)
          {
            pasosgrad = pasosgrad + posanterior - posnueva; //en este caso seria hacia el este pero no ocurriria 
          }*/
          if((pasosgrad>=0.25) && (nomove==0))
          {
            for(rew=1;rew<800;rew++)
            {
              conteo++;
              lm++;
              pasosgrad=pasosgrad-0.25;
              if(pasosgrad<0.25)
              {
                break;
              }
            }
            posanterior = posnueva;
          }
        }
    
        if((fnal>=posnueva) && (h>=1.0))//Esto es para habilitar el motor moverse al inicio
        {
          if(ini<posnueva)
          {
            nomove=0;
          }
        }
        if(fnal<posnueva)//Esto es para que se detenga antes de llegar al final deshabilitar motor paso a paso y regrese a inicial (43°)
        {
          //verifi++;
          if(nomove==1 || nomove==0)
          {
            nomove=1;
          }

          if(((h<=2.0) && (nomove==1) && (h>=0.0)) || (x==6)) //Cuando la altura h tenga un valor por debajo de 0.5 devolver a su posicion original en el Este, hasta esperar
          {/*
            if(yl<0.0)
            {
              aux=-yl;
              for(i=0;i<aux;i++)
              {
                pos=pos-i-1;
                myservo.write(pos);
                delay(70);
              }
            }
            else if(yl>=0.0)
            {
              for(i=0;i<yl;i++)
              {
                pos=pos+i+1;
                myservo.write(pos);
                delay(70);
              }
            }*/
            for(rew=0;conteo>rew;rew++)
            {
              //verifi++;
              stepper.enable();
              //delay(50);
              stepper.move((-615)*(1)); //Aqui va variable pasos   -MOTOR_STEPS*MICROSTEPS
              //Serial.println("1° grado al Este");
              //delay(50);
              stepper.disable();
            }
            conteo=0;
            posanterior=4200.0;
            nomove=2;
            rew=0;
            lm=0;
            x=99;
          }
        }
      }
      /*if()//Esta funcion es para regresarse cuando la altitud sea cercana a cero
      {
        
      }*/
  //------------------------------------------------------------------------------------------------------------------------------------- 
     if(unavez==0)
     {
        //Funcion para convertir grados de declinacion local a grados para Motor servo
        /*if(yl>=0.0)
        {
          for(i=0;i<yl;i++)
          {
            pos=pos-i-1;
            myservo.write(pos);
            delay(70);       
          }
        }
        else */
        if(yl<0.0)
        {
          aux = int(-yl);
          for(i=0;aux+38>i;i++)
          {
            pos=pos+1;
            myservo.write(pos);
            delay(70);
          }
        }
        unavez=1;
     }
      
      /*mostrar = (String) pos;
      Serial.println(mostrar);
      mostrar = (String) posnueva;
      Serial.println(mostrar);
      delay(200);
      mostrar = (String) fnal;
      Serial.println(mostrar);
      mostrar = (String) x;
      Serial.println(mostrar);*/
  
       if((lm >= 1) && (nomove==0))
       {
        if(lm>=30)
        {
          for(rew=0;lm>rew;rew++)
          {
            verifi++;
            stepper.enable();
            //delay(50);
            stepper.move((615)*(1)); //Aqui va variable pasos   -MOTOR_STEPS*MICROSTEPS
            //Serial.println("5° grados a la derecha");
            //delay(50);
            stepper.disable();
          }
          rew=0;
          lm=0;
          x=99;
        }
        else
        {
          stepper.enable();
          //delay(50);
          /*if(lm==0)
          {
            lm=1;
          }*/
          stepper.move((615)*(lm)); //Aqui va variable pasos   -MOTOR_STEPS*MICROSTEPS
          //Serial.println("5° grados a la derecha");
          //delay(50);
          stepper.disable();
          rew=0;
          lm=0;
          x=99;
        }
       }
    
       /*if(Serial.available())
       {
         String in_char = Serial.readStringUntil('\n'); // read until the newline
         Serial.println("recibido");
         //Serial.print(in_char);
         x = in_char.toInt();
  
         if(x==2)
         {
           //for (i = pos; i < grados; i += 1) 
           //{
             myservo.write(pos+i);              
             //delay(50); 
           //}
           //pos = i;
           i++;
           x=99;
         }
         else if(x==9)
         {
           //for (i = pos; i > grados; i -= 1) 
           //{
             myservo.write(pos+i);              
             //delay(50); 
           //}
           //pos = i;
           i--;
           x=99;
         }
       }*/
      
       if(Serial.available()) //Si la comunicacion serial es utilizable, pregunta aqui.
       {
          char c=Serial.read(); // Se lee el monitor serial y esperando solo un caracter
          // se almacena en una variable tipo char que llamamos c 
              
          if(c=='0') //pregunta por el contenido de la variable c
          {
            //digitalWrite(10,HIGH); // coloca en ALTO la salida digital PIN 10
            mostrar = (String) p;
            Serial.println(mostrar); 
            c=0x99;
            //delay(250); //Los retardos son necesarios en la practica para mejorar desempeño
          }
          if(c=='1') //pregunta por el contenido de la variable c
          {
            //digitalWrite(10,LOW);
            mostrar = (String) v;
            Serial.println(mostrar); 
            c=0x99;
            //delay(250); //Los retardos son necesarios en la practica para mejorar desempeño
          }
       }
    }
    else
    {
      digitalWrite(13,LOW);
    }
  }
