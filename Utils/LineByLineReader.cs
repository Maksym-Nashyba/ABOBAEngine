namespace ABOBAEngine.Utils;

public class LineByLineReader : IDisposable
{
    public bool IsEmpty { get; private set; }
    private StreamReader _streamReader;
    private int _position;
    
    private TextBuffer[] _buffers;
    private TextBuffer _activeBuffer;
    private readonly int _bufferLength;

    private Func<ReadOnlyMemory<char>> LineReadDelegate;
    
    public LineByLineReader(StreamReader streamReader, int bufferLength = 32000)
    {
        _streamReader = streamReader;
        _bufferLength = bufferLength;
        
        _buffers = new[] { new TextBuffer(bufferLength), new TextBuffer(bufferLength) };
        _activeBuffer = _buffers[0];
        InitializeBuffers();
    }

    public ReadOnlyMemory<char> ReadLine()
    {
        ReadOnlyMemory<char> result = LineReadDelegate.Invoke();
        _position += result.Length+1;
        if (_position >= _bufferLength - 1) SwapBuffers();
        return result;
    }

    private ReadOnlyMemory<char> GetNextLine()
    {
        ReadOnlySpan<char> span = _activeBuffer.ReadOnlyMemory.Span;
        int iteratorPosition = _position;
        while (span[iteratorPosition] != '\n')
        {
            if (iteratorPosition + 1 >= _bufferLength) return GetNextLineOnVerge();
            iteratorPosition++;
        }
        return _activeBuffer.ReadOnlyMemory.Slice(_position, iteratorPosition - _position);
    }
    

    private ReadOnlyMemory<char> GetNextLineWithChecks()
    {
        ReadOnlySpan<char> span = _activeBuffer.ReadOnlyMemory.Span;
        int iteratorPosition = _position;
        while (span[iteratorPosition] != '\n')
        {
            if (iteratorPosition < _activeBuffer.Length)
            {
                IsEmpty = true;
                break;
            }
            if (iteratorPosition + 1 >= _bufferLength) return GetNextLineOnVerge();
            iteratorPosition++;
        }
        return _activeBuffer.ReadOnlyMemory.Slice(_position, iteratorPosition - _position);
    }

    private ReadOnlyMemory<char> GetNextLineOnVerge()
    {
        ReadOnlySpan<char> span = _activeBuffer.ReadOnlyMemory.Span;
        List<char> result = new List<char>();
        int startPosition = _position;
        int iteratorPosition;

        for (iteratorPosition = startPosition; iteratorPosition < _bufferLength; iteratorPosition++)
        {
            if (span[iteratorPosition] == '\n') return new ReadOnlyMemory<char>(result.ToArray());
            result.Add(span[iteratorPosition]);
        }
        SwapBuffers();
        ReadOnlyMemory<char> afterTear = LineReadDelegate.Invoke();
        result.AddRange(afterTear.ToArray());
        return new ReadOnlyMemory<char>(result.ToArray());
    }

    private void SwapBuffers()
    {
        ReadToBuffer(_buffers[1]);
        LineReadDelegate = _buffers[1].IsFull ? GetNextLine : GetNextLineWithChecks;
        
        _activeBuffer = _activeBuffer.Equals(_buffers[0]) ? _buffers[1] : _buffers[0];
        _position -= _bufferLength;
    }
    
    private void InitializeBuffers()
    {
        ReadToBuffer(_buffers[0]);
        if (_buffers[0].IsFull)
        {
            LineReadDelegate = GetNextLine;
            ReadToBuffer(_buffers[1]);
        }
        else
        {
            LineReadDelegate = GetNextLineWithChecks; 
        }
    }
    
    private void ReadToBuffer(TextBuffer buffer)
    {
        int readLength = _streamReader.Read(buffer.Memory.Span);
        buffer.Length = readLength;
    }
    
    public void Dispose()
    {
        _streamReader.Dispose();
    }

    private class TextBuffer
    {
        public ReadOnlyMemory<char> ReadOnlyMemory => Memory;
        public bool IsFull => Length == Capacity;
        public int Length;
        public readonly Memory<char> Memory;
        private readonly int Capacity;

        public TextBuffer(int bufferLength)
        {
            Capacity = bufferLength;
            Memory = new Memory<char>(new char[bufferLength]);
        }
    }
}
