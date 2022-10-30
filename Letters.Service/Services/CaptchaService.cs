using Letters.Service.Exceptions;
using Letters.Service.Interfaces;
using Letters.Service.Model;
using Letters.Service.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Letters.Service.Services
{
    public class CaptchaService : ICaptchaService
    {
        private Captcha _capchaSettings;
        private readonly IMemoryCache _cache;

        public CaptchaService(IOptions<Captcha> capchaSettings, IMemoryCache cache)
        {
            _capchaSettings = capchaSettings.Value;
            _cache = cache;
        }

        public bool Validate(Guid id, string value)
        {
            if (!_cache.TryGetValue(id, out CapthcaModel capthca))
            {
                throw new OwnException($"капча по идентификатору {id}");
            }

            return string.Equals(capthca.Value, value, StringComparison.OrdinalIgnoreCase);
        }

        public CapthcaModel GetCapcha()
        {
            var captcha = new CapthcaModel
            {
                Id = Guid.NewGuid(),
                Value = GenerateCapcha(_capchaSettings.LenghtSymbols)
            };

            captcha.Image = GenerateImage(captcha.Value, _capchaSettings.Width, _capchaSettings.Height);
            captcha.Audio = GenerateAudio(captcha.Value);

            _cache.Set(captcha.Id, captcha, TimeSpan.FromMinutes(10));

            return captcha;
        }

        public Core.Models.CaptchaModel Update(Guid key)
        {
            var captchaModel = new Core.Models.CaptchaModel();

            if (_cache.TryGetValue(key, out CapthcaModel captcha))
            {
                captcha.Value = GenerateCapcha(_capchaSettings.LenghtSymbols).ToUpper();
                captcha.Image = GenerateImage(captcha.Value, _capchaSettings.Width, _capchaSettings.Height);
                captcha.Audio = GenerateAudio(captcha.Value);

                _cache.Set(captcha.Id, captcha, TimeSpan.FromMinutes(10));

                captchaModel.Id = captcha.Id;
                captchaModel.Audio = captcha.Audio;
                captchaModel.Image = captcha.Image;
            }
            else
                throw new OwnException($"Иденфтикатор капчи{captcha} не найден");

            return captchaModel;
        }

        private static string GenerateCapcha(int count)
        {
            if (count <= 0)
                throw new OwnException("Не верное кол-во символов для капчи");

            var chars = "абвгдежзиклмнпрстуфхцчшщэюя".ToCharArray();

            var capchaValue = string.Empty;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                capchaValue += chars[random.Next(chars.Length)];
            }

            return capchaValue;
        }

        private static string GenerateImage(string capchaCode, int width, int height)
        {
            FontCollection collection = new();

            FontFamily family = collection.Add("Font/BureauAP.ttf");

            Font font = family.CreateFont(10, FontStyle.Regular);

            using var image = new Image<Rgba32>(width, height);
            image.Mutate(picture =>
            {
                picture.DrawText(capchaCode, font, Color.Black, new PointF(10, 10));

                picture.SetGraphicsOptions(_ => _.Antialias = true);
            });

            using var stream = new MemoryStream();

            image.Save(stream, PngFormat.Instance);

            return Convert.ToBase64String(stream.ToArray());
        }

        private static string GenerateAudio(string key)
        {
            var wavReader = new List<ISampleProvider>();

            foreach (var element in key.ToCharArray())
            {
                wavReader.Add(new WaveFileReader($"Audio/{element}.wav").ToSampleProvider());
            }

            var merged = new ConcatenatingSampleProvider(wavReader);

            var waveProvider = merged.ToWaveProvider16();

            using var stream = new MemoryStream();

            WaveFileWriter.WriteWavFileToStream(stream, waveProvider);

            return Convert.ToBase64String(stream.ToArray());
        }


        public byte[] Test(string key = "БВГД")
        {
            key = GenerateCapcha(6).ToLower();

            var wavReader = new List<ISampleProvider>();

            foreach (var element in key.ToCharArray())
            {
                wavReader.Add(new WaveFileReader($"Audio/{element}.wav").ToSampleProvider());
            }

            var merged = new ConcatenatingSampleProvider(wavReader);

            var waveProvider = merged.ToWaveProvider16();

            using var stream = new MemoryStream();

            WaveFileWriter.WriteWavFileToStream(stream, waveProvider);

            return stream.ToArray();
        }
    }
}
