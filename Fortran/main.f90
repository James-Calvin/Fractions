program fixed_precision_rational_approximation
  implicit none

  ! Take in a integer denominator and a real value
  integer :: precision, numerator, denominator
  real :: realValue

  precision = 120
  realValue = 0.666666_sp ! single precisions value

  numerator = anint(precision * realValue)

  function gcd(a,b)
    
  end function gcd

end program fixed_precision_rational_approximation