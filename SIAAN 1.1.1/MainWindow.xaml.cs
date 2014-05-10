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
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SIAAN_1._1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Reconhecer reconhecedor;
        string conteudo;
        StringBuilder cons = new StringBuilder();
        
        public MainWindow()
        {
            InitializeComponent();
            
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);

            key.CreateSubKey("SIAAN");
            key = key.OpenSubKey("SIAAN", true);


            if (key.GetValue("Informado") == null)
            {
                key.SetValue("Informado", "ok");
                MessageBox.Show("Baixe os pacotes necessários para o programa funcionar.");
            }

            

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

                try
                {
                    string linha ;
                    StreamReader falas = new StreamReader(Environment.CurrentDirectory + @"\Falas.txt");
                    List<string> ressu = new List<string>();
                    while((linha=falas.ReadLine())!=null){
                       //if(Regex.IsMatch(linha, @"^[a-zA-Z\s]+$")){
ressu.Add(linha);
                       //}
                       
                        
                    }
                    /*if (!ressu.Any())
                    {
                        MessageBox.Show("Por favor, só é permitido no arquivo de falas letras, números e espaços", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);  
                    }*/
                    falas.Close();
                    if (ressu.Any())
                    {
                        reconhecedor.escolhas = ressu.ToArray();
                        reconhecedor.gramatica = new GrammarBuilder();





                        reconhecedor.Recognize(RecognizeMode.Multiple);

                    }
                    else
                    {
                        MessageBox.Show("Não há nada neste arquivo.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        
                    }
                    
                    //Regex.IsMatch(input, @"^[a-zA-Z]+$");
                }
                catch (InvalidOperationException ee)
                {
                    
                    MessageBox.Show("Um erro ocorreu: \n\n" + ee.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileNotFoundException ee)
                {
                    MessageBox.Show("Um erro ocorreu: \n\n" + ee.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //reconhecedor.escolhas = new string[] {"fale sobre o sistema operacional","cite os planetas do sistema solar","feche o visual studio","gin defina a equivalência massa-energia","ajuda aqui", "desligar a máquina","gin feche o programa","fala teste","ajuda aqui","gin informe o clima" ,"positivo","sim","não","negativo","falas presentes"};

               
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

        


        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
           Acoes acoes = new Acoes();
            acoes.janela = janela;
            acoes.retornno = retornno;
            acoes.slider = slider;
            acoes.texto = e.Result.Text;
            acoes.reconhecer();
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

        private void pulae(object sender, RoutedEventArgs e)
        {
            string maria ;
           string oo;
            try
            {
               StreamReader cconteudo  = new StreamReader(Environment.CurrentDirectory + @"\falas.txt");

               while((oo=cconteudo.ReadLine())!=null)
               {
                   cons.AppendLine(oo);
               }
               cconteudo.Close();
            }
               
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            Console.WriteLine(cons.ToString());
            CompileAndRun(cons.ToString());

        }
        static void CompileAndRun(string code)
        {
            var csc = new CSharpCodeProvider();
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "teste.exe", true)
            {
                GenerateExecutable = true
            };
            CompilerResults compiledAssembly = csc.CompileAssemblyFromSource(parameters,
            code);

            var errors = compiledAssembly.Errors
                                         .Cast<CompilerError>()
                                         .ToList();

            if (errors.Any())
            {
                errors.ForEach(Console.WriteLine);
                return;
            }

            Module module = compiledAssembly.CompiledAssembly.GetModules()[0];

            Type mt = null;
            if (module != null)
                mt = module.GetType("Program");

            MethodInfo methInfo = null;
            if (mt != null)
                methInfo = mt.GetMethod("Main");

            if (methInfo != null)
                Console.WriteLine(methInfo.Invoke(null, null));
        }

        private void download_reconhecimento(object sender, RoutedEventArgs e)
        {

            Process.Start("http://download.microsoft.com/download/4/0/D/40D6347A-AFA5-417D-A9BB-173D937BEED4/MSSpeech_SR_pt-BR_TELE.msi.msi");
            
        }

        private void sdk_download(object sender, RoutedEventArgs e)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                Process.Start("http://download.microsoft.com/download/2/2/5/225F4CFA-8B54-41DB-98C1-47F5A300BBF6/x64_MicrosoftSpeechPlatformSDK/MicrosoftSpeechPlatformSDK.msi");
            }
            else
            {
                Process.Start("http://download.microsoft.com/download/2/2/5/225F4CFA-8B54-41DB-98C1-47F5A300BBF6/x86_MicrosoftSpeechPlatformSDK/MicrosoftSpeechPlatformSDK.msi");
            }
        }

        private void sintese_download(object sender, RoutedEventArgs e)
        {
            Process.Start("http://download.microsoft.com/download/4/0/D/40D6347A-AFA5-417D-A9BB-173D937BEED4/MSSpeech_TTS_pt-BR_Heloisa.msi");
        }

        private void runtime_speech(object sender, RoutedEventArgs e)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                Process.Start("http://download.microsoft.com/download/A/6/4/A64012D6-D56F-4E58-85E3-531E56ABC0E6/x64_SpeechPlatformRuntime/SpeechPlatformRuntime.msi");
            }
            else
            {
                Process.Start("http://download.microsoft.com/download/A/6/4/A64012D6-D56F-4E58-85E3-531E56ABC0E6/x86_SpeechPlatformRuntime/SpeechPlatformRuntime.msi");
            }
        }
        

        
        

       
        
        
       
    }
}
