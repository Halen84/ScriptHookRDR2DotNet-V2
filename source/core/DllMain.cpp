//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/ScriptHookRDR2dotnet#license
//

#pragma managed(push, off)

#include <Windows.h>

#pragma managed(pop)

// Has to be a managed variable since C++ exceptions will be ruined if this is unmanaged one
bool sGameReloaded = false;

// Import C# code base
#using "ScriptHookRDRDotNet.netmodule"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

[assembly:AssemblyTitle("Script Hook RDR2 .NET")] ;
[assembly:AssemblyDescription("An ASI plugin for Red Dead Redemption 2, which allows running scripts written in any .NET language in-game.")] ;
[assembly:AssemblyCompany("Salty, SHVDN: crosire & contributors")] ;
[assembly:AssemblyProduct("ScriptHookRDRDotNet")] ;
[assembly:AssemblyCopyright("Copyright © 2015 crosire | Copyright © 2019 Salty")] ;
[assembly:AssemblyVersion("1.5.5.4")] ;
[assembly:AssemblyFileVersion("1.5.5.4")] ;
// Sign with a strong name to distinguish from older versions and cause .NET framework runtime to bind the correct assemblies
// There is no version check performed for assemblies without strong names (https://docs.microsoft.com/en-us/dotnet/framework/deployment/how-the-runtime-locates-assemblies)
[assembly:AssemblyKeyFileAttribute("PublicKeyToken.snk")] ;


