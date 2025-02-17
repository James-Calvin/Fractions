using System.Diagnostics;

int GreatestCommonDivisor(int a, int b)
{
  while (b != 0)
  {
    int temp = b;
    b = a % b;
    a = temp;
  }
  return a;
}

// Place Value
(int numerator, int denominator) PlaceValue(float target)
{
  float fraction = target - (float)Math.Floor(target);
  int decimalPositions = 0;

  while (fraction > 0)
  {
    decimalPositions++;
    fraction *= 10;
    fraction -= (float)Math.Floor(fraction);
  }

  int denominator = (int)Math.Pow(10, decimalPositions);
  int numerator = (int)(target * denominator);
  int gcd = GreatestCommonDivisor(numerator, denominator);
  return (numerator / gcd, denominator / gcd);
}


Random rng = new Random();
float d = (float)(rng.NextDouble() * rng.NextDouble());
float tolerance = 0.00001f;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
(int a, int b) = PlaceValue(d);
stopwatch.Stop();
float c = (float)a / b;
float e = d - c;
var t = stopwatch.Elapsed.TotalNanoseconds;
Console.WriteLine($"PlaceValue {d}={a}/{b}={c} | {e} ({t}ns)");

// Fixed Precision
(int numerator, int denominator) FixedPrecision(float target, int precision)
{
  int numerator = (int)Math.Round(target * precision);

  // Since precision is an int and we're focused on values on the unit interval
  //  we don't need to worry about losing information in casting to int
  int gcd = GreatestCommonDivisor(numerator, precision);

  return (numerator / gcd, precision / gcd);
}

stopwatch.Reset();
stopwatch.Start();
(a, b) = FixedPrecision(d, 2520);
stopwatch.Stop();
c = (float)a / b;
e = d - c;
t = stopwatch.Elapsed.TotalNanoseconds;
Console.WriteLine($"FixedPrecision {d}={a}/{b}={c} | {e} ({t}ns)");

// Unit-Interval Stern-Brocot Tree Search
(int numerator, int denominator, int iterations)
  SternBrocot(float target, float tolerance)
{
  int numerator = 1, denominator = 2;
  int previousNumerator = 1, previousDenominator = 1;
  int lastChangeNumerator = 0, lastChangeDenominator = 1;
  int previousDirection = -1;

  float approximation = (float)numerator / denominator;
  float error = Math.Abs(target - approximation);

  int iterations = 0;
  while (error > tolerance)
  {
    iterations++;

    // -1 is to the left (getting smaller) 1 is to the right
    int direction = approximation > target ? -1 : 1;

    if (direction != previousDirection)
    {
      lastChangeNumerator = previousNumerator;
      lastChangeDenominator = previousDenominator;
    }

    previousNumerator = numerator;
    previousDenominator = denominator;

    numerator += lastChangeNumerator;
    denominator += lastChangeDenominator;

    previousDirection = direction;

    approximation = (float)numerator / denominator;
    error = Math.Abs(target - approximation);
  }

  return (numerator, denominator, iterations);
}

stopwatch.Reset();
stopwatch.Start();
(a, b, int iterations) = SternBrocot(d, tolerance);
stopwatch.Stop();
c = (float)a / b;
e = d - c;
t = stopwatch.Elapsed.TotalNanoseconds;
Console.WriteLine(
  $"SternBrocot {d}={a}/{b}={c} | {e} ({t}ns - {iterations} iterations)");

// Continued Fractions
(int numerator, int denominator, int iterations)
  ContinuedFraction(double value, double tolerance = 1e-10)
{
  int iterations = 0;

  if (value == 0) return (0, 1, iterations);

  long wholePart = (long)Math.Floor(value);
  double fractionalPart = value - wholePart;

  if (fractionalPart < tolerance) // If there's no meaningful fractional part
    return ((int)wholePart, 1, iterations);

  long numerator1 = 1, denominator1 = 0;
  long numerator2 = (int)wholePart, denominator2 = 1;

  while (true)
  {
    iterations++;

    double reciprocal = 1.0 / fractionalPart;
    long nextWholePart = (long)Math.Floor(reciprocal);

    long numerator = nextWholePart * numerator2 + numerator1;
    long denominator = nextWholePart * denominator2 + denominator1;

    // If the fraction is within the tolerance, return the result
    if (Math.Abs(value - (double)numerator / denominator) < tolerance)
      return ((int)numerator, (int)denominator, iterations);

    // Update for the next iteration
    numerator1 = numerator2;
    denominator1 = denominator2;
    numerator2 = numerator;
    denominator2 = denominator;

    fractionalPart = reciprocal - nextWholePart;
  }
}

stopwatch.Reset();
stopwatch.Start();
(a, b, iterations) = ContinuedFraction(d, tolerance);
stopwatch.Stop();
c = (float)a / b;
e = d - c;
t = stopwatch.Elapsed.TotalNanoseconds;
Console.WriteLine(
  $"ContinuedFraction {d}={a}/{b}={c} | {e} ({t}ns - {iterations} iterations)");