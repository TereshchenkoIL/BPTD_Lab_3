using System.Numerics;

namespace HashCore;

public class HashCipher
{
    private string[] _chunks = Array.Empty<string>();
    private uint[] _chunksResults = Array.Empty<uint>();
    private int _maxChunkValue;
    private int _chunkSize;

    public uint GetDigest(byte[] info, int chunkSize = 4)
    {
        if (info == null || info.Length == 0)
        {
            throw new ArgumentException();
        }

        _chunkSize = chunkSize;
        
        _maxChunkValue = Convert.ToInt32(new string('1', chunkSize), 2);
        
        string binaryRepresentation = String.Join("", info.Select(x => 
            Convert.ToString(x, 2).PadLeft(8, '0')));

        CreateChunks(binaryRepresentation, chunkSize);

        HashChunks();

        return _chunksResults.Last();
    }

    private void HashChunks()
    {
        _chunksResults[0] = ((uint)(0b10101010 ^ Convert.ToInt32(_chunks[0], 2)));

        var sum = _chunksResults[0];

        for (int i = 1; i < _chunks.Length; i++)
        {
            uint currentValue = Convert.ToUInt32(_chunks[i], 2);

            sum += (uint)((sum + currentValue) % _maxChunkValue);

            //sum = (uint)(sum >> (int)_chunksResults[i]) | (sum << (_chunkSize - (int)_chunkSize));

            uint xoredValue = _chunksResults[i - 1] ^ currentValue;

            _chunksResults[i] = (uint)((sum + xoredValue * (_chunksResults[i - 1] + 1)) % _maxChunkValue);
        }
    }

    private void CreateChunks(string binaryRepresentation, int chunkSize = 4)
    {
        int remainder = binaryRepresentation.Length % chunkSize;

        if (remainder != 0)
        {
            int totalAmountOfAdds = (int) Math.Ceiling((chunkSize - remainder) / (decimal)2);
            binaryRepresentation += new string('0', totalAmountOfAdds);
        }
        
        int i = 0;
        
        _chunks = binaryRepresentation.GroupBy(_ => i++ / chunkSize).Select(g => string.Join("", g)).ToArray();
        _chunksResults = new uint[_chunks.Length];
    }
}