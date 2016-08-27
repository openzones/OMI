using OMInsurance.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OMInsurance.WebApps.Security
{
    public class Cryptographer
    {
        #region Fields

        private readonly byte[] _encryptionKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="encryptionKey">Key that should be used to encrypt/decrypt data.</param>
        public Cryptographer(byte[] encryptionKey)
        {
            if (encryptionKey == null)
            {
                throw new ArgumentNullException("encryptionKey");
            }

            _encryptionKey = encryptionKey;
        }

        #endregion

        #region Encrypt Methods

        public byte[] Encrypt(byte[] input)
        {
            byte[] iv;
            byte[] encryptedValue = EncryptValue(input, out iv);
            byte[] encryptedIV = EncryptIV(iv);

            byte[] output = new byte[encryptedIV.Length + encryptedValue.Length];
            Array.Copy(encryptedIV, 0, output, 0, encryptedIV.Length);
            Array.Copy(encryptedValue, 0, output, encryptedIV.Length, encryptedValue.Length);

            return output;
        }

        private byte[] EncryptValue(byte[] value, out byte[] iv)
        {
            byte[] paddedValue = PadUtils.Pad(value, 16, 0);

            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.None;
                rijndael.GenerateIV();

                using (ICryptoTransform encryptor = rijndael.CreateEncryptor(_encryptionKey, rijndael.IV))
                using (MemoryStream encryptedStream = new MemoryStream(paddedValue.Length * 2))
                using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(paddedValue, 0, paddedValue.Length);

                    iv = rijndael.IV;
                    return encryptedStream.ToArray();
                }
            }
        }

        private byte[] EncryptIV(byte[] iv)
        {
            byte[] paddedIV = PadUtils.Pad(iv, 16, 0);

            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.ECB;
                rijndael.Padding = PaddingMode.None;

                using (ICryptoTransform encryptor = rijndael.CreateEncryptor(_encryptionKey, null))
                using (MemoryStream encryptedStream = new MemoryStream(paddedIV.Length * 2))
                using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(paddedIV, 0, paddedIV.Length);
                    return encryptedStream.ToArray();
                }
            }
        }

        #endregion

        #region Decrypt Methods

        public byte[] Decrypt(byte[] input)
        {
            // Retrieve encrypted IV, encrypted IV has 128 bits
            byte[] encryptedIV = new byte[16];
            byte[] encryptedValue = new byte[input.Length - encryptedIV.Length];

            Array.Copy(input, 0, encryptedIV, 0, encryptedIV.Length);
            Array.Copy(input, encryptedIV.Length, encryptedValue, 0, encryptedValue.Length);

            byte[] decryptedIV = DecryptIV(encryptedIV);
            byte[] decryptedValue = DecryptValue(encryptedValue, decryptedIV);

            return decryptedValue;
        }

        private byte[] DecryptValue(byte[] encryptedValue, byte[] iv)
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.None;

                using (ICryptoTransform decryptor = rijndael.CreateDecryptor(_encryptionKey, iv))
                using (MemoryStream decryptedStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedValue, 0, encryptedValue.Length);
                    return PadUtils.Unpad(decryptedStream.ToArray(), 0);
                }
            }
        }

        /// <summary>
        /// Decrypts initialization vector that was used to encrypt primary value.
        /// </summary>
        /// <param name="encryptedIV">Encrypted initialization vector.</param>
        /// <returns>Decrypted initialization vector.</returns>
        private byte[] DecryptIV(byte[] encryptedIV)
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.BlockSize = 128;
                rijndael.Mode = CipherMode.ECB;
                rijndael.Padding = PaddingMode.None;

                using (ICryptoTransform decryptor = rijndael.CreateDecryptor(_encryptionKey, null))
                using (MemoryStream decryptedStream = new MemoryStream(encryptedIV.Length * 2))
                using (CryptoStream cryptoStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedIV, 0, encryptedIV.Length);
                    return decryptedStream.ToArray();
                }
            }
        }

        #endregion

        #region Helper Methods

        public static byte[] GetRsaBasedPrivateKey()
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "AppContainerName";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;

            using (RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams))
            {
                string rsaKeyXml = rsaKey.ToXmlString(true);
                byte[] rsaKeyBinary = Encoding.Default.GetBytes(rsaKeyXml);

                // Convert RSA binary data to byte array that has the same length as RSA key size
                int keySizeinBytes = rsaKey.KeySize / 8;
                using (MD5 md5 = MD5.Create())
                {
                    return GetFixedSizeHash(md5, keySizeinBytes, rsaKeyBinary);
                }
            }
        }

        private static byte[] GetFixedSizeHash(HashAlgorithm hashAlgorithm, int hashSize, byte[] data)
        {
            if (hashSize <= 0)
            {
                throw new ArgumentOutOfRangeException("hashSize", "Hash size must have a positive value.");
            }
            if (hashSize > data.Length)
            {
                throw new ArgumentOutOfRangeException("hashSize", "Specified hash size is larger than data length.");
            }

            List<byte> hash = new List<byte>(hashSize);

            // Fill full blocks of hash (if specified hash size if larger than output of hash algorithm, then resulting hash will be populated as several blocks)
            int fullBlocksCount = hashSize / hashAlgorithm.HashSize;
            if (fullBlocksCount > 0)
            {
                int dataBlockSize = data.Length / fullBlocksCount;  // This value is always positive, because data.Length > hashSize
                for (int dataOffset = 0; dataOffset < data.Length; dataOffset += dataBlockSize)
                {
                    byte[] hashBlock = hashAlgorithm.ComputeHash(data, dataOffset, dataBlockSize);
                    hash.AddRange(hashBlock);
                }
            }

            // If there's a remainder, then we need to fill last block of desired hash with a partial result produced by hash algorithm
            int partialBlockSize = hashSize % hashAlgorithm.HashSize;
            if (partialBlockSize > 0)
            {
                byte[] fullHashBlock = hashAlgorithm.ComputeHash(data);
                hash.AddRange(fullHashBlock.Take(partialBlockSize));
            }

            return hash.ToArray();
        }

        #endregion
    }
}