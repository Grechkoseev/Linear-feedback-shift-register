namespace Lab_2;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Регистр сдвига с линейной обратной связью определяется характеристическим многочленом");
        Console.WriteLine("1 + x^2 + x^6 + x^8");

        Console.WriteLine("\nВведите начальное значение регистра (целое число в диапазоне от 1 до 255)");
        int.TryParse(Console.ReadLine(), out var initialValue);
        Console.WriteLine();

        Lfsr lfsr = new(new[] { 1, 5, 7 }, initialValue);
        lfsr.PutNumberToRegister(initialValue);

        Console.WriteLine("Последовательность значений регистра: ");
        do
        {
            lfsr.PrintNumberFromRegister();
            lfsr.CalculateNextSequenceMember();
        } while (!lfsr.IsCurrentSequenceMemberEqualsInitialNumber());

        lfsr.PrintBinaryOutputSequence();
        lfsr.PrintSequenceLength();
        lfsr.PrintByteSequenceRepresentation();
        lfsr.CalculateEvenOddElements();
        lfsr.CalculateZerosOnesElements();
        lfsr.PrintEvenOddCounters();
        lfsr.PrintZerosOnesCounters();

        Console.ReadKey();
    }
}