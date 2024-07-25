﻿using System.Text;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace ThinQ.CLI.Models;

public static class X509CertificateHelper
{
    public static string CreateCsr(out AsymmetricCipherKeyPair keyPair)
    {
        var subjectName = "CN=AWS IoT Certificate O=Amazon";

        // Create new Object for Issuer and Subject
        var issuer = new X509Name(subjectName);
        var subject = new X509Name(subjectName);

        // Generate the key Value Pair, which in our case is a public Key
        var randomGenerator = new CryptoApiRandomGenerator();
        var random = new SecureRandom(randomGenerator);
        const int strength = 2048;
        var keyGenerationParameters = new KeyGenerationParameters(random, strength);

        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        AsymmetricCipherKeyPair subjectKeyPair = keyPairGenerator.GenerateKeyPair();
        AsymmetricCipherKeyPair issuerKeyPair = subjectKeyPair;

        //PKCS #10 Certificate Signing Request
        Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest("SHA256WITHRSA", subject, issuerKeyPair.Public, null, issuerKeyPair.Private);

        //Convert BouncyCastle CSR to .PEM file.
        StringBuilder CSRPem = new StringBuilder();
        PemWriter CSRPemWriter = new PemWriter(new StringWriter(CSRPem));
        CSRPemWriter.WriteObject(csr);
        CSRPemWriter.Writer.Flush();

        keyPair = subjectKeyPair;

        //get CSR text
        return CSRPem.ToString();
    }
}
