using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

// ReSharper disable CheckNamespace
public class PostProcessor : MonoBehaviour 
// ReSharper restore CheckNamespace 
{
    //999 is to execute after all other postbuild processes
    [PostProcessBuild(999)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //Where the post processor python script is
        string postProcessorScript = Path.Combine(Application.dataPath, "Editor/GameWhizzes/Support files/PostProcessor/GWPostProcessor.py");
		
        //Directory that contains .projmods json files
        string packagePath = Path.Combine(Application.dataPath, "Editor/GameWhizzes");

        //Combine everything to feed as parameters to python runtime executable
        string argsForPythonScript = "\"" + postProcessorScript + "\" \""+pathToBuiltProject+"\" \""+packagePath+"\"";
		
		
		//Call python with the args above
		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
			  FileName = "python2.6",
			  Arguments = argsForPythonScript,
			  UseShellExecute = false,
			  RedirectStandardOutput = true,
			  CreateNoWindow = true
			}
		};
		process.Start();
        process.WaitForExit();

	}
}