public ref class ScriptHookRDRDotNet // This is not a static class, so that console scripts can inherit from it for ConsoleInput class
{
public:
	[RDR2DN::ConsoleCommand("Print the default help")]
	static void Help()
	{
		console->PrintInfo("~c~--- Help ---");
		console->PrintInfo("The console accepts ~h~C# expressions~h~ as input and has full access to the scripting API. To print the result of an expression, simply add \"return\" in front of it.");
		console->PrintInfo("You can use \"P\" as a shortcut for the player character and \"V\" for the current vehicle (without the quotes).");
		console->PrintInfo("Example: \"return P.IsAlive\" will print a boolean value to the console indicating whether the player is currently alive.");
		console->PrintInfo("~c~--- Commands ---");
		console->PrintHelpText();
	}

	[RDR2DN::ConsoleCommand("Print the help for a specific command")]
	static void Help(String^ command)
	{
		console->PrintHelpText(command);
	}

	[RDR2DN::ConsoleCommand("Clear the console history and pages")]
	static void Clear()
	{
		console->Clear();
	}

	[RDR2DN::ConsoleCommand("Reload all scripts from the scripts directory")]
	static void Reload()
	{
		console->PrintInfo("~y~Reloading ...");

		// Force a reload on next tick
		sGameReloaded = true;
	}

	[RDR2DN::ConsoleCommand("Load scripts from a file")]
	static void Start(String^ filename)
	{
		if (!IO::Path::IsPathRooted(filename))
			filename = IO::Path::Combine(domain->ScriptPath, filename);
		if (!IO::Path::HasExtension(filename))
			filename += ".dll";

		String^ ext = IO::Path::GetExtension(filename)->ToLower();
		if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
			console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
			return;
		}

		domain->StartScripts(filename);
	}

	[RDR2DN::ConsoleCommand("Abort all scripts from a file")]
	static void Abort(String^ filename)
	{
		if (!IO::Path::IsPathRooted(filename))
			filename = IO::Path::Combine(domain->ScriptPath, filename);
		if (!IO::Path::HasExtension(filename))
			filename += ".dll";

		String^ ext = IO::Path::GetExtension(filename)->ToLower();
		if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
			console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
			return;
		}

		domain->AbortScripts(filename);
	}

	[RDR2DN::ConsoleCommand("Abort all scripts currently running")]
	static void AbortAll()
	{
		domain->Abort();

		console->PrintInfo("Stopped all running scripts. Use \"Start(filename)\" to start them again.");
	}

	[RDR2DN::ConsoleCommand("List all loaded scripts")]
	static void ListScripts()
	{
		console->PrintInfo("~c~--- Loaded Scripts ---");
		for each (auto script in domain->RunningScripts)
			console->PrintInfo(IO::Path::GetFileName(script->FileName) + " ~h~" + script->Name + (script->IsRunning ? (script->IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
	}

internal:
	static RDR2DN::Console^ console = nullptr;
	static RDR2DN::ScriptDomain^ domain = RDR2DN::ScriptDomain::CurrentDomain;
	static WinForms::Keys reloadKey = WinForms::Keys::None;
	static WinForms::Keys consoleKey = WinForms::Keys::F8;
	static unsigned int scriptTimeoutThreshold = 5000;

	static void SetConsole()
	{
		console = (RDR2DN::Console^)AppDomain::CurrentDomain->GetData("Console");
	}
};

static void ForceCLRInit()
{
	// Just a function that doesn't do anything, except for being compiled to MSIL
}

static void ScriptHookRDRDotNet_ManagedInit()
{
	RDR2DN::Console^% console = ScriptHookRDRDotNet::console;
	RDR2DN::ScriptDomain^% domain = ScriptHookRDRDotNet::domain;
	List<String^>^ stashedConsoleCommandHistory = gcnew List<String^>();

	// Unload previous domain (this unloads all script assemblies too)
	if (domain != nullptr)
	{
		// Stash the command history if console is loaded 
		if (console != nullptr)
		{
			stashedConsoleCommandHistory = console->CommandHistory;
		}

		RDR2DN::ScriptDomain::Unload(domain);
	}


	// Clear log from previous runs
	RDR2DN::Log::Clear();

	// Load configuration
	String^ scriptPath = "scripts";

	try
	{
		array<String^>^ config = IO::File::ReadAllLines(IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));

		for each (String ^ line in config)
		{
			// Perform some very basic key/value parsing
			line = line->Trim();
			if (line->StartsWith("//"))
				continue;
			array<String^>^ data = line->Split('=');
			if (data->Length != 2)
				continue;

			// May fail to parse without trimming whitespaces
			String^ keyStr = data[0]->Trim();
			String^ valueStr = data[1]->Trim();

			if (String::Equals(keyStr, "ReloadKey", StringComparison::OrdinalIgnoreCase))
				Enum::TryParse(valueStr, true, ScriptHookRDRDotNet::reloadKey);
			else if (String::Equals(keyStr, "ConsoleKey", StringComparison::OrdinalIgnoreCase))
				Enum::TryParse(valueStr, true, ScriptHookRDRDotNet::consoleKey);
			else if (String::Equals(keyStr, "ScriptTimeoutThreshold", StringComparison::OrdinalIgnoreCase))
			{
				unsigned int outVal;
				if (UInt32::TryParse(valueStr, outVal))
				{
					ScriptHookRDRDotNet::scriptTimeoutThreshold = outVal;
				}
			}
		}
	}
	catch (Exception^ ex)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "Failed to load config: ", ex->ToString());
	}

	// Create a separate script domain
	String^ directory = IO::Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location);
	domain = RDR2DN::ScriptDomain::Load(directory, scriptPath);
	if (domain == nullptr)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "ScriptDomain::Load() returned null in ", scriptPath);
		return;
	}

	domain->ScriptTimeoutThreshold = ScriptHookRDRDotNet::scriptTimeoutThreshold;

	// Console Stuff
	try
	{
		// Instantiate console inside script domain, so that it can access the scripting API
		console = (RDR2DN::Console^)domain->AppDomain->CreateInstanceFromAndUnwrap(
			RDR2DN::Console::typeid->Assembly->Location, RDR2DN::Console::typeid->FullName);

		// Restore the console command history (set a empty history for the first time)
		console->CommandHistory = stashedConsoleCommandHistory;

		// Print welcome message
		console->PrintInfo("~c~--- Community Script Hook RDR2 .NET V2 ---");
		console->PrintInfo("~c~--- Type \"Help()\" to print an overview of available commands ---");

		// Update console pointer in script domain
		domain->AppDomain->SetData("Console", console);
		domain->AppDomain->DoCallBack(gcnew CrossAppDomainDelegate(&ScriptHookRDRDotNet::SetConsole));

		// Add default console commands
		console->RegisterCommands(ScriptHookRDRDotNet::typeid);
	}
	catch (Exception^ ex)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "Failed to create console: ", ex->ToString());
	}

	// Start scripts in the newly created domain
	domain->Start();
}

