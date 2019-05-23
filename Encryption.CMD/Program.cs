using Encryption.BL.Model;
using System;

namespace Encryption.CMD {
	class Program {
		static void Main(string[] args) {
			#region lr1
			/*
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

			//Console.WriteLine("\nВариантный шифр:");
			//Console.WriteLine(Encrypt.Variant("ИНФОРМАТИКА", "МАТЕМАТИКА"));

			Console.WriteLine("\nШифр Виженера:");
			Console.WriteLine(Encrypt.Vigenere("ИНФОРМАТИКА", "ДИПЛОМ"));

			Console.WriteLine("\nСовмещенный шифр:");
			Console.WriteLine(Encrypt.Combined("ИНФОРМАТИКА", "МАТЕМАТИКА"));
			*/
			#endregion
			
			Console.WriteLine("Шифр простой одинарной перестановки:");
			Console.WriteLine(Encrypt.SimpleSinglePermutation("информатика", out int[] key));

			Console.WriteLine("\nШифр блочной одинарной перестановки:");
			Console.WriteLine(Encrypt.BlockSinglePermutation("информатика", 3, out key));

			Console.WriteLine("\nШифр табличной маршрутной перестановки:");
			Console.WriteLine(Encrypt.TableRoutePermutation("информационная безопасность", 5, 6, 
				Route.LeftToRight_TopToBottom, Route.TopToBottom_LeftToRight));
			
			Console.WriteLine("\nШифр вертикальной перестановки:");
			Console.WriteLine(Encrypt.VerticalPermutation("информационная безопасность", "КОЛОННА"));

			Console.WriteLine("\nШифр \"Перекрёсток\":");
			Console.WriteLine(Encrypt.Crossroad("информационная безопасность", 2, CrossroadsMethod.LeftClockwise));

			Console.WriteLine("\nШифр \"Поворотная решетка\":");
			bool[,] grid = { { true,  false, true,  false},
							 { false, false, false, false},
							 { false, true,  false, true },
							 { false, false, false, false} };
			Console.WriteLine(Encrypt.RotaryGrid("наука математика", grid, 
				Turn.Vertically, Turn.Horizontally, Turn.Vertically, Route.TopToBottom_LeftToRight));

			Console.WriteLine("\nШифр \"Магический квадрат\":");
			int[,] magicSquare = { { 16, 3,  2,  13 },
								   { 5,  10, 11, 8  },
								   { 9,  6,  7,  12 },
								   { 4,  15, 14, 1  } };
			Console.WriteLine(Encrypt.MagicSquare("информатика", magicSquare));
			
			Console.WriteLine("\nШифр двойной перестановки:");
			Console.WriteLine(Encrypt.DoublePermutation("наука математика", 4, 4, 
				Route.RightToLeft_TopToBottom, Route.TopToBottom_LeftToRight, out int[] keyN, out int[] keyM));
					   
			Console.ReadKey();
		}
	}
}
