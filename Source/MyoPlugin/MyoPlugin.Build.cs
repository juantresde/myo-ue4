// Copyright 1998-2014 Epic Games, Inc. All Rights Reserved.

namespace UnrealBuildTool.Rules
{
    using System.IO;

	public class MyoPlugin : ModuleRules
	{
        private string ModulePath
        {
            get { return Path.GetDirectoryName(RulesCompiler.GetModuleFilename(this.GetType().Name)); }
        }

        private string ThirdPartyPath
        {
            get { return Path.GetFullPath(Path.Combine(ModulePath, "../ThirdParty/")); }
        }

        private string LibrariesPath
        {
            get { return Path.Combine(ThirdPartyPath, "Thalmic", "Lib"); }
        }

        private string IncludePath
        {
            get { return Path.Combine(ThirdPartyPath, "Thalmic", "Include"); }
        }

        private string BinariesPath
        {
            get { return Path.Combine(ThirdPartyPath, "Thalmic", "Binaries"); }
        }

        public MyoPlugin(TargetInfo Target)
		{
			PublicIncludePaths.AddRange(
				new string[] {
                    "MyoPlugin/Public",
					// ... add public include paths required here ...
				}
				);

			PrivateIncludePaths.AddRange(
				new string[] {
					"MyoPlugin/Private",
                    IncludePath,
					// ... add other private include paths required here ...
				}
				);

			PublicDependencyModuleNames.AddRange(
				new string[]
				{
					"Core",
					"CoreUObject",
                    "Engine",
                    "InputCore",
                    "Slate",
                    "SlateCore"
					// ... add other public dependencies that you statically link with here ...
				}
				);

			PrivateDependencyModuleNames.AddRange(
				new string[]
				{
					// ... add private dependencies that you statically link with here ...
				}
				);

			DynamicallyLoadedModuleNames.AddRange(
				new string[]
				{
					// ... add any modules that your module loads dynamically here ...
				}
				);

            LoadMyoLib(Target);
		}

        public bool LoadMyoLib(TargetInfo Target)
        {
            bool isLibrarySupported = false;

            if ((Target.Platform == UnrealTargetPlatform.Win64) || (Target.Platform == UnrealTargetPlatform.Win32))
            {
                isLibrarySupported = true;

                string PlatformString = (Target.Platform == UnrealTargetPlatform.Win64) ? "64" : "32";

                PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "myo" + PlatformString + ".lib"));

                string DLLString = Path.Combine(BinariesPath, "Win" + PlatformString, "myo" + PlatformString + ".dll");
                PublicDelayLoadDLLs.Add(DLLString);
                RuntimeDependencies.Add(new RuntimeDependency(Path.Combine(BinariesPath, "Win64", "myo64.dll")));

                //Include Path
                PublicIncludePaths.Add(Path.Combine(ThirdPartyPath, "Myo", "Include"));
            }

            Definitions.Add(string.Format("WITH_MYO_BINDING={0}", isLibrarySupported ? 1 : 0));

            return isLibrarySupported;
        }
	}

}