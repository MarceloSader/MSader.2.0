using System;
using System.IO;
using msader.Helpers;
using NUnit.Framework;
using OpenAI.Audio;

namespace MSader.OpenAI.Audio
{
    public partial class AudioFeatures
    {
        [Test]
        public string SimpleTranscription()
        {
            AudioClient client = new("whisper-1", MyConstants.openAIKey);

            string audioFilePath = Path.Combine("OpenAI/Assets", "audio_houseplant_care.mp3");

            AudioTranscription transcription = client.TranscribeAudio(audioFilePath);

            return $"{transcription.Text}";
        }
    }
}
