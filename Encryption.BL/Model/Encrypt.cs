using System;

namespace Encryption.BL.Model {
	/// <summary>
	/// Виды маршрутов.
	/// </summary>
	public enum Route {
		/// <summary>
		/// Снизу вверх - слева направо.
		/// </summary>
		BottomToTop_LeftToRight,
		/// <summary>
		/// Снизу вверх - справа налево.
		/// </summary>
		BottomToTop_RightToLeft,
		/// <summary>
		/// Слева направо - снизу вверх.
		/// </summary>
		LeftToRight_BottomToTop,
		/// <summary>
		/// Слева направо - сверху вниз.
		/// </summary>
		LeftToRight_TopToBottom,
		/// <summary>
		/// Справа налево - снизу вверх.
		/// </summary>
		RightToLeft_BottomToTop,
		/// <summary>
		/// Справа налево - сверху вниз.
		/// </summary>
		RightToLeft_TopToBottom,
		/// <summary>
		/// Сверху вниз - слева направо.
		/// </summary>
		TopToBottom_LeftToRight,
		/// <summary>
		/// Сверху вниз - справа налево.
		/// </summary>
		TopToBottom_RightToLeft
	}

	public enum CrossroadsMethod {
		BottomClockwise,
		BottomCounterclockwise,
		LeftClockwise,
		LeftCounterclockwise,
		RightClockwise,
		RightCounterclockwise,
		TopClockwise,
		TopCounterclockwise
	}

	public enum Turn {
		Horizontally,
		Vertically
	}

	/// <summary>
	/// Библиотека шифрований.
	/// </summary>
	public static class Encrypt {
		//TODO: проверки аргументов

		/// <summary>
		/// Шифр Цезаря.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Caesar(string message) {
			string result = "";
			foreach(char ch in message) {
				int index = GetIndexInAlphabet(ch);
				if (index == -1) 
					result += ch;
				else if (index + 3 < Alphabet.Length)
					result += Alphabet[index + 3];
				else
					result += Alphabet[index - Alphabet.Length + 3];
			}
			return result;
		}

