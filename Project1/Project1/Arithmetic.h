#pragma once

#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */

namespace HTA
{
	namespace Cpp
	{
		public ref class Arithmetic
		{
		public:
			Arithmetic();
			~Arithmetic();
			int Rnd(int, int);
			static int Add(int, int);
			static int Sub(int, int);
			static int Mul(int, int);
		};
	}
}
