﻿using System.Text;

namespace ABOBAEngine.Rendering.Models;

public abstract class ObjLineParser
{
    public abstract char[] Pattern();

    public bool LineFits(ReadOnlyMemory<char> line)
    {
        ReadOnlySpan<char> span = line.Span;
        char[] pattern = Pattern();
        if (span.Length < pattern.Length) return false;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] != span[i]) return false;
        }

        return true;
    }

    public abstract void Parse(ReadOnlyMemory<char> line);
}

public sealed class VertexObjLineParser : ObjLineParser
{
    public readonly List<float> Vertices = new List<float>();
    private readonly char[] _pattern = { 'v', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        ReadOnlySpan<char> span = line.Span;
        int position = 0;
        while (span[position] != ' ') position++;
        span = span.Slice(position + 1);
        position = 0;
        for (int i = 0; i < 2; i++)
        {
            while (span[position] != ' ') position++;
            Vertices.Add(float.Parse(span.Slice(0, position)));
            span = span.Slice(++position);
            position = 0;
        }

        Vertices.Add(float.Parse(span));
    }
}

public sealed class TriangleObjLineParser : ObjLineParser
{
    public readonly List<uint> Triangles = new List<uint>();
    private readonly char[] _pattern = { 'f', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        ReadOnlySpan<char> span = line.Span;
        for (int i = 0; i < 3; i++)
        {
            int position = 0;
            while (span[position] != ' ') position++;
            span = span.Slice(position + 1);
            position = 0;
            while (span[position] != '/') position++;
            Triangles.Add(uint.Parse(span.Slice(0, position)) - 1);
            span = span.Slice(++position);
        }
    }
}

public sealed class AlbedoUVObjLineParser : ObjLineParser
{
    public readonly List<float> AlbedoUVs = new List<float>();
    private readonly char[] _pattern = { 'v', 't', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        ReadOnlySpan<char> span = line.Span;
        int position = 0;
        while (span[position] != ' ') position++;
        span = span.Slice(position + 1);
        position = 0;

        while (span[position] != ' ') position++;
        AlbedoUVs.Add(float.Parse(span.Slice(0, position)));
        span = span.Slice(++position);

        AlbedoUVs.Add(float.Parse(span));
    }
}