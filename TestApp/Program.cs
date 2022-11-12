using System.Text;
using HashCore;


var bytes = new byte[] { 0b00001000, 0b00100100, 0b00011010, 0b10101100 };

HashCipher cipher = new HashCipher();

int chunkSize = 2;
Console.WriteLine(Convert.ToString(cipher.GetDigest(bytes, chunkSize), 2).PadLeft(chunkSize, '0'));

bytes = new byte[] { 0b00001000, 0b01100100, 0b00011010, 0b10101100 };

Console.WriteLine(Convert.ToString(cipher.GetDigest(bytes, chunkSize), 2).PadLeft(chunkSize, '0'));