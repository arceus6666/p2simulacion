using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace p2simulacion {
	class Program {

		static Random RND = new Random();

		private static int[] generate() {
			var nums = new List<int>();
			for(int a = 0; a < 10; a++) {
				int numa = a;
				for(int b = 0; b < 10; b++) {
					if(b == a)
						continue;
					int numb = numa * 10 + b;
					for(int c = 0; c < 10; c++) {
						if(c == a || c == b)
							continue;
						int numc = numb * 10 + c;
						for(int d = 0; d < 10; d++) {
							if(d == a || d == b || d == c)
								continue;
							//Console.WriteLine(numc * 10 + d);
							nums.Add(numc * 10 + d);
						}
					}
				}
			}
			return nums.ToArray();
		}

		static int[] numeros = generate();

		private static int RandomNumber() {
			int rnd = RND.Next(numeros.Length);
			//Console.Write("rnd: " + rnd + "; ");
			return numeros[rnd];
		}

		private static string printArray(double[] arr) {
			string res = "\n[\n";
			int l = arr.Length;
			for(int i = 0; i < l - 1; i++) {
				res += "  " + arr[i] + ",\n";
				//res += "\t" +  + ",\n";
			}
			return res + "  " + arr[l - 1] + "\n]\n";
		}

		private static string printArray(int[] arr) {
			string res = "\n[\n";
			int l = arr.Length;
			for(int i = 0; i < l - 1; i++) {
				res += "  " + arr[i] + ",\n";
				//res += "\t" +  + ",\n";
			}
			return res + "  " + arr[l - 1] + "\n]\n";
		}

		private static double[] generateRandomSeries(int size) {
			double[] res = new double[size];
			for(int i = 0; i < size; i++) {
				double n = RandomNumber();
				n = Math.Pow(n, 2);
				string ns = n + "";
				if(ns.Length == 8) {
					ns = "0." + ns.Substring(2, 4);
				} else {
					ns = "0." + ns.Substring(1, 4);
				}
				n = double.Parse(ns);
				res[i] = n;
			}
			return res;
		}

		private static void uniforme(double[] r, double a, double b) {
			int l = r.Length;
			//int sl = 10;
			string[] blanks = {
				"",
				" ",
				"  ",
				"   ",
				"    ",
				"     ",
				"      ",
				"       ",
				"        ",
				"         "
			};
			string res = "+----------+----------+----------+\n";
			res += "| Medición |    ri    |    xi    |\n";
			res += "+----------+----------+----------+\n";
			double xi(double ri) => a + (b - a) * ri;
			for(int i = 0; i < l; i++) {
				string ms = (i + 1) + "";
				string rs = r[i] + "";
				string xs = xi(r[i]) + "";
				res += $"|{ms}{blanks[10 - ms.Length]}|{rs}{blanks[10 - rs.Length]}|{xs}{blanks[10 - xs.Length]}|\n";
			}
			res += "+----------+----------+----------+\n";
			Console.WriteLine(res);
		}

		private static void exponencial(double[] r, double lambda) {
			int l = r.Length;
			//int sl = 10;
			string[] blanks = {
				"",
				" ",
				"  ",
				"   ",
				"    ",
				"     ",
				"      ",
				"       ",
				"        ",
				"         "
			};
			string res = "+----------+----------+----------+\n";
			res += "| Medición |    ri    |    xi    |\n";
			res += "+----------+----------+----------+\n";
			double xi(double ri) => -(1 / lambda) * Math.Log(1 - ri);
			for(int i = 0; i < l; i++) {
				string ms = (i + 1) + "";
				string rs = r[i] + "";
				string xs = xi(r[i]) + "";
				if(xs.Length > 10) {
					xs = xs.Substring(0, 10);
				}
				res += $"|{ms}{blanks[10 - ms.Length]}|{rs}{blanks[10 - rs.Length]}|{xs}{blanks[10 - xs.Length]}|\n";
			}
			res += "+----------+----------+----------+\n";
			Console.WriteLine(res);
		}

		private static void bernoulli(double[] r, double p) {
			int l = r.Length;
			//int sl = 10;
			string[] blanks = {
				"",
				" ",
				"  ",
				"   ",
				"    ",
				"     ",
				"      ",
				"       ",
				"        ",
				"         "
			};
			string res = "+----------+----------+----------+----------+\n";
			res += "| Medición |    ri    |    xi    |  evento  |\n";
			res += "+----------+----------+----------+----------+\n";
			int xi(double ri) => ri < p ? 0 : 1;
			for(int i = 0; i < l; i++) {
				string ms = (i + 1) + "";
				string rs = r[i] + "";
				string xs = xi(r[i]) + "";
				if(xs.Length > 10) {
					xs = xs.Substring(0, 10);
				}
				string xe = xs == "0" ? "No Falla  " : "Falla     ";
				res += $"|{ms}{blanks[10 - ms.Length]}|{rs}{blanks[10 - rs.Length]}|{xs}{blanks[9]}|{xe}|\n";
			}
			res += "+----------+----------+----------+----------+\n";
			Console.WriteLine(res);
		}

		static bool salir = false;

		static double[] numerosr = null;

		private static void menu() {
			//double[] numeros = null;
			Console.WriteLine("Escoja la opción deseada:");
			Console.WriteLine("\ta)\tGenerar serie.");
			Console.WriteLine("\tb)\tMostar serie.");
			Console.WriteLine("\tc)\tPrueba de Distribución Uniforme.");
			Console.WriteLine("\td)\tPrueba de Distribución Exponencial.");
			Console.WriteLine("\te)\tPrueba de Distribución de Bernoulli.");
			Console.WriteLine("\totro)\tSalir.");
			Console.Write("Opción: ");
			switch(Console.ReadLine()) {
				case "a":
					Console.Write("Introduzca el tamaño de la serie: ");
					string rl = Console.ReadLine();
					int size = int.Parse(rl);
					numerosr = generateRandomSeries(size);
					Console.WriteLine("Serie generada");
					break;
				case "b":
					if(numerosr == null) {
						Console.WriteLine("Ninguna serie fue generada.");
					} else {
						Console.WriteLine(printArray(numerosr));
					}
					break;
				case "c":
					if(numerosr == null) {
						Console.WriteLine("Ninguna serie fue generada.");
					} else {
						Console.WriteLine("Introduzca sus límites:");
						Console.Write("a: ");
						int a = int.Parse(Console.ReadLine());
						Console.Write("b: ");
						int b = int.Parse(Console.ReadLine());
						uniforme(numerosr, a, b);
					}
					break;
				case "d":
					if(numerosr == null) {
						Console.WriteLine("Ninguna serie fue generada.");
					} else {
						Console.Write("Introduzca su lambda: ");
						var ui = Console.ReadLine();
						double lm;
						bool isNum = double.TryParse(ui, out lm);
						bool isDiv = ui.Contains("/");

						//double lm = double.Parse(Console.ReadLine());
						//Console.WriteLine(isNum ? "sip" + lm : "nop" + lm);
						if(isNum) {
							exponencial(numerosr, lm);
						} else {
							if(isDiv) {
								var nd = ui.Split('/');
								double n = double.Parse(nd[0]);
								double d = double.Parse(nd[1]);
								lm = n / d;
								exponencial(numerosr, lm);
							} else {
								Console.WriteLine("Valor de lambda inválido");
							}
						}
					}
					break;
				case "e":
					if(numerosr == null) {
						Console.WriteLine("Ninguna serie fue generada.");
					} else {
						Console.Write("Introduzca su p: ");
						double p = double.Parse(Console.ReadLine());
						bernoulli(numerosr, p);
					}
					break;
				case "arc":
					string ai = Console.ReadLine();
					string[] nn = ai.Split(',');
					numerosr = new double[nn.Length];
					for(int i = 0; i < nn.Length; i++) {
						double num = double.Parse(nn[i]);
						numerosr[i] = num;
					}
					break;
				default:
					salir = true;
					break;
			}
			Console.WriteLine("\n");
		}

		static void Main(string[] args) {
			Console.WriteLine("***************************");
			Console.WriteLine("*       Bienvenido        *");
			Console.WriteLine("* Simulación - Práctica 2 *");
			Console.WriteLine("* Daniel Mendoza          *");
			Console.WriteLine("***************************\n");
			while(!salir) {
				menu();
			}
			Console.WriteLine("Adios!");
			System.Threading.Thread.Sleep(700);
		}
	}
}
