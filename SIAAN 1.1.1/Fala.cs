using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIAAN_1._1._1
{
    class Fala
    {
        
            private static Fala GerenciaInstancia = null;
            private static object objLock = new object();
            private static Button refBotao = null;

            public int Volume { get; set; }
            public int Velocidade { get; set; }

            SpeechSynthesizer SpeechEngine = new SpeechSynthesizer();

            public void Falar(string sTr)
            {
                SpeechEngine.Rate = this.Velocidade;
                SpeechEngine.Volume = this.Volume;



                SpeechEngine.StateChanged += SpeechEngine_StateChanged;
                if (!string.IsNullOrWhiteSpace(sTr))
                {
                    switch (SpeechEngine.State)
                    {
                        case SynthesizerState.Ready:
                            SpeechEngine.SetOutputToDefaultAudioDevice();
                            SpeechEngine.SpeakAsync(sTr);

                            break;
                        case SynthesizerState.Speaking:
                            SpeechEngine.Pause();
                            break;
                        case SynthesizerState.Paused:
                            SpeechEngine.Resume();
                            break;
                    }
                }
            }
            public void arquivo_audio(string sTr, string path)
            {
                SpeechEngine.Rate = this.Velocidade;
                SpeechEngine.Volume = this.Volume;



                SpeechEngine.StateChanged += SpeechEngine_StateChanged;
                if (!string.IsNullOrWhiteSpace(sTr))
                {
                    switch (SpeechEngine.State)
                    {
                        case SynthesizerState.Ready:
                            SpeechEngine.SetOutputToWaveFile(path);

                            break;
                        case SynthesizerState.Speaking:
                            SpeechEngine.Pause();
                            break;
                        case SynthesizerState.Paused:
                            SpeechEngine.Resume();
                            break;
                    }
                }
            }

            void SpeechEngine_StateChanged(object sender, StateChangedEventArgs e)
            {
                if (refBotao != null)
                {
                    switch (e.State)
                    {
                        case SynthesizerState.Ready:


                            refBotao.Content = "Falar";

                            break;

                        case SynthesizerState.Speaking:


                            refBotao.Content = "Pausar";


                            break;

                        case SynthesizerState.Paused:


                            refBotao.Content = "Continuar";

                            break;



                    }
                }
            }
            public static Fala getInstance(ref Button Botao)
            {
                lock (objLock)
                {
                    if (GerenciaInstancia == null)
                    {
                        GerenciaInstancia = new Fala();
                        refBotao = Botao;
                    }
                    return GerenciaInstancia;
                }
            }
            public static Fala getInstancea()
            {
                lock (objLock)
                {
                    GerenciaInstancia = new Fala();


                    return GerenciaInstancia;
                }
            }
            public static void transformanull()
            {
                GerenciaInstancia = null;
            }

            public void falass()
            {
                foreach (InstalledVoice ee in SpeechEngine.GetInstalledVoices())
                {
                    VoiceInfo info = ee.VoiceInfo;
                    MessageBox.Show(info.Name);
                }
            }


        }
    
}
