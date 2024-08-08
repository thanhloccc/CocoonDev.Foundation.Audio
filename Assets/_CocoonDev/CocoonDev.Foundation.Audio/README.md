# CocoonDev.Foundation.Audio

AmbientMusic: Music that is generally long and heavy that will be played for a long time.
	- CompressedInMemory : AudioClipLoadType.Streaming
	- compressionFormat : AudioCompressionFormat.Vorbis
	- quality = 60

FrequentSound: Sound that is generally short, not very heavy and will be played many times (shot, steps, UI...)
	- compressionFormat = AudioCompressionFormat.ADPCM
	- loadType = AudioClipLoadType.DecompressOnLoad
	- quality = 100

OccasionalSound: A sound that is generally short, not very heavy, and will not be played very frequently
	- compressionFormat = AudioCompressionFormat.Vorbis
	- loadType = AudioClipLoadType.CompressedInMemory
	- quality = 35


