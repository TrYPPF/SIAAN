using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Speech.Recognition;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Management;

namespace SIAAN_1._1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Reconhecer reconhecedor;
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.SessionEnding += Current_SessionEnding;
        }

        void Current_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            if (e.ReasonSessionEnding == ReasonSessionEnding.Shutdown)
            {
                if (MessageBoxResult.No == MessageBox.Show("Deseja mesmo fechar o SIAAN?","Fechar o SIAAN",MessageBoxButton.YesNo,MessageBoxImage.Question))
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                    Application.Current.Shutdown();
                }
            }
        }

       

      
        private void falar_botao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                reconhecedor = new Reconhecer();

                parar_botao.IsEnabled = true;

                falar_botao.IsEnabled = false;
                SpeechRecognitionEngine engine = reconhecedor.engine;

                engine.RecognizeCompleted += engine_RecognizeCompleted;

                engine.SpeechRecognized += engine_SpeechRecognized;

                reconhecedor.escolhas = new string[] {"fale sobre o sistema operacional","cite os planetas do sistema solar","feche o visual studio","gin defina a equivalência massa-energia","ajuda aqui", "desligar a máquina","gin feche o programa","fala teste","ajuda aqui","gin informe o clima" ,"positivo","sim","não","negativo","falas presentes"};

                reconhecedor.gramatica = new GrammarBuilder();

                

                

                reconhecedor.Recognize(RecognizeMode.Multiple);
            }
            catch (InvalidOperationException ee)
            {
                switch (ee.Message)
                {
                    
                    case "Cannot find the requested data item, such as a data key or value.":
                        MessageBox.Show("Um erro ocorreu, talvez você não tenha um dispositivo de gravação conectado. \n\n"+ee.Message,"Erro!",MessageBoxButton.OK,MessageBoxImage.Error);
                        parar_botao.IsEnabled = false;
                falar_botao.IsEnabled = true;
                        break;
                    default:
                        MessageBox.Show("Um erro ocorreu: \n\n" + ee.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                }
                
                
            }
            catch (Exception eee)
            {
                MessageBox.Show("Um erro ocorreu: \n\n" + eee.Message, "Erro...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        int desligarmento=0;
        int iraoppf = 0;
        private int fechar_visual=0;


        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if ((e.Result.Text=="positivo"||e.Result.Text == "sim") && desligarmento == 1)
            {
                falar("Desligando...");
                Process.Start("shutdown", "-f -t 0 -s");
                desligarmento = 0;
            }
            if (desligarmento == 1 && (e.Result.Text == "negativo" || e.Result.Text == "não"))
            {
                falar("Processo de desligamento cancelado!");
                desligarmento = 0;
            }
            if ((e.Result.Text == "positivo" || e.Result.Text == "sim") && fechar_visual == 1)
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
            if (fechar_visual == 1 && (e.Result.Text == "negativo" || e.Result.Text == "não"))
            {
                falar("Processo de fechamento cancelado!");
               fechar_visual = 0;
            }
            if ((e.Result.Text == "positivo" || e.Result.Text == "sim") && iraoppf == 1)
            {
                falar("Abrindo página web!");
                Process.Start("http://www.power-pixel.net/t57094-siaan-comandos-versao-teste");
                iraoppf = 0;
            }
            if (iraoppf == 1 && (e.Result.Text == "negativo" || e.Result.Text == "não"))
            {
                falar("Cancelado!!");
                iraoppf = 0;
            }
            try
            {
                switch (e.Result.Text)
                {
                    case "fale sobre o sistema operacional":
                        StringBuilder eae = new StringBuilder();
                        eae.AppendLine(String.Format("Nome: {0}",Environment.OSVersion));
                        eae.AppendLine(String.Format("Arquitetura: {0} bits", Environment.Is64BitOperatingSystem?"64":"32"));
                        fala.Text += eae;
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

        void engine_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            parar_botao.IsEnabled = false;
            falar_botao.IsEnabled = true;
        }
        private void parar_botao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                reconhecedor.engine.RecognizeAsyncCancel();
                parar_botao.IsEnabled = false;
                falar_botao.IsEnabled = true;
                
            }
            catch (Exception ee)
            {
                MessageBox.Show("Ocorreu um erro, informe o desenvolvedor deste programa: \n" + ee.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void slider_mover(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            porcentagem_vel.Content = Math.Floor(slider.Value) + "%";
        }

        private void falarr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fala.Text))
                {
                    Fala falla = Fala.getInstance(ref falarr);
                    falla.Velocidade = 0;
                    falla.Volume = (int)slider.Value;

                    falla.Falar(fala.Text);
                }
                else
                {
                    MessageBox.Show("Insira um texto!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                
            }

            catch (Exception ee)
            {
                MessageBox.Show("Ocorreu um erro, informe o desenvolvedor deste programa: \n"+ee.Message,"Erro!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void audio_criador_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Na versão de teste, este comando ainda não está disponível!","Informação",MessageBoxButton.OK,MessageBoxImage.Information);
            
            
        }
        

        
        

       
        
        
       
    }
}