static void ScriptHookRDRDotNet_ManagedTick()
{
	RDR2DN::Console^ console = ScriptHookRDRDotNet::console;
	if (console != nullptr)
		console->DoTick();

	RDR2DN::ScriptDomain^ scriptdomain = ScriptHookRDRDotNet::domain;
	if (scriptdomain != nullptr)
		scriptdomain->DoTick();
}

static void ScriptHookRDRDotNet_ManagedKeyboardMessage(unsigned long keycode, bool keydown, bool ctrl, bool shift, bool alt)
{
	// Filter out invalid key codes
	if (keycode <= 0 || keycode >= 256)
		return;

	// Convert message into a key event
	auto keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	RDR2DN::Console^ console = ScriptHookRDRDotNet::console;
	if (console != nullptr)
	{
		if (keydown && keys == ScriptHookRDRDotNet::reloadKey)
		{
			// Force a reload
			ScriptHookRDRDotNet::Reload();
			return;
		}
		if (keydown && keys == ScriptHookRDRDotNet::consoleKey)
		{
			// Toggle open state
			console->IsOpen = !console->IsOpen;
			return;
		}

		// Send key events to console
		console->DoKeyEvent(keys, keydown);

		// Do not send keyboard events to other running scripts when console is open
		if (console->IsOpen)
			return;
	}

	RDR2DN::ScriptDomain ^scriptDomain = ScriptHookRDRDotNet::domain;
	if (scriptDomain != nullptr)
	{
		// Send key events to all scripts
		scriptDomain->DoKeyEvent(keys, keydown);
	}
}

#pragma unmanaged

#include <Main.h>

PVOID sGameFiber = nullptr;

static void ScriptMain()
{
	// ScriptHookRDR2 already turned the current thread into a fiber, so we can safely retrieve it.
	sGameFiber = GetCurrentFiber();

	while (true)
	{
		sGameReloaded = false;

		ScriptHookRDRDotNet_ManagedInit();

		while (!sGameReloaded)
		{
			// ScriptHookRDR2 creates a new fiber only right after a "Started thread" message is written to the log
			const PVOID currentFiber = GetCurrentFiber();
			if (currentFiber != sGameFiber)
			{
				sGameFiber = currentFiber;
				sGameReloaded = true;
				break;
			}

			ScriptHookRDRDotNet_ManagedTick();
			scriptWait(0);
		}
	}
}

static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	ScriptHookRDRDotNet_ManagedKeyboardMessage(
		key,
		!isUpNow,
		(GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0,
		(GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0,
		isWithAlt != FALSE);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
	case DLL_PROCESS_ATTACH:
		// Avoid unnecessary DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications
		DisableThreadLibraryCalls(hModule);
		// Call a managed function to force the CLR to initialize immediately
		// This is technically a very bad idea (https://learn.microsoft.com/cpp/dotnet/initialization-of-mixed-assemblies), but fixes a crash that would otherwise occur when the CLR is initialized later on
		if (!GetModuleHandle(TEXT("clr.dll")))
			ForceCLRInit();
		// Register ScriptHookRDRDotNet native script
		scriptRegister(hModule, ScriptMain);
		// Register handler for keyboard messages
		keyboardHandlerRegister(ScriptKeyboardMessage);
		break;
	case DLL_PROCESS_DETACH:
		// Unregister ScriptHookRDRDotNet native script
		scriptUnregister(hModule);
		// Unregister handler for keyboard messages
		keyboardHandlerUnregister(ScriptKeyboardMessage);
		break;
	}

	return TRUE;
}


