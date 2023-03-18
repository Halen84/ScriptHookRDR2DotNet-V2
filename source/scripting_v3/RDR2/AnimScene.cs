using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public class AnimScene : INativeValue
	{
		public AnimScene(int handle)
		{
			Handle = handle;
		}

		public int Handle { get; private set; }

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> has loaded.
		/// </summary>
		public bool IsLoaded => ANIMSCENE.IS_ANIM_SCENE_LOADED(Handle, true, false);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> has its metadata loaded.
		/// </summary>
		public bool IsMetadataLoaded => ANIMSCENE.IS_ANIM_SCENE_METADATA_LOADED(Handle, false);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> is loading.
		/// </summary>
		public bool IsLoading => ANIMSCENE._IS_ANIM_SCENE_LOADING(Handle, true);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> is running.
		/// </summary>
		public bool IsRunning => ANIMSCENE.IS_ANIM_SCENE_RUNNING(Handle, false);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> is finished.
		/// </summary>
		public bool IsFinished => ANIMSCENE.IS_ANIM_SCENE_FINISHED(Handle, false);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> is currently exiting this frame.
		/// </summary>
		public bool IsExitingThisFrame => ANIMSCENE.IS_ANIM_SCENE_EXITING_THIS_FRAME(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> has exited.
		/// </summary>
		public bool HasExited => ANIMSCENE.HAS_ANIM_SCENE_EXITED(Handle, false);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> is aborted.
		/// </summary>
		public bool IsAborted => ANIMSCENE._IS_ANIM_SCENE_ABORTED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> can be skipped.
		/// </summary>
		public bool IsSkippable => ANIMSCENE._IS_ANIM_SCENE_SKIPPABLE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> can be skipped.
		/// </summary>
		/// <remarks>This is an alias for <see cref="IsSkippable"/></remarks>
		public bool CanBeSkipped => IsSkippable;

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> was skipped.
		/// </summary>
		public bool WasSkipped => ANIMSCENE.WAS_ANIM_SCENE_SKIPPED(Handle);

		/// <summary>
		/// Gets or sets whether this <see cref="AnimScene"/> is paused.
		/// </summary>
		public bool IsPaused
		{
			get => ANIMSCENE._IS_ANIM_SCENE_PAUSED(Handle);
			set => ANIMSCENE.SET_ANIM_SCENE_PAUSED(Handle, value);
		}

		/// <summary>
		/// Gets the dictionary hash of this <see cref="AnimScene"/>
		/// </summary>
		public uint Dictionary => ANIMSCENE._GET_ANIM_SCENE_DICT(Handle);

		/// <summary>
		/// Gets the phase this <see cref="AnimScene"/> is at
		/// </summary>
		public float Phase => ANIMSCENE.GET_ANIM_SCENE_PHASE(Handle);

		/// <summary>
		/// Gets the duration length of this <see cref="AnimScene"/>
		/// </summary>
		public float Duration => ANIMSCENE._GET_ANIM_SCENE_DURATION(Handle);

		/// <summary>
		/// Gets the current time this <see cref="AnimScene"/> is at
		/// </summary>
		public float Time => ANIMSCENE._GET_ANIM_SCENE_TIME(Handle);

		/// <summary>
		/// Gets or sets this <see cref="AnimScene"/>'s rate.
		/// </summary>
		public float Rate
		{
			get => ANIMSCENE._GET_ANIM_SCENE_RATE(Handle);
			set => ANIMSCENE.SET_ANIM_SCENE_RATE(Handle, value);
		}

		/// <summary>
		/// Load this <see cref="AnimScene"/>
		/// </summary>
		public void Load()
		{
			if (!IsLoaded && !IsLoading) {
				ANIMSCENE.LOAD_ANIM_SCENE(Handle);
			}
		}

		/// <summary>
		/// Start this <see cref="AnimScene"/>
		/// </summary>
		public void Start()
		{
			if (IsLoaded && !IsRunning) {
				ANIMSCENE.START_ANIM_SCENE(Handle);
			}
		}

		/// <summary>
		/// Abort this <see cref="AnimScene"/>
		/// </summary>
		public void Abort()
		{
			ANIMSCENE.ABORT_ANIM_SCENE(Handle, true);
		}

		/// <summary>
		/// Trigger a skip for this <see cref="AnimScene"/>
		/// </summary>
		public void Skip()
		{
			if (CanBeSkipped) {
				ANIMSCENE.TRIGGER_ANIM_SCENE_SKIP(Handle);
			}
		}


		#region Entities

		public bool DoesEntityIDExist(string entityId)
		{
			return ANIMSCENE._DOES_ENTITY_WITH_ID_EXIST_IN_ANIM_SCENE(Handle, entityId);
		}

		public void AddEntity(string entityId, Entity entity)
		{
			ANIMSCENE.SET_ANIM_SCENE_ENTITY(Handle, entityId, entity, 0);
		}

		public void RemoveEntity(string entityId, Entity entity)
		{
			ANIMSCENE.REMOVE_ANIM_SCENE_ENTITY(Handle, entityId, entity);
		}

		public bool IsEntityPlayingAnimScene(Entity entity)
		{
			return ANIMSCENE.IS_ENTITY_PLAYING_ANIM_SCENE(entity, Handle);
		}

		#endregion

		#region Playlists

		//ANIMSCENE.SET_ANIM_SCENE_PLAYBACK_LIST(Handle, name);
		//ANIMSCENE._DOES_ANIM_SCENE_PLAY_LIST_EXIST(Handle, name);
		//ANIMSCENE.SET_ANIM_SCENE_PLAY_LIST(Handle, name, true);
		//ANIMSCENE._IS_ANIM_SCENE_PLAYBACK_LIST_PHASE_ACTIVE(Handle, name);
		//ANIMSCENE.REQUEST_ANIM_SCENE_PLAY_LIST(Handle, name);
		//ANIMSCENE._RELEASE_ANIM_SCENE_PLAY_LIST(Handle, name);
		//ANIMSCENE._IS_ANIM_SCENE_PLAYBACK_LIST_PHASE_LOADED(Handle, name);
		//ANIMSCENE._IS_ANIM_SCENE_PLAYBACK_LIST_PHASE_LOADING(Handle, name);

		public bool IsInSection(string sectionName)
		{
			return ANIMSCENE.IS_ANIM_SCENE_IN_SECTION(Handle, sectionName, true);
		}

		#endregion

		#region Position & Rotation

		public void SetOrigin(Vector3 position, Vector3 rotation)
		{
			ANIMSCENE.SET_ANIM_SCENE_ORIGIN(Handle, position, rotation, 2);
		}

		public unsafe void GetOrigin(Vector3* outPosition, Vector3* outRotation)
		{
			ANIMSCENE.GET_ANIM_SCENE_ORIGIN(Handle, outPosition, outRotation, 2);
		}

		#endregion


		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimScene"/> exists.
		/// </summary>
		public bool Exists()
		{
			return ANIMSCENE.DOES_ANIM_SCENE_EXIST(Handle);
		}

		/// <summary>
		/// Delete this <see cref="AnimScene"/>
		/// </summary>
		public void Delete()
		{
			ANIMSCENE._DELETE_ANIM_SCENE(Handle);
		}

		public static implicit operator int(AnimScene e) => e.Handle;
	}
}
