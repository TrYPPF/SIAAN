using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIAAN_1._1._1
{
    class Reconhecer
    {
        public static Reconhecer Inss = null;
        public static object objlock = new object();
        public Button botao;

        public bool running { get; set; }


        public GrammarBuilder gramatica { get; set; }

        public string[] escolhas { get; set; }
        public SpeechRecognitionEngine engine = new SpeechRecognitionEngine();

        public void init()
        {
            engine.SetInputToDefaultAudioDevice();
        }


        public void Recognize(RecognizeMode modo)
        {


            engine.SetInputToDefaultAudioDevice();

            gramatica.Append(new Choices(escolhas));

            engine.LoadGrammar(new Grammar(gramatica));

            engine.RecognizeAsync(modo);









        }
        public static Reconhecer getInstance()
        {
            lock (objlock)
            {
                if (Inss == null)
                {
                    Inss = new Reconhecer();

                }
                return Inss;
            }
        }
        
       
    }
}
