Documentation for Enterprise Library 5 can be found here: http://msdn.microsoft.com/en-us/library/ff632023.aspx
Nuget.exe must be added to PATH on build system

* To change the location of the log file set:
	<app|web>.config
		loggingConfiguration
			listeners
				add
					fileName="<path\nameoffile.txt>"