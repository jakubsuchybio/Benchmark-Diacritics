using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark_Diacritics
{
    public class Program
    {
        private const string _ascii = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _diacritics = "žščřďťňáéěíóúůýŽŠČŘĎŤŇÁÉĚÍÓÚŮÝ";

        public static string GenerateWord(int length)
        {
            var sb = new StringBuilder(length);
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                bool diac = random.Next(0, 2) == 1;
                if (diac)
                    sb.Append(_diacritics[random.Next(0, _diacritics.Length)]);
                else
                    sb.Append(_ascii[random.Next(0, _ascii.Length)]);
            }

            return sb.ToString();
        }

        [MemoryDiagnoser]
        public class Diacritics_OneLarge
        {
            private string _data;

            [Params(1_000_000)]
            public int N;

            [GlobalSetup]
            public void Setup()
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _data = GenerateWord(N);
            }

            [Benchmark(Baseline = true)]
            public string WithNormalize() =>
                string.Concat(_data
                    .Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                .Normalize(NormalizationForm.FormC);

            [Benchmark]
            public string FromStackOverflow()
            {
                string stFormD = _data.Normalize(NormalizationForm.FormD);
                int len = stFormD.Length;
                var sb = new StringBuilder();
                for (int i = 0; i < len; i++)
                    if (CharUnicodeInfo.GetUnicodeCategory(stFormD[i]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                        sb.Append(stFormD[i]);

                return (sb.ToString().Normalize(NormalizationForm.FormC));
            }

            [Benchmark]
            public string WithEncoding() => Encoding.ASCII.GetString(Encoding.GetEncoding("windows-1250").GetBytes(_data));
        }

        [MemoryDiagnoser]
        public class Diacritics_MultipleOfSmallOnes
        {
            private string[] _data;

            [Params(1_000_000)]
            public int N;

            [GlobalSetup]
            public void Setup()
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                _data = new string[N];
                var random = new Random();
                for (int i = 0; i < N; i++)
                    _data[i] = GenerateWord(random.Next(30, 256));
            }

            [Benchmark(Baseline = true)]
            public string[] WithNormalize()
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    _data[i] = string.Concat(_data[i]
                        .Normalize(NormalizationForm.FormD)
                        .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                    .Normalize(NormalizationForm.FormC);
                }

                return _data;
            }

            [Benchmark]
            public string[] FromStackOverflow()
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    string stFormD = _data[i].Normalize(NormalizationForm.FormD);
                    int len = stFormD.Length;
                    var sb = new StringBuilder();
                    for (int j = 0; j < len; j++)
                        if (CharUnicodeInfo.GetUnicodeCategory(stFormD[j]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                            sb.Append(stFormD[j]);

                    _data[i] = sb.ToString().Normalize(NormalizationForm.FormC);
                }

                return _data;
            }

            [Benchmark]
            public string[] WithEncoding()
            {
                for (int i = 0; i < _data.Length; i++)
                    _data[i] = Encoding.ASCII.GetString(Encoding.GetEncoding("windows-1250").GetBytes(_data[i]));

                return _data;
            }
        }

        public class Diacritics_MultipleWithEncoding
        {
            private string[] _data;

            [Params(1_000_000)]
            public int N;

            [GlobalSetup]
            public void Setup()
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                _data = new string[N];
                var random = new Random();
                for (int i = 0; i < N; i++)
                    _data[i] = GenerateWord(random.Next(30, 256));
            }

            [Benchmark(Baseline = true)]
            public void Encoding_Serial()
            {
                foreach(var data in _data)
                    Encoding.ASCII.GetString(Encoding.GetEncoding("windows-1250").GetBytes(data));
            }

            [Benchmark]
            public void Encoding_ParallelForEach()
            {
                Parallel.ForEach(_data, data => Encoding.ASCII.GetString(Encoding.GetEncoding("windows-1250").GetBytes(data)));
            }

        }

        public static void Main()
        {
            BenchmarkRunner.Run<Diacritics_OneLarge>();
            BenchmarkRunner.Run<Diacritics_MultipleOfSmallOnes>();
            BenchmarkRunner.Run<Diacritics_MultipleWithEncoding>();
        }
    }
}
