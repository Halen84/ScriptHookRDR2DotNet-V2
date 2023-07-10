using RDR2.Native;

namespace RDR2.UI
{
	internal sealed class Prompt : INativeValue
	{
		public int Handle { get; private set; }

		public Prompt(int handle)
		{
			Handle = handle;
		}

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Create a interactable HUD prompt.
		/// </summary>
		public static Prompt CreatePrompt(eInputType control, eUseContextMode mode, string text, int numMashes = 0, int holdTimeMs = 4000, int depletionTimeMs = 4000, int fillTimeMs = 4000, PromptTimingEvent timedEvent = 0)
		{
			Prompt prompt = new Prompt(HUD._UI_PROMPT_REGISTER_BEGIN());
			HUD._UI_PROMPT_SET_CONTROL_ACTION(prompt.Handle, (uint)control);
			prompt.Text = text;
			prompt.Priority = 3;

			switch (mode)
			{
				case eUseContextMode.Press:
					HUD._UI_PROMPT_SET_STANDARD_MODE(prompt.Handle, false);
					break;
				case eUseContextMode.TimedPress:
					HUD._UI_PROMPT_SET_PRESSED_TIMED_MODE(prompt.Handle, depletionTimeMs);
					break;
				case eUseContextMode.Release:
					HUD._UI_PROMPT_SET_STANDARD_MODE(prompt.Handle, true);
					break;
				case eUseContextMode.Hold:
					HUD._UI_PROMPT_SET_HOLD_INDEFINITELY_MODE(prompt.Handle);
					break;
				case eUseContextMode.TimedEvent:
					HUD._UI_PROMPT_SET_STANDARDIZED_HOLD_MODE(prompt.Handle, (uint)timedEvent);
					break;
				case eUseContextMode.AutoFill:
					HUD._UI_PROMPT_SET_HOLD_AUTO_FILL_MODE(prompt.Handle, fillTimeMs, holdTimeMs);
					break;
				case eUseContextMode.AutoFillWithDecay:
					HUD._UI_PROMPT_SET_HOLD_AUTO_FILL_WITH_DECAY_MODE(prompt.Handle, fillTimeMs, holdTimeMs);
					break;
				case eUseContextMode.Mash:
					HUD._UI_PROMPT_SET_MASH_MODE(prompt.Handle, numMashes);
					break;
				case eUseContextMode.MashAutoFill:
					HUD._UI_PROMPT_SET_MASH_AUTO_FILL_MODE(prompt.Handle, fillTimeMs, numMashes);
					break;
				case eUseContextMode.MashResistance:
					HUD._UI_PROMPT_SET_MASH_WITH_RESISTANCE_MODE(prompt.Handle, numMashes, 0.0f, 0.0f); // TODO: Figure out what these floats do
					break;
				case eUseContextMode.MashResistanceCanFail:
					HUD._UI_PROMPT_SET_MASH_WITH_RESISTANCE_CAN_FAIL_MODE(prompt.Handle, numMashes, 0.0f, 0.0f); // TODO: Figure out what these floats do
					break;
				case eUseContextMode.MashResistanceDynamic:
					HUD._UI_PROMPT_SET_MASH_MANUAL_MODE(prompt.Handle, (1.0f / 10.0f), 0.0f, 0.0f, 0); // TODO: Figure out what these floats do
					break;
				case eUseContextMode.MashResistanceDynamicCanFail:
					HUD._UI_PROMPT_SET_MASH_MANUAL_CAN_FAIL_MODE(prompt.Handle, (1.0f / 10.0f), 0.0f, 0.0f, 0); // TODO: Figure out what these floats do
					break;
				case eUseContextMode.MashIndefinitely:
					HUD._UI_PROMPT_SET_MASH_INDEFINITELY_MODE(prompt.Handle);
					break;
				case eUseContextMode.Rotate:
					HUD._UI_PROMPT_SET_ROTATE_MODE(prompt.Handle, 1.0f, false);
					HUD._UI_PROMPT_SET_ATTRIBUTE(prompt.Handle, 10, true);
					break;
				case eUseContextMode.TargetMeter:
					HUD._UI_PROMPT_SET_TARGET_MODE(prompt.Handle, 0.5f, 0.1f, 0);
					break;
				default:
					break;
			}

			HUD._UI_PROMPT_REGISTER_END(prompt.Handle);
			prompt.Visible = false;
			prompt.Enabled = false;
			return prompt;
		}

		private eInputType _controlAction = eInputType.UndefinedInput;
		/// <summary>
		/// Gets or sets the control action (<see cref="eInputType"/>) of this <see cref="Prompt"/>.
		/// </summary>
		public eInputType ControlAction
		{
			get => _controlAction;
			set
			{
				_controlAction = value;
				HUD._UI_PROMPT_SET_CONTROL_ACTION(Handle, (uint)_controlAction);
			}
		}

