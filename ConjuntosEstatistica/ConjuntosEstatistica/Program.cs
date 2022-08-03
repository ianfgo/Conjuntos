// Programa desenvolvido para minha aula de Estatística Aplicada a Ciências Aeronáuticas
// Ian Francisco Gaiecki Oliveira - PUCRS Ciências Aeronáuticas (1º Semestre)
// O uso desse programa é livre - Open Source

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace estatistica
{
	public class Program
	{
		public static void Main(string[] args)
		{
			bool detalhes = false;
			while (true)
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

				Stopwatch stopwatch = new Stopwatch();

				Console.WriteLine("[1] Digite o Conjunto ou digite \"conjunto\" para que seja analisado o Conjunto salvo em algum arquivo de texto (.txt)");
				Console.WriteLine("[2] Para formular um Conjunto deve ser escrito da seguinte forma \"-1,1,1.5,2.2,5\" (o Conjunto salvo no arquvio de texto também deverá estar escrito dessa forma)");
				Console.WriteLine("[3] Para ativar o modo detalhado dos cálculos, digite: \"detalhes:true\", para desativar, digite: \"detalhes:false\".");

				string answereString = Console.ReadLine().Replace(" ", string.Empty);
				List<double> conjunto = new List<double>();

				if (answereString.ToLower() == "detalhes:true")
				{
					detalhes = true;
					WriteLineColor("Modo detalhado ativado.\n", ConsoleColor.Green);
					continue;
				}
				else if (answereString.ToLower() == "detalhes:false")
				{
					detalhes = false;
					WriteLineColor("Modo detalhado desativado.\n", ConsoleColor.Blue);
					continue;
				}
				if (answereString.Length <= 0) continue;
				var splittedString = answereString.Split(',');

				if (splittedString[0] == "conjunto")
				{
					string path = "conjunto.txt";
					if (!File.Exists(path))
					{
						Console.WriteLine("Não há nenhum arquivo chamado 'conjunto' na pasta onde está sendo rodado o programa -> " + path + "\n\n");
						continue;
					}
					else
					{
						try
						{
							string textString = File.ReadAllText(path);
							var splittedTextString = textString.Replace(" ", string.Empty).Split(',');
							foreach (var item in splittedTextString)
							{
								conjunto.Add(double.Parse(item, System.Globalization.NumberStyles.Any));
							}
						}
						catch (Exception)
						{
							Console.WriteLine(splittedString[0]);
							throw;
						}
					}
				}
				else
				{
					try
					{
						foreach (var item in splittedString)
						{
							conjunto.Add(double.Parse(item, System.Globalization.NumberStyles.Any));
						}
					}
					catch (Exception)
					{
						WriteLineColor(answereString+" -> Comando não existente.\n", ConsoleColor.Red);
						continue;
						throw;
					}
				}

				if (conjunto.Count > 100)
				{
					Console.WriteLine("Há mais de 100 itens no conjunto e não será mostrado -> "+conjunto.Count);
				}
				else
				{
					Console.WriteLine("\nConjutno ->{" + String.Join(", ", conjunto)+"}");
				}
				if (conjunto.Count < 4)
				{
					Console.WriteLine("O Conjunto deve conter no mínimo quatro valores!\n\n");
					continue;
				}

				Console.WriteLine("Aperte ENTER para realizar os calculos do conjunto (para cancelar digite 'c')");
				string a = Console.ReadLine();
				if (a == "c" || a == "C") continue;

				stopwatch.Start();
				MostrarDados(conjunto, "-> ", detalhes);
				stopwatch.Stop();

				Console.WriteLine("\n\nTempo necessário para execução do código: " + stopwatch.ElapsedMilliseconds + "ms.\n\n");

				media = double.NaN;
				mediana = double.NaN;
				variancia = double.NaN;
				varianciaAmostral = double.NaN;
				desvioPadrao = double.NaN;
				desvioPadraoAmostral = double.NaN;
				coeficienteVariacao = double.NaN;
				amplitude = double.NaN;
				amplitudeInterquartilica = double.NaN;
				moda = new List<double>();
				roll = new List<double>();
			}
		}

		public static double media = double.NaN;
		public static double mediana = double.NaN;
		public static double variancia = double.NaN;
		public static double varianciaAmostral = double.NaN;
		public static double desvioPadrao = double.NaN;
		public static double desvioPadraoAmostral = double.NaN;
		public static double coeficienteVariacao = double.NaN;
		public static double amplitude = double.NaN;
		public static double amplitudeInterquartilica = double.NaN;
		public static List<double> roll = new List<double>();
		public static List<double> moda = new List<double>();

		public static void MostrarDados(List<double> conjunto, string preTexto = "", bool detalhes = false)
		{
			List<double> _conjunto = new List<double>(conjunto);

			Console.WriteLine(preTexto + "Tamanho do conjunto: " + _conjunto.Count);
			Console.WriteLine(preTexto + "Média: " + Media(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Mediana: " + Mediana(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Variância: " + Variancia(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Variância Amostral: " + VarianciaAmostral(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Desvio padrão: " + DesvioPadrao(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Desvio padrão amostral: " + DesvioPadraoAmostral(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Coeficiente de Variação: " + CoeficienteVariacao(_conjunto, false, detalhes) + "%");
			Console.WriteLine(preTexto + "Coeficiente de Variação Amostral: " + CoeficienteVariacao(_conjunto, true, detalhes) + "%");
			Console.WriteLine(preTexto + "Amplitude: " + Amplitude(_conjunto, detalhes));
			Console.WriteLine(preTexto + "Amplitude Interquartilica: " + AmplitudeInterquartilica(_conjunto, detalhes));
			var moda = Moda(_conjunto);
			if (moda.Count == _conjunto.Count)
			{
				Console.WriteLine("Moda: Amodal");
			}
			else
			{
				Console.WriteLine(preTexto + "Moda:" + String.Join(", ", Moda(_conjunto)));
			}
		}

		public static double Media(List<double> conjunto, bool detalhes = true)
		{
			double somatorio = 0;
			foreach (double dado in conjunto)
			{
				somatorio += dado;
			}

			if (detalhes == true) WriteLineColor("\n|Cálculo da Média|", ConsoleColor.Yellow);
			if (detalhes == true) Console.WriteLine("**Soma de todos os números: " + somatorio);

			media = somatorio / conjunto.Count;

			return media;
		}

		public static double Mediana(List<double> conjunto, bool detalhes = true)
		{
			if (roll.Count == 0) Roll(conjunto);

			if (detalhes == true) WriteLineColor("\n|Cálculo da Mediana|", ConsoleColor.Yellow);
			if (detalhes == true) MostrarRoll();

			if (roll.Count % 2 != 0)
			{
				return roll[roll.Count / 2];
			}
			else
			{
				int idPrimeiroNumero = (int)(roll.Count / 2.0d);
				int idSegundoNumero = (int)(roll.Count / 2.0d) + 1;

				double primeiroNumero = roll[idPrimeiroNumero - 1];
				double segundoNumero = roll[idSegundoNumero - 1];

				if (detalhes == true) Console.WriteLine("**Primeiro Número -> Posição: "+idPrimeiroNumero+" Valor: "+primeiroNumero);
				if (detalhes == true) Console.WriteLine("**Primeiro Número -> Posição: "+idSegundoNumero+" Valor: "+segundoNumero);
				return (primeiroNumero + segundoNumero) / 2d;
			}
		}

		public static double Mediana(List<double> conjunto, string args, bool detalhes = false)
		{
			List<double> roll = Roll(conjunto, true);

			if (detalhes == true) WriteLineColor("\n|Cálculo da Mediana|", ConsoleColor.Yellow);
			if (detalhes == true) MostrarRoll();

			if (roll.Count % 2 != 0)
			{
				return roll[roll.Count / 2];
			}
			else
			{
				int idPrimeiroNumero = (int)(roll.Count / 2.0d);
				int idSegundoNumero = (int)(roll.Count / 2.0d) + 1;

				double primeiroNumero = roll[idPrimeiroNumero - 1];
				double segundoNumero = roll[idSegundoNumero - 1];

				if (detalhes == true) Console.WriteLine("**Primeiro Número -> Posição: " + idPrimeiroNumero + " Valor: " + primeiroNumero);
				if (detalhes == true) Console.WriteLine("**Primeiro Número -> Posição: " + idSegundoNumero + " Valor: " + segundoNumero);
				return (primeiroNumero + segundoNumero) / 2d;
			}
		}

		public static List<double> Roll(List<double> conjunto, bool derivative = false)
		{
			List<double> novoConjunto = new List<double>();
			List<double> conjuntoAnalisar = new List<double>(conjunto);

			double menorValorAtual = double.MaxValue;

			int valorIndex = 0;
			int index = 0;

			while (conjuntoAnalisar.Count > 0)
			{
				foreach (double dado in conjuntoAnalisar)
				{
					if (dado <= menorValorAtual)
					{
						menorValorAtual = dado;
						valorIndex = index;
					}
					index++;
				}
				novoConjunto.Add(menorValorAtual);
				conjuntoAnalisar.RemoveAt(valorIndex);

				menorValorAtual = int.MaxValue;

				valorIndex = 0;
				index = 0;
			}
			if (derivative == false)
				roll = novoConjunto;
			return novoConjunto;
		}

		public static double Variancia(List<double> conjunto, bool detalhes = true)
		{
			if (detalhes == true) WriteLineColor("\n|Cálculo da Variância|", ConsoleColor.Yellow);
			double somatorio = 0;
			double mediaSomatorio;

			if (media == double.NaN) Media(conjunto);
			double mediaNormal = media;

			foreach (double dado in conjunto)
			{
				somatorio += (double)dado * (double)dado;
			}

			if (detalhes == true) Console.WriteLine("**Soma de todos os dados ao quadrado: "+somatorio);

			mediaSomatorio = somatorio / conjunto.Count;

			if (detalhes == true) Console.WriteLine("**Média da soma de todos os dados: " + mediaSomatorio);

			double quadradoMedia = mediaNormal * mediaNormal;

			if (detalhes == true) Console.WriteLine("**Quadrado da média: " + quadradoMedia);

			variancia = mediaSomatorio - quadradoMedia;
			return variancia;
		}

		public static double VarianciaAmostral(List<double> conjunto, bool detalhes = true)
		{
			if (variancia == double.NaN) Variancia(conjunto);
			if (detalhes == true) WriteLineColor("\n|Cálculo da Variância Amostral|", ConsoleColor.Yellow);

			double nDividido = (double)conjunto.Count / (double)(conjunto.Count - 1);

			if (detalhes == true) Console.WriteLine("**Variância: " + variancia);
			if (detalhes == true) Console.WriteLine("**N dividido por N - 1: " + nDividido);
			varianciaAmostral = variancia * nDividido;
			return varianciaAmostral;
		}

		public static double DesvioPadrao(List<double> conjunto, bool detalhes = true)
		{
			if (detalhes == true) WriteLineColor("\n|Cálculo do Desvio Padrão|", ConsoleColor.Yellow);
			if (variancia == double.NaN) Variancia(conjunto, detalhes);
			if (detalhes == true) Console.WriteLine("**Variância: " + variancia);
			desvioPadrao = Math.Sqrt(variancia);
			return desvioPadrao;
		}

		public static double DesvioPadraoAmostral(List<double> conjunto, bool detalhes = true)
		{
			if (detalhes == true) WriteLineColor("\n|Cálculo do Desvio Padrão Amostral|", ConsoleColor.Yellow);
			if (varianciaAmostral == double.NaN) VarianciaAmostral(conjunto, detalhes);
			if (detalhes == true) Console.WriteLine("**Variância Amostral: " + varianciaAmostral);
			desvioPadraoAmostral = Math.Sqrt(varianciaAmostral);
			return desvioPadraoAmostral;
		}

		public class Frequencia
		{
			public class grupo
			{
				public double valor = 0;
				public int x = 0;
			}

			public List<grupo> grupos = new List<grupo>();

			public int GetIndex(double valor)
			{
				int index = 0;
				int indexToReturn = -1;
				foreach (grupo item in grupos)
				{
					if (item.valor == valor)
					{
						indexToReturn = index;
					}
					index++;
				}
				return indexToReturn;
			}

			public void Definir(List<double> conjunto)
			{
				foreach (double dado in conjunto)
				{
					int dadoIndex = GetIndex(dado);
					if (dadoIndex != -1)
					{
						grupos[dadoIndex].x++;
					}
					else
					{
						grupos.Add(new grupo() { valor = dado, x = 1 });
					}
				}
			}

			public List<double> Moda()
			{
				int maiorAparicao = int.MinValue;
				int idMaior = 0;

				int index = 0;
				foreach (grupo item in grupos)
				{
					if (maiorAparicao < item.x) { maiorAparicao = item.x; idMaior = index; }
					index++;
				}

				List<double> result = new List<double>();
				foreach (grupo item in grupos)
				{
					if (item.x == maiorAparicao)
					{
						if (!result.Contains(item.valor)) result.Add(item.valor);
					}
				}

				moda = result;
				return moda;
			}
		}

		public static List<double> Moda(List<double> conjunto)
		{
			if (moda.Count > 0) return moda;

			Frequencia listaModa = new Frequencia();
			listaModa.Definir(conjunto);

			return listaModa.Moda();
		}

		public static double CoeficienteVariacao(List<double> conjunto, bool amostral, bool detalhes = false)
		{
			if (desvioPadraoAmostral == double.NaN) DesvioPadraoAmostral(conjunto);
			if (desvioPadrao == double.NaN) DesvioPadrao(conjunto);
			if (media == double.NaN) Media(conjunto);

			if (amostral == true)
			{
				if (detalhes == true) WriteLineColor("\n|Cálculo do Coeficiente de Variação Amostral|", ConsoleColor.Yellow);
				if (detalhes == true) Console.WriteLine("**Desvio Padrão Amostral: " + desvioPadraoAmostral);
				if (detalhes == true) Console.WriteLine("**Média: " + media);
				return desvioPadraoAmostral / media * 100;
			}

			if (detalhes == true) WriteLineColor("\n|Cálculo do Coeficiente de Variação|", ConsoleColor.Yellow);
			if (detalhes == true) Console.WriteLine("**Desvio Padrão: " + desvioPadrao);
			if (detalhes == true) Console.WriteLine("**Média: " + media);
			return desvioPadrao / media * 100;
		}

		public static double Amplitude(List<double> conjunto, bool detalhes = true)
		{
			if (detalhes == true) WriteLineColor("\n|Amplitude|", ConsoleColor.Yellow);

			double max = Max(conjunto);
			double min = Min(conjunto);

			if (detalhes == true) Console.WriteLine("**Valor máximo: " + max);
			if (detalhes == true) Console.WriteLine("**Valor mínimo: " + min);
			return max - min;
		}

		public static double AmplitudeInterquartilica(List<double> conjunto, bool detalhes = true)
		{
			if (detalhes == true) WriteLineColor("\n|Cálculo da Amplitude Interquartilica|", ConsoleColor.Yellow);
			if (roll.Count == 0) Roll(conjunto);

			if (detalhes == true) MostrarRoll();

			double Q1;
			double Q3;

			/*
			int primeiroNumeroQ1 = (int)Math.Floor((double)conjunto.Count * 0.25d);
			int segundoNumeroQ1 = (int)Math.Ceiling((double)conjunto.Count * 0.25d);
			int primeiroNumeroQ3 = (int)Math.Floor((double)conjunto.Count * 0.75d);
			int segundoNumeroQ3 = (int)Math.Ceiling((double)conjunto.Count * 0.75d);
			
			
			if (detalhes == true) Console.WriteLine("**Índice do primeiro número Q1: "+primeiroNumeroQ1);
			if (detalhes == true) Console.WriteLine("**Índice do segundo número Q1: "+segundoNumeroQ1);
			if (detalhes == true) Console.WriteLine("**Índice do primeiro número Q3: " + primeiroNumeroQ3);
			if (detalhes == true) Console.WriteLine("**Índice do segundo número Q3: " + segundoNumeroQ3);

			//Q1 = (roll[primeiroNumeroQ1 - 1] + roll[segundoNumeroQ1 - 1]) / 2d;
			//Q3 = (roll[primeiroNumeroQ3 - 1] + roll[segundoNumeroQ3 - 1]) / 2d;
			*/

			List<double> Q1Conjunto = new List<double>(conjunto);
			List<double> Q3Conjunto = new List<double>(conjunto);

			double halfArray = conjunto.Count / 2d;

			int toRemoveQ1 = (int)Math.Floor(halfArray);
			int toRemoveQ3 = (int)Math.Ceiling(halfArray);

			Q1Conjunto.RemoveRange(toRemoveQ1, conjunto.Count - toRemoveQ1);
			Q3Conjunto.RemoveRange(0, conjunto.Count - toRemoveQ3);

			if (detalhes == true)
			{
				//Console.WriteLine("**Índice máximo do conjunto inferior: " + toRemoveQ1);
				//Console.WriteLine("**Quantos itens deve remover do conjunto inferior: " + (conjunto.Count - toRemoveQ1));
				Console.Write("**Conjunto inferior: "); foreach (var item in Q1Conjunto) Console.Write(item + ", ");
				//Console.WriteLine("\n**Índice máximo do conjunto superior: " + toRemoveQ3);
				//Console.WriteLine("**Quantos itens deve remover do conjunto superior: " + (conjunto.Count - toRemoveQ3));
				Console.Write("\n**Conjunto superior: "); foreach (var item in Q3Conjunto) Console.Write(item + ", "); Console.Write("\n");
			}

			Q1 = Mediana(Q1Conjunto, "", false);
			Q3 = Mediana(Q3Conjunto, "", false);

			if (detalhes == true) Console.WriteLine("**Valor de Q1: " + Q1);
			if (detalhes == true) Console.WriteLine("**Valor de Q3: " + Q3);

			return Q3 - Q1;
		}

		public static double Max(List<double> conjunto)
		{
			double maiorNumero = double.MinValue;
			foreach (var item in conjunto)
			{
				if (item > maiorNumero) maiorNumero = item;
			}
			return maiorNumero;
		}
		public static double Min(List<double> conjunto)
		{
			double menorNumero = double.MaxValue;
			foreach (var item in conjunto)
			{
				if (item < menorNumero) menorNumero = item;
			}
			return menorNumero;
		}
		public static void MostrarRoll()
		{
			if (roll.Count > 100)
			{
				Console.WriteLine("**Conjutno em Roll -> Não será mostrado, pois há mais de 100 itens no conjunto");
				return;
			}
			Console.WriteLine("**Conjutno em Roll ->{" + String.Join(", ", roll) + "}");
		}

		private static void WriteLineColor(string text, ConsoleColor color = ConsoleColor.Gray)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}