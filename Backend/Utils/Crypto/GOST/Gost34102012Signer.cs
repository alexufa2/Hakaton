using GostCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GostCrypto
{
	public class Gost34102012Signer
	{
		private BigInteger PrivateKey_d { get; set; }
		private CECPoint PublicKey_Q { get; set; }

		readonly BigInteger p = new BigInteger("6277101735386680763835789423207666416083908700390324961279", 10);
		readonly BigInteger a = new BigInteger("-3", 10);
		readonly BigInteger b = new BigInteger("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1", 16);
		readonly byte[] xG = fromHexStringToByte("03188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012");
		readonly BigInteger n = new BigInteger("ffffffffffffffffffffffff99def836146bc9b1b4d22831", 16);

		CStribog hash { get; set; }
		CDS DS { get; set; }

		public Gost34102012Signer(BigInteger privateKey)
		{
			DS = new CDS(p, a, b, n, xG);
			PrivateKey_d = privateKey;
			PublicKey_Q = DS.genPublicKey(PrivateKey_d);
			hash = new CStribog(512);
		}

		private static byte[] fromHexStringToByte(string input)
		{
			byte[] data = new byte[input.Length / 2];
			string HexByte = "";
			for (int i = 0; i < data.Length; i++)
			{
				HexByte = input.Substring(i * 2, 2);
				data[i] = Convert.ToByte(HexByte, 16);
			}
			return data;
		}

		public string Sign(string message)
		{
			byte[] H = hash.GetHash(Encoding.UTF8.GetBytes(message));
			return DS.genDS(H, PrivateKey_d);
		}

		public bool SignIsValid(string message, string signature)
		{
			byte[] H2 = hash.GetHash(Encoding.UTF8.GetBytes(message));
			return DS.verifDS(H2, signature, PublicKey_Q);
		}
	}
}
