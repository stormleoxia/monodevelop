﻿//
// ProjectConfiguration.cs
//
// Author:
//       Todd Berman  <tberman@off.net>
//       Lluis Sanchez Gual <lluis@novell.com>
//       Viktoria Dudka  <viktoriad@remobjects.com>
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2004 Todd Berman
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// Copyright (c) 2009 RemObjects Software
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Text;
using MonoDevelop.Core;

namespace MonoDevelop.Ide.Templates
{
	public class ProjectConfiguration
	{
		public ProjectConfiguration ()
		{
			ProjectName = String.Empty;
			SolutionName = String.Empty;
			Location = String.Empty;
			ProjectFileExtension = String.Empty;

			CreateProjectDirectoryInsideSolutionDirectory = true;

			CreateGitIgnoreFile = true;
			UseGit = true;
		}

		public string ProjectName { get; set; }
		public string ProjectFileExtension { get; set; }
		public string SolutionName { get; set; }
		public string Location { get; set; }

		public string ProjectFileName {
			get { return ProjectName + ProjectFileExtension; }
		}

		public string SolutionFileName {
			get { return SolutionName + ".sln"; }
		}

		public bool CreateProjectDirectoryInsideSolutionDirectory { get; set; }
		public bool CreateSolution { get; set; }

		public bool UseGit { get; set; }
		public bool CreateGitIgnoreFile { get; set; }

		public string SolutionLocation {
			get {
				if (CreateProjectDirectoryInsideSolutionDirectory)
					return System.IO.Path.Combine (Location, GetValidDir (SolutionName));
				else
					return System.IO.Path.Combine (Location, GetValidDir (ProjectName));
			}
		}

		public string ProjectLocation {
			get {
				string path = Location;
				if (CreateProjectDirectoryInsideSolutionDirectory)
					path = System.IO.Path.Combine (path, GetValidDir (SolutionName));

				return System.IO.Path.Combine (path, GetValidDir (ProjectName));
			}
		}

		string GetValidDir (string name)
		{
			name = name.Trim ();
			var sb = new StringBuilder ();
			for (int n = 0; n < name.Length; n++) {
				char c = name [n];
				if (Array.IndexOf (System.IO.Path.GetInvalidPathChars(), c) != -1)
					continue;
				if (c == System.IO.Path.DirectorySeparatorChar || c == System.IO.Path.AltDirectorySeparatorChar || c == System.IO.Path.VolumeSeparatorChar)
					continue;
				sb.Append (c);
			}
			return sb.ToString ();
		}

		public bool IsValid ()
		{
			string solution = SolutionName;
			string name     = ProjectName;
			string location = ProjectLocation;

			if (solution.Equals ("")) solution = name; //This was empty when adding after first combine

			return !((CreateSeparateSolutionDirectory && !FileService.IsValidPath (solution)) ||
				!FileService.IsValidFileName (name) ||
				name.IndexOf (' ') >= 0 ||
				!FileService.IsValidPath (location));
		}

		bool CreateSeparateSolutionDirectory {
			get { return CreateSolution && CreateProjectDirectoryInsideSolutionDirectory; }
		}
	}
}

