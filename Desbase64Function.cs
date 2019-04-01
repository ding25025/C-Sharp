
        //DES加解密
        //字串加密
        String encStr="12345678";
        private string desEncryptBase64(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(encStr);
            byte[] iv = Encoding.ASCII.GetBytes(encStr);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

            des.Key = key;
            des.IV = iv;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            return encrypt;
        }

        //字串解密
        private string desDecryptBase64(string encrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(encStr);
            byte[] iv = Encoding.ASCII.GetBytes(encStr);
            des.Key = key;
            des.IV = iv;

            byte[] dataByteArray = Convert.FromBase64String(encrypt);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        //檔案加密
        private void desEncryptFile(string sourceFile, string encryptFile)
        {
            if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(encryptFile))
            {
                return;
            }
            if (!File.Exists(sourceFile))
            {
                return;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(encStr);
            byte[] iv = Encoding.ASCII.GetBytes(encStr);

            des.Key = key;
            des.IV = iv;

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Create, FileAccess.Write))
            {
                //檔案加密
                byte[] dataByteArray = new byte[sourceStream.Length];
                sourceStream.Read(dataByteArray, 0, dataByteArray.Length);

                using (CryptoStream cs = new CryptoStream(encryptStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                }
            }
        }
         //檔案解密
        private void desDecryptFile(string encryptFile, string decryptFile)
        {
            if (string.IsNullOrEmpty(encryptFile) || string.IsNullOrEmpty(decryptFile))
            {
                return;
            }
            if (!File.Exists(encryptFile))
            {
                return;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(encStr);
            byte[] iv = Encoding.ASCII.GetBytes(encStr);

            des.Key = key;
            des.IV = iv;

            using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Open, FileAccess.Read))
            using (FileStream decryptStream = new FileStream(decryptFile, FileMode.Create, FileAccess.Write))
            {
                byte[] dataByteArray = new byte[encryptStream.Length];
                encryptStream.Read(dataByteArray, 0, dataByteArray.Length);
                using (CryptoStream cs = new CryptoStream(decryptStream, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                }
            }
        }