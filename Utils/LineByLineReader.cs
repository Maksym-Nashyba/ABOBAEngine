namespace ABOBAEngine.Utils;

public class LineByLineReader : IDisposable
{
    private StreamReader _streamReader;
    private int _position;
    
    
    private readonly int _expectedMaxLineLength;
    private readonly int _mainBufferLength;
    
    public LineByLineReader(StreamReader streamReader, int expectedMaxLineLength = 64, int mainBufferLength = 32000)
    {
        _streamReader = streamReader;
        _expectedMaxLineLength = expectedMaxLineLength;
        _mainBufferLength = mainBufferLength;
        
        _mainBuffer = new Memory<char>(new char[_mainBufferLength]);
        _reserveBuffer = new Memory<char>(new char[_expectedMaxLineLength]);
    }

    public ReadOnlyMemory<char> ReadLine()
    {
           
    }

    public ReadOnlyMemory<char> PeekLine()
    {
        int positionBefore = _position;
        ReadOnlyMemory<char> result = GetLineFromPosition(_position);
        _position = positionBefore;
        return result;
    }

    private ReadOnlyMemory<char> GetLineFromCurrentPosition()
    {
        return GetLineFromPosition(_position);
    }
    
    private ReadOnlyMemory<char> GetLineFromPosition(int position)
    {
        throw new NotImplementedException();
    }

    private void ConsumeChars(int charsToConsume)
    {
        _position += charsToConsume;
        if(_position > )
    }
    
    public void Dispose()
    {
        _streamReader.Dispose();
    }

    private struct CharBuffer
    {
        public readonly Memory<char> Memory;
        public readonly int Capacity;

        public CharBuffer(int capacity)
        {
            Capacity = capacity;
            Memory = new Memory<char>(new char[Capacity]);
        }
    }
}