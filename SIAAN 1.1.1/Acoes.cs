using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SIAAN_1._1;
using System.Windows;

namespace SIAAN_1._1._1
{
    class Acoes
    {
      public string texto {get; set;}

        public int desligarmento=0;
        public int iraoppf = 0;
        public int fechar_visual=0;

        public MainWindow janela {get;set;}
        public TextBox retornno {get;set;}

        public Slider slider { get; set; }
        public void reconhecer()
        {

              if ((texto=="positivo"||texto == "sim") && desligarmento == 1)
             {
                 falar("Desligando...");
                 Process.Start("shutdown", "-f -t 0 -s");
                 desligarmento = 0;
             }
              if (desligarmento == 1 && (texto == "negativo" || texto == "não"))
             {
                 falar("Processo de desligamento cancelado!");
                 desligarmento = 0;
             }
             if ((texto == "positivo" || texto == "sim") && fechar_visual == 1)
             {
                 falar("Fechando...");
                 Process[] pro = Process.GetProcessesByName("devenv");

                 janela.Close();
                 foreach (Process pp in pro)
                 {
                     pp.Kill();
                 }
                 fechar_visual = 0;
             }
             if (fechar_visual == 1 && (texto == "negativo" || texto == "não"))
             {
                 falar("Processo de fechamento cancelado!");
                fechar_visual = 0;
             }
             if ((texto == "positivo" || texto == "sim") && iraoppf == 1)
             {
                 falar("Abrindo página web!");
                 Process.Start("http://www.power-pixel.net/t57094-siaan-comandos-versao-teste");
                 iraoppf = 0;
             }
             if (iraoppf == 1 && (texto == "negativo" || texto == "não"))
             {
                 falar("Cancelado!!");
                 iraoppf = 0;
             }
             try
             {
                 switch (texto)
                 {
                     case "fale sobre o sistema operacional":
                         StringBuilder eae = new StringBuilder();
                         eae.AppendLine(String.Format("Nome: {0}",Environment.OSVersion));
                         eae.AppendLine(String.Format("Arquitetura: {0} bits", Environment.Is64BitOperatingSystem?"64":"32"));
                         retornno.Text += eae;
                         falar("Informações adicionadas a área de texto.");
                         break;
                     case "cite os planetas do sistema solar":
                       
                         falar("Sim senhor, os planetas rochosos são, a partir da distância do Sol: Mercúrio, Vênus, Terra, Marte. E os planetas jupiterianos, os gigantes gasosos: Júpiter, Saturno, Urano e Netuno. Plutão foi rebaixado a planeta-anão em 2006.");
                         break;
                     case "gin defina a equivalência massa-energia":
                         falar("Foi uma teoria de Albert Einstein que massa e energia estão sempre associados, a equação é:  É igual a mc ao quadrado, isto é, a energia em Jaules é igual a massa vezes a luz no vácuo ao quadrado, velocidade da luz no vácuo é 299792458 metros por segundo!");
                         break;
                     case "falas presentes":
                         falar("O senhor deseja que eu liste as falas presentes? Necessito abrir uma págna web!");
                         iraoppf = 1;
                         break;
                     case "ajuda aqui":
                         falar("Caro senhor, meu nome é Gin, este é o projeto SIAAN, ele é um projeto com finalidade de criar uma assitente virtual que reconheça voz e face para facilitar diversos trabalhos, como esta é a versão de teste o Design e funcionalidades são limitadas. Breve o será adicionado mais comandos, outros idiomas e um melhor design. Existem alguns comandos secretos nesse programa, tente descobri-los!");
                         break;
                     case "desligar a máquina":
                         falar("O senhor deseja mesmo desligar a máquina?");
                         desligarmento = 1;
                         break;
                     case "gin que horas são?":
                         falar("Senhor, são: " + DateTime.Now.Hour + " e " + DateTime.Now.Minute + "");

                         break;
                     case "gin feche o programa":
                         falar("Até mais senhor!");
                         janela.Close();
                         break;
                     case "fala teste":
                         falar("Escutei sua voz , tudo funcionando 100%");
                         break;
                     case "gin informe o clima":
                         Funcionalidades ee = new Funcionalidades();
                         ee.obter_Temperatura("http://weather.yahooapis.com/forecastrss?w=455825&u=c", true, true, true);
                         falar(ee.retorno);
                         break;
                     case "feche o visual studio":
                         falar("O senhor deseja mesmo fechar o visual studio forçado?");
                         fechar_visual = 1;

                         break;
                 }
             }
             catch (InvalidOperationException eee)
             {
                 MessageBox.Show("Um erro ocorreu: \n\n" + eee.Message, "Erro...", MessageBoxButton.OK, MessageBoxImage.Error);

             }
             catch (Exception eee)
             {
                 MessageBox.Show("Um erro ocorreu: \n\n" + eee.Message, "Erro...", MessageBoxButton.OK, MessageBoxImage.Error);
             }
                }

        private void falar(string p)
        {
            try
            {
                Fala Speech = Fala.getInstancea();


                Speech.Volume = (int)slider.Value;
                Speech.Velocidade = 0;


                Speech.Falar(p);
            }
            catch (InvalidOperationException e)
            {

                MessageBox.Show("Um erro ocorreu: \n\n" + e.Message, "Erro...", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (Exception eee)
            {
                MessageBox.Show("Um erro ocorreu: \n\n" + eee.Message, "Erro...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        }
    }





