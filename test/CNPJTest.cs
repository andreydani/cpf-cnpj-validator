using System.Linq;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using System;

namespace DocumentoHelper.Test;

public class CNPJTest
{
    [Theory]

    // formatted

    [InlineData("06.352.066/0001-27")]
    [InlineData("11.744.636/0001-64")]
    [InlineData("25.713.061/0001-27")]
    [InlineData("31.547.776/0001-50")]
    [InlineData("49.131.124/0001-03")]
    [InlineData("54.458.515/0001-69")]
    [InlineData("68.872.146/0001-60")]
    [InlineData("78.644.026/0001-60")]
    [InlineData("84.717.824/0001-77")]
    [InlineData("93.141.967/0001-74")]

    // unformatted

    [InlineData("06352066000127")]
    [InlineData("11744636000164")]
    [InlineData("25713061000127")]
    [InlineData("31547776000150")]
    [InlineData("49131124000103")]
    [InlineData("54458515000169")]
    [InlineData("68872146000160")]
    [InlineData("78644026000160")]
    [InlineData("84717824000177")]
    [InlineData("93141967000174")]
    public void Validate(string doc)
    {
        CNPJ.Validate(doc).Should().BeTrue();
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    [Theory]

    // formatted

    [InlineData("00.D5H.PL9/AONL-84")]
    [InlineData("4O.X01.0AO/7DD9-03")]
    [InlineData("50.WXI.YTL/4O33-00")]
    [InlineData("61.HXL.XIY/33TQ-64")]
    [InlineData("A8.9Y4.JB9/VLHE-18")]
    [InlineData("B0.8JJ.D0M/FJ3M-84")]
    [InlineData("D2.108.T5M/R2IQ-20")]
    [InlineData("EW.WXK.YC8/HTB2-84")]
    [InlineData("GL.SPN.P58/EQC7-74")]
    [InlineData("IU.BYB.2WV/GJKT-39")]
    [InlineData("LN.L4L.VPN/LDIU-24")]
    [InlineData("QE.V53.HE5/NY08-13")]

    // unformatted

    [InlineData("00D5HPL9AONL84")]
    [InlineData("4OX010AO7DD903")]
    [InlineData("50WXIYTL4O3300")]
    [InlineData("61HXLXIY33TQ64")]
    [InlineData("A89Y4JB9VLHE18")]
    [InlineData("B08JJD0MFJ3M84")]
    [InlineData("D2108T5MR2IQ20")]
    [InlineData("EWWXKYC8HTB284")]
    [InlineData("GLSPNP58EQC774")]
    [InlineData("IUBYB2WVGJKT39")]
    [InlineData("LNL4LVPNLDIU24")]
    [InlineData("QEV53HE5NY0813")]
    public void ValidateAlphaNumeric(string doc)
    {
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    [Theory]
    [InlineData("06@352(066)0001&27")]
    [InlineData("11744$63*600%0164)")]
    public void Validate_ignores_non_digits(string doc)
    {
        CNPJ.Validate(doc).Should().BeTrue();
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();

    }

    [Theory]
    [InlineData("--@#$%----06352066000127.............")]
    [InlineData("¨#$%11¨%¨&*(744)(*63600%¨&*+++0;;;164")]
    public void Validate_has_no_limit_for_string_length(string doc)
    {
        CNPJ.Validate(doc).Should().BeTrue();
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GenerateSource))]
    public void Validate_generate_values(string doc)
    {
        doc.Should().MatchRegex(@"^\d{8}0001\d{2}$");
        CNPJ.Validate(doc).Should().BeTrue();
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GenerateSourceAlphaNumeric))]
    public void Validate_generate_values_alphanumeric(string doc)
    {
        doc.Should().MatchRegex(@"^[a-zA-Z0-9]{12}\d{2}$");
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    public static IEnumerable<object[]> GenerateSource() =>
        Enumerable
        .Range(0, 1000)
        .Select(_ => new[] { CNPJ.Generate() });

    public static IEnumerable<object[]> GenerateSourceAlphaNumeric() =>
        Enumerable
        .Range(0, 1000)
        .Select(_ => new[] { CNPJ.GenerateAlphaNumeric() });

    [Theory]
    [MemberData(nameof(GenerateFormattedSource))]
    public void Validate_generate_formatted_values(string doc)
    {
        doc.Should().MatchRegex(@"^\d{2}\.\d{3}\.\d{3}/0001-\d{2}$");
        CNPJ.Validate(doc).Should().BeTrue();
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GenerateFormattedSourceAlphaNumeric))]
    public void Validate_generate_formatted_values_alpha_numeric(string doc)
    {
        Console.WriteLine(doc);
        doc.Should().MatchRegex(@"^[a-zA-Z0-9]{2}\.[a-zA-Z0-9]{3}\.[a-zA-Z0-9]{3}/[a-zA-Z0-9]{4}-\d{2}$");
        // All previous CNPJs should pass the new formula
        CNPJ.ValidateAlphaNumeric(doc).Should().BeTrue();
    }

    public static IEnumerable<object[]> GenerateFormattedSource() =>
        Enumerable
        .Range(0, 1000)
        .Select(_ => new[] { CNPJ.GenerateFormatted() });

    public static IEnumerable<object[]> GenerateFormattedSourceAlphaNumeric() =>
        Enumerable
        .Range(0, 1000)
        .Select(_ => new[] { CNPJ.GenerateFormattedAlphaNumeric() });

}
