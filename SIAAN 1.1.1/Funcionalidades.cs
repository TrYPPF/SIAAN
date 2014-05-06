using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Management;

namespace SIAAN_1._1._1
{
    class Funcionalidades
    {
        string temperatura_;
        string vento_;
        string umidade_;

       public  string retorno { get; set; }
        public void obter_Temperatura(string uri, bool temperatura, bool vento, bool umidade)
        {
            
                XmlTextReader reader = new XmlTextReader(uri);
            try{
                while (reader.Read() && reader.LineNumber < 278)
                {
                    if (temperatura && reader.Name == "yweather:condition")
                    {
                        temperatura_ = reader.GetAttribute("temp");
                    }
                    if (vento && reader.Name == "yweather:wind")
                    {
                        vento_ = reader.GetAttribute("speed");
                    }
                    if (umidade && reader.Name == "yweather:atmosphere")
                    {
                        umidade_ = reader.GetAttribute("humidity");
                    }
                }
                this.retorno = "Senhor, peguei os climas do Rio de Janeiro, temperatura: " + temperatura_ + " graus Celsius" + ", o vento está a " + vento_ + " quilômetros por hora, e a umidade está " + umidade_ + " por cento!";
            }
            catch (Exception ee)
            {
                switch (ee.Message)
                {
                    case "The remote name could not be resolved: 'weather.yahooapis.com'":
                        this.retorno = "Senhor, não pude me conectar aos servidores do Yahoo!";
                        break;
                    default:
                        this.retorno = "Senhor, houve um erro ao pegar o clima. ";
                System.Windows.MessageBox.Show("Um erro ocorreu: \n" + ee.Message, "Erro", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        break;
                }
                
            }
            
        }
        public void informacoesdosistema()
        {
            
        }
    }
}
