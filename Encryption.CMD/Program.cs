using Encryption.BL.Model;
using System;

namespace Encryption.CMD {
	class Program {
		static void Main(string[] args) {
			//Console.WriteLine("Шифр Цезаря:");
			//Console.WriteLine(Encrypt.Caesar("ИНФОРМАТИКА_ЭЮЯАБВЕЁЖ"));

			Console.WriteLine("\nШифр Атбаш:");
			Console.WriteLine(Encrypt.Atbash("ИНФОРМАТИКА"));

			//Console.WriteLine("\nПолибианский квадрат:");
			//Console.WriteLine(Encrypt.PolybianSquare("ИНФОРМАТИКА"));
			
			Console.WriteLine("\nШифрующая система Трисемуса:");
			Console.WriteLine(Encrypt.TrisemusSystem("ИНФОРМАТИКА", "МАТЕМАТИКА"));

			Console.WriteLine("\nШифр PlayFair:");
			Console.WriteLine(Encrypt.PlayFair("ЗАШИФРОВАННОЕСООБЩЕНИЕ", "МАТЕМАТИКА"));
			Console.WriteLine("Test for Kostya: " + Encrypt.PlayFair("ИНФОРМАТИКА", "МАТЕМАТИКА"));

			//Console.WriteLine("\nВариантный шифр:");
			//Console.WriteLine(Encrypt.Variant("ИНФОРМАТИКА", "МАТЕМАТИКА"));

			Console.WriteLine("\nШифр Виженера:");
			Console.WriteLine(Encrypt.Vigenere("ИНФОРМАТИКА", "ДИПЛОМ"));
			//Console.WriteLine(Encrypt.Vigenere("ВЕЛОСИПЕД", "ЛОЗУНГ"));

			Console.WriteLine("\nСовмещенный шифр:");
			Console.WriteLine(Encrypt.Combined("ИНФОРМАТИКА", "МАТЕМАТИКА"));
			//Console.WriteLine(Encrypt.Combined("ВЕЛОСИПЕД", "ЛОЗУНГ"));
			
			Console.ReadKey();
		}
	}
}
