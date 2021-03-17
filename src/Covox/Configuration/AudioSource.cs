using System.IO;

#nullable disable

namespace Covox
{
    public class AudioSource
    {
        protected AudioSource() { }

        public static AudioSource FromDefaultMicrophone() =>
            new DefaultMicrophoneAudioSource();

        public static AudioSource FromMicrophone(string microphoneId) =>
            new MicrophoneAudioSource(microphoneId);

        public static AudioSource FromStream(Stream stream) =>
            new StreamAudioSource(stream);
    }

    internal class DefaultMicrophoneAudioSource : AudioSource
    {
    }

    internal class MicrophoneAudioSource : AudioSource
    {
        public MicrophoneAudioSource(string microphoneId) =>
            MicrophoneId = microphoneId;

        public string MicrophoneId { get; }
    }

    internal class StreamAudioSource : AudioSource
    {
        public StreamAudioSource(Stream stream) =>
            Stream = stream;

        public Stream Stream { get; }
    }
}
