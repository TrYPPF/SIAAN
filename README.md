<b>THE S.I.A.A.N Project [pt-br]</b>
=====
Bom SIAAN é um projeto que eu tenho com o objetivo de criar um reconhecimento de voz, e o programa retornar:

Vou deixar o repositório dele:
https://github.com/TrYPPF/SIAAN

Quem quiser baixar ele como .zip:
https://github.com/TrYPPF/SIAAN/archive/master.zip 

O programa em si está na pasta <i>SIAAN 1.1.1 >> bin >> Debug</i>, arquivo <b>SIAAN 1.1.1.exe</b>, o arquivo <u>Falas.txt</u> é as falas que você deseja que o programa reconheça, na pasta SIAAN 1.1.1 tem um arquivo <u>Acoes.cs</u>, ele faz com que o programa faça determinada ação quando a fala for reconhecida, só não modifique:

<code>
         public string texto {get; set;}
        
        public int desligarmento=0;
        public int iraoppf = 0;
        public int fechar_visual=0;
        public MainWindow janela {get;set;}
        public TextBox retornno {get;set;}
         public Slider slider { get; set; }
</code>
Esses são arquivos necessários, pra o programa fazer a ação quando a voz for reconhecida <b>DE ACORDO COM AS PALAVRAS QUE TEM NO ARQUIVO</b> <u>Falas.txt</u>, modifique dentro do void reconhecer, mais ou menos assim:
<code>
public void reconhecer()
        { try
             {
                 switch (texto)
                 {
case "Hello World": // fala reconhecida, ou sejam quando o usuário disse essa frase
Console.WriteLine("Reconheci sua voz, você disse: Hello World"); //acao do programa
break; 
}
catch(Exception ee)
{

}
}
</code>
