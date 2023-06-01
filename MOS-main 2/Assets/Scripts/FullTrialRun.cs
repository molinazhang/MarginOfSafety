using System.Collections.Generic;
using System;

[Serializable]
public class FullTrialRun 
{
	public string GUID, ProlificID;
	public List<Trial> Trials;

	public FullTrialRun() 
	{
		Trials = new List<Trial>();
		GUID = Guid.NewGuid().ToString();
	}
}
