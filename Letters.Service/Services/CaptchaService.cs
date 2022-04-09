﻿using Letters.Service.Exceptions;
using Letters.Service.Settings;
using Letters.Service.Model;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Letters.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace Letters.Service.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly Captcha _capchaSettings;
        private readonly IMemoryCache _cache;

        public CaptchaService(IOptions<Captcha> capchaSettings, IMemoryCache cache)
        {
            _capchaSettings = capchaSettings.Value;
            _cache = cache;
        }

        public CapthcaModel GetCapcha()
        {
            var captcha = new CapthcaModel
            {
                Id = Guid.NewGuid(),
                Value = GenerateCapcha(_capchaSettings.LenghtSymbols).ToUpper()
            };

            captcha.Image = GenerateImage(captcha.Value, _capchaSettings.Width, _capchaSettings.Height);
            captcha.Audio = GenerateAudio(captcha.Value);

            _cache.Set(captcha.Id, captcha, TimeSpan.FromMinutes(10));

            return captcha;
        }

        public Core.Models.CaptchaModel UpdateCaptcha(Guid key)
        {
            var captchaModel = new Letters.Core.Models.CaptchaModel();

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

            var chars = "абвгдежзиклмнпрстуфхцчшщэюя123456789".ToCharArray();

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

        private string GenerateAudio(string key)
        {
            var wavReader = new List<ISampleProvider>();

            foreach(var element in key.ToCharArray())
            {
                wavReader.Add(new WaveFileReader($"Audio/{element}.wav").ToSampleProvider());
            }

            var merged = new ConcatenatingSampleProvider(wavReader);

            var waveProvider =  merged.ToWaveProvider16();

            using var stream = new MemoryStream();

            WaveFileWriter.WriteWavFileToStream(stream, waveProvider);

            return Convert.ToBase64String(stream.ToArray());
        }


        public byte[] Test(string key = "БВГД")
        {
            FontCollection collection = new();

            FontFamily family = collection.Add("Font/BureauAP.ttf");

            Font font = family.CreateFont(25, FontStyle.Regular);

            using var image = new Image<Rgba32>(150, 96);
            image.Mutate(picture =>
            {
                picture.DrawText(key, font, Color.Black, new PointF(10, 10));

                picture.SetGraphicsOptions(_ => _.Antialias = true);
            });

            using var stream = new MemoryStream();

            image.Save(stream, PngFormat.Instance);

            return stream.ToArray();
        }
    }
}