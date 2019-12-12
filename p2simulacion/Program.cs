using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace p2simulacion {
	class Program {
		static long N;
		internal class DecInteger {
			private const long mod = 100000000L;
			private int[] digits;
			private int digitsLength;
			public DecInteger(long value) {
				digits = new int[] { (int) value, (int) (value >> 32) };
				digitsLength = 2;
			}

			private DecInteger(int[] digits, int length) {
				this.digits = digits;
				digitsLength = length;
			}
			public static DecInteger Pow2(int e) {
				if(e < 31)
					return new DecInteger((int) System.Math.Pow(2, e));
				return Pow2(e / 2) * Pow2(e - e / 2);
			}
			public static DecInteger operator *(DecInteger a, DecInteger b) {
				int alen = a.digitsLength, blen = b.digitsLength;
				int clen = alen + blen + 1;
				int[] digits = new int[clen];
				for(int i = 0; i < alen; i++) {
					long temp = 0;
					for(int j = 0; j < blen; j++) {
						temp = temp + ((long) a.digits[i] * (long) b.digits[j]) + digits[i + j];
						digits[i + j] = (int) (temp % mod);
						temp = temp / mod;
					}
					digits[i + blen] = (int) temp;
				}
				int k = clen - 1;
				while(digits[k] == 0)
					k--;
				return new DecInteger(digits, k + 1);
			}
			public override string ToString() {
				var sb = new System.Text.StringBuilder(digitsLength * 10);
				sb = sb.Append(digits[digitsLength - 1]);
				for(int j = digitsLength - 2; j >= 0; j--) {
					sb = sb.Append((digits[j] + (int) mod).ToString().Substring(1));
				}
				return sb.ToString();
			}
		}

		private static DecInteger Product(int n) {
			int m = n / 2;
			if(m == 0)
				return new DecInteger(N += 2);
			if(n == 2)
				return new DecInteger((N += 2) * (N += 2));
			return Product(n - m) * Product(m);
		}

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

		static string[] blanks = {
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

		private static void poisson(double[] r, double lambda) {
			int l = r.Length;
			int Factorial(int n) {
				if(n < 0)
					return 0;
				if(n < 2)
					return 1;
				DecInteger pp = new DecInteger(1);
				DecInteger rr = new DecInteger(1);
				N = 1;
				int h = 0, shift = 0, high = 1;
				int log2n = (int) Math.Floor(Math.Log(n) * 1.4426950408889634);
				while(h != n) {
					shift += h;
					h = n >> log2n--;
					int len = high;
					high = (h - 1) | 1;
					len = (high - len) / 2;
					if(len > 0) {
						pp *= Product(len);
						rr *= pp;
					}
				}
				rr *= DecInteger.Pow2(shift);
				return int.Parse(rr.ToString());
			}
			double p(int x) => Math.Pow(lambda, x) * Math.Pow(Math.E, -lambda) / Factorial(x);
			//double[] px = new double[10];
			double[] pxa = new double[11];
			pxa[0] = 0;
			//double prev = 0;
			string[] zeros = { "", "0", "00", "000" };
			for(int i = 0; i < 10; i++) {
				pxa[i + 1] = p(i) + pxa[i];
			}
			//Console.WriteLine("pxa1:\n" + printArray(pxa));
			for(int i = 0; i < pxa.Length; i++) {
				double pi = pxa[i];
				int pv = (int) (pi * 10000);
				int pvs = 4 - (pv + "").Length;
				double dpv;
				if(pvs < 0) {
					dpv = 1;
				} else {
					dpv = double.Parse($"0.{zeros[pvs]}{pv}");
				}
				pxa[i] = dpv;
			}
			//Console.WriteLine("pxa2:\n" + printArray(pxa));
			double[] xi = new double[l];
			for(int i = 0; i < l; i++) {
				for(int j = 0; j < pxa.Length; j++) {
					if(pxa[j] < r[i] && r[i] < pxa[j + 1]) {
						xi[i] = j;
						break;
					}
				}
			}
			//Console.WriteLine("xi:\n"+printArray(xi));
			string res = "+-----+--------+----+\n";
			res += "|  i  |   ri   | xi |\n";
			res += "+-----+--------+----+\n";
			for(int i = 0; i < l; i++) {
				int il = (i + "").Length;
				int ril = (r[i] + "").Length;
				int xil = (xi[i] + "").Length;
				Console.WriteLine($"{i} {ril} {xil}");
				res += $"| {i}{blanks[4 - il]}| {r[i]}{blanks[7 - ril]}| {xi[i]}{blanks[3 - xil]}|\n";
			}
			res += "+------+--------+----+\n";
			Console.WriteLine(res);
		}

		private static void demanda(int[] demanda, double[] r) {
			int l = r.Length;
			HashSet<int> unique = new HashSet<int>();
			foreach(int d in demanda) {
				unique.Add(d);
			}
			//foreach(int e in unique) {
			//	Console.WriteLine(e);
			//}
			//Console.WriteLine(unique.Count);

			double[] px = new double[unique.Count];
			foreach(int d in demanda) {
				px[d] += 1.0;
			}
			//Console.WriteLine(printArray(px));
			//Console.WriteLine("l: {0}", l);
			for(int i = 0; i < px.Length; i++) {
				px[i] /= demanda.Length;
			}

			//Console.WriteLine(printArray(px));
			double[] acc = new double[px.Length + 1];
			acc[0] = 0;
			for(int i = 0; i < px.Length; i++) {
				acc[i + 1] = acc[i] + px[i];
			}

			//Console.WriteLine(printArray(acc));

			int[] rd = new int[l];

			for(int i = 0; i < l; i++) {
				double rn = r[i];
				for(int j = 1; j < acc.Length; j++) {
					double li = acc[j - 1], ls = acc[j];
					if(li < rn && rn < ls) {
						rd[i] = j - 1;
						break;
					}
				}
			}

			//Console.WriteLine(printArray(rd));

			string res = "+-----+--------+-------+\n";
			res += "| Día |   ri   |  Dem  |\n";
			res += "+-----+--------+-------+\n";
			for(int i = 0; i < l; i++) {
				int s = ((i + 1) + "").Length;
				int sr = (r[i] + "").Length;
				int sd = (rd[i] + "").Length;
				Console.WriteLine("{0}, {1}", sr, sd);
				res += $"| {i + 1}{blanks[3 - s]} | {r[i]}{blanks[6 - (sr)]} |  {rd[i]}{blanks[4 - (sd)]} |\n";
			}
			res += "+-----+--------+-------+\n";
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
			Console.WriteLine("\tf)\tPrueba de Distribución de Poisson.");
			Console.WriteLine("\tg)\tPrueba de Estimación de Demanda.");
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
				case "f":
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
							poisson(numerosr, lm);
						} else {
							if(isDiv) {
								var nd = ui.Split('/');
								double n = double.Parse(nd[0]);
								double d = double.Parse(nd[1]);
								lm = n / d;
								poisson(numerosr, lm);
							} else {
								Console.WriteLine("Valor de lambda inválido");
							}
						}
					}
					break;
				case "g":
					if(numerosr == null) {
						Console.WriteLine("Ninguna serie fue generada.");
					} else {
						Console.WriteLine("Introduzca su observación de demanda (con valores separados por \",\"): ");
						string v = Console.ReadLine();
						v = v.Replace(" ", "");
						string[] va = v.Split(',');
						int[] d = new int[va.Length];
						for(int i = 0; i < va.Length; i++) {
							d[i] = int.Parse(va[i]);
						}
						demanda(d, numerosr);
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
