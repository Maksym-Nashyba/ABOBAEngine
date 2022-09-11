namespace ABOBAEngine.Utils;

public class LineByLineReader : IDisposable
{
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

    public ReadOnlyMemory<char> PeekLine()
    {
        return LineReadDelegate.Invoke();
    }

    private ReadOnlyMemory<char> GetNextLine()
    {
        ReadOnlySpan<char> span = _activeBuffer.ReadOnlyMemory.Span;
        int position = _position;
        while (span[position] != '\n') position++;
        return _activeBuffer.ReadOnlyMemory.Slice(0, position - _position - 1);
    }
    

    private ReadOnlyMemory<char> GetNextLineWithChecks()
    {
        throw new NotImplementedException();
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
        buffer.IsFull = readLength == _bufferLength;
    }
    
    public void Dispose()
    {
        _streamReader.Dispose();
    }

    private class TextBuffer
    {
        public ReadOnlyMemory<char> ReadOnlyMemory => Memory;
        public bool IsFull;
        public readonly Memory<char> Memory;

        public TextBuffer(int bufferLength)
        {
            Memory = new Memory<char>(new char[bufferLength]);
        }
    }
}
