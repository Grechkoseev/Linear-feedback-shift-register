namespace Lab_2;

internal class Lfsr
{
    private readonly int[] _indices;
    private readonly int[] _register = new int[8];
    private readonly int _initialValue;

    private int SequenceLength { get; set; }
    private int EvenCounter { get; set; }
    private int OddCounter { get; set; }
    private int ZeroCounter { get; set; }
    private int OneCounter { get; set; }
    private List<int> BinaryOutputSequence { get; }
    private List<int> ByteSequenceList { get; }

    public Lfsr(int[] indices, int initialValue)
    {
        _indices = new int[indices.Length];

        for (var i = 0; i < _indices.Length; i++)
        {
            _indices[i] = indices[i];
        }

        _initialValue = initialValue;
        BinaryOutputSequence = new List<int>();
        ByteSequenceList = new List<int>();

    }

    public void PutNumberToRegister(int number)
    {
        var binaryString = Convert.ToString(number, 2);
        var arr = binaryString.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray();

        for (int i = 0, j = 0; i < _register.Length; i++)
        {
            if (arr.Length >= _register.Length - i)
            {
                _register[i] = arr[j];
                j++;
            }
            else
            {
                _register[i] = 0;
            }
        }
    }

    public void PrintNumberFromRegister()
    {
        ConvertRegisterToNumber(out var registerNumber);
        Console.Write("{0} ", registerNumber);
    }

    private void ConvertRegisterToNumber(out int number)
    {
        var stringRegister = string.Concat(_register);
        number = Convert.ToInt32(stringRegister, 2);
    }

    public void CalculateNextSequenceMember()
    {
        BinaryOutputSequence.Add(_register[^1]);
        ConvertRegisterToNumber(out var number);
        var firstBit = _indices.Aggregate(0, (current, index) => current ^ _register[index]);
        PutNumberToRegister(number / 2);
        _register[0] = firstBit;
        SequenceLength++;
    }

    public bool IsCurrentSequenceMemberEqualsInitialNumber()
    {
        ConvertRegisterToNumber(out var currentNumber);

        return currentNumber == _initialValue;
    }

    public void PrintSequenceLength()
    {
        Console.WriteLine("\n\nДлина периодна псевдослучайной последовательности равна {0} бит", SequenceLength);
    }

    public void PrintBinaryOutputSequence()
    {
        Console.WriteLine("\n\nВыходная псевдослучайная последовательность бит: "); 
        foreach (var bit in BinaryOutputSequence)
        {
            Console.Write("{0} ", bit );
        }
    }

    public void PrintByteSequenceRepresentation()
    {
        var byteSequenceCount = 0;

        do
        {
            if (BinaryOutputSequence.Count - byteSequenceCount <= 8)
            {
                ByteSequenceList.Add(ConvertListToNumber(BinaryOutputSequence.GetRange(byteSequenceCount,
                    BinaryOutputSequence.Count - byteSequenceCount)));
                byteSequenceCount += BinaryOutputSequence.Count;
            }
            else
            {
                ByteSequenceList.Add(ConvertListToNumber(BinaryOutputSequence.GetRange(byteSequenceCount, 8)));
                byteSequenceCount += 8;
            }
        } while (BinaryOutputSequence.Count - byteSequenceCount > 0);

        Console.WriteLine("\nОднобайтовое представление выходной последовательности:");

        foreach (var byteSequenceNumber in ByteSequenceList)
        {
            Console.Write("{0} ", byteSequenceNumber);
        }
    }

    private static int ConvertListToNumber(List<int> list)
    {
        var stringList = string.Concat(list);
        return Convert.ToInt32(stringList, 2);
    }

    public void CalculateEvenOddElements()
    {
        foreach (var number in ByteSequenceList)
        {
            switch (number % 2)
            {
                case 0:
                    EvenCounter++;
                    break;
                case 1:
                    OddCounter++;
                    break;
            }
        }
    }

    public void CalculateZerosOnesElements()
    {
        foreach (var bit in BinaryOutputSequence)
        {
            switch (bit)
            {
                case 0:
                    ZeroCounter++;
                    break;
                case 1:
                    OneCounter++;
                    break;
            }
        }
    }

    public void PrintEvenOddCounters()
    {
        Console.WriteLine("\n\nКоличество четных элементов последовательности {0}", EvenCounter);
        Console.WriteLine("Количество нечетных элементов последовательности {0}", OddCounter);
    }

    public void PrintZerosOnesCounters()
    {
        Console.WriteLine("\nКоличество нулей в битовом представлении последовательности {0}", ZeroCounter);
        Console.WriteLine("Количество единиц в битовом представлении последовательности {0}", OneCounter);
    }
}