		private string _text = string.Empty;
		/// <summary>
		/// Gets or sets the text on this <see cref="Prompt"/>.
		/// </summary>
		public string Text
		{
			// TODO: Fix this. Only works with strings already registered in the database.
			// VAR_STRING does not work with this. Why?
			get => _text;
			set
			{
				_text = MISC.VAR_STRING(10, "LITERAL_STRING", value);
				HUD._UI_PROMPT_SET_TEXT(Handle, _text);
			}
		}

		/// <summary>
		/// Gets or sets the visibility on this <see cref="Prompt"/>.
		/// </summary>
		public bool Visible
		{
			get => HUD._UI_PROMPT_IS_ACTIVE(Handle);
			set => HUD._UI_PROMPT_SET_VISIBLE(Handle, value);
		}

		/// <summary>
		/// Gets or sets if this <see cref="Prompt"/> is enabled.
		/// </summary>
		public bool Enabled
		{
			get => HUD._UI_PROMPT_IS_ENABLED(Handle);
			set => HUD._UI_PROMPT_SET_ENABLED(Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/> is pressed.
		/// </summary>
		public bool IsPressed => HUD._UI_PROMPT_IS_PRESSED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/> is just pressed.
		/// </summary>
		public bool IsJustPressed => HUD._UI_PROMPT_IS_JUST_PRESSED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/> is released.
		/// </summary>
		public bool IsReleased => HUD._UI_PROMPT_IS_RELEASED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/> is just pressed.
		/// </summary>
		public bool IsJustReleased => HUD._UI_PROMPT_IS_JUST_RELEASED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s hold mode has completed.
		/// </summary>
		public bool HasHoldModeCompleted => HUD._UI_PROMPT_HAS_HOLD_MODE_COMPLETED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s mash mode has completed.
		/// </summary>
		public bool HasMashModeCompleted => HUD._UI_PROMPT_HAS_MASH_MODE_COMPLETED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s mash mode has failed.
		/// </summary>
		public bool HasMashModeFailed => HUD._UI_PROMPT_HAS_MASH_MODE_FAILED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s timed event completed.
		/// </summary>
		public bool HasTimedEventCompleted => HUD._UI_PROMPT_HAS_PRESSED_TIMED_MODE_COMPLETED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s timed event failed.
		/// </summary>
		public bool HasTimedEventFailed => HUD._UI_PROMPT_HAS_PRESSED_TIMED_MODE_FAILED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/>'s <see cref="eUseContextMode.Press"/> or <see cref="eUseContextMode.Release"/> mode has been completed.
		/// </summary>
		public bool HasCompleted => HUD._UI_PROMPT_HAS_STANDARD_MODE_COMPLETED(Handle, 0);

		/// <summary>
		/// Sets the <see cref="Prompt"/> priority level
		/// </summary>
		public int Priority
		{
			set => HUD._UI_PROMPT_SET_PRIORITY(Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Prompt"/> is pulsing.
		/// </summary>
		public bool IsPulsing
		{
			get => HUD._UI_PROMPT_GET_URGENT_PULSING_ENABLED(Handle);
			set => HUD._UI_PROMPT_SET_URGENT_PULSING_ENABLED(Handle, value);
		}

		/// <summary>
		/// Set whether this <see cref="Prompt"/> should activate when the <see cref="ControlAction"/> has been released
		/// </summary>
		public bool ActivateOnRelease
		{
			set => HUD._UI_PROMPT_SET_STANDARD_MODE(Handle, value);
		}

		/// <summary>
		/// Set whether this <see cref="Prompt"/> should activate when the <see cref="ControlAction"/> was just pressed
		/// </summary>
		public bool ActivateOnPress
		{
			set => HUD._UI_PROMPT_SET_STANDARD_MODE(Handle, !value);
		}

		public bool Exists()
		{
			return HUD._UI_PROMPT_IS_VALID(Handle);
		}

		public void Delete()
		{
			HUD._UI_PROMPT_DELETE(Handle);
		}

		public bool Equals(Prompt other)
		{
			return !ReferenceEquals(null, other) && other.Handle == Handle;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Prompt)obj);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}

		public static implicit operator int(Prompt prompt) => prompt.Handle;
	}

	public enum eUseContextMode : int
	{
		Press,
		TimedPress,
		Release,
		Hold,
		TimedEvent,
		AutoFill,
		AutoFillWithDecay,
		Mash,
		MashAutoFill,
		MashResistance,
		MashResistanceCanFail,
		MashResistanceDynamic,
		MashResistanceDynamicCanFail,
		MashIndefinitely,
		/// <summary>
		/// Gamepad Only
		/// </summary>
		Rotate,
		TargetMeter,
	}

	public enum PromptTimingEvent : uint
	{
		Short = 0x65943D74,
		Medium = 0xCF1E51DE,
		Long = 0x87A8DDB3,
		RustlingCalmTiming = 0xBA926371,
		PlayerFocusTiming = 0x687E21CA,
		PlayerReactionTiming = 0x70BF5857,
	}

}
