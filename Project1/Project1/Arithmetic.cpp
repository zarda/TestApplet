#include "Arithmetic.h"

HTA::Cpp::Arithmetic::Arithmetic()
{
	srand(time(NULL));
}

HTA::Cpp::Arithmetic::~Arithmetic()
{
}

int HTA::Cpp::Arithmetic::Rnd(int min, int max)
{
	return rand() % (max - min) + min;
}

int HTA::Cpp::Arithmetic::Add(int a, int b)
{
	return a + b;
}

int HTA::Cpp::Arithmetic::Sub(int a, int b)
{
	return a - b;
}

int HTA::Cpp::Arithmetic::Mul(int a, int b)
{
	return a*b;
}
