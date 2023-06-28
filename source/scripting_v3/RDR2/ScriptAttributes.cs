//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace RDR2
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ScriptAttributes : Attribute
	{
		public string Author;
		public string SupportURL;
		public bool NoScriptThread;
		public bool NoDefaultInstance;
	}
}