		/// <summary>
		/// Шифр Атбаш.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Atbash(string message) {
			string result = "";
			foreach(char ch in message) {
				int index = GetIndexInAlphabet(ch);
				result += (index == -1) ? ch : Alphabet[Alphabet.Length - index - 1];
			}
			return result;
		}

		/// <summary>
		/// Полибианский квадрат.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string PolybianSquare(string message) {
			char[,] matrix = { { 'А', 'Б', 'В', 'Г', 'Д', 'Е' },
							   { 'Ё', 'Ж', 'З', 'И', 'Й', 'К' },
							   { 'Л', 'М', 'Н', 'О', 'П', 'Р' },
							   { 'С', 'Т', 'У', 'Ф', 'Х', 'Ц' },
							   { 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь' },
							   { 'Э', 'Ю', 'Я', '-', '-', '-' } };
			string result = "";
			bool flag;
			
			foreach(char ch in message) {
				flag = false;
				for(int i = 0; i < 6; i++) {
					for(int j = 0; j < 6; j++) {
						if(ch == matrix[i, j]) {
							result += (i + 1) + "" + (j + 1);
							flag = true;
						}							
					}
				}
				if (!flag)
					result += "00";
			}
			return result;
		}
		
		/// <summary>
		/// Шифрующая система Трисемуса.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string TrisemusSystem(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = GetTrisemusMatrix(key);
			string result = "";
			bool flag;

			foreach(char ch in message) {
				flag = false;
				for(int i = 0; i < 6; i++) {
					for(int j = 0; j < 6; j++) {
						if(ch == matrix[i, j]) {
							result += matrix[(i + 1) % 6, j];
							flag = true;
						}
					}
				}
				if(!flag)
					result += ch;
			}

			return result;
		}

		/// <summary>
		/// Шифр PlayFair.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string PlayFair(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = GetTrisemusMatrix(key);
			string result = "";

			int index = 0;
			while(index < message.Length) {
				char LowCh = message[index];
				char HighCh = (index + 1 == message.Length) ? 'Я' : message[index + 1];
				if(LowCh == HighCh) {
					HighCh = 'Я';
					index++;
				}
				else
					index += 2;
				// пара собрана

				int i_lc = -1,
					j_lc = -1,
					i_hc = -1,
					j_hc = -1;
				for(int i = 0; i < 6; i++) {
					for(int j = 0; j < 6; j++) {
						if(LowCh == matrix[i, j]) {
							i_lc = i;
							j_lc = j;
						}
						if(HighCh == matrix[i, j]) {
							i_hc = i;
							j_hc = j;
						}
						if(i_lc != -1 && i_hc != -1)
							goto End;
					}
				}
			End:
				if(i_lc == -1 || i_hc == -1)
					result += "--";
				else if(i_lc == i_hc)
					result += matrix[i_lc, (j_lc + 1) % 6] + "" + matrix[i_hc, (j_hc + 1) % 6];
				else if(j_lc == j_hc)
					result += matrix[(i_lc + 1) % 6, j_lc] + "" + matrix[(i_hc + 1) % 6, j_hc];
				else
					result += matrix[i_lc, j_hc] + "" + matrix[i_hc, j_lc];
			}
			
			return result;
		}

		/// <summary>
		/// Вариантный шифр.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Variant(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = GetTrisemusMatrix(key);
			char[,] i_chars = { { 'Ф', 'Ы' }, { 'В', 'А' }, { 'П', 'Р' }, { 'О', 'Л' }, { 'Д', 'Ж' }, { 'Э', 'Я' } };
			char[,] j_chars = { { 'Й', 'Ц' }, { 'У', 'К' }, { 'Е', 'Н' }, { 'Г', 'Ш' }, { 'Щ', 'З' }, { 'Х', 'Ъ' } };
			string result = "";

			for(int index = 0; index < message.Length; index++) {
				char ch = message[index];
				for(int i = 0; i < 6; i++) {
					for(int j = 0; j < 6; j++) {
						if(ch == matrix[i, j]) {
							int count = 0; // какой раз приходится шифровать данную букву
							string substring = message.Substring(0, index);
							foreach(char c in substring)
								if(c == ch)
									count++;
														
							if(count / 4 % 2 == 0) // если встретилась 1, 2, 3, 4, 9, 10 и т.д. раз								
								result += i_chars[i, count / 2] + "" + j_chars[j, count % 2];	// 00 01 10 11
							else
								result += +j_chars[j, count % 2] + "" + i_chars[i, count / 2];
							goto End;
						}
					}
				}
				result += "--";
			End:;
			}

			return result;
		}
		
		/// <summary>
		/// Шифр Виженера.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Vigenere(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = new char[key.Length + 1, 33];

			for(int j = 0; j < 33; j++)
				matrix[0, j] = Alphabet[j];

			for(int i = 1; i < key.Length + 1; i++) {
				int index = GetIndexInAlphabet(key[i - 1]);
				for(int j = 0; j < 33; j++)
					matrix[i, j] = Alphabet[(j + index) % 33];
			}

			string result = "";

			for(int i = 0; i < message.Length; i++) {
				int j = GetIndexInAlphabet(message[i]);
				result += matrix[i % key.Length + 1, j];
			}

			return result;
		}

		/// <summary>
		/// Совмещенный шифр.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Combined(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));
			
			int i = 0;
			while(i < key.Length - 1) {
				while(true) {
					int index = key.Substring(i + 1).IndexOf(key[i]);
					if(index == -1)
						break;
					//key = key.Substring(0, index) + key.Substring(index + 1);
					key = key.Remove(index + i + 1, 1);
				}
				i++;
			}			
			int n = key.Length;
			if (n > 10)
				throw new ArgumentException("Ключ содержит более 10 различных символов.", nameof(key));

			char[,] matrix;
			if (n > 2)
				matrix = new char[4, 10];
			else
				matrix = new char[5, 10];

			for(int j = 0; j < n; j++)
				matrix[0, j == 0 ? 0 : 10 - j] = key[j];

			int indexA = 0;
			for(i = 0; i < 33 - n; i++) {
				while(true) {
					if(key.IndexOf(Alphabet[indexA]) == -1) {
						int j = (i % 10 == 0) ? 0 : 10 - i % 10;
						matrix[i / 10 + 1, j] = Alphabet[indexA++];
						break;
					}
					indexA++;
				}
			}
			
			string result = "";

			foreach(char ch in message) {
				for(i = 0; i < matrix.Length / 10; i++) {
					for(int j = 0; j < 10; j++) {
						if(ch == matrix[i, j]) {
							result += i == 0 ? j.ToString() : "" + i + j;
							goto End;
						}
					}
				}
				result += ch;
			End:;
			}

			return result;
		}

		/// <summary>
		/// Шифр простой одинарной перестановки.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Таблица перестановки. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string SimpleSinglePermutation(string message, out int[] key) {
			key = GetPermutationTable(message.Length);
			char[] result = new char[message.Length];

			for(int i = 0; i < result.Length; i++)
				result[key[i]] = message[i];

			return new string(result);
		}

		/// <summary>
		/// Шифр блочной одинарной перестановки.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="n"> Размер блока. </param>
		/// <param name="key"> Таблица перестановки. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string BlockSinglePermutation(string message, int n, out int[] key) {
			key = GetPermutationTable(n);
			int resultsLength = (message.Length % n == 0) ? message.Length : (message.Length / n) * (n + 1); 
			char[] result = new char[resultsLength];
			
			for (int i = 0; i < resultsLength; i++) {
				int iRes = n * (i / n) + key[i % n];
				result[iRes] = (i < message.Length) ? message[i] : '_';
			}
			return new string(result);
		}

		/// <summary>
		/// Шифр табличной маршрутной перестановки.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="n"> Количество строк. </param>
		/// <param name="m"> Количество столбцов. </param>
		/// <param name="inputRoute"> Маршрут вписывания. </param>
		/// <param name="outputRoute"> Маршрут выписывания. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string TableRoutePermutation(string message, int n, int m, Route inputRoute, Route outputRoute) {
			if (n * m < message.Length)
				throw new ArgumentException("Недопустимый размер матрицы.");

			char[,] table = GetTableFromStringByRoute(message, n, m, inputRoute);

			return GetStringFromTableByRoute(table, outputRoute);
		}

		/// <summary>
		/// Шифр вертикальной перестановки.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="key"> Ключ. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string VerticalPermutation(string message, string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			int m = key.Length;
			int n = (int)Math.Ceiling((double)message.Length / m);

			char[,] table = GetTableFromStringByRoute(message, n, m, Route.LeftToRight_TopToBottom);

			int[] permutationTable = new int[m];
			for(int i = 0; i < m; i++)
				permutationTable[i] = -1;

			for(int i = 0; i < m; i++) {
				int indexChar = -1;
				int znachChar = 33;
				for(int j = 0; j < m; j++) {
					if (GetIndexInAlphabet(key[j]) < znachChar && permutationTable[j] == -1) {
						znachChar = GetIndexInAlphabet(key[j]);
						indexChar = j;
					}
				}
				permutationTable[indexChar] = i;
			}

			table = PermutationTablesColumns(table, permutationTable);

			return GetStringFromTableByRoute(table, Route.TopToBottom_LeftToRight);
		}

		/// <summary>
		/// Шифр "Перекрёсток".
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="n"> Размер блока. </param>
		/// <param name="method"> Метод вписывания. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Crossroad(string message, int n, CrossroadsMethod method) {
			int numberOfCrossroads = (int)Math.Ceiling(Math.Ceiling(message.Length / 4.0) / n) * n;
			message = message.Replace(' ', '_');
			message = message.PadRight(numberOfCrossroads * 4, '_');

			char[,,] crossroads = new char[numberOfCrossroads, 2, 2];

			for(int k = 0; k < numberOfCrossroads; k++) {
				string substring = message.Substring(0, 4);
				message = message.Remove(0, 4);

				switch(method) {
				case CrossroadsMethod.BottomClockwise:
					crossroads[k, 1, 1] = substring[0];
					crossroads[k, 1, 0] = substring[1];
					crossroads[k, 0, 0] = substring[2];
					crossroads[k, 0, 1] = substring[3];
					break;
				case CrossroadsMethod.BottomCounterclockwise:
					crossroads[k, 1, 1] = substring[0];
					crossroads[k, 0, 1] = substring[1];
					crossroads[k, 0, 0] = substring[2];
					crossroads[k, 1, 0] = substring[3];
					break;
				case CrossroadsMethod.LeftClockwise:
					crossroads[k, 1, 0] = substring[0];
					crossroads[k, 0, 0] = substring[1];
					crossroads[k, 0, 1] = substring[2];
					crossroads[k, 1, 1] = substring[3];
					break;
				case CrossroadsMethod.LeftCounterclockwise:
					crossroads[k, 1, 0] = substring[0];
					crossroads[k, 1, 1] = substring[1];
					crossroads[k, 0, 1] = substring[2];
					crossroads[k, 0, 0] = substring[3];
					break;
				case CrossroadsMethod.RightClockwise:
					crossroads[k, 0, 1] = substring[0];
					crossroads[k, 1, 1] = substring[1];
					crossroads[k, 1, 0] = substring[2];
					crossroads[k, 0, 0] = substring[3];
					break;
				case CrossroadsMethod.RightCounterclockwise:
					crossroads[k, 0, 1] = substring[0];
					crossroads[k, 0, 0] = substring[1];
					crossroads[k, 1, 0] = substring[2];
					crossroads[k, 1, 1] = substring[3];
					break;
				case CrossroadsMethod.TopClockwise:
					crossroads[k, 0, 0] = substring[0];
					crossroads[k, 0, 1] = substring[1];
					crossroads[k, 1, 1] = substring[2];
					crossroads[k, 1, 0] = substring[3];
					break;
				case CrossroadsMethod.TopCounterclockwise:
					crossroads[k, 0, 0] = substring[0];
					crossroads[k, 1, 0] = substring[1];
					crossroads[k, 1, 1] = substring[2];
					crossroads[k, 0, 1] = substring[3];
					break;
				}
			}

			string result = "";
			for(int i = 0; i < numberOfCrossroads / n; i++) {
				for(int j = 0; j < n; j++)
					result += crossroads[i * n + j, 0, 0];
				for(int j = 0; j < n; j++) {
					result += crossroads[i * n + j, 1, 0];
					result += crossroads[i * n + j, 0, 1];
				}
				for(int j = 0; j < n; j++)
					result += crossroads[i * n + j, 1, 1];
			}
			return result;
		}

		/// <summary>
		/// Шифр "Поворотная решетка".
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="grid"> Трафарет. </param>
		/// <param name="turn1"> Первый поворот. </param>
		/// <param name="turn2"> Второй поворот. </param>
		/// <param name="turn3"> Третий поворот. </param>
		/// <param name="route"> Маршрут выписывания. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string RotaryGrid(string message, bool[,] grid, Turn turn1, Turn turn2, Turn turn3, Route route) {
			int n = grid.GetLength(0);
			int m = grid.GetLength(1);
			if(n * m < message.Length)
				throw new ArgumentException("Недопустимый размер матрицы.");

			message = message.Replace(' ', '_');
			message = message.PadRight(n * m, '_');
			char[,] table = new char[n, m];
			int[] keyN = new int[n];
			for(int i = 0; i < n; i++)
				keyN[i] = n - i - 1;
			int[] keyM = new int[m];
			for(int i = 0; i < m; i++)
				keyM[i] = m - i - 1;

			table = InscribeWithGrid(table, grid, ref message);
			grid = (turn1 == Turn.Vertically) ? PermutationTablesColumns(grid, keyM) :
												PermutationTablesRows(grid, keyN);
			table = InscribeWithGrid(table, grid, ref message);
			grid = (turn2 == Turn.Vertically) ? PermutationTablesColumns(grid, keyM) :
												PermutationTablesRows(grid, keyN);
			table = InscribeWithGrid(table, grid, ref message);
			grid = (turn3 == Turn.Vertically) ? PermutationTablesColumns(grid, keyM) :
												PermutationTablesRows(grid, keyN);
			table = InscribeWithGrid(table, grid, ref message);

			return GetStringFromTableByRoute(table, route);
		}

		/// <summary>
		/// Магический квадрат.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="magicSquare"> Магический квадрат. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string MagicSquare(string message, int[,] magicSquare) {
			int n = magicSquare.GetLength(0);
			if(n * n < message.Length)
				throw new ArgumentException("Недопустимый размер матрицы.");
			if(message == "")
				return "";
			
			message = message.Replace(' ', '_');
			message = message.PadRight(n * n, '_');

			int max = magicSquare[0, 0];
			int min = magicSquare[0, 0];

			for(int i = 0; i < n; i++) {
				for(int j = 0; j < n; j++) {
					if(magicSquare[i, j] < min)
						min = magicSquare[i, j];
					if(magicSquare[i, j] > max)
						max = magicSquare[i, j];
				}
			}

			int divider = (max - min + 1) / (n * n);
			int delta = min / divider;

			char[,] table = new char[n, n];

			for(int i = 0; i < n; i++) {
				for(int j = 0; j < n; j++) {
					int index = magicSquare[i, j] / divider - delta;
					table[i, j] = message[index];
				}
			}

			return GetStringFromTableByRoute(table, Route.LeftToRight_TopToBottom);
		}

		/// <summary>
		/// Шифр двойной перестановки. 
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <param name="n"> Количество строк. </param>
		/// <param name="m"> Количество столбцов. </param>
		/// <param name="inputRoute"> Маршрут вписывания. </param>
		/// <param name="outputRoute"> Маршрут выписывания. </param>
		/// <param name="keyN"> Ключ перестановок строк. </param>
		/// <param name="keyM"> Ключ перестановок столбцов. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string DoublePermutation(string message, int n, int m, Route inputRoute, Route outputRoute, 
											   out int[] keyN, out int[] keyM) {
			if(n * m < message.Length)
				throw new ArgumentException("Недопустимый размер матрицы.");
			
			keyN = GetPermutationTable(n);
			keyM = GetPermutationTable(m);

			keyM[0] = 3; keyM[1] = 0; keyM[2] = 2; keyM[3] = 1;
			keyN[0] = 2; keyN[1] = 0; keyN[2] = 3; keyN[3] = 1;

			char[,] table = GetTableFromStringByRoute(message, n, m, inputRoute);

			table = PermutationTablesColumns(table, keyM);
			table = PermutationTablesRows(table, keyN);

			return GetStringFromTableByRoute(table, outputRoute);
		}
		

		private static char[,] InscribeWithGrid(char[,] table, bool[,] grid, ref string message) {
			int n = table.GetLength(0);
			int m = table.GetLength(1);
			int k = 0;
			for(int i = 0; i < n; i++)
				for(int j = 0; j < m; j++)
					if(grid[i, j])
						table[i, j] = message[k++];

			message = message.Remove(0, n * m / 4);
			return table;
		}

		private static T[,] PermutationTablesColumns<T>(T[,] table, int[] key) {
			int n = table.GetLength(0);
			int m = table.GetLength(1);
			T[,] newTable = new T[n, m];

			for(int i = 0; i < n; i++)
				for(int j = 0; j < m; j++) 
					newTable[i, key[j]] = table[i, j];

			return newTable;
		}

		private static T[,] PermutationTablesRows<T>(T[,] table, int[] key) {
			int n = table.GetLength(0);
			int m = table.GetLength(1);
			T[,] newTable = new T[n, m];

			for(int i = 0; i < n; i++)
				for(int j = 0; j < m; j++)
					newTable[key[i], j] = table[i, j];

			return newTable;
		}

		private static string GetStringFromTableByRoute(char[,] table, Route route) {
			if(table.Length == 0)
				return "";

			int n = table.GetLength(0);
			int m = table.GetLength(1);
			string message = "";

			switch(route) {
			case Route.BottomToTop_LeftToRight:
				for(int j = 0; j < m; j++)
					for(int i = n - 1; i >= 0; i--)
						message += table[i, j];
				break;
			case Route.BottomToTop_RightToLeft:
				for(int j = m - 1; j >= 0; j--)
					for(int i = n - 1; i >= 0; i--)
						message += table[i, j];
				break;
			case Route.LeftToRight_BottomToTop:
				for(int i = n - 1; i >= 0; i--)
					for(int j = 0; j < m; j++)
						message += table[i, j];
				break;
			case Route.LeftToRight_TopToBottom:
				for(int i = 0; i < n; i++)
					for(int j = 0; j < m; j++)
						message += table[i, j];
				break;
			case Route.RightToLeft_BottomToTop:
				for(int i = n - 1; i >= 0; i--)
					for(int j = m - 1; j >= 0; j--)
						message += table[i, j];
				break;
			case Route.RightToLeft_TopToBottom:
				for(int i = 0; i < n; i++)
					for(int j = m - 1; j >= 0; j--)
						message += table[i, j];
				break;
			case Route.TopToBottom_LeftToRight:
				for(int j = 0; j < m; j++)
					for(int i = 0; i < n; i++)
						message += table[i, j];
				break;
			case Route.TopToBottom_RightToLeft:
				for(int j = m - 1; j >= 0; j--)
					for(int i = 0; i < n; i++)
						message += table[i, j];
				break;
			}
			return message;
		}

		private static char[,] GetTableFromStringByRoute(string message, int n, int m, Route route) {
			message = message.Replace(' ', '_');
			message = message.PadRight((n * m), '_');
			
			char[,] table = new char[n, m];
			int index = 0;
			switch(route) {
			case Route.BottomToTop_LeftToRight:
				for(int j = 0; j < m; j++)
					for(int i = n - 1; i >= 0; i--)
						table[i, j] = message[index++];
				break;
			case Route.BottomToTop_RightToLeft:
				for(int j = m - 1; j >= 0; j--)
					for(int i = n - 1; i >= 0; i--)
						table[i, j] = message[index++];
				break;
			case Route.LeftToRight_BottomToTop:
				for(int i = n - 1; i >= 0; i--)
					for(int j = 0; j < m; j++)
						table[i, j] = message[index++];
				break;
			case Route.LeftToRight_TopToBottom:
				for(int i = 0; i < n; i++)
					for(int j = 0; j < m; j++)
						table[i, j] = message[index++];
				break;
			case Route.RightToLeft_BottomToTop:
				for(int i = n - 1; i >= 0; i--)
					for(int j = m - 1; j >= 0; j--)
						table[i, j] = message[index++];
				break;
			case Route.RightToLeft_TopToBottom:
				for(int i = 0; i < n; i++)
					for(int j = m - 1; j >= 0; j--)
						table[i, j] = message[index++];
				break;
			case Route.TopToBottom_LeftToRight:
				for(int j = 0; j < m; j++)
					for(int i = 0; i < n; i++)
						table[i, j] = message[index++];
				break;
			case Route.TopToBottom_RightToLeft:
				for(int j = m - 1; j >= 0; j--)
					for(int i = 0; i < n; i++)
						table[i, j] = message[index++];
				break;
			}
			return table;
		}

		private static void Swap<T>(ref T var1, ref T var2) {
			T var3 = var1;
			var1 = var2;
			var2 = var3;
		}        

		private static int[] GetPermutationTable(int length) {
			int[] table = new int[length];
			for (int i = 0; i < length; i++)
				table[i] = i;

			Random rnd = new Random();
			int newIndex;
			for(int i = 0; i < length * 3; i++) {
				newIndex = rnd.Next(length);
				Swap(ref table[i / length], ref table[newIndex]);
			}
			return table;
		}

		private static int GetIndexInAlphabet(char target) {
			for(int i = 0; i < Alphabet.Length; i++)
				if(target == Alphabet[i])
					return i;
			return -1;
		}

		private static char[,] GetTrisemusMatrix(string key) {
			foreach(char ch in key)
				if(GetIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			int i = 0;
			while(i < key.Length - 1) {
				while(true) {
					int index = key.Substring(i + 1).IndexOf(key[i]);
					if(index == -1)
						break;
					//key = key.Substring(0, index) + key.Substring(index + 1);
					key = key.Remove(index + i + 1, 1);
				}
				i++;
			}

			char[,] matrix = new char[6, 6];

			int n = key.Length;
			for(i = 0; i < n; i++)
				matrix[i / 6, i % 6] = key[i];

			int indexA = 0;
			for(i = n; i < 33; i++) {
				while(true) {
					if(key.IndexOf(Alphabet[indexA]) == -1) {
						matrix[i / 6, i % 6] = Alphabet[indexA++];
						break;
					}
					indexA++;
				}
			}
			matrix[5, 3] = '-';
			matrix[5, 4] = '1';
			matrix[5, 5] = '2';

			return matrix;
		}

		private static readonly char[] Alphabet = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л',
				'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
	}
}
