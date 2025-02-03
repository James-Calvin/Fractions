program fixed_precision_rational_approximation
    implicit none
    real :: realValue
    integer :: precision, numerator, denominator, gcdValue
    character(len=100) :: arg1, arg2

    if (command_argument_count() /= 2) then
        print *, "Usage: fixed_precision_rational_approximation <realValue> <precision>"
        stop
    end if

    call get_command_argument(1, arg1)
    call get_command_argument(2, arg2)

    read(arg1, *) realValue
    read(arg2, *) precision

    numerator = nInt(realValue * precision)
    gcdValue = gcd(numerator, precision)

    numerator = numerator / gcdValue
    denominator = precision / gcdValue

    print *, "Approximate fraction:", numerator, "/", denominator

contains

    function gcd(a, b) result(g)
        implicit none
        integer, intent(in) :: a, b
        integer :: g, x, y

        x = abs(a)
        y = abs(b)

        do while (y /= 0)
            g = y
            y = mod(x, y)
            x = g
        end do

        g = x
    end function gcd

end program fixed_precision_rational_approximation
