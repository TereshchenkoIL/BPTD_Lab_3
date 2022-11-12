using System.Text;
using HashCore;


var bytes = Encoding.ASCII.GetBytes("1233333333333333333333333");

HashCipher cipher = new HashCipher();


Console.WriteLine(cipher.GetDigest(bytes, 4));