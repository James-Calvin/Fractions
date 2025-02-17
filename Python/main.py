def greatest_common_divisor(a:int, b:int):
  while b != 0:
    temp = b
    b = a % b
    a = temp

  return a

def fixed_precision_rational_approximation(real: float, precision: int):
  numerator = round(real * precision)
  
  gcd = greatest_common_divisor(numerator, precision)

  numerator = numerator / gcd
  denominator = precision / gcd
  
  return (numerator, denominator)


