using RDR2.Native;

namespace RDR2
{
	public static class Audio
	{
		public static bool PrepareSoundset(string soundset)
		{
			return AUDIO.PREPARE_SOUNDSET(soundset, false);
		}

		public static bool PrepareSound(string soundName, string soundset)
		{
			return AUDIO.PREPARE_SOUND(soundName, soundset, -2);
		}

		public static int PlaySoundFrontend(string soundName, string soundset)
		{
			AUDIO.PLAY_SOUND_FRONTEND(soundName, soundset, true, 0);
			return AUDIO.GET_SOUND_ID();
		}

		public static int PlaySound(string soundName, string soundset)
		{
			AUDIO.PLAY_SOUND(soundName, soundset, false, 0, true, 0);
			return AUDIO.GET_SOUND_ID();
		}

		public static void StopSound(string soundName, string soundset)
		{
			AUDIO._STOP_SOUND_WITH_NAME(soundName, soundset);
		}

		public static void StopSound(int id)
		{
			AUDIO._STOP_SOUND_WITH_ID(id);
		}

		public static void ReleaseSound(int id)
		{
			AUDIO.RELEASE_SOUND_ID(id);
		}

		public static void SetAudioFlag(string flagName, bool toggle)
		{
			AUDIO.SET_AUDIO_FLAG(flagName, toggle);
		}

		public static void SetAudioVariable(string soundName, string soundSet, string variableName, float variableValue)
		{
			AUDIO._SET_VARIABLE_ON_SOUND_WITH_NAME(variableName, variableValue, soundName, soundSet);
		}

		public static void SetAudioVariable(int id, string variableName, float variableValue)
		{
			AUDIO._SET_VARIABLE_ON_SOUND_WITH_ID(id, variableName, variableValue);
		}
	}
}
