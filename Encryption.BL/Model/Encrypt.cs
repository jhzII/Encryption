using System;

namespace Encryption.BL.Model {
	/// <summary>
	/// Библиотека шифрований.
	/// </summary>
	public static class Encrypt {
		
		/// <summary>
		/// Шифр Цезаря.
		/// </summary>
		/// <param name="message"> Открытое сообщение. </param>
		/// <returns> Закрытое сообщение. </returns>
		public static string Caesar(string message) {
			string result = "";
			foreach(char ch in message) {
				int index = getIndexInAlphabet(ch);
				if (index == -1) 
					result += ch;
				else if (index + 3 < alphabet.Length)
					result += alphabet[index + 3];
				else
					result += alphabet[index - alphabet.Length + 3];
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
				int index = getIndexInAlphabet(ch);
				if(index == -1)
					result += ch;
				else
					result += alphabet[alphabet.Length - index - 1];
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
				if(getIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = getTrisemusMatrix(key);
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
				if(getIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = getTrisemusMatrix(key);
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
				if(getIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = getTrisemusMatrix(key);
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
				if(getIndexInAlphabet(ch) == -1)
					throw new ArgumentException("Ключ содержит недопустимые символы.", nameof(key));

			char[,] matrix = new char[key.Length + 1, 33];

			for(int j = 0; j < 33; j++)
				matrix[0, j] = alphabet[j];

			for(int i = 1; i < key.Length + 1; i++) {
				int index = getIndexInAlphabet(key[i - 1]);
				for(int j = 0; j < 33; j++)
					matrix[i, j] = alphabet[(j + index) % 33];
			}

			string result = "";

			for(int i = 0; i < message.Length; i++) {
				int j = getIndexInAlphabet(message[i]);
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
				if(getIndexInAlphabet(ch) == -1)
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
					if(key.IndexOf(alphabet[indexA]) == -1) {
						int j = (i % 10 == 0) ? 0 : 10 - i % 10;
						matrix[i / 10 + 1, j] = alphabet[indexA++];
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
		
		private static int getIndexInAlphabet(char target) {
			for(int i = 0; i < alphabet.Length; i++)
				if(target == alphabet[i])
					return i;
			return -1;
		}

		private static char[,] getTrisemusMatrix(string key) {
			foreach(char ch in key)
				if(getIndexInAlphabet(ch) == -1)
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
					if(key.IndexOf(alphabet[indexA]) == -1) {
						matrix[i / 6, i % 6] = alphabet[indexA++];
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

		private static char[] alphabet = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н',
							'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
	}
}
