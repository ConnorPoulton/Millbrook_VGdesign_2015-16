using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MathFun
{
    class Program
    {
        static void Main(string[] args)
        {
            bouncyNumbers();
            Console.ReadLine();
        }

		static void bouncyNumbers()
		{
			//Working from left-to-right if no digit is exceeded by the digit to its left it is called an increasing number; for example, 134468.
			//Similarly if no digit is exceeded by the digit to its right it is called a decreasing number; for example, 66420.
			//We shall call a positive integer that is neither increasing nor decreasing a "bouncy" number; for example, 155349.
			//Clearly there cannot be any bouncy numbers below one-hundred, but just over half of the numbers below one-thousand (525) are bouncy. In fact, the least number for which the proportion of bouncy numbers first reaches 50% is 538.
			//Surprisingly, bouncy numbers become more and more common and by the time we reach 21780 the proportion of bouncy numbers is equal to 90%.
			//This finds the least number for which the proportion of bouncy numbers is exactly 99%.
			//And it would probably run a lot faster if I re-wrote it in Python.
			
			int increasingNums = 0, decreasingNums = 0, bouncyNums = 0;
			double percentBouncy = 0.00;
			for (int x = 10; x < 100000000; x++)
			{
				string intAsStr = x.ToString();
				int numsLess = 0, numsGreater = 0, lastDig = 0;
				foreach (char digit in intAsStr)
					if (lastDig == 0)
						lastDig = digit;
					else
					{
						if (digit < lastDig)
							numsLess += 1;
						else if (digit > lastDig)
							numsGreater += 1;

						lastDig = digit;
					}
				if (numsGreater == 0 && numsLess > 0)
					decreasingNums += 1;
				else if (numsLess == 0 && numsGreater > 0)
					increasingNums += 1;
				else if (numsLess > 0 && numsGreater > 0)
					bouncyNums += 1;

				percentBouncy = (bouncyNums * 1.0) / x;

				Console.WriteLine(percentBouncy);

				if (percentBouncy == 0.99)
				{
					Console.WriteLine(x);
					break;
				}
			}
		}
	}
}